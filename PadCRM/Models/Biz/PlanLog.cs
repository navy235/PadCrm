namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PlanLog
    {
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime PlanTime { get; set; }

        public string Content { get; set; }

        public int AddUser { get; set; }


        public Member AddMember { get; set; }

        public virtual CustomerCompany CustomerCompany { get; set; }
    }
}