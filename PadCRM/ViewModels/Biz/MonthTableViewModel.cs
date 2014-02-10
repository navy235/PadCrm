using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class MonthTableViewModel
    {


        public int MemberID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int MaxRows { get; set; }

        public int FirstRowIndex { get; set; }

        public int DayCount { get; set; }
    }
}