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
using System.IO;
using System.Text;
using NPOI.HSSF.UserModel;

namespace PadCRM.Controllers.Ajax
{
    public class AjaxServiceController : Controller
    {
        private IIndustryCateService IndustryCateService;
        private ICityCateService CityCateService;
        private ICustomerCompanyService CustomerCompanyService;
        private IRelationCateService RelationCateService;
        private ICustomerCateService CustomerCateService;
        private IMemberService MemberService;
        private IDepartmentService DepartmentService;
        private IPermissionsService PermissionsService;
        private IJobTitleCateService JobTitleCateService;
        private IJobCateService JobCateService;
        private IContractCateService ContractCateService;
        public AjaxServiceController(
            IIndustryCateService IndustryCateService
            , ICityCateService CityCateService
            , ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , IMemberService MemberService
            , IDepartmentService DepartmentService
            , IPermissionsService PermissionsService
            , IJobTitleCateService JobTitleCateService
            , IJobCateService JobCateService
            , IContractCateService ContractCateService
            )
        {
            this.IndustryCateService = IndustryCateService;
            this.CityCateService = CityCateService;
            this.CustomerCompanyService = CustomerCompanyService;
            this.RelationCateService = RelationCateService;
            this.CustomerCateService = CustomerCateService;
            this.MemberService = MemberService;
            this.DepartmentService = DepartmentService;
            this.PermissionsService = PermissionsService;
            this.JobTitleCateService = JobTitleCateService;
            this.JobCateService = JobCateService;
            this.ContractCateService = ContractCateService;
        }


        public ActionResult Permission()
        {
            ViewBag.managerPermission = CookieHelper.CheckPermission("manager");
            ViewBag.punishPermission = CookieHelper.CheckPermission("punish");
            ViewBag.caiwuPermission = CookieHelper.CheckPermission("caiwu");
            ViewBag.bossPermission = CookieHelper.CheckPermission("boss");
            return PartialView();
        }

