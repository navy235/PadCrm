using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class CustomerCateService : ICustomerCateService
    {
        private readonly IUnitOfWork db;

        public CustomerCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<CustomerCate> GetALL()
        {
            return db.Set<CustomerCate>();
        }

        public IQueryable<CustomerCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<CustomerCate>();
        }

        public void Create(CustomerCate model)
        {
            db.Add<CustomerCate>(model);
            db.Commit();
        }

        public void Update(CustomerCate model)
        {
            var target = Find(model.ID);
            db.Attach<CustomerCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(CustomerCate model)
        {
            var target = Find(model.ID);
            db.Remove<CustomerCate>(target);
            db.Commit();
        }

        public CustomerCate Find(int ID)
        {
            return db.Set<CustomerCate>().Single(x => x.ID == ID);
        }
    }
}