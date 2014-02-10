﻿using System;
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
    public class TraceLogViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "请选择关系程度")]
        [Display(Name = "关系程度")]
        [UIHint("DropDownList")]
        public int RelationID { get; set; }

        [Required(ErrorMessage = "请填写内容")]
        [Display(Name = "内容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }



    }
}