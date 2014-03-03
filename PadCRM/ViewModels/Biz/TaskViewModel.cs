namespace PadCRM.ViewModels
{
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

    public class TaskViewModel
    {

        [HiddenInput(DisplayValue = false)]
        public int TaskID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        //[Required(ErrorMessage = "请输入任务名称")]
        //[Display(Name = "任务名称")]
        //public string Title { get; set; }

        [Required(ErrorMessage = "请输入任务详情")]
        [Display(Name = "任务详情")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "提醒时间")]
        [UIHint("DateTime")]
        public DateTime Start
        {
            get;
            set;
        }

        //[Display(Name = "结束时间")]
        //[UIHint("DateTime")]
        //public DateTime End
        //{
        //    get;
        //    set;
        //}
    }
}