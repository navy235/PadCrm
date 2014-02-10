namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ContactRequire
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int CompanyID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int SenderID { get; set; }

        public int ResolveID { get; set; }

        [MaxLength(500)]
        public string AttachmentPath { get; set; }

        public int DepartmentID { get; set; }

        public int AddUser { get; set; }

        public int IsRoot { get; set; }

        public int Status { get; set; }

        public int PID { get; set; }

        public DateTime AddTime { get; set; }

        public virtual Department Department { get; set; }
    }
}