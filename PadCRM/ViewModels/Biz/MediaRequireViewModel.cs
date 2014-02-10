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
    public class MediaRequireViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SenderID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }

        [Required(ErrorMessage = "请选择媒介人员")]
        [Display(Name = "媒介人员")]
        [UIHint("RadioUser")]
        public int ResolveID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IsRoot { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int PID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DepartmentID { get; set; }

        [Display(Name = "上传文件")]
        [UIHint("UploadFile")]
        public string AttachmentPath { get; set; }

        [Display(Name = "内容描述")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }


    public class MediaRequireAppendViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SenderID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int ResolveID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int IsRoot { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Status { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int PID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int DepartmentID { get; set; }

        [Display(Name = "上传文件")]
        [UIHint("UploadFile")]
        public string AttachmentPath { get; set; }

        [Display(Name = "内容描述")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}