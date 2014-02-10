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

using PadCRM.Filters;

namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class CustomerCompanyController : Controller
    {
        private ICustomerCompanyService CustomerCompanyService;
        private IRelationCateService RelationCateService;
        private ICustomerCateService CustomerCateService;
        private ICityCateService CityCateService;
        private IIndustryCateService IndustryCateService;
        private ICustomerService CustomerService;
        private ITraceLogService TraceLogService;
        private ICustomerShareService CustomerShareService;
        private IMemberService MemberService;
        private IPermissionsService PermissionsService;
        private IContactRequireService ContactRequireService;
        public CustomerCompanyController(
            ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , ICityCateService CityCateService
            , IIndustryCateService IndustryCateService
            , ICustomerService CustomerService
            , ITraceLogService TraceLogService
            , ICustomerShareService CustomerShareService
            , IMemberService MemberService
            , IPermissionsService PermissionsService
            , IContactRequireService ContactRequireService
            )
        {
            this.CustomerCompanyService = CustomerCompanyService;
            this.RelationCateService = RelationCateService;
            this.CustomerCateService = CustomerCateService;
            this.CityCateService = CityCateService;
            this.IndustryCateService = IndustryCateService;
            this.CustomerService = CustomerService;
            this.TraceLogService = TraceLogService;
            this.CustomerShareService = CustomerShareService;
            this.MemberService = MemberService;
            this.PermissionsService = PermissionsService;
            this.ContactRequireService = ContactRequireService;
        }


        #region baseRead
        public ActionResult Index()
        {
            SearchCompanyViewModel model = new SearchCompanyViewModel();
            ViewBag.Data_CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL(),
                x => x.ID,
                x => x.CateName,
                true, true);
            return View(model);
        }
        public ActionResult Delete()
        {
            SetCommonData();
            return View();
        }

        public ActionResult Share()
        {
            SetCommonData();
            return View();
        }
        public ActionResult Common()
        {
            SetCommonData();
            return View();
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
            headerRow.CreateCell(0).SetCellValue("客户ID");
            headerRow.CreateCell(1).SetCellValue("公司名称");
            headerRow.CreateCell(2).SetCellValue("品牌名称");
            headerRow.CreateCell(3).SetCellValue("城市");
            headerRow.CreateCell(4).SetCellValue("行业");
            headerRow.CreateCell(5).SetCellValue("当前关系程度");
            headerRow.CreateCell(6).SetCellValue("传真");
            headerRow.CreateCell(7).SetCellValue("电话");
            headerRow.CreateCell(8).SetCellValue("地址");
            headerRow.CreateCell(9).SetCellValue("录入者");
            headerRow.CreateCell(10).SetCellValue("录入时间");


            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;

            var model = CustomerCompanyService.GetKendoALL()
                .Include(x => x.RelationCate)
                .Include(x => x.AddMember)
                .Where(x => x.AddUser == CookieHelper.MemberID && x.Status > (int)CustomerCompanyStatus.Delete).ToList();

            //var shares = CustomerShareService.GetALL()
            //    .Include(x => x.CustomerCompany)
            //    .Where(x => x.MemberID == CookieHelper.MemberID)
            //    .Select(x => x.CustomerCompany).ToList();
            //model.AddRange(shares);

            //Populate the sheet with values from the grid data
            foreach (CustomerCompany cc in model)
            {
                //Create a new row
                var row = sheet.CreateRow(rowNumber++);

                //Set values for the cells
                row.CreateCell(0).SetCellValue(cc.ID);
                row.CreateCell(1).SetCellValue(cc.Name);
                row.CreateCell(2).SetCellValue(cc.BrandName);
                var cityIds = Utilities.GetIdList(cc.CityValue);
                var citys = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
                var cityName = string.Join("-", citys);

                var industryIds = Utilities.GetIdList(cc.IndustryValue);
                var industrys = IndustryCateService.GetALL().Where(x => industryIds.Contains(x.ID)).Select(x => x.CateName).ToList();
                var industryName = string.Join("-", industrys);

                row.CreateCell(3).SetCellValue(cityName);
                row.CreateCell(4).SetCellValue(industryName);
                row.CreateCell(5).SetCellValue(cc.RelationCate.CateName);
                row.CreateCell(6).SetCellValue(cc.Fax);
                row.CreateCell(7).SetCellValue(cc.Phone);
                row.CreateCell(8).SetCellValue(cc.Address);
                row.CreateCell(9).SetCellValue(cc.AddMember.NickName);
                row.CreateCell(10).SetCellValue(cc.AddTime.ToString("yyyy-MM-dd"));

                var customers = CustomerService.GetALL()
                    .Include(x => x.AddMember)
                    .Include(x => x.JobCate)
                    .Where(x => x.CompanyID == cc.ID);
                if (customers.Any())
                {
                    var childtitlerow = sheet.CreateRow(rowNumber++);
                    childtitlerow.CreateCell(1).SetCellValue("客户人员信息");
                    var childheadrow = sheet.CreateRow(rowNumber++);
                    childheadrow.CreateCell(1).SetCellValue("类型");
                    childheadrow.CreateCell(2).SetCellValue("姓名");
                    childheadrow.CreateCell(3).SetCellValue("职位");
                    childheadrow.CreateCell(4).SetCellValue("录入者");
                    childheadrow.CreateCell(5).SetCellValue("生日类型");
                    childheadrow.CreateCell(6).SetCellValue("生日");
                    childheadrow.CreateCell(7).SetCellValue("电话");
                    childheadrow.CreateCell(8).SetCellValue("手机");
                    childheadrow.CreateCell(9).SetCellValue("地址");
                    childheadrow.CreateCell(10).SetCellValue("QQ");
                    childheadrow.CreateCell(11).SetCellValue("爱好");
                    childheadrow.CreateCell(12).SetCellValue("邮箱");
                }
                foreach (var customer in customers)
                {
                    var childrow = sheet.CreateRow(rowNumber++);
                    childrow.CreateCell(1).SetCellValue(customer.JobCate.CateName);
                    childrow.CreateCell(2).SetCellValue(customer.Name);
                    childrow.CreateCell(3).SetCellValue(customer.Jobs);
                    childrow.CreateCell(4).SetCellValue(customer.AddMember.NickName);
                    childrow.CreateCell(5).SetCellValue(customer.IsLeap ? "农历" : "阳历");
                    childrow.CreateCell(6).SetCellValue(customer.IsLeap
                    ? (Utilities.ConvertToChineseYearStyle(customer.BirthDay.Year) + customer.BirthDay1)
                    : customer.BirthDay.ToString("yyyy-MM-dd"));
                    childrow.CreateCell(7).SetCellValue(customer.Phone);
                    childrow.CreateCell(8).SetCellValue(customer.Mobile);
                    childrow.CreateCell(9).SetCellValue(customer.Address);
                    childrow.CreateCell(10).SetCellValue(customer.QQ);
                    childrow.CreateCell(11).SetCellValue(customer.Favorite);
                    childrow.CreateCell(12).SetCellValue(customer.Email);
                }

                sheet.CreateRow(rowNumber++);
            }

            //Write the workbook to a memory stream
            MemoryStream output = new MemoryStream();

            workbook.Write(output);

            //Return the result to the end user

            return File(output.ToArray(),   //The binary data of the XLS file
                "application/vnd.ms-excel", //MIME type of Excel files
                "客户信息.xls");     //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        public ActionResult My()
        {
            var model = CustomerCompanyService.GetKendoALL().Include(x => x.AddMember)
               .Where(x => x.AddUser == CookieHelper.MemberID && x.Status > (int)CustomerCompanyStatus.Delete
               ).Select(x => new CustomerCompanyItemViewModel()
               {
                   Address = x.Address,
                   AddTime = x.AddTime,
                   BrandName = x.BrandName,
                   Fax = x.Fax,
                   ID = x.ID,
                   MemberID = x.AddUser,
                   Name = x.Name,
                   Phone = x.Phone,
                   UserName = x.AddMember.NickName

               }).OrderByDescending(x => x.AddTime).ToList();

            var gxmodels = CustomerShareService.GetALL()
             .Include(x => x.CustomerCompany)
             .Include(x => x.CustomerCompany.AddMember)
             .Where(x => x.MemberID == CookieHelper.MemberID)
             .Select(x => new CustomerCompanyItemViewModel()
             {
                 Address = x.CustomerCompany.Address,
                 AddTime = x.CustomerCompany.AddTime,
                 BrandName = x.CustomerCompany.BrandName,
                 Fax = x.CustomerCompany.Fax,
                 ID = x.CustomerCompany.ID,
                 MemberID = x.CustomerCompany.AddUser,
                 Name = x.CustomerCompany.Name,
                 Phone = x.CustomerCompany.Phone,
                 UserName = x.CustomerCompany.AddMember.NickName
             });

            model.AddRange(gxmodels);
            model = model.Distinct(x => x.ID).ToList();

            return View(model);
        }

        public ActionResult Data_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = CustomerCompanyService.GetKendoALL()
                        .Where(x => x.AddUser == CookieHelper.MemberID && x.Status > (int)CustomerCompanyStatus.Delete);
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult DataDelete_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = CustomerCompanyService.GetKendoALL()
                        .Where(x => x.AddUser == CookieHelper.MemberID && x.Status == (int)CustomerCompanyStatus.Delete);
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult DataShare_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = CustomerShareService.GetALL()
                .Include(x => x.CustomerCompany)
                .Where(x => x.MemberID == CookieHelper.MemberID && x.CustomerCompany.Status > (int)CustomerCompanyStatus.Delete)
                .Select(x => x.CustomerCompany);
            return Json(model.ToDataSourceResult(request));
        }

        public ActionResult DataCommon_Read([DataSourceRequest] DataSourceRequest request)
        {
            var model = CustomerCompanyService.GetKendoALL()
                       .Where(x => x.IsCommon == true && x.Status > (int)CustomerCompanyStatus.Delete);
            return Json(model.ToDataSourceResult(request));
        }

        public void SetCommonData()
        {
            ViewBag.RelationID = Utilities.GetSelectListData(RelationCateService.GetALL()
              , x => x.ID,
              x => x.CateName,
              true);
            ViewBag.CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL()
             , x => x.ID,
             x => x.CateName,
             true);
        }
        #endregion



        #region createEdit

        public ActionResult Create()
        {
            var model = new CustomerCompanyViewModel();
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, true);
            ViewBag.Data_CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerCompanyViewModel model)
        {
            ServiceResult result = new ServiceResult();

            TempData["Service_Result"] = result;
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    CustomerCompanyService.Create(model);
                    result.Message = "添加客户成功！";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = "添加客户失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加客户失败!", ex);
                }

            }
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, true);
            ViewBag.Data_CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }


        public ActionResult Edit(int ID)
        {
            var entity = CustomerCompanyService.Find(ID);
            var model = new CustomerCompanyViewModel()
            {
                ID = entity.ID,
                Name = entity.Name,
                Phone = entity.Phone,
                IndustryCode = entity.IndustryValue,
                CustomerCateID = entity.CustomerCateID,
                RelationID = entity.RelationID,
                Fax = entity.Fax,
                Description = entity.Description,
                CityCode = entity.CityValue,
                BrandName = entity.BrandName,
                Address = entity.Address
            };
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, model.RelationID, true);
            ViewBag.Data_CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL(), x => x.ID, x => x.CateName, model.CustomerCateID, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerCompanyViewModel model)
        {
            ServiceResult result = new ServiceResult();

            TempData["Service_Result"] = result;
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    CustomerCompanyService.Update(model);
                    //标记为已合作客户
                    if (model.CustomerCateID == 5)
                    {
                        var contact = new ContactRequireViewModel()
                        {
                            CompanyID = model.ID,
                            //财务部
                            DepartmentID = 8,
                            Name = model.Name + "财务合同请求",
                            Description = model.Name + "财务合同请求",
                            SenderID = CookieHelper.MemberID,
                            IsRoot = 1
                        };
                        ContactRequireService.Create(contact);
                    }
                    result.Message = "编辑客户成功！";
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    result.Message = "编辑客户失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑客户失败!", ex);
                }
            }
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, model.RelationID, true);
            ViewBag.Data_CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL(), x => x.ID, x => x.CateName, model.CustomerCateID, true);
            return View(model);
        }
        #endregion

        public ActionResult isEditable(int ID)
        {
            var entity = CustomerCompanyService.Find(ID);
            return Json((DateTime.Now - entity.AddTime).Days < 15, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetDelete(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                CustomerCompanyService.ChangeStatus(ids, CustomerCompanyStatus.Delete);
                result.Message = "客户信息删除成功！";
            }
            catch (Exception ex)
            {
                result.Message = "客户信息删除失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "客户信息删除失败!", ex);
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult SetRefresh(string ids)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                CustomerCompanyService.ChangeStatus(ids, CustomerCompanyStatus.Default);
                result.Message = "客户信息恢复成功！";
            }
            catch (Exception ex)
            {
                result.Message = "客户信息恢复失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "客户信息恢复失败!", ex);
            }
            return Json(result);
        }

        #region details
        public ActionResult Details(int ID)
        {
            var entity = CustomerCompanyService.Find(ID);
            var hasPermission = entity.AddUser == CookieHelper.MemberID
                || PermissionsService.CheckPermission("boss", "controller", CookieHelper.MemberID);
            ViewBag.hasPermission = hasPermission;
            var model = new CustomerCompanyViewModel()
            {
                Address = entity.Address,
                BrandName = entity.BrandName,
                CityCode = entity.CityValue,
                CustomerCateID = entity.CustomerCateID,
                Description = entity.Description,
                Fax = entity.Fax,
                IndustryCode = entity.IndustryValue,
                Name = entity.Name,
                RelationID = entity.RelationID,
                Phone = entity.Phone,
                ID = entity.ID,
                MemberID = entity.AddUser
            };
            var cityIds = entity.CityValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var cityValues = CityCateService.GetALL().Where(x => cityIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            ViewBag.Data_CityCode = cityValues;

            var industryIds = entity.IndustryValue.Split(',').Select(x => Convert.ToInt32(x)).ToList();
            var industryValues = IndustryCateService.GetALL().Where(x => industryIds.Contains(x.ID)).Select(x => x.CateName).ToList();
            ViewBag.Data_IndustryCode = industryValues;
            return View(model);

        }
        #endregion


        public ActionResult Search(SearchCompanyViewModel model, int page = 1)
        {
            const int pageSize = 20;

            var query = GetSearch(model);

            var totalCount = query.Count();

            var data = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            return PartialView(data);
        }

        private IQueryable<CustomerCompany> GetSearch(SearchCompanyViewModel model)
        {
            var query = CustomerCompanyService.GetALL()
            .Include(x => x.AddMember)
            .Include(x => x.CustomerCate)
            .Include(x => x.Customer);

            if (model.ID != 0)
            {
                query = query.Where(x => x.ID == model.ID);
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                query = query.Where(x => x.Name.Contains(model.Name));
            }
            if (!string.IsNullOrEmpty(model.BrandName))
            {
                query = query.Where(x => x.BrandName.Contains(model.BrandName));
            }
            if (!string.IsNullOrEmpty(model.Customer))
            {
                query = query.Where(x => x.Customer.Any(c => c.Name.Contains(model.Customer)));
            }
            if (!string.IsNullOrEmpty(model.UserName))
            {
                query = query.Where(x => x.AddMember.NickName.Contains(model.UserName));
            }
            if (model.CustomerCateID != 0)
            {
                query = query.Where(x => x.CustomerCateID == model.CustomerCateID);
            }
            if (!string.IsNullOrEmpty(model.Mobile))
            {
                query = query.Where(x =>
                    x.Customer.Any(c =>
                        c.Mobile.Contains(model.Mobile)
                        || c.Mobile1.Contains(model.Mobile)));
            }

            if (!string.IsNullOrEmpty(model.Phone))
            {
                query = query.Where(x =>
                   x.Customer.Any(c =>
                       c.Phone.Contains(model.Phone)) || x.Phone.Contains(model.Phone));

            }

            if (!string.IsNullOrEmpty(model.QQ))
            {
                query = query.Where(x => x.Customer.Any(c => c.QQ.Contains(model.QQ)));
            }

            if (!string.IsNullOrEmpty(model.Address))
            {
                query = query.Where(x =>
                    x.Customer.Any(c =>
                        c.Address.Contains(model.Address))
                        || x.Address.Contains(model.Address));
            }
            if (!string.IsNullOrEmpty(model.Fax))
            {
                query = query.Where(x => x.Fax.Contains(model.Fax));
            }

            query = query.Where(x => x.AddTime < model.EndTime
                && x.AddTime > model.StartTime
                && x.Status > (int)CustomerCompanyStatus.Delete
                && x.AddUser == CookieHelper.MemberID).OrderByDescending(x => x.AddTime);
            return query;
        }

    }
}
