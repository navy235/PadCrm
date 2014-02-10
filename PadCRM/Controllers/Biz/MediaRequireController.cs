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
    public class MediaRequireController : Controller
    {
        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IMediaRequireService MediaRequireService;
        private IPermissionsService PermissionsService;
        private ICustomerCompanyService CustomerCompanyService;
        public MediaRequireController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IPermissionsService PermissionsService
            , IMediaRequireService MediaRequireService
            , ICustomerCompanyService CustomerCompanyService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.PermissionsService = PermissionsService;
            this.MediaRequireService = MediaRequireService;
            this.CustomerCompanyService = CustomerCompanyService;
        }

        public ActionResult Index(int ID, int page = 1)
        {
            const int pageSize = 20;
            var Medias = MediaRequireService.GetALL()
                .Where(x => x.CompanyID == ID).Select(x => new MediaRequireItemViewModel()
                {
                    AddTime = x.AddTime,
                    CompanyID = x.CompanyID,
                    Description = x.Description,
                    ID = x.ID,
                    Status = x.Status,
                    Name = x.Name,
                    AddUser = x.AddUser,
                    SenderID = x.SenderID
                }).OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var totalCount = MediaRequireService.GetALL()
                .Count(x => x.CompanyID == ID);
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            ViewBag.CompanyID = ID;
            return PartialView(Medias);
        }

        public ActionResult My()
        {
            return View();
        }

        public ActionResult Data_Read(int Status = 0, int page = 1)
        {
            const int pageSize = 20;
            var member = MemberService.Find(CookieHelper.MemberID);
            var Medias = MediaRequireService.GetALL()
                .Where(x => x.SenderID == CookieHelper.MemberID
                && x.Status == Status
                && x.IsRoot == 1).Select(x => new MediaRequireGroupViewModel()
                {
                    AddTime = x.AddTime,
                    CompanyID = x.CompanyID,
                    Description = x.Description,
                    ID = x.ID,
                    IsRoot = x.IsRoot,
                    Status = x.Status,
                    Name = x.Name,
                    AddUser = x.AddUser,
                    SenderID = x.SenderID,
                    ResolveID = x.ResolveID

                }).OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var totalCount = MediaRequireService.GetALL()
                .Count(x => x.SenderID == CookieHelper.MemberID
                    && x.Status == Status
                    && x.IsRoot == 1);
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            foreach (var item in Medias)
            {
                var selfItem = new MediaRequireItemViewModel()
                {
                    AddTime = item.AddTime,
                    CompanyID = item.CompanyID,
                    Description = item.Description,
                    ID = item.ID,
                    Status = item.Status,
                    Name = item.Name,
                    SenderID = item.SenderID,
                    AddUser = item.AddUser,
                    ResolveID = item.ResolveID
                };
                item.MediaRequires.Add(selfItem);
                var MediaRequires = MediaRequireService.GetALL()

                    .Where(x => x.PID == item.ID)
                    .Select(o => new MediaRequireItemViewModel()
                    {
                        AddTime = o.AddTime,
                        CompanyID = o.CompanyID,
                        Description = o.Description,
                        ID = o.ID,
                        Status = o.Status,
                        Name = o.Name,
                        SenderID = o.SenderID,
                        ResolveID = o.ResolveID,
                        AddUser = o.AddUser,
                        AttachmentPath = o.AttachmentPath
                    }).OrderBy(o => o.AddTime).ToList();

                item.MediaRequires.AddRange(MediaRequires);
            }
            ViewBag.Status = "category" + Status.ToString();

            return PartialView(Medias);
        }

        public ActionResult AjaxCreate(int ID)
        {
            var entity = CustomerCompanyService.Find(ID);
            var model = new MediaRequireViewModel()
            {
                CompanyID = ID,
                SenderID = CookieHelper.MemberID,
                Name = "媒介策略请求:" + entity.Name,
                IsRoot = 1
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(MediaRequireViewModel model)
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
                    var resolver = MemberService.Find(model.ResolveID);
                    model.DepartmentID = resolver.DepartmentID;
                    MediaRequireService.Create(model);
                    result.Message = "添加媒体请求成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "添加媒体请求失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加媒体请求失败!", ex);
                }
            }
            return Json(result);
        }


        public ActionResult AjaxAppend(int ID)
        {
            var currentModel = MediaRequireService.Find(ID);
            var model = new MediaRequireAppendViewModel()
            {
                CompanyID = currentModel.CompanyID,
                DepartmentID = currentModel.DepartmentID,
                IsRoot = 0,
                ResolveID = currentModel.ResolveID,
                SenderID = currentModel.SenderID,
                Name = "回复:" + currentModel.Name,
                PID = ID
            };
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxAppend(MediaRequireAppendViewModel model)
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
                    var entity = new MediaRequireViewModel()
                    {
                        DepartmentID = model.DepartmentID,
                        AttachmentPath = model.AttachmentPath,
                        CompanyID = model.CompanyID,
                        Description = model.Description,
                        IsRoot = model.IsRoot,
                        ID = model.ID,
                        Name = model.Name,
                        ResolveID = model.ResolveID,
                        PID = model.PID,
                        SenderID = model.SenderID,
                        Status = model.Status

                    };
                    MediaRequireService.Create(entity);
                    result.Message = "追加回复成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "追加回复失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "追加回复失败!", ex);
                }
            }
            return Json(result);
        }



        [HttpPost]
        public ActionResult ChangStatus(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var model = MediaRequireService.Find(ID);
                var entity = new MediaRequire()
                {
                    ID = model.ID,
                    Description = model.Description,
                    AttachmentPath = model.AttachmentPath,
                    IsRoot = model.IsRoot,
                    Status = 1,
                    Name = model.Name
                };
                MediaRequireService.Update(entity);
                LogHelper.WriteLog("更改媒介请求状态成功");
                result.Message = "更改媒介请求状态成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "更改媒介请求状态错误！";
                result.AddServiceError("更改媒介请求状态错误!");
                LogHelper.WriteLog("更改媒介请求状态错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
