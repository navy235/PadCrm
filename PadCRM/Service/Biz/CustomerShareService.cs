using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using System.Data.Entity;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class CustomerShareService : ICustomerShareService
    {
        private readonly IUnitOfWork db;

        public CustomerShareService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CustomerShare> GetALL()
        {
            return db.Set<CustomerShare>();
        }

        public IQueryable<CustomerShare> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CustomerShare>();
        }

        public void Create(CustomerShare model)
        {
            db.Add<CustomerShare>(model);
            db.Commit();
        }

        public void Update(CustomerShare model)
        {
            var target = Find(model.ID);
            db.Attach<CustomerShare>(target);
            target.CompanyID = model.CompanyID;
            target.MemberID = model.MemberID;
            db.Commit();
        }

        public void Delete(CustomerShare model)
        {
            var target = Find(model.ID);
            db.Remove<CustomerShare>(target);
            db.Commit();
        }

        public CustomerShare Find(int ID)
        {
            return db.Set<CustomerShare>().Single(x => x.ID == ID);
        }


        public CustomerShare Create(ViewModels.CustomerShareViewModel model)
        {
            CustomerShare entity = new CustomerShare();
            entity.CompanyID = model.CompanyID;
            entity.MemberID = model.MemberID;
            entity.AddUser = model.AddUser;
            entity.AddTime = DateTime.Now;
            db.Add<CustomerShare>(entity);
            db.Commit();
            return entity;
        }


        public CustomerShare Update(ViewModels.CustomerShareViewModel model)
        {
            CustomerShare entity = Find(model.ID);
            db.Attach<CustomerShare>(entity);
            entity.CompanyID = model.CompanyID;
            entity.MemberID = model.MemberID;
            db.Commit();
            return entity;
        }


        public List<int> GetMemberShareCompanyIDs(int MemberID)
        {
            List<int> result = new List<int>();
            result = GetALL().Where(x => x.MemberID == MemberID)
                .Select(x => x.CompanyID).Distinct().ToList();
            return result;
        }
    }
}