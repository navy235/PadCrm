
namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Notice
    {
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public string Content { get; set; }

        public int DepartmentID { get; set; }

        public int AddUser { get; set; }

        public DateTime AddTime { get; set; }

        public int LastUser { get; set; }

        public DateTime LastTime { get; set; }

        public virtual Department Department { get; set; }
    }
}