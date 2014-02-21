using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using Maitonn.Core;


namespace PadCRM.ViewModels
{
    public class SearchCompanyViewModel
    {

        public SearchCompanyViewModel()
        {
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now.AddDays(1);
        }


        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Display(Name = "公司名称")]
        public string Name { get; set; }

        [Display(Name = "品牌名称")]
        public string BrandName { get; set; }

        [Display(Name = "录入者")]
        public string UserName { get; set; }

        [Display(Name = "客户人员")]
        public string Customer { get; set; }

        [Display(Name = "公司类型")]
        [UIHint("DropDownList")]
        public int CustomerCateID { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "手机")]
        public string Mobile { get; set; }

        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [Display(Name = "传真")]
        public string Fax { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "之后录入")]
        [UIHint("Date")]
        public DateTime StartTime { get; set; }

        [Display(Name = "之前录入")]
        [UIHint("Date")]
        public DateTime EndTime { get; set; }

    }


    public class PlanSearchViewModel
    {
        public PlanSearchViewModel()
        {
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now.AddDays(1);
        }

        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Display(Name = "之后录入")]
        [UIHint("Date")]
        public DateTime StartTime { get; set; }

        [Display(Name = "之前录入")]
        [UIHint("Date")]
        public DateTime EndTime { get; set; }
    }

    public class TraceSearchViewModel
    {

        public TraceSearchViewModel()
        {
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now.AddDays(1);
        }

        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Display(Name = "之后录入")]
        [UIHint("Date")]
        public DateTime StartTime { get; set; }

        [Display(Name = "之前录入")]
        [UIHint("Date")]
        public DateTime EndTime { get; set; }
    }
}