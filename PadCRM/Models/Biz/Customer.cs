namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Customer
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int JobID { get; set; }

        public int CompanyID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(50)]
        public string Mobile1 { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        public bool IsLeap { get; set; }

        public DateTime BirthDay { get; set; }

        public string BirthDay1 { get; set; }

        [MaxLength(50)]
        public string QQ { get; set; }

        [MaxLength(50)]
        public string Jobs { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Favorite { get; set; }

        [MaxLength(50)]
        public string Address { get; set; }

        [MaxLength(50)]
        public string ReMark { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime LastTime { get; set; }

        public int Status { get; set; }

        public int AddUser { get; set; }

        public int LastUser { get; set; }

        public virtual JobCate JobCate { get; set; }

        public Member AddMember { get; set; }

        public virtual CustomerCompany CustomerCompany { get; set; }

    }
}