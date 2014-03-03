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
    public class FireShareSearchViewModel
    {

        [Display(Name = "文件名称")]
        public string Name { get; set; }

        [Display(Name = "所属部门")]
        [UIHint("DropDownList")]
        public int DepartmentID { get; set; }
    }
}