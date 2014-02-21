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
    public class PunishSearchViewModel
    {

        public PunishSearchViewModel()
        {
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now.AddDays(1);
        }

        [Display(Name = "正负分")]
        [UIHint("DropDownList")]
        public int Reward { get; set; }

        [Display(Name = "奖惩规则")]
        [UIHint("DropDownList")]
        public int RuleID { get; set; }

        [Display(Name = "之后录入")]
        [UIHint("Date")]
        public DateTime StartTime { get; set; }

        [Display(Name = "之前录入")]
        [UIHint("Date")]
        public DateTime EndTime { get; set; }
    }
}