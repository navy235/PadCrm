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
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork db;

        public TaskService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Task> GetALL()
        {
            return db.Set<Task>();
        }

        public IQueryable<Task> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Task>();
        }

        public void Create(Task model)
        {
            db.Add<Task>(model);
            db.Commit();
        }

        public void Update(Task model)
        {
            var target = Find(model.ID);
            db.Attach<Task>(target);
            target.Start = model.Start;
            target.End = model.End;
            target.EndTimeZone = model.EndTimeZone;
            target.StartTimeZone = model.StartTimeZone;

            target.Title = model.Title;
            target.Description = model.Description;
            db.Commit();
        }

        public void Delete(Task model)
        {
            var target = Find(model.ID);
            db.Remove<Task>(target);
            db.Commit();
        }

        public Task Find(int ID)
        {
            return db.Set<Task>().Single(x => x.ID == ID);
        }


        public Task Create(ViewModels.TaskViewModel model)
        {

            if (ToadyCount() < 5)
            {
                Task entity = new Task();
                entity.Start = model.Start;
                entity.End = model.End;
                entity.EndTimeZone = model.End.ToString("yyyy-MM-dd");
                entity.StartTimeZone = model.Start.ToString("yyyy-MM-dd");

                entity.Title = model.Title;
                entity.Description = model.Description;
                entity.MemberID = model.MemberID;
                entity.AddUser = CookieHelper.MemberID;
                entity.AddTime = DateTime.Now;
                if (model.MemberID != CookieHelper.MemberID)
                {
                    entity.IsOtherAdd = true;
                }
                db.Add<Task>(entity);
                db.Commit();
                return entity;
            }
            else
            {
                throw new Exception("当天任务安排不能超过5条");
            }
        }

        public Task Update(ViewModels.TaskViewModel model)
        {
            Task entity = Find(model.TaskID);
            db.Attach<Task>(entity);
            entity.Start = model.Start;
            entity.End = model.End;
            entity.EndTimeZone = model.End.ToString("yyyy-MM-dd");
            entity.StartTimeZone = model.Start.ToString("yyyy-MM-dd");
            entity.Title = model.Title;
            entity.Description = model.Description;
            if (model.MemberID != CookieHelper.MemberID)
            {
                entity.IsOtherAdd = true;
            }
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