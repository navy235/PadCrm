using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using Maitonn.Core;

namespace PadCRM.Service
{
    public class TraceLogService : ITraceLogService
    {
        private readonly IUnitOfWork db;

        public TraceLogService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<TraceLog> GetALL()
        {
            return db.Set<TraceLog>();
        }

        public IQueryable<TraceLog> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<TraceLog>();
        }

        public void Create(TraceLog model)
        {
            db.Add<TraceLog>(model);
            db.Commit();
        }

        public void Update(TraceLog model)
        {
            var target = Find(model.ID);
            db.Attach<TraceLog>(target);
            target.CompanyID = model.CompanyID;
            target.Content = model.Content;
            db.Commit();
        }

        public void Delete(TraceLog model)
        {
            var target = Find(model.ID);
            db.Remove<TraceLog>(target);
            db.Commit();
        }

        public TraceLog Find(int ID)
        {
            return db.Set<TraceLog>().Single(x => x.ID == ID);
        }


        public TraceLog Create(ViewModels.TraceLogViewModel model)
        {
            TraceLog entity = new TraceLog();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.CompanyID = model.CompanyID;
            entity.Content = model.Content;
            entity.RelationID = model.RelationID;
            db.Add<TraceLog>(entity);
            db.Commit();
            return entity;
        }

        public TraceLog Update(ViewModels.TraceLogViewModel model)
        {
            TraceLog entity = Find(model.ID);
            db.Attach<TraceLog>(entity);
            entity.Content = model.Content;
            entity.RelationID = model.RelationID;
            db.Commit();
            return entity;
        }

        private int ToadyCount()
        {
            var count = GetALL().Count(x => SqlFunctions.DateDiff("day", x.AddTime, DateTime.Now) == 0);
            return count;
        }
    }
}