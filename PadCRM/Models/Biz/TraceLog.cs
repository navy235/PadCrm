
namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class TraceLog
    {
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public DateTime AddTime { get; set; }

        public string Content { get; set; }

        public int AddUser { get; set; }

        public int RelationID { get; set; }

        public virtual RelationCate RelationCate { get; set; }

        public Member AddMember { get; set; }

        public virtual CustomerCompany CustomerCompany { get; set; }
    }
}