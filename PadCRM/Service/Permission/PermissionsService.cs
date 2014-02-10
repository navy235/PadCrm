using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class PermissionsService : IPermissionsService
    {
        private readonly IUnitOfWork db;
        private readonly IMemberService MemberService;

        public PermissionsService(IUnitOfWork db
            , IMemberService MemberService

            )
        {
            this.db = db;
            this.MemberService = MemberService;

        }

        public IQueryable<Permissions> GetALL()
        {
            return db.Set<Permissions>();
        }

        public IQueryable<Permissions> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<Permissions>();
        }

        public void Create(Permissions model)
        {
            db.Add<Permissions>(model);
            db.Commit();
        }

        public void Update(Permissions model)
        {
            var target = Find(model.ID);
            db.Attach<Permissions>(target);
            target.Name = model.Name;
            target.Action = model.Action;
            target.Namespace = model.Namespace;
            target.Controller = model.Controller;
            target.Description = model.Description;
            target.DepartmentID = model.DepartmentID;
            db.Commit();
        }

        public void Delete(Permissions model)
        {
            var target = Find(model.ID);
            db.Remove<Permissions>(target);
            db.Commit();
        }

        public Permissions Find(int ID)
        {
            return db.Set<Permissions>().Single(x => x.ID == ID);
        }


        public bool CheckPermission(string controller, string action, int MemberID)
        {
            int groupID = MemberService.Find(MemberID).GroupID;
            var hasPermission = false;
            var query = db.Set<Group>()
                .Include(x => x.Roles)
                .Where(g =>
                    (g.Roles.Any(r =>
                        r.Permissions.Count(p =>
                            p.Controller.Equals(controller, StringComparison.OrdinalIgnoreCase)
                            &&
                            (p.Action.Equals(action, StringComparison.OrdinalIgnoreCase) || p.Action.Equals("controller", StringComparison.OrdinalIgnoreCase))) > 0))
                    && g.ID == groupID);
            hasPermission = query.Any();
            return hasPermission;
        }
    }
}