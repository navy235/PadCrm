using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class CustomerShareItemViewModel
    {
        public int ID { get; set; }

        public int MemberID { get; set; }

        public string UserName { get; set; }

        public int CompanyID { get; set; }

        public int DepartmentID { get; set; }

        public DateTime AddTime { get; set; }
    }
}