namespace PadCRM.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ContractInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(50)]
        public string Key { get; set; }

        public int ContractCateID { get; set; }

        public int CompanyID { get; set; }

        public int SenderID { get; set; }

        [MaxLength(500)]
        public string AttachmentPath { get; set; }

        public DateTime SigningTime { get; set; }

        public DateTime PlayTime { get; set; }

        public DateTime ExpiryTime { get; set; }

        public DateTime SubscribeTime { get; set; }


        public decimal Price { get; set; }

        public int SignerID { get; set; }

        [MaxLength(50)]
        public string FinancePhone { get; set; }

        [MaxLength(50)]
        public string FinanceFax { get; set; }

        [MaxLength(150)]
        public string Payment { get; set; }

        public DateTime NextTime { get; set; }

        public DateTime AddTime { get; set; }

        public int AddUser { get; set; }

        public DateTime LastTime { get; set; }

        public int LastUser { get; set; }

        public virtual Member Signer { get; set; }

        public virtual ContractCate ContractCate { get; set; }

    }
}