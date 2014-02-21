using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
using PadCRM.Utils;


namespace PadCRM.Service
{
    public class NoticeService : INoticeService
    {
        private readonly IUnitOfWork db;

        public NoticeService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Notice> GetALL()
        {
            return db.Set<Notice>();
        }

        public IQueryable<Notice> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Notice>();
        }

        public void Create(Notice model)
        {
            db.Add<Notice>(model);
            db.Commit();
        }

        public void Update(Notice model)
        {
            var target = Find(model.ID);
            db.Attach<Notice>(target);
            target.Name = model.Name;
            target.Content = model.Content;
            target.DepartmentID = model.DepartmentID;
            db.Commit();
        }

        public void Delete(Notice model)
        {
            var target = Find(model.ID);
            db.Remove<Notice>(target);
            db.Commit();
        }

        public Notice Find(int ID)
        {
            return db.Set<Notice>().Single(x => x.ID == ID);
        }


        public Notice Create(ViewModels.NoticeViewModel model)
        {
            Notice entity = new Notice();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.DepartmentID = model.DepartmentID;
            entity.Content = model.Content;
            entity.Name = model.Name;
            db.Add<Notice>(entity);
            db.Commit();
            return entity;
        }

        public Notice Update(ViewModels.NoticeViewModel model)
        {
            Notice entity = Find(model.ID);
            db.Attach<Notice>(entity);
            entity.Content = model.Content;
            entity.Name = model.Name;
            entity.DepartmentID = model.DepartmentID;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            db.Commit();
            return entity;
        }
    }
}