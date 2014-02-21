using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using Maitonn.Core;
using PadCRM.Utils;

namespace PadCRM.Service
{
    public class PlanLogService : IPlanLogService
    {
        private readonly IUnitOfWork db;

        public PlanLogService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<PlanLog> GetALL()
        {
            return db.Set<PlanLog>();
        }

        public IQueryable<PlanLog> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<PlanLog>();
        }

        public void Create(PlanLog model)
        {
            db.Add<PlanLog>(model);
            db.Commit();
        }

        public void Update(PlanLog model)
        {
            var target = Find(model.ID);
            db.Attach<PlanLog>(target);
            target.CompanyID = model.CompanyID;
            target.Content = model.Content;
            target.PlanTime = model.PlanTime;
            db.Commit();
        }

        public void Delete(PlanLog model)
        {
            var target = Find(model.ID);
            db.Remove<PlanLog>(target);
            db.Commit();
        }

        public PlanLog Find(int ID)
        {
            return db.Set<PlanLog>().Single(x => x.ID == ID);
        }


        public PlanLog Create(ViewModels.PlanLogViewModel model)
        {
            PlanLog entity = new PlanLog();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.CompanyID = model.CompanyID;
            entity.Content = model.Content;
            entity.PlanTime = model.PlanTime;
            db.Add<PlanLog>(entity);
            db.Commit();
            return entity;
        }

        public PlanLog Update(ViewModels.PlanLogViewModel model)
        {
            PlanLog entity = Find(model.ID);
            db.Attach<PlanLog>(entity);
            entity.Content = model.Content;
            entity.PlanTime = model.PlanTime;
            db.Commit();
            return entity;
        }


        public PlanLog Update(ViewModels.PlanLogEditViewModel model)
        {
            PlanLog entity = Find(model.ID);
            db.Attach<PlanLog>(entity);
            entity.Content = model.Content;
            entity.PlanTime = model.PlanTime;
            db.Commit();
            return entity;
        }

        public int DayCount(DateTime time)
        {
            var count = GetALL().Count(x => SqlFunctions.DateDiff("day", x.AddTime, time) == 0);
            return count;
        }
    }
}