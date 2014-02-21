using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Maitonn.Core;
using PadCRM.Service.Interface;
using PadCRM.Models;
using PadCRM.ViewModels;
using PadCRM.Utils;
using System.IO;
using System.Text;

namespace PadCRM.Controllers
{
    public class LoginController : Controller
    {
        private IMemberService MemberService;
        private IGroupService GroupService;
        private IDepartmentService DepartmentService;
        private IPermissionsService PermissionsService;
        public LoginController(
          IMemberService MemberService
            , IGroupService GroupService
            , IDepartmentService DepartmentService
            , IPermissionsService PermissionsService)
        {
            this.MemberService = MemberService;
            this.GroupService = GroupService;
            this.DepartmentService = DepartmentService;
            this.PermissionsService = PermissionsService;
        }

        public ActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model, string ReturnUrl = null, bool Remember = false)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string Md5Password = CheckHelper.StrToMd5(model.Password);
                    var status = MemberService.Login(model.UserName, Md5Password);
                    if (status == 1)
                    {
                        ViewBag.Message = null;

                        if (!string.IsNullOrEmpty(ReturnUrl))
                            return Redirect(ReturnUrl);
                        else
                            return RedirectToAction("index", "welcome");
                    }
                    else
                    {
                        if (status == 0)
                        {
                            ViewBag.Message = "您的用户名和密码不匹配";
                        }
                        else if (status < 0)
                        {
                            ViewBag.Message = "您的用户名已经被禁用如要恢复请联系你的主管";
                        }
                        return View(model);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog("用户:" + model.UserName + "登录失败!", ex);
                    ViewBag.Message = "服务器错误，请刷新页面重新登录";
                    return View(model);
                }
            }
            else
            {
                ViewBag.Message = "您的输入有误请确认后提交";
                return View(model);
            }
        }


        public ActionResult LogOut(string returnUrl = null)
        {
            CookieHelper.ClearCookie();
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("index", "login");
            }
            else
            {
                return Redirect(returnUrl);
            }
        }
    }
}
