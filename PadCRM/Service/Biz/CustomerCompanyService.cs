using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using PadCRM.Utils;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class CustomerCompanyService : ICustomerCompanyService
    {
        private readonly IUnitOfWork db;
        private readonly ITraceLogService TraceLogService;
        private readonly IPermissionsService PermissionsService;
        private readonly IMemberService MemberService;
        private readonly ICustomerShareService CustomerShareService;
        public CustomerCompanyService(IUnitOfWork db
            , ITraceLogService TraceLogService
            , IPermissionsService PermissionsService
            , IMemberService MemberService
            , ICustomerShareService CustomerShareService
            )
        {
            this.db = db;
            this.TraceLogService = TraceLogService;
            this.PermissionsService = PermissionsService;
            this.MemberService = MemberService;
            this.CustomerShareService = CustomerShareService;
        }

        public IQueryable<CustomerCompany> GetALL()
        {
            return db.Set<CustomerCompany>();
        }

        public IQueryable<CustomerCompany> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CustomerCompany>();
        }

        public void Create(CustomerCompany model)
        {
            db.Add<CustomerCompany>(model);
            db.Commit();
        }

        public void Update(CustomerCompany model)
        {
            var target = Find(model.ID);
            db.Attach<CustomerCompany>(target);
            target.Address = model.Address;
            target.BrandName = model.BrandName;
            target.CustomerCateID = model.CustomerCateID;
            target.IndustryID = model.IndustryID;
            target.IndustryValue = model.IndustryValue;
            target.CityID = model.CityID;
            target.RelationID = model.RelationID;
            target.Description = model.Description;
            target.Fax = model.Fax;
            target.LastTime = model.LastTime;
            target.LastUser = model.LastUser;
            target.Name = model.Name;
            target.Phone = model.Phone;
            target.Finance = model.Finance;
            target.FinancePhone = model.FinancePhone;
            target.ProxyName = model.ProxyName;
            target.ProxyPhone = model.ProxyPhone;
            target.ProxyAddress = model.ProxyAddress;
            target.IsCommon = false;
            target.AddUser = CookieHelper.MemberID;
            db.Commit();
        }

        public void Delete(CustomerCompany model)
        {
            var target = Find(model.ID);
            db.Remove<CustomerCompany>(target);
            db.Commit();
        }

        public CustomerCompany Find(int ID)
        {
            return db.Set<CustomerCompany>().Single(x => x.ID == ID);
        }


        public CustomerCompany Create(ViewModels.CustomerCompanyViewModel model)
        {
            CustomerCompany entity = new CustomerCompany();
            entity.Name = model.Name;
            entity.BrandName = model.BrandName;
            entity.IndustryID = Utilities.GetCascadingId(model.IndustryCode);
            entity.IndustryValue = model.IndustryCode;
            entity.CustomerCateID = model.CustomerCateID;
            entity.CityID = Utilities.GetCascadingId(model.CityCode);
            entity.CityValue = model.CityCode;
            entity.Address = model.Address;
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.Description = model.Description;
            entity.Fax = model.Fax;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.Phone = model.Phone;

            entity.Finance = model.Finance;
            entity.FinancePhone = model.FinancePhone;
            entity.ProxyName = model.ProxyName;
            entity.ProxyPhone = model.ProxyPhone;
            entity.ProxyAddress = model.ProxyAddress;
            entity.RelationID = model.RelationID;
            db.Add<CustomerCompany>(entity);
            db.Commit();
            return entity;
        }


        public bool IsReplaceable(int ID)
        {
            return Find(ID).IsCommon;
        }


        public void ChangeStatus(string ids, CustomerCompanyStatus status)
        {
            var IdsArray = Utilities.GetIdList(ids);
            db.Set<CustomerCompany>().Where(x => IdsArray.Contains(x.ID)).ToList()
            .ForEach(x =>
            {
                x.Status = (int)status;

            });
            db.Commit();
        }


        public CustomerCompany Update(ViewModels.CustomerCompanyViewModel model)
        {
            CustomerCompany entity = Find(model.ID);
            db.Attach<CustomerCompany>(entity);
            entity.Name = model.Name;
            entity.BrandName = model.BrandName;
            entity.IndustryID = Utilities.GetCascadingId(model.IndustryCode);
            entity.IndustryValue = model.IndustryCode;
            entity.CustomerCateID = model.CustomerCateID;
            entity.CityID = Utilities.GetCascadingId(model.CityCode);
            entity.CityValue = model.CityCode;
            entity.Address = model.Address;
            entity.Description = model.Description;
            entity.Fax = model.Fax;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.Phone = model.Phone;
            entity.RelationID = model.RelationID;
            entity.IsCommon = false;
            entity.Finance = model.Finance;
            entity.FinancePhone = model.FinancePhone;
            entity.ProxyName = model.ProxyName;
            entity.ProxyPhone = model.ProxyPhone;
            entity.ProxyAddress = model.ProxyAddress;
            //替换从这里开始
            if (CookieHelper.MemberID != entity.AddUser)
            {
                entity.AddUser = CookieHelper.MemberID;
                entity.AddTime = DateTime.Now;
            }
            db.Commit();
            return entity;
        }


        public void ChangeRelation(int ID, int RelationID)
        {
            CustomerCompany entity = Find(ID);
            db.Attach<CustomerCompany>(entity);
            entity.RelationID = RelationID;
            db.Commit();
        }


        public List<int> GetMemberCompanyIDs(int MemberID)
        {
            var list = new List<int>();
            var user = MemberService.Find(MemberID);
            if (PermissionsService.CheckPermission("manager", "controller", MemberID))
            {
                var memberIds = MemberService.GetMemberIDs(user.DepartmentID);
                list = GetALL().Where(x => memberIds.Contains(x.AddUser)).Select(x => x.ID).ToList();
            }
            else
            {
                list = GetALL().Where(x => x.AddUser == MemberID).Select(x => x.ID).ToList();
            }

            var sharelist = CustomerShareService.GetMemberShareCompanyIDs(MemberID);
            list.AddRange(sharelist);
            list.Distinct();

            return list;
        }
    }
}