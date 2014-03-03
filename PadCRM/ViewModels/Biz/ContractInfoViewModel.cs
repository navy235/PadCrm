using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.Mvc;
using Maitonn.Core;

namespace PadCRM.ViewModels
{
    public class ContractInfoViewModel
    {


        public ContractInfoViewModel()
        {
            this.SigningTime = DateTime.Now;
            this.PlayTime = DateTime.Now;
            this.ExpiryTime = DateTime.Now;
            this.SubscribeTime = DateTime.Now;
            this.NextTime = DateTime.Now.AddMonths(1);
        }


        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }


        [HiddenInput(DisplayValue = false)]
        public int RequireID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int CompanyID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SenderID { get; set; }

        [Required(ErrorMessage = "请选择合同类型")]
        [Display(Name = "合同类型")]
        [UIHint("DropDownList")]
        public int ContractCateID { get; set; }

        [Display(Name = "合同附件")]
        [UIHint("UploadFile")]
        public string AttachmentPath { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "签订时间")]
        [UIHint("Date")]
        public DateTime SigningTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "播放时间")]
        [UIHint("Date")]
        public DateTime PlayTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "到期时间")]
        [UIHint("Date")]
        public DateTime ExpiryTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "下单时间")]
        [UIHint("Date")]
        public DateTime SubscribeTime { get; set; }

        [Display(Name = "合同总额")]
        [UIHint("Price")]
        [AdditionalMetadata("Price", "0,1000")]
        [AdditionalMetadata("PriceUnit", "万元")]
        [HintClass("price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "请选择签订人")]
        [Display(Name = "签订人")]
        [UIHint("RadioUser")]
        public int SignerID { get; set; }

        [Display(Name = "财务电话")]
        public string FinancePhone { get; set; }

        [Display(Name = "财务传真")]
        public string FinanceFax { get; set; }

        [Display(Name = "收款方式")]
        public string Payment { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "下次收款")]
        [UIHint("Date")]
        public DateTime NextTime { get; set; }
    }
}