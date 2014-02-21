using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Web;
using PadCRM.Models;
using PadCRM.Utils;
using PadCRM.ViewModels;
using PadCRM.Service.Interface;
using System.Data.Entity;
using Maitonn.Core;

namespace PadCRM.Service
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork db;
        private readonly IMessageService MessageService;
        private readonly IDepartmentService DepartmentService;
        public MemberService(IUnitOfWork db
            , IMessageService MessageService
            , IDepartmentService DepartmentService

            )
        {
            this.db = db;
            this.MessageService = MessageService;
            this.DepartmentService = DepartmentService;
        }

        #region base

        public IQueryable<Member> GetALL()
        {
            return db.Set<Member>();
        }

        public IQueryable<Member> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Member>();
        }

        public void Create(Member model)
        {
            db.Add<Member>(model);
            db.Commit();
        }


        public void Delete(Member model)
        {
            var target = Find(model.MemberID);
            db.Remove<Member>(target);
            db.Commit();
        }

        public Member Find(int ID)
        {
            return db.Set<Member>().Single(x => x.MemberID == ID);
        }
        #endregion

        #region Login

        public void SetLoginCookie(Member member)
        {
            var permissionStr = string.Empty;
            var listCheckstr = new string[]{
            "manager",
            "punish",
            "boss"
            };
            foreach (var s in listCheckstr)
            {
                var result = CheckPermission(s, "controller", member.MemberID);
                permissionStr += (s + "|" + result.ToString());
                permissionStr += ",";
            }
            permissionStr = permissionStr.Substring(0, permissionStr.Length - 1);
            CookieHelper.LoginCookieSave(member.MemberID.ToString(),
              member.Email,
              member.NickName,
              member.AvtarUrl,
              member.GroupID.ToString(),
              member.Status.ToString(),
              permissionStr,
              member.Password,
              "1",
              "DepartmentID|" + member.DepartmentID.ToString()
             );
        }

        public int Login(string UserName, string Md5Password)
        {
            var LoginUser = db.Set<Member>()
               .SingleOrDefault(x => x.NickName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)
                   && x.Password.Equals(Md5Password, StringComparison.CurrentCultureIgnoreCase));
            if (LoginUser != null)
            {
                if (LoginUser.Status == (int)MemberCurrentStatus.Delete)
                {
                    return -1;
                }
                else
                {
                    db.Attach<Member>(LoginUser);
                    LoginUser.LastIP = HttpHelper.IP;
                    LoginUser.LastTime = DateTime.Now;
                    LoginUser.LoginCount = LoginUser.LoginCount + 1;
                    int memberAction = (int)MemberActionType.Login;
                    Member_Action ma = new Member_Action();
                    ma.ActionType = memberAction;
                    ma.AddTime = DateTime.Now;
                    ma.IP = HttpHelper.IP;
                    ma.Description = "登录";
                    LoginUser.Member_Action.Add(ma);
                    db.Commit();
                    SetLoginCookie(LoginUser);
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }

        #endregion


        private bool CheckPermission(string controller, string action, int MemberID)
        {
            int groupID = Find(MemberID).GroupID;
            var hasPermission = false;
            var query = db.Set<Group>()
                .Include(x => x.Roles)
                .Where(g =>
                    (g.Roles.Any(r =>
                        r.Permissions.Count(p =>
                            p.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                            &&
                            (p.Action.Equals(action, StringComparison.OrdinalIgnoreCase) || p.Action.Equals("controller", StringComparison.OrdinalIgnoreCase))) > 0))
                    && g.ID == groupID);
            hasPermission = query.Any();
            return hasPermission;
        }
        #region validate

        public bool ValidatePassword(int MemberID, string Password)
        {
            string Md5Password = CheckHelper.StrToMd5(Password);
            return db.Set<Member>().Count(x => x.Password.Equals(Md5Password, StringComparison.CurrentCultureIgnoreCase) && x.MemberID == MemberID) > 0;
        }

        #endregion

        #region change


        public void ResetPassword(Member member, string newpassword)
        {
            db.Attach<Member>(member);
            member.Password = CheckHelper.StrToMd5(newpassword);
            db.Commit();
        }

        public bool ChangePassword(int MemberID, string oldpassword, string newpassword)
        {
            bool success = false;
            Member member = Find(MemberID);
            string Md5Password = CheckHelper.StrToMd5(oldpassword);
            if (Md5Password == member.Password)
            {
                success = true;
                db.Attach<Member>(member);
                member.Password = CheckHelper.StrToMd5(newpassword);
                db.Commit();
                SetLoginCookie(member);
            }
            return success;
        }

        #endregion

        public Member FindDescriptionMemberInLimitTime(string description, int limitHours)
        {

            DateTime LimitDate = DateTime.Now.AddHours(-limitHours);
            var query = db.Set<Member>()
                .Include(x => x.Member_Action)
                .Where(x =>
                    (x.Member_Action
                        .Any(ma => ma.Description.Equals(description, StringComparison.OrdinalIgnoreCase)
                            && ma.AddTime > LimitDate
                            )
                     ));
            if (query.Count() > 0)
            {
                return query.Single();
            }
            else
            {
                return null;
            }
        }

        public bool HasGetPasswordActionInLimitTime(string email, int limitMin, int memberAction)
        {
            DateTime LimitDate = DateTime.Now.AddMinutes(-(limitMin));
            string IP = HttpHelper.IP;
            var query = db.Set<Member>()
                .Include(x => x.Member_Action)
                .Where(x =>
                    (x.Member_Action.Any(ma =>
                        ma.ActionType == memberAction
                        && ma.AddTime > LimitDate
                        && ma.IP == IP
                        )) && x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return query.Count() > 0;
        }


        public Member Create(ViewModels.MemberViewModel model)
        {
            var entity = new Member();
            entity.AddIP = HttpHelper.IP;
            entity.Address = model.Address;
            entity.AddTime = DateTime.Now;

            entity.DepartmentID = model.DepartmentID;
            entity.Description = model.Description;
            entity.Email = model.Email;
            entity.GroupID = model.GroupID;
            entity.IsLeader = model.IsLeader;
            entity.IsLeap = model.IsLeap;
            entity.AvtarUrl = model.AvtarUrl;
            if (entity.IsLeap)
            {
                entity.BirthDay1 = Utilities.GetLunarStringOnlyMonthDay(model.BirthDay);
            }
            entity.BirthDay = model.BirthDay;
            entity.LastIP = HttpHelper.IP;
            entity.LastTime = DateTime.Now;
            entity.Mobile = model.Mobile;
            entity.Mobile1 = model.Mobile1;
            entity.NickName = model.NickName;
            entity.Password = CheckHelper.StrToMd5(model.Password);
            entity.QQ = model.QQ;
            entity.Sex = model.Sex;
            entity.StudyExp = model.StudyExp;
            entity.IDNumber = model.IDNumber;
            entity.JobExp = model.JobExp;
            entity.JobTitleID = model.JobTitleID;
            entity.FamilySituation = model.FamilySituation;
            db.Add<Member>(entity);
            db.Commit();

            if (entity.IsLeader)
            {
                var department = DepartmentService.Find(entity.DepartmentID);
                db.Attach<Department>(department);
                department.LeaderID = entity.MemberID;
                db.Commit();
            }
            return entity;
        }


        public void ChangeStatus(string ids, Utils.MemberCurrentStatus status)
        {
            var IdsArray = Utilities.GetIdList(ids);
            db.Set<Member>().Where(x => IdsArray.Contains(x.MemberID)).ToList()
            .ForEach(x =>
            {
                x.Status = (int)status;

            });
            db.Commit();
        }


        public Member Update(MemberEditViewModel model)
        {
            var entity = Find(model.MemberID);
            db.Attach<Member>(entity);
            entity.Address = model.Address;

            entity.DepartmentID = model.DepartmentID;
            entity.Description = model.Description;
            entity.Email = model.Email;
            entity.GroupID = model.GroupID;
            entity.IsLeader = model.IsLeader;
            entity.AvtarUrl = model.AvtarUrl;
            if (entity.IsLeap)
            {
                entity.BirthDay1 = Utilities.GetLunarStringOnlyMonthDay(model.BirthDay);

            }
            entity.BirthDay = model.BirthDay;
            entity.LastIP = HttpHelper.IP;
            entity.LastTime = DateTime.Now;
            entity.Mobile = model.Mobile;
            entity.Mobile1 = model.Mobile1;
            entity.NickName = model.NickName;
            entity.QQ = model.QQ;
            entity.Sex = model.Sex;
            entity.StudyExp = model.StudyExp;
            entity.IDNumber = model.IDNumber;
            entity.JobExp = model.JobExp;
            entity.FamilySituation = model.FamilySituation;
            entity.JobTitleID = model.JobTitleID;
            db.Commit();
            if (entity.IsLeader)
            {
                var department = DepartmentService.Find(entity.DepartmentID);
                db.Attach<Department>(department);
                department.LeaderID = entity.MemberID;
                db.Commit();
            }
            return entity;
        }


        public IQueryable<Member> GetBirthMemberInDays(int day)
        {
            int rangeDay = 5;//生日提前天数   
            ChineseLunisolarCalendar clc = new ChineseLunisolarCalendar();
            string Luneryear = Utilities.ConvertToChineseYearStyle(clc.GetYear(DateTime.Now));//获取当前农历年,并将其转换为中文（此方法就不贴了，so easy）  

            //阳历农历按表中存的自动检索出对应的生日范围内的信息  
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("select MemberID from Member ");
            strSql.AppendLine(" where 1=1 ");
            /*阳历生日判断Start*/
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine(" convert(varchar(2),Month(Birthday))+'-'+convert(varchar(2),Day(Birthday))),23)) <= '" + rangeDay + "'");
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine("convert(varchar(2),Month(Birthday))+'-'+convert(varchar(2),Day(Birthday))),23)) >= 0");
            /*阳历生日判断End*/
            strSql.AppendLine(" and isLeap=0 and (Birthday is not null or Birthday!='')");

            strSql.AppendLine("union all");

            strSql.AppendLine("select MemberID from Member ");
            strSql.AppendLine(" where 1=1");
            /*农历生日判断Start*/
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine(" convert(varchar(2),Month(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))+'-'+convert(varchar(2),Day(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))),23)) <= '" + rangeDay + "'");
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine("convert(varchar(2),Month(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))+'-'+convert(varchar(2),Day(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))),23)) >= 0");
            /*农历生日判断End*/
            strSql.AppendLine(" and isLeap=1 and (BirthDay1 is not null or BirthDay1!='')");

            List<int> Ids = db.SqlQuery<int>(strSql.ToString()).ToList();

            var query = db.Set<Member>().Where(x => Ids.Contains(x.MemberID));
            return query;
        }


        public string GetLunar(DateTime time)
        {
            var timeStr = time.ToString("yyyy-MM-dd");
            string sql = "select dbo.fn_GetLunar('" + timeStr + "') as Lunar";
            string value = db.SqlQuery<string>(sql).First();
            return value;
        }


        public DateTime GetCalender(string lunar)
        {
            string sql = "select dbo.fn_GetDate('" + lunar + "') as Date";
            DateTime value = db.SqlQuery<DateTime>(sql).First();
            return value;
        }


        public List<int> GetMemberIDs(int DepartmentID)
        {
            var depart = DepartmentService.Find(DepartmentID);
            var maxCode = Utilities.GetMaxCode(depart.Code, depart.Level);
            var Ids = new List<int>();
            Ids = GetALL().Include(x => x.Department)
                 .Where(x => x.Department.Code <= maxCode && x.Department.Code >= depart.Code)
                 .Select(x => x.MemberID).ToList();
            return Ids;
        }
    }
}