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
    public class BossController : Controller
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
        public BossController(
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
        }

        public ActionResult Index()
        {
            SearchCompanyViewModel model = new SearchCompanyViewModel();
            ViewBag.Data_CustomerCateID = Utilities.GetSelectListData(CustomerCateService.GetALL(),
                x => x.ID,
                x => x.CateName,
                true, true);
            return View(model);
        }

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
                && x.Status > (int)CustomerCompanyStatus.Delete).OrderByDescending(x => x.AddTime);
            return query;
        }

        public ActionResult ExportAll(SearchCompanyViewModel model, int page = 1)
        {
            var query = GetSearch(model).ToList();

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



            //var shares = CustomerShareService.GetALL()
            //    .Include(x => x.CustomerCompany)
            //    .Where(x => x.MemberID == CookieHelper.MemberID)
            //    .Select(x => x.CustomerCompany).ToList();
            //model.AddRange(shares);

            //Populate the sheet with values from the grid data
            foreach (CustomerCompany cc in query)
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

                var customers = cc.Customer;
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
                    childheadrow.CreateCell(8).SetCellValue("手机1");
                    childheadrow.CreateCell(9).SetCellValue("手机2");
                    childheadrow.CreateCell(10).SetCellValue("地址");
                    childheadrow.CreateCell(11).SetCellValue("QQ");
                    childheadrow.CreateCell(12).SetCellValue("爱好");
                    childheadrow.CreateCell(13).SetCellValue("邮箱");
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
                    childrow.CreateCell(9).SetCellValue(customer.Mobile1);
                    childrow.CreateCell(10).SetCellValue(customer.Address);
                    childrow.CreateCell(11).SetCellValue(customer.QQ);
                    childrow.CreateCell(12).SetCellValue(customer.Favorite);
                    childrow.CreateCell(13).SetCellValue(customer.Email);
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


        public ActionResult ExportCurrentPage(SearchCompanyViewModel model, int page = 1)
        {

            const int pageSize = 20;

            var query = GetSearch(model);

            var data = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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



            //var shares = CustomerShareService.GetALL()
            //    .Include(x => x.CustomerCompany)
            //    .Where(x => x.MemberID == CookieHelper.MemberID)
            //    .Select(x => x.CustomerCompany).ToList();
            //model.AddRange(shares);

            //Populate the sheet with values from the grid data
            foreach (CustomerCompany cc in data)
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

                var customers = cc.Customer;
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
                    childheadrow.CreateCell(8).SetCellValue("手机1");
                    childheadrow.CreateCell(9).SetCellValue("手机2");
                    childheadrow.CreateCell(10).SetCellValue("地址");
                    childheadrow.CreateCell(11).SetCellValue("QQ");
                    childheadrow.CreateCell(12).SetCellValue("爱好");
                    childheadrow.CreateCell(13).SetCellValue("邮箱");
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
                    childrow.CreateCell(9).SetCellValue(customer.Mobile1);
                    childrow.CreateCell(10).SetCellValue(customer.Address);
                    childrow.CreateCell(11).SetCellValue(customer.QQ);
                    childrow.CreateCell(12).SetCellValue(customer.Favorite);
                    childrow.CreateCell(13).SetCellValue(customer.Email);
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

    }
}
