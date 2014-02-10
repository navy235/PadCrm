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
    public class MemberController : Controller
    {
        //

        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IJobTitleCateService JobTitleCateService;
        public MemberController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IJobTitleCateService JobTitleCateService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.JobTitleCateService = JobTitleCateService;
        }

        #region KendoGrid Action
        public ActionResult Index()
        {
            ViewBag.Data_GroupID = Utilities.GetSelectListData(GroupService.GetALL()
              , x => x.ID, x => x.Name, true);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, true);
            return View();
        }

        public ActionResult Editing_Read([DataSourceRequest] DataSourceRequest request)
        {

            var members = MemberService.GetKendoALL().Where(x => x.Status > (int)MemberCurrentStatus.Delete);
            return Json(members.ToDataSourceResult(request));
        }

        public ActionResult Delete()
        {
            ViewBag.Data_GroupID = Utilities.GetSelectListData(GroupService.GetALL()
              , x => x.ID, x => x.Name, true);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, true);
            return View();
        }

        public ActionResult DeleteEditing_Read([DataSourceRequest] DataSourceRequest request)
        {
            var members = MemberService.GetKendoALL().Where(x => x.Status == (int)MemberCurrentStatus.Delete);
            return Json(members.ToDataSourceResult(request));
        }

        #endregion


        #region Create Edit
        public ActionResult Create()
        {

            ViewBag.Data_GroupID = Utilities.GetSelectListData(GroupService.GetALL()
                , x => x.ID, x => x.Name, true);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, true);
            ViewBag.Data_JobTitleID = Utilities.GetSelectListData(
                JobTitleCateService.GetALL()
               , x => x.ID, x => x.CateName, true);
            return View(new MemberViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.Create(model);
                    result.Message = "添加会员信息成功！";
                    LogHelper.WriteLog("添加会员信息成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加会员信息错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            ViewBag.Data_GroupID = Utilities.GetSelectListData(GroupService.GetALL()
              , x => x.ID, x => x.Name, true);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, true);
            ViewBag.Data_JobTitleID = Utilities.GetSelectListData(
               JobTitleCateService.GetALL()
              , x => x.ID, x => x.CateName, true);
            return View(model);
        }


        public ActionResult Edit(int ID)
        {
            ChineseLunisolarCalendar clc = new System.Globalization.ChineseLunisolarCalendar();
            string Luneryear = Utilities.ConvertToChineseYearStyle(clc.GetYear(DateTime.Now));
            var entity = MemberService.Find(ID);
            var model = new MemberEditViewModel()
            {
                Address = entity.Address,
                BirthDay = entity.BirthDay,
                Sex = entity.Sex,
                QQ = entity.QQ,
                DepartmentID = entity.DepartmentID,
                Description = entity.Description,
                Email = entity.Email,
                GroupID = entity.GroupID,
                IsLeader = entity.IsLeader,
                AvtarUrl = entity.AvtarUrl,
                IsLeap = entity.IsLeap,
                Mobile = entity.Mobile,
                Mobile1 = entity.Mobile1,
                NickName = entity.NickName,
                MemberID = entity.MemberID,
                FamilySituation = entity.FamilySituation,
                IDNumber = entity.IDNumber,
                JobExp = entity.JobExp,
                StudyExp = entity.StudyExp,
                JobTitleID = entity.JobTitleID
            };

            ViewBag.Data_GroupID = Utilities.GetSelectListData(GroupService.GetALL()
           , x => x.ID, x => x.Name, model.GroupID, true);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, model.DepartmentID, true);
            ViewBag.Data_JobTitleID = Utilities.GetSelectListData(
               JobTitleCateService.GetALL()
              , x => x.ID, x => x.CateName, model.JobTitleID, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MemberEditViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    MemberService.Update(model);
                    result.Message = "编辑会员信息成功！";
                    LogHelper.WriteLog("编辑会员信息成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("编辑会员信息错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");
            }
            ViewBag.Data_GroupID = Utilities.GetSelectListData(GroupService.GetALL()
                  , x => x.ID, x => x.Name, model.GroupID, true);
            ViewBag.Data_DepartmentID = Utilities.GetSelectListData(DepartmentService.GetALL()
                , x => x.ID, x => x.Name, model.DepartmentID, true);
            ViewBag.Data_JobTitleID = Utilities.GetSelectListData(
             JobTitleCateService.GetALL()
            , x => x.ID, x => x.CateName, model.JobTitleID, true);
            return View(model);
        }


        [HttpPost]
        public ActionResult SetDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                MemberService.ChangeStatus(ids, MemberCurrentStatus.Delete);
                result.Message = "禁用员工帐号成功！";
            }
            catch (Exception ex)
            {
                result.Message = "禁用员工帐号失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "禁用员工帐号失败!", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult SetRefresh(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                MemberService.ChangeStatus(ids, MemberCurrentStatus.Default);
                result.Message = "恢复员工帐号成功！";
            }
            catch (Exception ex)
            {
                result.Message = "恢复员工帐号失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "恢复员工帐号失败!", ex);
            }
            return Json(result);
        }

        #endregion


        public ActionResult Export()
        {

            //Create new Excel workbook
            var workbook = new HSSFWorkbook();

            //Create new Excel sheet
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 30 * 256);
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
            headerRow.CreateCell(0).SetCellValue("会员ID");
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



            var members = MemberService.GetKendoALL().Include(x => x.Department)
                .Where(x => x.Status > (int)MemberCurrentStatus.Delete);

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

    }
}

