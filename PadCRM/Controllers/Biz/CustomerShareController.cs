
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
    public class CustomerShareController : Controller
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
        public CustomerShareController(
            ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , ICityCateService CityCateService
            , IIndustryCateService IndustryCateService
            , ICustomerService CustomerService
            , IJobCateService JobCateService
            , ICustomerShareService CustomerShareService
            , IMemberService MemberService

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
        }


        public ActionResult Index(int ID)
        {
            ViewBag.CompanyID = ID;
            var members = CustomerShareService.GetALL().Include(x => x.Member)
                .Where(x => x.CompanyID == ID)
                .Select(x => new CustomerShareItemViewModel()
                {
                    AddTime = x.AddTime,
                    CompanyID = x.CompanyID,
                    DepartmentID = x.Member.DepartmentID,
                    UserName = x.Member.NickName,
                    ID = x.ID,
                    MemberID = x.MemberID
                }).ToList();
            return View(members);
        }

        [HttpPost]
        public ActionResult Share(string ids, int companyId)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var idlist = Utilities.GetIdList(ids);
                foreach (var id in idlist)
                {
                    var shareitem = new CustomerShare()
                    {
                        MemberID = id,
                        AddTime = DateTime.Now,
                        AddUser = CookieHelper.MemberID,
                        CompanyID = companyId
                    };
                    CustomerShareService.Create(shareitem);
                }
                result.Message = "设置共享人员成功！";
            }
            catch (Exception ex)
            {
                result.Message = "设置共享人员失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "设置共享人员失败!", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var entity = CustomerShareService.Find(ID);
                CustomerShareService.Delete(entity);
                result.Message = "删除共享人员成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除共享人员失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除共享人员失败!", ex);
            }
            return Json(result);
        }

    }
}
