
namespace PadCRM.Models
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Group
    {
        public Group()
        {
            this.Member = new HashSet<Member>();
            this.Roles = new HashSet<Roles>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "群组名称")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Display(Name = "群组描述")]
        public string Description { get; set; }

        [Display(Name = "职责描述")]
        public string Content { get; set; }


        public virtual ICollection<Member> Member { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
    }
}