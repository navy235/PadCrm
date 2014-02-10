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
    public class CustomerViewModel
    {

        public CustomerViewModel()
        {
            this.BirthDay = DateTime.Now.AddYears(-30);
        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "请填写人员姓名")]
        [Display(Name = "人员姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请选择职业类型")]
        [Display(Name = "职业类型")]
        [UIHint("DropDownList")]
        public int JobID { get; set; }

        [Display(Name = "手机1")]
        public string Mobile { get; set; }

        [Display(Name = "手机2")]
        public string Mobile1 { get; set; }

        [Display(Name = "电话")]
        public string Phone { get; set; }

        [Display(Name = "生日类型")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "阳历,阴历")]
        public bool IsLeap { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "生日")]
        [UIHint("Date")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [Display(Name = "职位")]
        public string Jobs { get; set; }

        [Display(Name = "邮箱")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        public string Email { get; set; }

        [Display(Name = "爱好")]
        public string Favorite { get; set; }

        [Display(Name = "地址")]
        public string Address { get; set; }

        [Display(Name = "备注")]
        [DataType(DataType.MultilineText)]
        public string ReMark { get; set; }
    }
}