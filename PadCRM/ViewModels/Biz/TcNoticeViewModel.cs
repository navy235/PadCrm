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
    public class TcNoticeViewModel
    {

        public TcNoticeViewModel()
        {

        }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "请填写公告标题")]
        [Display(Name = "公告标题")]
        public string Name { get; set; }

        [Required(ErrorMessage = "请填写提成标准")]
        [Display(Name = "提成标准")]
        [UIHint("RichEditor")]
        public string Content { get; set; }

        [Required(ErrorMessage = "请选择事业部")]
        [Display(Name = "事业部")]
        [UIHint("CheckList")]
        public string DepartmentID { get; set; }

        [Display(Name = "相关附件")]
        [UIHint("UploadFile")]
        public string AttachmentPath { get; set; }


    }
}