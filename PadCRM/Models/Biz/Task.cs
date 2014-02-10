namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Task
    {
        public int ID { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public int MemberID { get; set; }

        public string StartTimeZone { get; set; }

        public string EndTimeZone { get; set; }

        public int AddUser { get; set; }

        public DateTime AddTime { get; set; }

        public bool IsOtherAdd { get; set; }

        public virtual Member Member { get; set; }

    }
}