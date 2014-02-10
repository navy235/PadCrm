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
    public class PunishViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请选择人员")]
        [Display(Name = "人员")]
        [UIHint("RadioUser")]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请选择奖惩规则")]
        [Display(Name = "奖惩规则")]
        [UIHint("DropDownList")]
        public int RuleID { get; set; }

        [Required(ErrorMessage = "请填写奖惩分数")]
        [Display(Name = "奖惩分数")]
        [UIHint("Integer")]
        public int Score { get; set; }

        [Display(Name = "奖惩描述")]
        public string Description { get; set; }

    }

    public class PunishEditViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Display(Name = "奖惩人员")]
        [HiddenInput(DisplayValue = true)]
        public string UserName { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请选择奖惩规则")]
        [Display(Name = "奖惩规则")]
        [UIHint("DropDownList")]
        public int RuleID { get; set; }


        [Required(ErrorMessage = "请填写奖惩分数")]
        [Display(Name = "奖惩分数")]
        [UIHint("Integer")]
        public int Score { get; set; }

        [Display(Name = "奖惩描述")]
        public string Description { get; set; }

    }
}