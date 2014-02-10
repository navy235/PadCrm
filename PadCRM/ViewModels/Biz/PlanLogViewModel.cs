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
    public class PlanLogViewModel
    {

        public PlanLogViewModel()
        {
            this.PlanTime = DateTime.Now;
        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "请填写内容")]
        [Display(Name = "内容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }


        [DataType(DataType.DateTime)]
        [Display(Name = "预计时间")]
        [UIHint("Date")]
        public DateTime PlanTime { get; set; }
    }

    public class PlanLogEditViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }


        [Required(ErrorMessage = "请填写内容")]
        [Display(Name = "内容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

    }
}