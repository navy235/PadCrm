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
    public class ContactRequireService : IContactRequireService
    {
        private readonly IUnitOfWork db;

        public ContactRequireService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<ContactRequire> GetALL()
        {
            return db.Set<ContactRequire>();
        }

        public IQueryable<ContactRequire> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<ContactRequire>();
        }

        public void Create(ContactRequire model)
        {
            db.Add<ContactRequire>(model);
            db.Commit();
        }

        public void Update(ContactRequire model)
        {
            var target = Find(model.ID);
            db.Attach<ContactRequire>(target);
            target.Description = model.Description;
            target.AttachmentPath = model.AttachmentPath;
            target.IsRoot = model.IsRoot;
            target.Status = model.Status;
            target.Name = model.Name;
            db.Commit();
        }

        public void Delete(ContactRequire model)
        {
            var target = Find(model.ID);
            db.Remove<ContactRequire>(target);
            db.Commit();
        }

        public ContactRequire Find(int ID)
        {
            return db.Set<ContactRequire>().Single(x => x.ID == ID);
        }


        public ContactRequire Create(ViewModels.ContactRequireViewModel model)
        {
            ContactRequire entity = new ContactRequire();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            //entity.AttachmentPath = model.AttachmentPath;
            entity.CompanyID = model.CompanyID;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DepartmentID = model.DepartmentID;
            entity.ResolveID = model.ResolveID;
            entity.SenderID = model.SenderID;
           
            entity.IsRoot = model.IsRoot;
            entity.PID = model.PID;
            entity.Status = model.Status;
            db.Add<ContactRequire>(entity);
            db.Commit();
            return entity;
        }

        public ContactRequire Update(ViewModels.ContactRequireViewModel model)
        {
            ContactRequire entity = Find(model.ID);
            db.Attach<ContactRequire>(entity);
        
            entity.CompanyID = model.CompanyID;
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.DepartmentID = model.DepartmentID;
            entity.ResolveID = model.ResolveID;
            entity.SenderID = model.SenderID;
            entity.IsRoot = model.IsRoot;
            entity.Status = model.Status;
            entity.PID = model.PID;
          
            db.Commit();
            return entity;
        }
    }
}