using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PadCRM.Models
{
    public class CustomerCate
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "类别名称")]
        public string CateName { get; set; }

        [Display(Name = "父级类别")]
        public int? PID { get; set; }

        [Display(Name = "类别代码")]
        public int Code { get; set; }

        [Display(Name = "类别级别")]
        public int Level { get; set; }

        [Display(Name = "排序代码")]
        public int OrderIndex { get; set; }

        [ScriptIgnore]
        public virtual CustomerCate PCate { get; set; }

        [ScriptIgnore]
        public virtual ICollection<CustomerCate> ChildCates { get; set; }

        public virtual ICollection<CustomerCompany> CustomerCompany { get; set; }

    }
}