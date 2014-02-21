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
    public class FileShareService : IFileShareService
    {
        private readonly IUnitOfWork db;

        public FileShareService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<FileShare> GetALL()
        {
            return db.Set<FileShare>();
        }

        public IQueryable<FileShare> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<FileShare>();
        }

        public void Create(FileShare model)
        {
            db.Add<FileShare>(model);
            db.Commit();
        }

        public void Update(FileShare model)
        {
            var target = Find(model.ID);
            db.Attach<FileShare>(target);
            target.Description = model.Description;
            target.FilePath = model.FilePath;
            target.FileCateID = model.FileCateID;
            target.Name = model.Name;
            db.Commit();
        }

        public void Delete(FileShare model)
        {
            var target = Find(model.ID);
            db.Remove<FileShare>(target);
            db.Commit();
        }

        public FileShare Find(int ID)
        {
            return db.Set<FileShare>().Single(x => x.ID == ID);
        }


        public FileShare Create(ViewModels.FileShareViewModel model)
        {
            FileShare entity = new FileShare();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.FilePath = model.FilePath;
            entity.FileCateID = model.FileCateID;
            entity.Name = model.Name;
            entity.Description = model.Description;
            db.Add<FileShare>(entity);
            db.Commit();
            return entity;
        }

        public FileShare Update(ViewModels.FileShareViewModel model)
        {
            FileShare entity = Find(model.ID);
            db.Attach<FileShare>(entity);
            entity.Description = model.Description;
            entity.FilePath = model.FilePath;
            entity.FileCateID = model.FileCateID;
            entity.Name = model.Name;
            entity.Description = model.Description;
            db.Commit();
            return entity;
        }
    }
}