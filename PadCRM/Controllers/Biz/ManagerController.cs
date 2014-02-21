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
    public class ManagerController : Controller
    {
        //

        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IContactRequireService ContactRequireService;
        private IPermissionsService PermissionsService;
        private IMediaRequireService MediaRequireService;
        public ManagerController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IPermissionsService PermissionsService
            , IContactRequireService ContactRequireService
            , IMediaRequireService MediaRequireService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.PermissionsService = PermissionsService;
            this.ContactRequireService = ContactRequireService;
            this.MediaRequireService = MediaRequireService;
        }

        public ActionResult Index(int page = 1)
        {
            const int pageSize = 20;
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, true);
            var user = MemberService.Find(CookieHelper.MemberID);
            var hasPermission = PermissionsService.CheckPermission("boss", "controller", CookieHelper.MemberID);
            var members = MemberService.GetKendoALL()
                .Where(x => x.Status > (int)MemberCurrentStatus.Delete && x.MemberID != CookieHelper.MemberID);
            if (!hasPermission)
            {
                var memberIds = MemberService.GetMemberIDs(user.DepartmentID);
                var models = members.Where(x => memberIds.Contains(x.MemberID))
                   .OrderBy(x => x.DepartmentID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList();

                var totalCount = memberIds.Count;
    
                ViewBag.PageInfo = new PagingInfo()
                {
                    TotalItems = totalCount,
                    CurrentPage = page,
                    ItemsPerPage = pageSize
                };

                return View(models);
            }
            else
            {
                var totalCount = MemberService.GetKendoALL()
                .Count(x => x.Status > (int)MemberCurrentStatus.Delete && x.MemberID != CookieHelper.MemberID);
                ViewBag.PageInfo = new PagingInfo()
                {
                    TotalItems = totalCount,
                    CurrentPage = page,
                    ItemsPerPage = pageSize
                };
                var models = members.OrderBy(x => x.MemberID)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList();
                return View(models);
            }


        }

        public ActionResult Create()
        {

            var member = MemberService.GetALL().Include(x => x.Department)
                .Single(x => x.MemberID == CookieHelper.MemberID);
            var departCode = member.Department.Code;
            var maxCode = Utilities.GetMaxCode(departCode, member.Department.Level);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
                DepartmentService.GetALL().Where(x => x.Code >= departCode && x.Code <= maxCode)
                , x => x.ID, x => x.Name, true);
            return View(new MemberClerkViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberClerkViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {

                    //groupID 6  指的是基本业务功能的会员群组
                    var entity = new MemberViewModel()
                    {
                        Address = model.Address,
                        BirthDay = model.BirthDay,
                        DepartmentID = model.DepartmentID,
                        Description = model.Description,
                        Email = model.Email,
                        IsLeader = model.IsLeader,
                        IsLeap = model.IsLeap,
                        Mobile = model.Mobile,
                        Mobile1 = model.Mobile1,
                        NickName = model.NickName,
                        Password = model.Password,
                        QQ = model.QQ,
                        Sex = model.Sex,
                        GroupID = 6,
                        FamilySituation = model.FamilySituation,
                        IDNumber = model.IDNumber,
                        JobExp = model.JobExp,
                        AvtarUrl = model.AvtarUrl,
                        StudyExp = model.StudyExp
                    };
                    MemberService.Create(entity);
                    result.Message = "添加业务人员成功！";
                    LogHelper.WriteLog("添加业务人员成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加业务人员错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            var member = MemberService.GetALL().Include(x => x.Department)
                 .Single(x => x.MemberID == CookieHelper.MemberID);
            var departCode = member.Department.Code;
            var maxCode = Utilities.GetMaxCode(departCode, member.Department.Level);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(
                DepartmentService.GetALL().Where(x => x.Code >= departCode && x.Code <= maxCode)
                , x => x.ID, x => x.Name, true);
            return View(model);
        }

        public ActionResult Resume(int ID)
        {
            var model = MemberService.GetALL()
                .Include(x => x.Department)
                .Single(x => x.MemberID == ID);
            return View(model);
        }

        public ActionResult Download(int ID)
        {
            var model = MemberService.GetALL()
             .Include(x => x.Department)
             .Single(x => x.MemberID == ID);
            var html = Utilities.RenderPartialToString(this.ControllerContext,
                    "Download",
                    new ViewDataDictionary(model),
                    new TempDataDictionary());
            byte[] data = System.Text.Encoding.UTF8.GetBytes(html);
            return File(data, "text/html", model.NickName + "-简历.html");
        }

        public ActionResult Export()
        {

            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 10 * 256);
            sheet.SetColumnWidth(1, 30 * 256);
            sheet.SetColumnWidth(2, 20 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 30 * 256);
            sheet.SetColumnWidth(7, 30 * 256);
            sheet.SetColumnWidth(8, 30 * 256);
            sheet.SetColumnWidth(9, 30 * 256);
            sheet.SetColumnWidth(10, 30 * 256);
            //Create a header row
            var headerRow = sheet.CreateRow(0);

            //Set the column names in the header row
            headerRow.CreateCell(0).SetCellValue("会员 ID");
            headerRow.CreateCell(1).SetCellValue("部门");
            headerRow.CreateCell(2).SetCellValue("负责人");
            headerRow.CreateCell(3).SetCellValue("姓名");
            headerRow.CreateCell(4).SetCellValue("手机1");
            headerRow.CreateCell(5).SetCellValue("手机2");
            headerRow.CreateCell(6).SetCellValue("邮箱地址");
            headerRow.CreateCell(7).SetCellValue("QQ");
            headerRow.CreateCell(8).SetCellValue("地址");
            headerRow.CreateCell(9).SetCellValue("生日类型");
            headerRow.CreateCell(10).SetCellValue("生日");

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;


            var user = MemberService.Find(CookieHelper.MemberID);
            var memberIds = MemberService.GetMemberIDs(user.DepartmentID);
            var members = MemberService.GetKendoALL().Include(x => x.Department)
                .Where(x => x.Status > (int)MemberCurrentStatus.Delete
                && memberIds.Contains(x.MemberID) && x.MemberID != CookieHelper.MemberID);

            //Populate the sheet with values from the grid data
            foreach (Member member in members)
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(member.MemberID);
                row.CreateCell(1).SetCellValue(member.Department.Name);
                row.CreateCell(2).SetCellValue(member.IsLeader ? "是" : "否");
                row.CreateCell(3).SetCellValue(member.NickName);
                row.CreateCell(4).SetCellValue(member.Mobile);
                row.CreateCell(5).SetCellValue(member.Mobile1);
                row.CreateCell(6).SetCellValue(member.Email);
                row.CreateCell(7).SetCellValue(member.QQ);
                row.CreateCell(8).SetCellValue(member.Address);
                row.CreateCell(9).SetCellValue(member.IsLeap ? "农历" : "阳历");
                row.CreateCell(10).SetCellValue(member.IsLeap
                    ? (Utilities.ConvertToChineseYearStyle(member.BirthDay.Year) + member.BirthDay1)
                    : member.BirthDay.ToString("yyyy-MM-dd"));

            }

            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "人员信息.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        public ActionResult Resolve()
        {
            return View();
        }

        public ActionResult ResolveContact(int Status = 0, int page = 1)
        {

            const int pageSize = 20;

            var member = MemberService.Find(CookieHelper.MemberID);

            var contacts = ContactRequireService.GetALL()
                .Where(x => x.DepartmentID == member.DepartmentID
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
                .Count(x => x.DepartmentID == member.DepartmentID && x.IsRoot == 1);

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
                ResolveID = CookieHelper.MemberID,
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


                    result.Message = "添加合同处理成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "添加合同处理失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加合同处理失败!", ex);
                }
            }

            return Json(result);
        }


        public ActionResult Mediarequire()
        {
            return View();
        }

        public ActionResult Mediarequire_Read(int Status = 0, int page = 1)
        {
            const int pageSize = 20;
            var member = MemberService.Find(CookieHelper.MemberID);
            var Medias = MediaRequireService.GetALL()
                .Where(x => x.ResolveID == CookieHelper.MemberID
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
                .Count(x => x.ResolveID == CookieHelper.MemberID
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
    }
}
