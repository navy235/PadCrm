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
    public class ContractInfoController : Controller
    {
        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IContactRequireService ContactRequireService;
        private IContractInfoService ContractInfoService;
        private IContractCateService ContractCateService;
        public ContractInfoController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IContactRequireService ContactRequireService
            , IContractInfoService ContractInfoService
            , IContractCateService ContractCateService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.ContactRequireService = ContactRequireService;
            this.ContractInfoService = ContractInfoService;
            this.ContractCateService = ContractCateService;
        }

        public ActionResult Index()
        {
            var model = new ContractInfoSearchViewModel();
            ViewBag.Data_ContractCateID = Utilities.GetSelectListData(ContractCateService.GetALL(),
             x => x.ID,
             x => x.CateName,
             true, true);
            return View(model);
        }

        public ActionResult Receivables()
        {
            var model = new ContractInfoSearchViewModel();
            ViewBag.Data_ContractCateID = Utilities.GetSelectListData(ContractCateService.GetALL(),
          x => x.ID,
          x => x.CateName,
          true, true);
            return View(model);
        }

        public ActionResult Details(int ID)
        {
            var entity = ContractInfoService.GetALL()
                .Include(x => x.Signer)
                .Include(x => x.ContractCate)
                .Single(x => x.ID == ID);


            return View(entity);
        }

        public ActionResult Log(int CompanyID, int page = 1)
        {
            const int pageSize = 20;
            var logs = ContactRequireService.GetALL()
                .Where(x => x.CompanyID == CompanyID)
                .OrderByDescending(x => x.AddTime)
                 .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            var totalCount = ContactRequireService.GetALL()
           .Count(x => x.CompanyID == CompanyID);

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            return PartialView(logs);
        }


        public ActionResult Wait(int page = 1)
        {
            if (!CookieHelper.CheckPermission("caiwu"))
            {
                return RedirectToAction("index");
            }

            const int pageSize = 20;

            var member = MemberService.Find(CookieHelper.MemberID);

            var contacts = ContactRequireService.GetALL()
                .Where(x => x.Status == 0
                && x.IsRoot == 1).OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            var totalCount = ContactRequireService.GetALL()
                .Count(x => x.Status == 0
                    && x.IsRoot == 1);

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return View(contacts);
        }


        public ActionResult Search(ContractInfoSearchViewModel model, int page = 1)
        {
            const int pageSize = 20;

            var query = ContractInfoService.GetALL()
                .Include(x => x.Signer)
                .Include(x => x.ContractCate);

            if (model.ContractCateID != 0)
            {
                query = query.Where(x => x.ContractCateID == model.ContractCateID);
            }

            if (!string.IsNullOrEmpty(model.Key))
            {
                query = query.Where(x => x.Key.Contains(model.Key));
            }

            //if (CookieHelper.CheckPermission("boss"))
            //{

            //}
            //else if (CookieHelper.CheckPermission("manager"))
            //{
            //    var memberIds = MemberService.GetMemberIDs(CookieHelper.GetDepartmentID());
            //    query = query.Where(x => memberIds.Contains(x.SenderID));
            //}

            query = query.Where(x => x.AddTime < model.EndTime
             && x.AddTime > model.StartTime).OrderByDescending(x => x.AddTime);

            var totalCount = query.Count();

            var data = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            return PartialView(data);
        }

        public ActionResult ReceivablesSearch(ContractInfoSearchViewModel model, int page = 1)
        {
            const int pageSize = 20;

            var query = ContractInfoService.GetALL()
                .Include(x => x.Signer)
                .Include(x => x.ContractCate);

            if (model.ContractCateID != 0)
            {
                query = query.Where(x => x.ContractCateID == model.ContractCateID);
            }

            if (!string.IsNullOrEmpty(model.Key))
            {
                query = query.Where(x => x.Key.Contains(model.Key));
            }

            //if (CookieHelper.CheckPermission("boss"))
            //{

            //}
            //else if (CookieHelper.CheckPermission("manager"))
            //{
            //    var memberIds = MemberService.GetMemberIDs(CookieHelper.GetDepartmentID());
            //    query = query.Where(x => memberIds.Contains(x.SenderID));
            //}

            query = query.Where(x => x.NextTime < model.EndTime
             && x.NextTime > model.StartTime).OrderByDescending(x => x.AddTime);

            var totalCount = query.Count();

            var data = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            return PartialView(data);
        }

        public ActionResult AjaxCreate(int RequireID, int CompanyID, int SenderID)
        {
            var model = new ContractInfoViewModel()
            {
                RequireID = RequireID,
                CompanyID = CompanyID,
                SenderID = SenderID
            };
            ViewBag.Data_ContractCateID = Utilities.GetSelectListData(ContractCateService.GetALL(),
             x => x.ID,
             x => x.CateName,
             true);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(ContractInfoViewModel model)
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
                    var requiremodel = ContactRequireService.Find(model.RequireID);
                    var entity = new ContactRequire()
                    {
                        ID = requiremodel.ID,
                        Description = requiremodel.Description,
                        AttachmentPath = requiremodel.AttachmentPath,
                        IsRoot = requiremodel.IsRoot,
                        Status = 1,
                        Name = requiremodel.Name
                    };
                    ContactRequireService.Update(entity);
                    ContractInfoService.Create(model);
                    result.Message = "处理合同请求成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "处理合同请求失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "处理合同请求失败!", ex);
                }
            }
            return Json(result);
        }


    }
}
