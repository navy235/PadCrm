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
    public class MessageController : Controller
    {
        private ICustomerCompanyService CustomerCompanyService;
        private IRelationCateService RelationCateService;
        private ICustomerCateService CustomerCateService;
        private ICityCateService CityCateService;
        private IIndustryCateService IndustryCateService;
        private ICustomerService CustomerService;
        private IJobCateService JobCateService;
        private ICustomerShareService CustomerShareService;
        private IMemberService MemberService;
        private IPermissionsService PermissionsService;
        private IGroupService GroupService;
        private IRolesService RolesService;
        private INoticeService NoticeService;
        public MessageController(
            ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , ICityCateService CityCateService
            , IIndustryCateService IndustryCateService
            , ICustomerService CustomerService
            , IJobCateService JobCateService
            , ICustomerShareService CustomerShareService
            , IMemberService MemberService
            , IPermissionsService PermissionsService
            , IGroupService GroupService
            , IRolesService RolesService
            , INoticeService NoticeService
            )
        {
            this.CustomerCompanyService = CustomerCompanyService;
            this.RelationCateService = RelationCateService;
            this.CustomerCateService = CustomerCateService;
            this.CityCateService = CityCateService;
            this.IndustryCateService = IndustryCateService;
            this.CustomerService = CustomerService;
            this.JobCateService = JobCateService;
            this.CustomerShareService = CustomerShareService;
            this.MemberService = MemberService;
            this.PermissionsService = PermissionsService;
            this.GroupService = GroupService;
            this.RolesService = RolesService;
            this.NoticeService = NoticeService;
        }

        public ActionResult Index(int page = 1)
        {
            const int pageSize = 20;

            var model = NoticeService.GetALL().OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            var totalCount = NoticeService.GetALL()
                      .Count();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return View(model);
        }

        public ActionResult Birthday()
        {
            return View();
        }

        public ActionResult MemberBirth()
        {
            var members = MemberService.GetBirthMemberInDays(5).ToList();
            return View(members);
        }

        public ActionResult CustomerBirth()
        {
            var companyIds = CustomerCompanyService.GetMemberCompanyIDs(CookieHelper.MemberID);
            if (companyIds.Any())
            {
                var customers = CustomerService.GetBirthCustomerInDays(5, companyIds).ToList();
                return View(customers);
            }
            else
            {
                return View(new List<Customer>());
            }

        }

    }
}
