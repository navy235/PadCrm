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
using System.Data.Objects.SqlClient;
using PadCRM.Filters;

namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class PlanLogController : Controller
    {
        private ICustomerCompanyService CustomerCompanyService;
        private IRelationCateService RelationCateService;
        private ICustomerCateService CustomerCateService;
        private ICityCateService CityCateService;
        private IIndustryCateService IndustryCateService;
        private ICustomerService CustomerService;
        private IPlanLogService PlanLogService;
        public PlanLogController(
            ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , ICityCateService CityCateService
            , IIndustryCateService IndustryCateService
            , ICustomerService CustomerService
            , IPlanLogService PlanLogService
            )
        {
            this.CustomerCompanyService = CustomerCompanyService;
            this.RelationCateService = RelationCateService;
            this.CustomerCateService = CustomerCateService;
            this.CityCateService = CityCateService;
            this.IndustryCateService = IndustryCateService;
            this.CustomerService = CustomerService;
            this.PlanLogService = PlanLogService;
        }

        public ActionResult Index(int ID, int page = 1)
        {
            const int pageSize = 20;
            var logs = PlanLogService.GetALL().Where(x => x.CompanyID == ID
               )
                .OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var totalCount = PlanLogService.GetALL()
                .Count(x => x.CompanyID == ID
                  );
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            ViewBag.CompanyID = ID;
            return View(logs);
        }

        public ActionResult My(int page = 1)
        {
            const int pageSize = 1;
            var model = CustomerCompanyService.GetALL().Include(x => x.AddMember)
                .Include(x => x.PlanLog)
                .Where(x => x.AddUser == CookieHelper.MemberID)
                .Select(x => new PlanLogGroupViewModel()
                {
                    ID = x.ID,
                    AddTime = x.AddTime,
                    Name = x.Name,
                    MemberID = x.AddUser,
                    UserName = x.AddMember.NickName,

                }).OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            var totalCount = CustomerCompanyService.GetALL()
                        .Count(x => x.AddUser == CookieHelper.MemberID);

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            foreach (var item in model)
            {
                var planlogs = PlanLogService.GetALL()
                    .Include(x => x.AddMember)
                    .Where(x => x.CompanyID == item.ID)
                    .Select(o => new PlanLogItemViewModel()
                    {
                        CompanyID = o.CompanyID,
                        Content = o.Content,
                        ID = o.ID,
                        PlanTime = o.PlanTime,
                        AddTime = o.AddTime,
                        UserName = o.AddMember.NickName

                    }).OrderByDescending(o => o.AddTime).ToList();

                item.PlanLogs = planlogs;
            }

            return View(model);
        }

        //
        // GET: /Customer/

        #region nomarlform

        public ActionResult Create(int ID)
        {
            var model = new PlanLogViewModel()
            {
                CompanyID = ID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlanLogViewModel model)
        {
            ServiceResult result = new ServiceResult();

            TempData["Service_Result"] = result;
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    PlanLogService.Create(model);
                    result.Message = "添加计划日志成功！";
                    return RedirectToAction("details", "customercompany", new { id = model.CompanyID });
                }
                catch (Exception ex)
                {
                    result.Message = "添加计划日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加计划日志失败!", ex);
                }
            }
            return View(model);
        }
        public ActionResult Edit(int ID)
        {
            var entity = PlanLogService.Find(ID);
            var model = new PlanLogEditViewModel()
            {
                CompanyID = entity.CompanyID,

                ID = entity.ID,
                Content = entity.Content
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlanLogEditViewModel model)
        {
            ServiceResult result = new ServiceResult();

            TempData["Service_Result"] = result;
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    PlanLogService.Update(model);
                    result.Message = "编辑计划日志成功！";
                    return RedirectToAction("details", "customercompany", new { id = model.CompanyID });
                }
                catch (Exception ex)
                {
                    result.Message = "编辑计划日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑计划日志失败!", ex);
                }

            }

            return View(model);
        }

        #endregion

        #region ajaxform


        public ActionResult countvalidate(string date)
        {
            var time = Convert.ToDateTime(date);
            ServiceResult result = new ServiceResult();
            if (PlanLogService.DayCount(time) >= 8)
            {
                result.Message = "当天计划日志已有8条,是否继续添加?";
                result.AddServiceError("当天计划日志已有8条,是否继续添加?");
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxCreate(int ID)
        {
            var model = new PlanLogViewModel()
            {
                CompanyID = ID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(PlanLogViewModel model)
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
                    PlanLogService.Create(model);
                    result.Message = "添加计划日志成功！";
                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加计划日志失败!", ex);
                }
            }
            return Json(result);
        }

        public ActionResult AjaxEdit(int ID)
        {
            var entity = PlanLogService.Find(ID);
            var model = new PlanLogEditViewModel()
            {
                CompanyID = entity.CompanyID,

                ID = entity.ID,
                Content = entity.Content
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxEdit(PlanLogEditViewModel model)
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
                    PlanLogService.Update(model);
                    result.Message = "编辑计划日志成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "编辑计划日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑计划日志失败!", ex);
                }

            }
            return Json(result);
        }
        #endregion




        public ActionResult isEditable(int ID)
        {
            var entity = PlanLogService.Find(ID);
            return Json(entity.AddUser == CookieHelper.MemberID, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var entity = PlanLogService.Find(ID);
                PlanLogService.Delete(entity);
                result.Message = "删除计划日志成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除计划日志失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除计划日志失败!", ex);
            }
            return Json(result);
        }

    }
}
