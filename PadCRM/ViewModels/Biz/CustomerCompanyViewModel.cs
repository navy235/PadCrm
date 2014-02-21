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
    public class CustomerCompanyViewModel
    {

        public CustomerCompanyViewModel()
        {
            RelationID = 1;
        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int RelationID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请填写公司全称")]
        [Display(Name = "公司全称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请填写品牌名称")]
        [Display(Name = "品牌名称")]
        public string BrandName { get; set; }

        [Required(ErrorMessage = "请填写联系电话")]
        [Display(Name = "联系电话")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "请填写传真号码")]
        [Display(Name = "传真号码")]
        public string Fax { get; set; }

        [Required(ErrorMessage = "请填写公司地址")]
        [Display(Name = "公司地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "请选择行业类别")]
        [Display(Name = "行业类别")]
        [UIHint("Cascading")]
        public string IndustryCode { get; set; }

        [Required(ErrorMessage = "请选择所在城市")]
        [Display(Name = "所在城市")]
        [UIHint("Cascading")]
        public string CityCode { get; set; }



        [Required(ErrorMessage = "请选择客户类别")]
        [Display(Name = "客户分类")]
        [UIHint("DropDownList")]
        public int CustomerCateID { get; set; }

        [Display(Name = "代理公司")]
        public string ProxyName { get; set; }

        [Display(Name = "代理公司电话")]
        public string ProxyPhone { get; set; }

        [Display(Name = "代理公司地址")]
        public string ProxyAddress { get; set; }

        [Display(Name = "备注")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}