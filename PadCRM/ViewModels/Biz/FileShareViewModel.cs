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
    public class FileShareViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请输入文件名称")]
        [Display(Name = "文件名称")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请上传文件")]
        [Display(Name = "上传文件")]
        [UIHint("UploadFile")]
        public string FilePath { get; set; }

        [Required(ErrorMessage = "请选择文件类型")]
        [Display(Name = "文件类型")]
        [UIHint("DropDownList")]
        public int FileCateID { get; set; }


        [Display(Name = "所属部门")]
        [UIHint("DropDownList")]
        [HintLabel("不选择默认全公司")]
        public int DepartmentID { get; set; }



        [Display(Name = "文件描述")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}