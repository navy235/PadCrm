using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using System.Globalization;
using System.Web.Mvc;
using Maitonn.Core;


namespace PadCRM.ViewModels
{
    public class MemberViewModel
    {

        public MemberViewModel()
        {
            BirthDay = DateTime.Now.AddYears(-25);
        }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请输入姓名")]
        [Display(Name = "姓名")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "昵称含有非法字符.")]
        public string NickName { get; set; }


        [Required(ErrorMessage = "请选择性别")]
        [Display(Name = "性别")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "男,女")]
        public bool Sex { get; set; }

        [Display(Name = "住址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "请输入手机号码1")]
        [Display(Name = "手机号码")]
        public string Mobile { get; set; }

        [Display(Name = "手机号码2")]
        public string Mobile1 { get; set; }

        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [Required(ErrorMessage = "请输入电子邮箱")]
        [Display(Name = "电子邮箱")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请设置用户所属群组")]
        [Display(Name = "用户群组")]
        [UIHint("DropDownList")]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "请设置用户所属部门")]
        [Display(Name = "所在部门")]
        [UIHint("DropDownList")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "请设置用户职称")]
        [Display(Name = "用户职称")]
        [UIHint("DropDownList")]
        public int JobTitleID { get; set; }

        [Display(Name = "负责人")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "否,是")]
        public bool IsLeader { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(15, ErrorMessage = "请输入{2}-{1}位密码", MinimumLength = 6)]
        [Display(Name = "设定密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "请确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }


        [Display(Name = "生日类型")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "阳历,阴历")]
        public bool IsLeap { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "出生日期")]
        [UIHint("Date")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "上传头像")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string AvtarUrl { get; set; }

        [Display(Name = "身份证")]
        public string IDNumber { get; set; }

        [Display(Name = "工作经验")]
        [DataType(DataType.MultilineText)]
        public string JobExp { get; set; }

        [Display(Name = "学习经验")]
        [DataType(DataType.MultilineText)]
        public string StudyExp { get; set; }

        [Display(Name = "家庭情况")]
        [DataType(DataType.MultilineText)]
        public string FamilySituation { get; set; }

        [Display(Name = "补充说明")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }


    public class MemberEditViewModel
    {

        public MemberEditViewModel()
        {
            BirthDay = DateTime.Now.AddYears(-25);
        }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请输入姓名")]
        [Display(Name = "姓名")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "姓名含有非法字符.")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "请选择性别")]
        [Display(Name = "性别")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "男,女")]
        public bool Sex { get; set; }

        [Display(Name = "住址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "请输入手机号码1")]
        [Display(Name = "手机号码")]
        public string Mobile { get; set; }

        [Display(Name = "手机号码2")]
        public string Mobile1 { get; set; }

        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [Required(ErrorMessage = "请输入电子邮箱")]
        [Display(Name = "电子邮箱")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请设置用户所属群组")]
        [Display(Name = "用户群组")]
        [UIHint("DropDownList")]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "请设置用户所在部门")]
        [Display(Name = "所在部门")]
        [UIHint("DropDownList")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "请设置用户职称")]
        [Display(Name = "用户职称")]
        [UIHint("DropDownList")]
        public int JobTitleID { get; set; }

        [Display(Name = "负责人")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "否,是")]
        public bool IsLeader { get; set; }

        [Display(Name = "生日类型")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "阳历,阴历")]
        public bool IsLeap { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "出生日期")]
        [UIHint("Date")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "上传头像")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string AvtarUrl { get; set; }

        [Display(Name = "身份证")]
        public string IDNumber { get; set; }

        [Display(Name = "工作经验")]
        [DataType(DataType.MultilineText)]
        public string JobExp { get; set; }

        [Display(Name = "学习经验")]
        [DataType(DataType.MultilineText)]
        public string StudyExp { get; set; }

        [Display(Name = "家庭情况")]
        [DataType(DataType.MultilineText)]
        public string FamilySituation { get; set; }

        [Display(Name = "补充说明")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }


    public class MemberClerkViewModel
    {

        public MemberClerkViewModel()
        {
            BirthDay = DateTime.Now.AddYears(-25);
        }

        [HiddenInput(DisplayValue = false)]
        public int MemberID { get; set; }

        [Required(ErrorMessage = "请输入姓名")]
        [Display(Name = "姓名")]
        [RegularExpression(@"^[\u4e00-\u9fa5|A-Za-z|0-9|_]+$", ErrorMessage = "昵称含有非法字符.")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "请选择性别")]
        [Display(Name = "性别")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "男,女")]
        public bool Sex { get; set; }

        [Display(Name = "住址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "请输入手机号码1")]
        [Display(Name = "手机号码")]
        public string Mobile { get; set; }

        [Display(Name = "手机号码2")]
        public string Mobile1 { get; set; }

        [Display(Name = "QQ")]
        public string QQ { get; set; }

        [Required(ErrorMessage = "请输入电子邮箱")]
        [Display(Name = "电子邮箱")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "输入的电子邮箱格式不正确.")]
        public string Email { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int GroupID { get; set; }

        [Required(ErrorMessage = "请设置用户所属群组")]
        [Display(Name = "所在部门")]
        [UIHint("DropDownList")]
        public int DepartmentID { get; set; }

        [Display(Name = "负责人")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "否,是")]
        public bool IsLeader { get; set; }

        [Required(ErrorMessage = "请输入密码")]
        [StringLength(15, ErrorMessage = "请输入{2}-{1}位密码", MinimumLength = 6)]
        [Display(Name = "设定密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "请确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "两次输入的密码不一致")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "生日类型")]
        [UIHint("RadioList")]
        [AdditionalMetadata("RadioList", "阳历,阴历")]
        public bool IsLeap { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "出生日期")]
        [UIHint("Date")]
        public DateTime BirthDay { get; set; }

        [Display(Name = "上传头像")]
        [UIHint("UploadImgEdit")]
        [AdditionalMetadata("UploadImgEdit", "200|200")]
        [AdditionalMetadata("mustUpload", false)]
        public string AvtarUrl { get; set; }

        [Display(Name = "身份证")]
        public string IDNumber { get; set; }

        [Display(Name = "工作经验")]
        [DataType(DataType.MultilineText)]
        public string JobExp { get; set; }

        [Display(Name = "学习经验")]
        [DataType(DataType.MultilineText)]
        public string StudyExp { get; set; }

        [Display(Name = "家庭情况")]
        [DataType(DataType.MultilineText)]
        public string FamilySituation { get; set; }

        [Display(Name = "补充说明")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}