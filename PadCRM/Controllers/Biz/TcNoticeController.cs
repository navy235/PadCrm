
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
    public class TcNoticeController : Controller
    {
        private IDepartmentService DepartmentService;
        private ITcNoticeService TcNoticeService;
        private IMemberService MemberService;
        private IPermissionsService PermissionsService;
        public TcNoticeController(
            IDepartmentService DepartmentService
            , ITcNoticeService TcNoticeService
            , IMemberService MemberService
            , IPermissionsService PermissionsService
            )
        {
            this.DepartmentService = DepartmentService;
            this.TcNoticeService = TcNoticeService;
            this.MemberService = MemberService;
            this.PermissionsService = PermissionsService;
        }

        public ActionResult Index(int page = 1)
        {
            const int pageSize = 20;
            var query = TcNoticeService.GetALL()
                .Include(x => x.Department)
                .OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            var totalCount = TcNoticeService.GetALL()
                .Count();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return View(query);
        }



        public ActionResult Create()
        {
            ViewBag.Data_DepartmentID = Utilities
                .GetSelectListData(
                DepartmentService.GetALL()
                .Where(x => x.PID.Equals(null)), x => x.ID, x => x.Name, false);
            return View(new TcNoticeViewModel());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TcNoticeViewModel model)
        {
            ServiceResult result = new ServiceResult();
            var DepartmentArray = new List<int>();

            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    DepartmentArray = model.DepartmentID.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    TcNoticeService.Create(model);
                    result.Message = "添加提成公告成功！";
                    LogHelper.WriteLog("添加提成公告成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加提成公告错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
                 DepartmentService.GetALL()
                 .Where(x => x.PID.Equals(null)),
                 x => x.ID, x => x.Name, DepartmentArray, false);

            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var entity = TcNoticeService.GetALL().Include(x => x.Department).Single(x => x.ID == ID);

            var model = new TcNoticeViewModel()
            {
                ID = entity.ID,
                Content = entity.Content,
                AttachmentPath = entity.AttachmentPath,
                Name = entity.Name,
                DepartmentID = String.Join(",", entity.Department.Select(x => x.ID)),
            };

            var DepartmentArray = new List<int>();

            if (!string.IsNullOrEmpty(model.DepartmentID))
            {
                DepartmentArray = model.DepartmentID.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            }
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
              DepartmentService.GetALL()
              .Where(x => x.PID.Equals(null)),
              x => x.ID, x => x.Name, DepartmentArray, false);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TcNoticeViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;

            var DepartmentArray = new List<int>();

            if (ModelState.IsValid)
            {
                try
                {
                    DepartmentArray = model.DepartmentID.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    TcNoticeService.Update(model);
                    result.Message = "编辑提成公告成功！";
                    LogHelper.WriteLog("编辑提成公告成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("编辑提成公告错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }

            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
              DepartmentService.GetALL()
              .Where(x => x.PID.Equals(null)),
              x => x.ID, x => x.Name, DepartmentArray, false);
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
                    var model = TcNoticeService.Find(IdArr[i]);
                    TcNoticeService.Delete(model);
                }
                LogHelper.WriteLog("删除提成公告成功");
                result.Message = "删除提成公告成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除提成公告错误！";
                result.AddServiceError("删除提成公告错误!");
                LogHelper.WriteLog("删除提成公告错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int ID)
        {
            var model = TcNoticeService.GetALL()
                .Single(x => x.ID == ID);
            return PartialView(model);
        }

    }
}
