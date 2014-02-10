using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
using PadCRM.Utils;
namespace PadCRM.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork db;
        private readonly IMemberService MemberService;

        public CustomerService(IUnitOfWork db, IMemberService MemberService)
        {
            this.db = db;
            this.MemberService = MemberService;
        }

        public IQueryable<Customer> GetALL()
        {
            return db.Set<Customer>();
        }

        public IQueryable<Customer> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Customer>();
        }

        public void Create(Customer model)
        {
            db.Add<Customer>(model);
            db.Commit();
        }

        public void Update(Customer model)
        {
            var target = Find(model.ID);
            db.Attach<Customer>(target);
            target.Address = model.Address;
            target.JobID = model.JobID;
            target.CompanyID = model.CompanyID;
            target.Email = model.Email;
            target.Favorite = model.Favorite;
            target.LastTime = model.LastTime;
            target.LastUser = model.LastUser;
            target.IsLeap = model.IsLeap;
            if (target.IsLeap)
            {
                target.BirthDay1 = Utilities.GetLunarStringOnlyMonthDay(model.BirthDay);

            }
            target.BirthDay = model.BirthDay;
            target.Jobs = model.Jobs;
            target.Mobile = model.Mobile;
            target.Mobile1 = model.Mobile1;
            target.Phone = model.Phone;
            target.QQ = model.QQ;
            target.ReMark = model.ReMark;
            db.Commit();
        }

        public void Delete(Customer model)
        {
            var target = Find(model.ID);
            db.Remove<Customer>(target);
            db.Commit();
        }

        public Customer Find(int ID)
        {
            return db.Set<Customer>().Single(x => x.ID == ID);
        }


        public Customer Create(ViewModels.CustomerViewModel model)
        {
            Customer entity = new Customer();
            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.ReMark = model.ReMark;
            entity.IsLeap = model.IsLeap;
            if (entity.IsLeap)
            {
                entity.BirthDay1 = Utilities.GetLunarStringOnlyMonthDay(model.BirthDay);
            }
            entity.BirthDay = model.BirthDay;
            entity.CompanyID = model.CompanyID;
            entity.Email = model.Email;
            entity.Favorite = model.Favorite;
            entity.JobID = model.JobID;
            entity.Jobs = model.Jobs;
            entity.LastTime = DateTime.Now;
            entity.LastUser = 1;
            entity.Mobile = model.Mobile;
            entity.Mobile1 = model.Mobile1;
            entity.Phone = model.Phone;
            entity.QQ = model.QQ;
            db.Add<Customer>(entity);
            db.Commit();
            return entity;
        }


        public Customer Update(ViewModels.CustomerViewModel model)
        {
            Customer entity = Find(model.ID);
            db.Attach<Customer>(entity);
            entity.Name = model.Name;
            entity.Address = model.Address;
            entity.ReMark = model.ReMark;
            entity.Email = model.Email;
            entity.Favorite = model.Favorite;
            entity.IsLeap = model.IsLeap;
            if (entity.IsLeap)
            {
                entity.BirthDay1 = Utilities.GetLunarStringOnlyMonthDay(model.BirthDay);

            }

            entity.BirthDay = model.BirthDay;

            entity.JobID = model.JobID;
            entity.Jobs = model.Jobs;
            entity.LastTime = DateTime.Now;
            entity.LastUser = 1;
            entity.Mobile = model.Mobile;
            entity.Mobile1 = model.Mobile1;
            entity.Phone = model.Phone;
            entity.QQ = model.QQ;
            db.Commit();
            return entity;
        }


        public IQueryable<Customer> GetBirthCustomerInDays(int day, List<int> companyIds)
        {
            int rangeDay = 5;//生日提前天数   

            string Luneryear = Utilities.GetCurrentLunarYear();

            string Ids = string.Join(",", companyIds);

            //阳历农历按表中存的自动检索出对应的生日范围内的信息  
            StringBuilder strSql = new StringBuilder();
            strSql.AppendLine("select ID from Customer ");
            strSql.AppendLine(" where 1=1 ");
            /*阳历生日判断Start*/
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine(" convert(varchar(2),Month(Birthday))+'-'+convert(varchar(2),Day(Birthday))),23)) <= '" + rangeDay + "'");
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine("convert(varchar(2),Month(Birthday))+'-'+convert(varchar(2),Day(Birthday))),23)) >= 0");
            /*阳历生日判断End*/
            strSql.AppendLine(" and isLeap=0 and (Birthday is not null or Birthday!='') and CompanyID in (" + Ids + ")");

            strSql.AppendLine("union all");

            strSql.AppendLine("select ID from Customer ");
            strSql.AppendLine(" where 1=1");
            /*农历生日判断Start*/
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine(" convert(varchar(2),Month(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))+'-'+convert(varchar(2),Day(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))),23)) <= '" + rangeDay + "'");
            strSql.AppendLine("and datediff(day,getdate(),Convert(varchar(100),(convert(varchar(4),Year(getdate()))+'-'+ ");
            strSql.AppendLine("convert(varchar(2),Month(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))+'-'+convert(varchar(2),Day(dbo.fn_GetDate('" + Luneryear + "'+BirthDay1)))),23)) >= 0");
            /*农历生日判断End*/
            strSql.AppendLine(" and isLeap=1 and (BirthDay1 is not null or BirthDay1!='') and CompanyID in (" + Ids + ")");
            List<int> cutomerIds = db.SqlQuery<int>(strSql.ToString()).ToList();
            var query = db.Set<Customer>().Where(x => cutomerIds.Contains(x.ID));
            return query;
        }
    }
}