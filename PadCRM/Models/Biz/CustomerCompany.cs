namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class CustomerCompany
    {

        public CustomerCompany()
        {
            this.Customer = new HashSet<Customer>();
            this.TraceLog = new HashSet<TraceLog>();
            this.PlanLog = new HashSet<PlanLog>();
            this.CustomerShare = new HashSet<CustomerShare>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string BrandName { get; set; }

        public int CityID { get; set; }


        [MaxLength(50)]
        public string CityValue { get; set; }

        public int IndustryID { get; set; }

        [MaxLength(50)]
        public string IndustryValue { get; set; }

        public int CustomerCateID { get; set; }

        public int RelationID { get; set; }

        [MaxLength(50)]
        public string Fax { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(50)]
        public string Address { get; set; }

        public string Description { get; set; }

        public bool IsCommon { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime LastTime { get; set; }

        public int Status { get; set; }

        public int AddUser { get; set; }

        public int LastUser { get; set; }

        [MaxLength(50)]
        public string Finance { get; set; }

        [MaxLength(50)]
        public string FinancePhone { get; set; }

        [MaxLength(50)]
        public string ProxyName { get; set; }

        [MaxLength(50)]
        public string ProxyAddress { get; set; }

        [MaxLength(50)]
        public string ProxyPhone { get; set; }

        public virtual Member AddMember { get; set; }

        public virtual IndustryCate IndustryCate { get; set; }

        public virtual CityCate CityCate { get; set; }

        public virtual RelationCate RelationCate { get; set; }

        public virtual CustomerCate CustomerCate { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }

        public virtual ICollection<PlanLog> PlanLog { get; set; }

        public virtual ICollection<TraceLog> TraceLog { get; set; }

        public virtual ICollection<CustomerShare> CustomerShare { get; set; }

    }
}