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
    public class MediaRequireService : IMediaRequireService
    {
        private readonly IUnitOfWork db;

        public MediaRequireService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<MediaRequire> GetALL()
        {
            return db.Set<MediaRequire>();
        }

        public IQueryable<MediaRequire> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<MediaRequire>();
        }

        public void Create(MediaRequire model)
        {
            db.Add<MediaRequire>(model);
            db.Commit();
        }

        public void Update(MediaRequire model)
        {
            var target = Find(model.ID);
            db.Attach<MediaRequire>(target);
            target.Description = model.Description;
            target.AttachmentPath = model.AttachmentPath;
            target.Name = model.Name;
            target.Status = model.Status;
            db.Commit();
        }

        public void Delete(MediaRequire model)
        {
            var target = Find(model.ID);
            db.Remove<MediaRequire>(target);
            db.Commit();
        }

        public MediaRequire Find(int ID)
        {
            return db.Set<MediaRequire>().Single(x => x.ID == ID);
        }


        public MediaRequire Create(ViewModels.MediaRequireViewModel model)
        {
            MediaRequire entity = new MediaRequire();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.AttachmentPath = model.AttachmentPath;
            entity.CompanyID = model.CompanyID;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DepartmentID = model.DepartmentID;
            entity.ResolveID = model.ResolveID;
            entity.SenderID = model.SenderID;
            entity.AttachmentPath = model.AttachmentPath;
            entity.IsRoot = model.IsRoot;
            entity.PID = model.PID;
            entity.Status = model.Status;
            db.Add<MediaRequire>(entity);
            db.Commit();
            return entity;
        }

        public MediaRequire Update(ViewModels.MediaRequireViewModel model)
        {
            MediaRequire entity = Find(model.ID);
            db.Attach<MediaRequire>(entity);
            entity.AttachmentPath = model.AttachmentPath;
            entity.CompanyID = model.CompanyID;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DepartmentID = model.DepartmentID;
            entity.ResolveID = model.ResolveID;
            entity.SenderID = model.SenderID;
            entity.AttachmentPath = model.AttachmentPath;
            entity.IsRoot = model.IsRoot;
            entity.PID = model.PID;
            entity.Status = model.Status;
            db.Commit();
            return entity;
        }
    }
}