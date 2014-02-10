using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class RelationCateService : IRelationCateService
    {
        private readonly IUnitOfWork db;

        public RelationCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<RelationCate> GetALL()
        {
            return db.Set<RelationCate>();
        }

        public IQueryable<RelationCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<RelationCate>();
        }

        public void Create(RelationCate model)
        {
            db.Add<RelationCate>(model);
            db.Commit();
        }

        public void Update(RelationCate model)
        {
            var target = Find(model.ID);
            db.Attach<RelationCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(RelationCate model)
        {
            var target = Find(model.ID);
            db.Remove<RelationCate>(target);
            db.Commit();
        }

        public RelationCate Find(int ID)
        {
            return db.Set<RelationCate>().Single(x => x.ID == ID);
        }
    }
}