using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class PlanLogGroupViewModel
    {

        public PlanLogGroupViewModel()
        {
            this.PlanLogs = new List<PlanLogItemViewModel>();
        }

        public int ID { get; set; }

        public int MemberID { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public DateTime AddTime { get; set; }

        public List<PlanLogItemViewModel> PlanLogs { get; set; }
    }


    public class PlanLogItemViewModel
    {
        public int ID { get; set; }

        public int CompanyID { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime PlanTime { get; set; }

        public DateTime AddTime { get; set; }
    }
}