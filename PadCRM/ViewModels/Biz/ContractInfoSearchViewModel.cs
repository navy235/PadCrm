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
    public class ContractInfoSearchViewModel
    {

        public ContractInfoSearchViewModel()
        {
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now.AddDays(1);
        }


        [Display(Name = "合同编号")]
        public string Key { get; set; }


        [Display(Name = "合同类型")]
        [UIHint("DropDownList")]
        public int ContractCateID { get; set; }

        [Display(Name = "开始时间")]
        [UIHint("Date")]
        public DateTime StartTime { get; set; }

        [Display(Name = "结束时间")]
        [UIHint("Date")]
        public DateTime EndTime { get; set; }
    }

    public class ContractInfoReceivablesViewModel
    {

        public ContractInfoReceivablesViewModel()
        {
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now.AddDays(1);
        }

        [Display(Name = "合同编号")]
        public string Key { get; set; }


        [Display(Name = "开始时间")]
        [UIHint("Date")]
        public DateTime StartTime { get; set; }

        [Display(Name = "结束时间")]
        [UIHint("Date")]
        public DateTime EndTime { get; set; }
    }
}