using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class TraceLogGroupViewModel
    {

        public TraceLogGroupViewModel()
        {
            this.TraceLogs = new List<TraceLogItemViewModel>();
        }

        public int ID { get; set; }

        public int MemberID { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public DateTime AddTime { get; set; }

        public List<TraceLogItemViewModel> TraceLogs { get; set; }
    }


    public class TraceLogItemViewModel
    {
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime AddTime { get; set; }
    }
}