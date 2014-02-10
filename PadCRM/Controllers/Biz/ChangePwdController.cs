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
    [LoginAuthorize]
    public class ChangePwdController : Controller
    {
        //
        // GET: /ChangePwd/

        private IMemberService MemberService;
        public ChangePwdController(
            IMemberService MemberService
            )
        {
            this.MemberService = MemberService;
        }

        public ActionResult Index()
        {
            Member member = MemberService.Find(CookieHelper.MemberID);
            return View(new ChangePasswordViewModel()
            {
                MemberID = member.MemberID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChangePasswordViewModel model)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            if (ModelState.IsValid)
            {
                try
                {
                    var memberID = Convert.ToInt32(CookieHelper.UID);
                    if (!MemberService.ChangePassword(memberID, model.OldPassword, model.NewPassword))
                    {
                        result.Message = "旧密码错误!";
                        result.AddServiceError("旧密码错误");
                        return View(model);
                    }
                    result.Message = "密码修改成功!";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = "密码修改失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + model.MemberID + "密码修改失败!", ex);
                }
            }
            return View(model);
        }


    }
}
