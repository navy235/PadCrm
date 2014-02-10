namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class CustomerShare
    {

        public int ID { get; set; }

        public int MemberID { get; set; }

        public int AddUser { get; set; }

        public DateTime AddTime { get; set; }

        public int CompanyID { get; set; }

        public int Status { get; set; }

        public virtual CustomerCompany CustomerCompany { get; set; }

        public virtual Member Member { get; set; }

    }
}