using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
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
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;

using PadCRM.Filters;

namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class FileShareController : Controller
    {
        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IJobTitleCateService JobTitleCateService;
        private IRuleCateService RuleCateService;
        private IFileShareService FileShareService;
        private IFileCateService FileCateService;
        private IPermissionsService PermissionsService;
        public FileShareController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IJobTitleCateService JobTitleCateService
            , IRuleCateService RuleCateService
            , IFileShareService FileShareService
            , IFileCateService FileCateService
            , IPermissionsService PermissionsService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.JobTitleCateService = JobTitleCateService;
            this.RuleCateService = RuleCateService;
            this.FileShareService = FileShareService;
            this.FileCateService = FileCateService;
            this.PermissionsService = PermissionsService;
        }

        public ActionResult Index()
        {
            ViewBag.FileCateID = Utilities.GetSelectListData(
              FileCateService.GetALL().Where(x => x.PID.Equals(null))
              , x => x.ID, x => x.CateName, true);

            ViewBag.DepartmentID = Utilities.GetSelectListData(
             DepartmentService.GetALL().Where(x => x.PID.Equals(null))
             , x => x.ID, x => x.Name, true);

            return View();
        }
        public ActionResult Data_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = FileShareService.GetKendoALL();
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.Data_FileCateID = Utilities.GetSelectListData(
              FileCateService.GetALL().Where(x => x.PID.Equals(null))
              , x => x.ID, x => x.CateName, true);

            var departlist = DepartmentService.GetALL().Where(x => x.PID.Equals(null));

            var hasPermission = PermissionsService.CheckPermission("boss", "controller", CookieHelper.MemberID);
            if (!hasPermission)
            {
                var member = MemberService.Find(CookieHelper.MemberID);
                var depart = DepartmentService.Find(member.DepartmentID);
                if (depart.Level == 0)
                {
                    departlist = departlist.Where(x => x.ID == depart.ID);
                }
                else
                {
                    var rootCode = Utilities.GetRootCode(depart.Code, depart.Level);
                    departlist = departlist.Where(x => x.Code == rootCode);
                }
            }
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
             departlist
           , x => x.ID, x => x.Name, true);
            return View(new FileShareViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FileShareViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    FileShareService.Create(model);
                    result.Message = "添加文件共享成功！";
                    LogHelper.WriteLog("添加文件共享告成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加文件共享错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }

            ViewBag.Data_RuleID = Utilities.GetSelectListData(
                RuleCateService.GetALL().Where(x => x.PID.Equals(null))
                , x => x.ID, x => x.CateName, true);

            return View(model);
        }

        public ActionResult Edit(int ID)
        {

            var entity = FileShareService.GetALL().Include(x => x.Member).Single(x => x.ID == ID);
            var model = new FileShareViewModel()
            {
                ID = entity.ID,
                Description = entity.Description,
                Name = entity.Name,
                FileCateID = entity.FileCateID,
                FilePath = entity.FilePath
            };
            ViewBag.Data_FileCateID = Utilities.GetSelectListData(
              FileCateService.GetALL().Where(x => x.PID.Equals(null))
              , x => x.ID, x => x.CateName, model.FileCateID, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FileShareViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {

                    FileShareService.Update(model);
                    result.Message = "编辑文件共享成功！";
                    LogHelper.WriteLog("编辑文件共享成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("编辑文件共享错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            ViewBag.Data_FileCateID = Utilities.GetSelectListData(
                FileCateService.GetALL().Where(x => x.PID.Equals(null))
                , x => x.ID, x => x.CateName, model.FileCateID, true);
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
                    var model = FileShareService.Find(IdArr[i]);
                    FileShareService.Delete(model);
                }
                LogHelper.WriteLog("删除文件共享成功");
                result.Message = "删除文件共享成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除文件共享错误！";
                result.AddServiceError("删除文件共享错误!");
                LogHelper.WriteLog("删除文件共享错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult My()
        {
            var fileCate = FileCateService.GetALL().ToList();
            return View(fileCate);
        }


        public ActionResult process(int page = 1)
        {
            const int pageSize = 20;
            var member = MemberService.Find(CookieHelper.MemberID);
            var rootDepart = DepartmentService.GetRoot(member.DepartmentID);
            var list = FileShareService.GetALL()
                .Where(x => (x.DepartmentID == rootDepart.ID || x.DepartmentID == 0) && x.FileCateID == 1)
                .OrderBy(x => x.ID)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();

            var totalCount = FileShareService.GetALL()
                .Count(x => (x.DepartmentID == rootDepart.ID || x.DepartmentID == 0) && x.FileCateID == 1);
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return View(list);

        }

        public ActionResult responsibilities(int page = 1)
        {
            const int pageSize = 20;
            var member = MemberService.Find(CookieHelper.MemberID);
            var rootDepart = DepartmentService.GetRoot(member.DepartmentID);
            var list = FileShareService.GetALL()
                .Where(x => (x.DepartmentID == rootDepart.ID || x.DepartmentID == 0) && x.FileCateID == 2)
                .OrderBy(x => x.ID)
                .Skip((page - 1) * pageSize).Take(pageSize)
                .ToList();

            var totalCount = FileShareService.GetALL()
                .Count(x => (x.DepartmentID == rootDepart.ID || x.DepartmentID == 0) && x.FileCateID == 2);
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return View(list);
        }

        public ActionResult List(int ID, int page = 1)
        {
            const int pageSize = 20;
            var logs = FileShareService.GetALL()
                .Include(x => x.FileCate)
                .Where(x => x.FileCateID == ID)
                .OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var totalCount = FileShareService.GetALL()
                .Where(x => x.FileCateID == ID).Count();
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            ViewBag.CategoryID = ID.ToString();
            return View(logs);
        }

    }
}
