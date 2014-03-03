
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Maitonn.Core;
using PadCRM.Service.Interface;
using PadCRM.Models;
using PadCRM.ViewModels;
using PadCRM.Utils;

using PadCRM.Filters;

namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class NoticeController : Controller
    {
        private IDepartmentService DepartmentService;
        private INoticeService NoticeService;
        private IMemberService MemberService;
        private IPermissionsService PermissionsService;
        public NoticeController(
            IDepartmentService DepartmentService
            , INoticeService NoticeService
            , IMemberService MemberService
            , IPermissionsService PermissionsService
            )
        {
            this.DepartmentService = DepartmentService;
            this.NoticeService = NoticeService;
            this.MemberService = MemberService;
            this.PermissionsService = PermissionsService;
        }

        public ActionResult Index()
        {
            ViewBag.DepartmentID = Utilities.GetSelectListData(
                DepartmentService.GetALL()
                , x => x.ID, x => x.Name, true);
            return View();
        }

        public ActionResult Data_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = NoticeService.GetKendoALL();
            var hasPermission = CookieHelper.CheckPermission("boss");
            if (!hasPermission)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                model = model.Where(x => x.DepartmentID == member.DepartmentID);
            }
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            var list = DepartmentService.GetALL();
            var hasPermission = CookieHelper.CheckPermission("boss");
            if (!hasPermission)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                list = list.Where(x => x.ID == member.DepartmentID);
            }
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
                list
                , x => x.ID, x => x.Name, true);
            return View(new NoticeViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoticeViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    NoticeService.Create(model);
                    result.Message = "添加部门公告成功！";
                    LogHelper.WriteLog("添加部门公告成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加部门公告错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            var list = DepartmentService.GetALL().Where(x => x.PID.Equals(null));
            var hasPermission = CookieHelper.CheckPermission("boss");
            if (!hasPermission)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                list = list.Where(x => x.ID == member.DepartmentID);
            }
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
                list
                , x => x.ID, x => x.Name, true);

            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var list = DepartmentService.GetALL().Where(x => x.PID.Equals(null));
            var hasPermission = CookieHelper.CheckPermission("boss");
            if (!hasPermission)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                list = list.Where(x => x.ID == member.DepartmentID);
            }

            var entity = NoticeService.Find(ID);
            var model = new NoticeViewModel()
            {
                ID = entity.ID,
                Content = entity.Content,
                DepartmentID = entity.DepartmentID,
                Name = entity.Name
            };
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
              list
              , x => x.ID, x => x.Name, model.DepartmentID, true);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoticeViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    NoticeService.Update(model);
                    result.Message = "编辑部门公告成功！";
                    LogHelper.WriteLog("编辑部门公告成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("编辑部门公告错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            var list = DepartmentService.GetALL().Where(x => x.PID.Equals(null));
            var hasPermission = CookieHelper.CheckPermission("boss");
            if (!hasPermission)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                list = list.Where(x => x.ID == member.DepartmentID);
            }

            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
               list
               , x => x.ID, x => x.Name, model.DepartmentID, true);

            return View(model);
        }

        [HttpPost]
        public ActionResult SetDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            var IdArr = ids.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            try
            {
                for (var i = 0; i < IdArr.Count; i++)
                {
                    var model = NoticeService.Find(IdArr[i]);
                    NoticeService.Delete(model);
                }
                LogHelper.WriteLog("删除部门公告成功");
                result.Message = "删除部门公告成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除部门公告错误！";
                result.AddServiceError("删除部门公告错误!");
                LogHelper.WriteLog("删除部门公告错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int ID)
        {
            var model = NoticeService.GetALL()
                .Include(x => x.Department)
                .Single(x => x.ID == ID);
            return PartialView(model);
        }

    }
}
