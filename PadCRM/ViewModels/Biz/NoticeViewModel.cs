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
    public class NoticeViewModel
    {

        public NoticeViewModel()
        {
         
        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请填写公告标题")]
        [Display(Name = "公告标题")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请填写所属部门")]
        [Display(Name = "所属部门")]
        [UIHint("DropDownList")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "请填写公告内容")]
        [Display(Name = "公告内容")]
        [UIHint("RichEditor")]
        public string Content { get; set; }

    }
}