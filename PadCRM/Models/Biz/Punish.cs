namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Punish
    {
        public int ID { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public int MemberID { get; set; }

        public int RuleID { get; set; }

        public int Score { get; set; }

        public int AddUser { get; set; }

        public DateTime AddTime { get; set; }

        public int LastUser { get; set; }

        public DateTime LastTime { get; set; }

        public virtual Member Member { get; set; }

        public virtual RuleCate RuleCate { get; set; }
    }
}