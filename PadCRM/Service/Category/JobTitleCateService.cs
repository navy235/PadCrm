using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class JobTitleCateService : IJobTitleCateService
    {
        private readonly IUnitOfWork db;

        public JobTitleCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<JobTitleCate> GetALL()
        {
            return db.Set<JobTitleCate>();
        }

        public IQueryable<JobTitleCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<JobTitleCate>();
        }

        public void Create(JobTitleCate model)
        {
            db.Add<JobTitleCate>(model);
            db.Commit();
        }

        public void Update(JobTitleCate model)
        {
            var target = Find(model.ID);
            db.Attach<JobTitleCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(JobTitleCate model)
        {
            var target = Find(model.ID);
            db.Remove<JobTitleCate>(target);
            db.Commit();
        }

        public JobTitleCate Find(int ID)
        {
            return db.Set<JobTitleCate>().Single(x => x.ID == ID);
        }
    }
}