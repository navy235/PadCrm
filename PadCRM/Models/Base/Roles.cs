
namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Roles
    {
        public Roles()
        {
            this.Group = new HashSet<Group>();
            this.Permissions = new HashSet<Permissions>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        [Display(Name = "角色名称")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Display(Name = "角色描述")]
        public string Description { get; set; }


        public virtual ICollection<Group> Group { get; set; }


        public virtual ICollection<Permissions> Permissions { get; set; }
    }
}