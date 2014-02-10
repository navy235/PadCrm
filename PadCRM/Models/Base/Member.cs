namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Member
    {
        public Member()
        {
            this.Member_Action = new HashSet<Member_Action>();
            this.CustomerCompany = new HashSet<CustomerCompany>();
            this.TraceLog = new HashSet<TraceLog>();
            this.Customer = new HashSet<Customer>();
            this.PlanLog = new HashSet<PlanLog>();
            this.CustomerShare = new HashSet<CustomerShare>();
            this.Punish = new HashSet<Punish>();
            this.Task = new HashSet<Task>();
            this.FileShare = new HashSet<FileShare>();
            this.MemberID = 100000;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "员工ID")]
        public int MemberID { get; set; }

        [Display(Name = "用户类型")]
        public int MemberType { get; set; }

        [MaxLength(50)]
        [Display(Name = "手机1")]
        public string Mobile { get; set; }

        [MaxLength(50)]
        [Display(Name = "手机2")]
        public string Mobile1 { get; set; }

        [MaxLength(50)]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [MaxLength(50)]
        [Display(Name = "姓名")]
        public string NickName { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(500)]
        public string AvtarUrl { get; set; }

        [Display(Name = "群组")]
        public int GroupID { get; set; }

        [Display(Name = "部门")]
        public int DepartmentID { get; set; }

        [Display(Name = "职称")]
        public int JobTitleID { get; set; }

        [Display(Name = "负责人")]
        public bool IsLeader { get; set; }

        public System.DateTime LastTime { get; set; }

        [MaxLength(50)]
        public string LastIP { get; set; }

        [Display(Name = "创建时间")]
        public System.DateTime AddTime { get; set; }

        public int OpenType { get; set; }

        [MaxLength(100)]
        public string OpenID { get; set; }

        [MaxLength(50)]
        public string AddIP { get; set; }

        public int LoginCount { get; set; }

        public int Status { get; set; }

        [Display(Name = "性别")]
        public bool Sex { get; set; }

        [Display(Name = "生日类型")]
        public bool IsLeap { get; set; }

        [Display(Name = "阳历生日")]
        public System.DateTime BirthDay { get; set; }

        [Display(Name = "农历生日")]
        public string BirthDay1 { get; set; }

        public int CityCode { get; set; }

        public string CityCodeValue { get; set; }

        [MaxLength(20)]
        public string RealName { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(20)]
        public string QQ { get; set; }

        [MaxLength(50)]
        public string MSN { get; set; }

        [Display(Name = "地址")]
        [MaxLength(50)]
        public string Address { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public string IDNumber { get; set; }

        public string JobExp { get; set; }

        public string StudyExp { get; set; }

        public string FamilySituation { get; set; }

        public virtual Group Group { get; set; }

        public virtual ICollection<Member_Action> Member_Action { get; set; }

        public virtual ICollection<CustomerCompany> CustomerCompany { get; set; }

        public virtual ICollection<PlanLog> PlanLog { get; set; }

        public virtual ICollection<TraceLog> TraceLog { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }

        public virtual ICollection<CustomerShare> CustomerShare { get; set; }

        public virtual ICollection<Punish> Punish { get; set; }

        public virtual ICollection<Task> Task { get; set; }

        public virtual ICollection<FileShare> FileShare { get; set; }

        public virtual Department Department { get; set; }

        public virtual JobTitleCate JobTitleCate { get; set; }

    }
}