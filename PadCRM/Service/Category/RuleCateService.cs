using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class RuleCateService : IRuleCateService
    {
        private readonly IUnitOfWork db;

        public RuleCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<RuleCate> GetALL()
        {
            return db.Set<RuleCate>();
        }

        public IQueryable<RuleCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<RuleCate>();
        }

        public void Create(RuleCate model)
        {
            db.Add<RuleCate>(model);
            db.Commit();
        }

        public void Update(RuleCate model)
        {
            var target = Find(model.ID);
            db.Attach<RuleCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(RuleCate model)
        {
            var target = Find(model.ID);
            db.Remove<RuleCate>(target);
            db.Commit();
        }

        public RuleCate Find(int ID)
        {
            return db.Set<RuleCate>().Single(x => x.ID == ID);
        }
    }
}