
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
    public class CustomerController : Controller
    {
        private ICustomerCompanyService CustomerCompanyService;
        private IRelationCateService RelationCateService;
        private ICustomerCateService CustomerCateService;
        private ICityCateService CityCateService;
        private IIndustryCateService IndustryCateService;
        private ICustomerService CustomerService;
        private IJobCateService JobCateService;
        private IPermissionsService PermissionsService;
        public CustomerController(
            ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , ICityCateService CityCateService
            , IIndustryCateService IndustryCateService
            , ICustomerService CustomerService
            , IJobCateService JobCateService
            , IPermissionsService PermissionsService
            )
        {
            this.CustomerCompanyService = CustomerCompanyService;
            this.RelationCateService = RelationCateService;
            this.CustomerCateService = CustomerCateService;
            this.CityCateService = CityCateService;
            this.IndustryCateService = IndustryCateService;
            this.CustomerService = CustomerService;
            this.JobCateService = JobCateService;
            this.PermissionsService = PermissionsService;
        }

        public ActionResult Index(int ID, int page = 1)
        {
            const int pageSize = 20;
            var customers = CustomerService
                .GetALL()
                .Include(x => x.JobCate)
                .Include(x => x.AddMember)
                .Where(x => x.CompanyID == ID)
                .OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            var totalCount = CustomerService.GetALL()
                .Count(x => x.CompanyID == ID);

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            ViewBag.CompanyID = ID;

            return View(customers);
        }



        //
        // GET: /Customer/

        #region normalform

        public ActionResult Create(int ID)
        {
            var model = new CustomerViewModel()
            {
                CompanyID = ID
            };
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerViewModel model)
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
                    CustomerService.Create(model);

                    result.Message = "添加客户人员信息成功！";
                    return RedirectToAction("details", "customercompany", new { id = model.CompanyID });
                }
                catch (Exception ex)
                {
                    result.Message = "添加客户人员信息失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加客户人员信息失败!", ex);
                }

            }
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var entity = CustomerService.Find(ID);
            var model = new CustomerViewModel()
            {
                CompanyID = entity.CompanyID,
                Address = entity.Address,
                BirthDay = entity.BirthDay,
                Email = entity.Email,
                Favorite = entity.Favorite,
                ID = entity.ID,
                IsLeap = entity.IsLeap,
                JobID = entity.JobID,
                Jobs = entity.Jobs,
                Mobile = entity.Mobile,
                Name = entity.Name,
                Phone = entity.Phone,
                QQ = entity.QQ,
                ReMark = entity.ReMark
            };
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, model.JobID, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerViewModel model)
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
                    CustomerService.Update(model);
                    result.Message = "编辑客户人员信息成功！";
                    return RedirectToAction("details", "customercompany", new { id = model.CompanyID });
                }
                catch (Exception ex)
                {
                    result.Message = "编辑客户人员信息失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑客户人员信息失败!", ex);
                }

            }
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, model.JobID, true);
            return View(model);
        }

        #endregion

        #region ajaxform
        public ActionResult AjaxCreate(int ID)
        {
            var model = new CustomerViewModel()
            {
                CompanyID = ID
            };
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(CustomerViewModel model)
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
                    CustomerService.Create(model);
                    result.Message = "添加客户人员信息成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "添加客户人员信息失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加客户人员信息失败!", ex);
                }
            }
            return Json(result);
        }

        public ActionResult AjaxEdit(int ID)
        {
            var entity = CustomerService.Find(ID);
            var model = new CustomerViewModel()
            {
                CompanyID = entity.CompanyID,
                Address = entity.Address,
                BirthDay = entity.BirthDay,
                Email = entity.Email,
                Favorite = entity.Favorite,
                ID = entity.ID,
                IsLeap = entity.IsLeap,
                JobID = entity.JobID,
                Jobs = entity.Jobs,
                Mobile = entity.Mobile,
                Name = entity.Name,
                Phone = entity.Phone,
                QQ = entity.QQ,
                ReMark = entity.ReMark
            };
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, model.JobID, true);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxEdit(CustomerViewModel model)
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
                    CustomerService.Update(model);
                    result.Message = "编辑客户人员信息成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "编辑客户人员信息失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑客户人员信息失败!", ex);
                }
            }
            ViewBag.Data_JobID = Utilities.GetSelectListData(JobCateService.GetALL(), x => x.ID, x => x.CateName, model.JobID, true);
            return Json(result);
        }

        #endregion

        public ActionResult isEditable(int ID)
        {
            var entity = CustomerService.Find(ID);
            var hasPermission = (entity.AddUser == CookieHelper.MemberID && (DateTime.Now - entity.AddTime).Days < 15)
               || PermissionsService.CheckPermission("boss", "controller", CookieHelper.MemberID);
            return Json(hasPermission, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var entity = CustomerService.Find(ID);
                CustomerService.Delete(entity);
                result.Message = "删除人员信息成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除人员信息失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除人员信息失败!", ex);
            }
            return Json(result);
        }

    }
}

