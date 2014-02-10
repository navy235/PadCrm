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
    public class ContactRequireController : Controller
    {
        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IContactRequireService ContactRequireService;
        private IPermissionsService PermissionsService;
        public ContactRequireController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IPermissionsService PermissionsService
            , IContactRequireService ContactRequireService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.PermissionsService = PermissionsService;
            this.ContactRequireService = ContactRequireService;
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Data_Read(int Status = 0, int page = 1)
        {

            const int pageSize = 20;

            var member = MemberService.Find(CookieHelper.MemberID);

            var contacts = ContactRequireService.GetALL()
                .Where(x => x.SenderID == CookieHelper.MemberID
                && x.Status == Status
                && x.IsRoot == 1).Select(x => new ContactRequireGroupViewModel()
                {
                    AddTime = x.AddTime,
                    CompanyID = x.CompanyID,
                    Description = x.Description,
                    ID = x.ID,
                    IsRoot = x.IsRoot,
                    Status = x.Status,
                    Name = x.Name,
                    AddUser = x.AddUser,
                    SenderID = x.SenderID
                }).OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();


            var totalCount = ContactRequireService.GetALL()
                .Count(x => x.SenderID == CookieHelper.MemberID
                    && x.Status == Status
                    && x.IsRoot == 1);


            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            foreach (var item in contacts)
            {
                var selfItem = new ContactRequireItemViewModel()
                {
                    AddTime = item.AddTime,
                    CompanyID = item.CompanyID,
                    Description = item.Description,
                    ID = item.ID,
                    Status = item.Status,
                    Name = item.Name,
                    SenderID = item.SenderID,
                    AddUser = item.AddUser
                };
                item.ContactRequires.Add(selfItem);
                var ContactRequires = ContactRequireService.GetALL()

                    .Where(x => x.PID == item.ID)
                    .Select(o => new ContactRequireItemViewModel()
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

                item.ContactRequires.AddRange(ContactRequires);
            }
            ViewBag.Status = "category" + Status.ToString();

            return PartialView(contacts);
        }


        public ActionResult AjaxCreate(int ID)
        {
            var currentModel = ContactRequireService.Find(ID);

            var model = new ContactRequireViewModel()
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
        public ActionResult AjaxCreate(ContactRequireViewModel model)
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
                    ContactRequireService.Create(model);
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
                var model = ContactRequireService.Find(ID);
                var entity = new ContactRequire()
                {
                    ID = model.ID,
                    Description = model.Description,
                    AttachmentPath = model.AttachmentPath,
                    IsRoot = model.IsRoot,
                    Status = 1,
                    Name = model.Name
                };
                ContactRequireService.Update(entity);
                LogHelper.WriteLog("更改合同状态成功");
                result.Message = "更改合同状态成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "更改合同状态错误！";
                result.AddServiceError("更改合同状态错误!");
                LogHelper.WriteLog("更改合同状态错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