        public ActionResult CityCode(int pid = 0)
        {
            var query = CityCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CityName(string value)
        {
            var cityIds = Utilities.GetIdList(value);
            var citys = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            var cityName = string.Join("-", citys);
            return Content(cityName);
        }

        public JsonResult ValidatePassword(string OldPassword)
        {
            if (MemberService.ValidatePassword(CookieHelper.MemberID, OldPassword))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult IndustryCode(int pid = 0)
        {
            var query = IndustryCateService.GetALL();

            if (pid == 0)
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            else
            {
                query = query.Where(x => x.PID == pid);
            }

            var selectlist = Utilities.CreateSelectList(
               query.ToList()
                , item => item.ID
                , item => item.CateName, false);

            return Json(selectlist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndustryName(string value)
        {
            var industryIds = Utilities.GetIdList(value);
            var industrys = IndustryCateService.GetALL()
                .Where(x => industryIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            var industryName = string.Join("-", industrys);
            return Content(industryName);
        }

        public ActionResult RelationIDName(int key)
        {
            return Content(RelationCateService.Find(key).CateName);
        }

        public ActionResult JobIDName(int key)
        {
            return Content(JobCateService.Find(key).CateName);
        }

        public ActionResult CustomerCateIDName(int key)
        {
            return Content(CustomerCateService.Find(key).CateName);
        }

        public ActionResult ContractCateIDName(int key)
        {
            return Content(ContractCateService.Find(key).CateName);
        }

        public ActionResult CustomerName(string text, int ID = 0)
        {
            var customers = CustomerCompanyService.GetALL().Select(x =>
                new CustomerCompanyViewModel()
                {
                    ID = x.ID,
                    Address = x.Address,
                    BrandName = x.BrandName,
                    CityCode = x.CityValue,
                    Description = x.Description,
                    Fax = x.Fax,
                    IndustryCode = x.IndustryValue,
                    CustomerCateID = x.CustomerCateID,
                    RelationID = x.RelationID,
                    Name = x.Name,
                    Phone = x.Phone
                }
                );
            if (!string.IsNullOrEmpty(text))
            {
                customers = customers.Where(p => p.Name.Contains(text));
            }
            if (ID != 0)
            {
                customers = customers.Where(p => p.ID != ID);
            }
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BrandName(string text, int ID = 0)
        {
            var customers = CustomerCompanyService.GetALL().Select(x =>
                new CustomerCompanyViewModel()
                {
                    ID = x.ID,
                    Address = x.Address,
                    BrandName = x.BrandName,
                    CityCode = x.CityValue,
                    Description = x.Description,
                    Fax = x.Fax,
                    IndustryCode = x.IndustryValue,
                    CustomerCateID = x.CustomerCateID,
                    RelationID = x.RelationID,
                    Name = x.Name,
                    Phone = x.Phone
                }
                );
            if (!string.IsNullOrEmpty(text))
            {
                customers = customers.Where(p => p.BrandName.Contains(text));
            }
            if (ID != 0)
            {
                customers = customers.Where(p => p.ID != ID);
            }
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MemberName(int ID)
        {
            if (ID == 0)
            {
                return Content("");
            }
            return Content(MemberService.Find(ID).NickName);
        }

        public ActionResult DepartmentName(int ID)
        {
            if (ID == 0)
            {
                return Content("");
            }
            return Content(DepartmentService.Find(ID).Name);
        }

        public ActionResult CompanyNameLink(int ID)
        {
            var company = CustomerCompanyService.Find(ID);
            return View(company);
        }


        public ActionResult GetCalendar(string Birth)
        {
            string Luneryear = Utilities.GetCurrentLunarYear();
            var date = MemberService.GetCalender(Luneryear + Birth);
            return Content(date.ToString("yyyy-MM-dd"));

        }

        public ActionResult GetUsers(int ID)
        {
            List<TreeViewItemViewModel> tree = new List<TreeViewItemViewModel>();
            GenerateTree(null, tree);
            if (tree.Count > 0)
            {
                tree[0].expanded = true;
            }
            return Json(tree, JsonRequestBehavior.AllowGet);
        }

        private void GenerateTree(int? ID, List<TreeViewItemViewModel> tree)
        {
            var query = DepartmentService.GetALL();
            var jobTitleList = JobTitleCateService.GetALL().ToList();
            if (ID.HasValue)
            {
                query = query.Where(x => x.PID == ID.Value);
            }
            else
            {
                query = query.Where(x => x.PID.Equals(null));
            }
            var departments = query.ToList();
            foreach (var depart in departments)
            {
                var treeitem = new TreeViewItemViewModel()
                {
                    id = "d_" + depart.ID,
                    spriteCssClass = "depart",
                    text = depart.Name
                };
                var members = MemberService.GetALL().Where(x => x.DepartmentID == depart.ID).ToList();
                foreach (var mb in members)
                {
                    if (mb.MemberID == CookieHelper.MemberID)
                    {
                        continue;
                    }
                    var treechilditem = new TreeViewItemViewModel()
                    {
                        id = mb.MemberID.ToString(),
                        text = mb.NickName + " (" + jobTitleList.Single(x => x.ID == mb.JobTitleID).CateName + ")",
                        items = null,
                        spriteCssClass = "user",
                    };
                    treeitem.items.Add(treechilditem);
                }
                tree.Add(treeitem);
                if (DepartmentService.GetALL().Any(x => x.PID == depart.ID))
                {
                    GenerateTree(depart.ID, treeitem.items);
                }
            }
        }


        public ActionResult CheckReplaceable(int ID)
        {
            var editable = CustomerCompanyService.IsReplaceable(ID);
            return Json(editable, JsonRequestBehavior.AllowGet);
        }

       

        public void GetValidateCode()
        {
            ValidateCode VCode = new ValidateCode("VCode", 100, 40);
        }

        public JsonResult ValidateVCode(string vcode)
        {
            bool status = false;
            if (Session["VCode"] != null)
            {
                status = Session["VCode"].ToString().Equals(vcode, StringComparison.OrdinalIgnoreCase);
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetForm(int ID)
        {
            FileStream fs = new FileStream(Server.MapPath("~/App_Data/templates/huwai.html"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            var html = sr.ReadToEnd();
            sr.Close();
            return Content(html);
        }


    }
}
