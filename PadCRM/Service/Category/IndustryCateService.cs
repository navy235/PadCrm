﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class IndustryCateService : IIndustryCateService
    {
        private readonly IUnitOfWork db;

        public IndustryCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<IndustryCate> GetALL()
        {
            return db.Set<IndustryCate>();
        }

        public IQueryable<IndustryCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<IndustryCate>();
        }

        public void Create(IndustryCate model)
        {
            db.Add<IndustryCate>(model);
            db.Commit();
        }

        public void Update(IndustryCate model)
        {
            var target = Find(model.ID);
            db.Attach<IndustryCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(IndustryCate model)
        {
            var target = Find(model.ID);
            db.Remove<IndustryCate>(target);
            db.Commit();
        }

        public IndustryCate Find(int ID)
        {
            return db.Set<IndustryCate>().Single(x => x.ID == ID);
        }
    }
}