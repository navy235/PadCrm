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

        [Display(Name = "其他分数")]
        [UIHint("Integer")]
        [HintLabel("如果需要特定分数请填写这个")]
        public int Score { get; set; }

        [Display(Name = "奖惩描述")]
        [DataType(DataType.MultilineText)]
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

        [Display(Name = "当前分数")]
        [UIHint("Integer")]
        public int Score { get; set; }

        [Display(Name = "奖惩描述")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}