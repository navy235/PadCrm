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
        private IRuleCateService RuleCateService;
        private IPunishService PunishService;
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
            , IRuleCateService RuleCateService
            , IPunishService PunishService
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
            this.RuleCateService = RuleCateService;
            this.PunishService = PunishService;
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

        public ActionResult MyPunish()
        {

            PunishSearchViewModel model = new PunishSearchViewModel();

            var Rewardlist = new List<SelectListItem>(){
              new SelectListItem(){
                  Value="0",
                  Text="请选择"
                 },
              new SelectListItem(){
              Value="1",
              Text="惩罚"
             },
              new SelectListItem(){
              Value="2",
              Text="奖励"
             }
            };
            ViewBag.Data_Reward = Rewardlist;
            ViewBag.Data_RuleID = Utilities.GetSelectListData(RuleCateService.GetALL(),
                x => x.ID,
                x => x.CateName,
                true, true);


            return View(model);

        }

        public ActionResult PunishSearch(PunishSearchViewModel model, int page = 1)
        {

            ViewBag.Data_RuleID = Utilities.GetSelectListData(RuleCateService.GetALL(),
             x => x.ID,
             x => x.CateName,
             true, true);

            const int pageSize = 20;

            var query = PunishService.GetALL().Where(x => x.MemberID == CookieHelper.MemberID);

            if (model.Reward == 1)
            {
                query = query.Where(x => x.Score < 0);
            }
            else if (model.Reward == 2)
            {
                query = query.Where(x => x.Score > 0);
            }

            if (model.RuleID != 0)
            {
                query = query.Where(x => x.RuleID == model.RuleID);
            }

            query = query.Where(x => x.AddTime < model.EndTime
                && x.AddTime > model.StartTime);


            var totalCount = query.Count();

            var data = query.OrderByDescending(x => x.AddTime).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return PartialView(data);
        }

    }
}
