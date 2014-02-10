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
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;
using PadCRM.Filters;

namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class TaskController : Controller
    {
        //
        // GET: /Task/

        private ITaskService TaskService;
        private IMemberService MemberService;
        private IPermissionsService PermissionsService;
        public TaskController(
            ITaskService TaskService,
            IMemberService MemberService,
            IPermissionsService PermissionsService
            )
        {
            this.TaskService = TaskService;
            this.MemberService = MemberService;
            this.PermissionsService = PermissionsService;
        }

        public bool CheckPermission(int ID)
        {
            var hasPermission = ID == CookieHelper.MemberID
            || PermissionsService.CheckPermission("boss", "controller", CookieHelper.MemberID);
            return hasPermission;
        }

        public ActionResult Index(int ID = 0, int Month = 0)
        {
            if (ID == 0)
            {
                ID = CookieHelper.MemberID;
            }
            if (!CheckPermission(ID))
            {
                return Content(Utilities.NoPermissionStr);
            }
            var month = new MonthTableViewModel();
            if (Month == 0)
            {
                Month = DateTime.Now.Month;
            }
            var time = new DateTime(DateTime.Now.Year, Month, 1);
            month.Year = time.Year;
            month.Month = time.Month;
            month.MemberID = ID;
            month.FirstRowIndex = (int)time.DayOfWeek;
            month.DayCount = Utilities.GetMonthDayCount(DateTime.Now.Year, Month);
            month.MaxRows = (int)Math.Ceiling(((double)month.DayCount + month.FirstRowIndex) / 7);
            if (month.MemberID == CookieHelper.MemberID)
            {
                ViewBag.NickName = CookieHelper.NickName;
            }
            else
            {
                ViewBag.NickName = MemberService.Find(month.MemberID).NickName;
            }

            return View(month);
        }


        public ActionResult AjaxCreate(int ID, string date)
        {

            var time = Convert.ToDateTime(date);
            var model = new TaskViewModel()
            {
                Start = time.AddHours(8),
                End = time.AddHours(18),
                MemberID = ID
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(TaskViewModel model)
        {

            ServiceResult result = new ServiceResult();
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    var entity = TaskService.Create(model);
                    result.SuccessData = new Task()
                    {
                        Start = entity.Start,
                        End = entity.End,
                        ID = entity.ID,
                        StartTimeZone = entity.StartTimeZone,
                        EndTimeZone = entity.EndTimeZone,
                        Description = entity.Description,
                        Title = entity.Title,
                        MemberID = entity.MemberID,
                        IsOtherAdd = entity.MemberID != CookieHelper.MemberID
                    };
                    result.Message = "添加任务安排成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "添加任务安排失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加任务安排失败!", ex);
                }
            }
            return Json(result);
        }

        public ActionResult AjaxEdit(int ID)
        {
            var entity = TaskService.Find(ID);
            var model = new TaskViewModel()
            {
                Start = entity.Start,
                End = entity.End,
                Title = entity.Title,
                Description = entity.Description,
                TaskID = entity.ID,
                MemberID = entity.MemberID
            };
            return PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxEdit(TaskViewModel model)
        {
            ServiceResult result = new ServiceResult();
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    var entity = TaskService.Update(model);
                    result.SuccessData = new Task()
                    {
                        Start = entity.Start,
                        End = entity.End,
                        ID = entity.ID,
                        StartTimeZone = entity.StartTimeZone,
                        EndTimeZone = entity.EndTimeZone,
                        Description = entity.Description,
                        Title = entity.Title,
                        MemberID = entity.MemberID,
                        IsOtherAdd = entity.MemberID != CookieHelper.MemberID
                    };
                    result.Message = "编辑任务安排成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "编辑任务安排失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑任务安排失败!", ex);
                }
            }
            return Json(result);
        }


        public ActionResult bind(int id, int year, int month)
        {
            var startTime = new DateTime(year, month, 1);
            var daycount = Utilities.GetMonthDayCount(year, month);
            var endTime = new DateTime(year, month, daycount);
            var model = TaskService.GetKendoALL().Where(x => x.MemberID == id
                && x.Start > startTime && x.End < endTime
                ).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var model = TaskService.Find(ID);
                TaskService.Delete(model);
                LogHelper.WriteLog("删除任务安排成功");
                result.Message = "删除任务安排成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除任务安排错误！";
                result.AddServiceError("删除任务安排错误!");
                LogHelper.WriteLog("删除任务安排错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
