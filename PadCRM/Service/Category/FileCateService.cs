using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class FileCateService : IFileCateService
    {
        private readonly IUnitOfWork db;

        public FileCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<FileCate> GetALL()
        {
            return db.Set<FileCate>();
        }

        public IQueryable<FileCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<FileCate>();
        }

        public void Create(FileCate model)
        {
            db.Add<FileCate>(model);
            db.Commit();
        }

        public void Update(FileCate model)
        {
            var target = Find(model.ID);
            db.Attach<FileCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(FileCate model)
        {
            var target = Find(model.ID);
            db.Remove<FileCate>(target);
            db.Commit();
        }

        public FileCate Find(int ID)
        {
            return db.Set<FileCate>().Single(x => x.ID == ID);
        }
    }
}