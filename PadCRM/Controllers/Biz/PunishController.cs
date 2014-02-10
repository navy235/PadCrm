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
    public class PunishController : Controller
    {
        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IJobTitleCateService JobTitleCateService;
        private IRuleCateService RuleCateService;
        private IPunishService PunishService;

        public PunishController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IJobTitleCateService JobTitleCateService
            , IRuleCateService RuleCateService
            , IPunishService PunishService
            )
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.JobTitleCateService = JobTitleCateService;
            this.RuleCateService = RuleCateService;
            this.PunishService = PunishService;
        }

        public ActionResult Index()
        {
            ViewBag.RuleID = Utilities.GetSelectListData(
              RuleCateService.GetALL().Where(x => x.PID.Equals(null))
              , x => x.ID, x => x.CateName, true);
            return View();
        }
        public ActionResult Data_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = PunishService.GetKendoALL();
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult Create()
        {
            ViewBag.Data_RuleID = Utilities.GetSelectListData(
              RuleCateService.GetALL().Where(x => x.PID.Equals(null))
              , x => x.ID, x => x.CateName, true);
            return View(new PunishViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PunishViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Score == 0)
                    {
                        var rule = RuleCateService.Find(model.RuleID);
                        model.Score = rule.OrderIndex;
                    }
                    PunishService.Create(model);
                    result.Message = "添加行政奖惩成功！";
                    LogHelper.WriteLog("添加行政奖惩告成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("添加行政奖惩错误", ex);
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

            var entity = PunishService.GetALL().Include(x => x.Member).Single(x => x.ID == ID);
            var model = new PunishEditViewModel()
            {
                ID = entity.ID,
                Description = entity.Description,
                MemberID = entity.MemberID,
                RuleID = entity.RuleID,
                Score = entity.Score,
                UserName = entity.Member.NickName
            };
            ViewBag.Data_RuleID = Utilities.GetSelectListData(
              RuleCateService.GetALL().Where(x => x.PID.Equals(null))
              , x => x.ID, x => x.CateName, model.RuleID, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PunishEditViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var entity = new PunishViewModel()
                    {
                        ID = model.ID,
                        Description = model.Description,
                        MemberID = model.MemberID,
                        RuleID = model.RuleID,
                        Score = model.Score
                    };
                    PunishService.Update(entity);
                    result.Message = "编辑行政奖惩成功！";
                    LogHelper.WriteLog("编辑行政奖惩成功");
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = Utilities.GetInnerMostException(ex);
                    result.AddServiceError(result.Message);
                    LogHelper.WriteLog("编辑行政奖惩错误", ex);
                }
            }
            else
            {
                result.Message = "请检查表单是否填写完整！";
                result.AddServiceError("请检查表单是否填写完整！");

            }
            ViewBag.Data_RuleID = Utilities.GetSelectListData(
            RuleCateService.GetALL().Where(x => x.PID.Equals(null))
            , x => x.ID, x => x.CateName, model.RuleID, true);
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
                    var model = PunishService.Find(IdArr[i]);
                    PunishService.Delete(model);
                }
                LogHelper.WriteLog("删除行政奖惩成功");
                result.Message = "删除行政奖惩成功！";
            }
            catch (DbEntityValidationException ex)
            {
                result.Message = "删除行政奖惩错误！";
                result.AddServiceError("删除行政奖惩错误!");
                LogHelper.WriteLog("删除行政奖惩错误", ex);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
