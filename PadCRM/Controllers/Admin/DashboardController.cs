﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PadCRM.ViewModels;
using Maitonn.Core;
using PadCRM.Models;

using PadCRM.Utils;
using PadCRM.Service.Interface;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using PadCRM.Service;

namespace PadCRM.Controllers
{
    public class DashboardController : Controller
    {

        //private IGroupService groupService;
        //private IMemberService memberService;
        //public DashboardController(
        //    IGroupService _groupService
        //  , IMemberService _memberService
        // )
        //{
        //    groupService = _groupService;
        //    memberService = _memberService;
        //}

        //[LoginAuthorize]
        public ActionResult Index()
        {
            //var Member = memberService.Find(Convert.ToInt32(CookieHelper.UID));

            //DashBoardViewModel model = new DashBoardViewModel()
            //{
            //    Name = "运营管理系统",
            //    GroupName = groupService.Find(Member.GroupID).Name,
            //    NickName = Member.NickName,
            //    Version = "1.0",
            //    CurrentIP = HttpHelper.IP,
            //    CurrentTime = DateTime.Now,
            //    LastIP = Member.LastIP,
            //    LastTime = Member.LastTime,
            //    LoginCount = Member.LoginCount

            //};
            //ViewBag.DashModel = model;

            return View();
        }

    }
}
