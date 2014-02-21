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
    public class ImportViewModel
    {

        [Display(Name = "选择文件")]
        [Required(ErrorMessage = "请选择要导入的数据文件")]
        [UIHint("UploadFile")]
        public string FilePath { get; set; }
    }
}