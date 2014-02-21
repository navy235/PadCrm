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
    public class PunishService : IPunishService
    {
        private readonly IUnitOfWork db;

        public PunishService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<Punish> GetALL()
        {
            return db.Set<Punish>();
        }

        public IQueryable<Punish> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Punish>();
        }

        public void Create(Punish model)
        {
            db.Add<Punish>(model);
            db.Commit();
        }

        public void Update(Punish model)
        {
            var target = Find(model.ID);
            db.Attach<Punish>(target);
            target.Description = model.Description;
            target.MemberID = model.MemberID;
            target.Score = model.Score;
            target.RuleID = model.RuleID;
            target.LastUser = CookieHelper.MemberID;
            target.LastTime = DateTime.Now;
            db.Commit();
        }

        public void Delete(Punish model)
        {
            var target = Find(model.ID);
            db.Remove<Punish>(target);
            db.Commit();
        }

        public Punish Find(int ID)
        {
            return db.Set<Punish>().Single(x => x.ID == ID);
        }


        public Punish Create(ViewModels.PunishViewModel model)
        {
            Punish entity = new Punish();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.Description = model.Description;
            entity.MemberID = model.MemberID;
            entity.RuleID = model.RuleID;
            entity.Score = model.Score;
            db.Add<Punish>(entity);
            db.Commit();
            return entity;
        }

        public Punish Update(ViewModels.PunishViewModel model)
        {
            Punish entity = Find(model.ID);
            db.Attach<Punish>(entity);
            entity.Description = model.Description;
            entity.MemberID = model.MemberID;
            entity.RuleID = model.RuleID;
            entity.Score = model.Score;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            db.Commit();
            return entity;
        }
    }
}