using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadCRM.ViewModels
{
    public class DepartmentViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入部门名称")]
        [Display(Name = "部门名称")]
        [StringCheckLength(4, 25)]
        public string Name { get; set; }

        [Display(Name = "父级部门")]
        [UIHint("DropDownList")]
        public int? PID { get; set; }

        [Required(ErrorMessage = "请输入部门代码")]
        [Display(Name = "部门代码")]
        [UIHint("Integer")]
        public int Code { get; set; }

        [Display(Name = "部门级别")]
        [Required(ErrorMessage = "请输入部门级别")]
        [UIHint("Integer")]
        public int Level { get; set; }


        [Required(ErrorMessage = "请输入部门描述")]
        [Display(Name = "部门描述")]
        [StringCheckLength(4, 75)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "请输入部门领导")]
        [Display(Name = "部门领导")]
        [StringCheckLength(4, 25)]
        public string Leader { get; set; }


    }
}