using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class CustomerCompanyItemViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public int MemberID { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public DateTime AddTime { get; set; }
    }
}