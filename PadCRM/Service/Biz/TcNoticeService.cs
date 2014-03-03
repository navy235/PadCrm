using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
using PadCRM.Utils;
using Kendo.Mvc.Extensions;
namespace PadCRM.Service
{
    public class TcNoticeService : ITcNoticeService
    {
        private readonly IUnitOfWork db;

        private readonly IDepartmentService DepartmentService;

        public TcNoticeService(IUnitOfWork db

            , IDepartmentService DepartmentService)
        {
            this.db = db;
            this.DepartmentService = DepartmentService;
        }

        public IQueryable<TcNotice> GetALL()
        {
            return db.Set<TcNotice>();
        }

        public IQueryable<TcNotice> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<TcNotice>();
        }

        public void Create(TcNotice model)
        {
            db.Add<TcNotice>(model);
            db.Commit();
        }

        public void Update(TcNotice model)
        {
            var target = Find(model.ID);
            db.Attach<TcNotice>(target);
            target.Name = model.Name;
            target.Content = model.Content;
            target.AttachmentPath = model.AttachmentPath;
            db.Commit();
        }

        public void Delete(TcNotice model)
        {
            var target = Find(model.ID);
            db.Remove<TcNotice>(target);
            db.Commit();
        }

        public TcNotice Find(int ID)
        {
            return db.Set<TcNotice>().Single(x => x.ID == ID);
        }


        public TcNotice Create(ViewModels.TcNoticeViewModel model)
        {
            TcNotice entity = new TcNotice();
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.AttachmentPath = model.AttachmentPath;
            entity.Content = model.Content;
            entity.Name = model.Name;

            if (!string.IsNullOrEmpty(model.DepartmentID))
            {
                var DepartmentArray = model.DepartmentID.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var DepartmentList = DepartmentService.GetALL().Where(x => DepartmentArray.Contains(x.ID));
                entity.Department.AddRange(DepartmentList);
            }
            db.Add<TcNotice>(entity);
            db.Commit();
            return entity;
        }

        public TcNotice Update(ViewModels.TcNoticeViewModel model)
        {
            TcNotice entity = Find(model.ID);
            db.Attach<TcNotice>(entity);
            entity.Content = model.Content;
            entity.Name = model.Name;
            entity.AttachmentPath = model.AttachmentPath;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;

            var DepartmentArray = new List<int>();
            if (string.IsNullOrEmpty(model.DepartmentID))
            {
                entity.Department = new List<Department>();
            }
            else
            {
                DepartmentArray = model.DepartmentID.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var DepartmentList = DepartmentService.GetALL().Where(x => DepartmentArray.Contains(x.ID));
                var currentDepartmentArray = entity.Department.Select(x => x.ID).ToList();
                foreach (Department ac in DepartmentService.GetALL())
                {
                    if (DepartmentArray.Contains(ac.ID))
                    {
                        if (!currentDepartmentArray.Contains(ac.ID))
                        {
                            entity.Department.Add(ac);
                        }
                    }
                    else
                    {
                        if (currentDepartmentArray.Contains(ac.ID))
                        {
                            entity.Department.Remove(ac);
                        }
                    }
                }
            }
            db.Commit();
            return entity;
        }
    }
}