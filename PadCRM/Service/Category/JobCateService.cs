using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class JobCateService : IJobCateService
    {
        private readonly IUnitOfWork db;

        public JobCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<JobCate> GetALL()
        {
            return db.Set<JobCate>();
        }

        public IQueryable<JobCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<JobCate>();
        }

        public void Create(JobCate model)
        {
            db.Add<JobCate>(model);
            db.Commit();
        }

        public void Update(JobCate model)
        {
            var target = Find(model.ID);
            db.Attach<JobCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(JobCate model)
        {
            var target = Find(model.ID);
            db.Remove<JobCate>(target);
            db.Commit();
        }

        public JobCate Find(int ID)
        {
            return db.Set<JobCate>().Single(x => x.ID == ID);
        }
    }
}