using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.Utils
{
    public enum CustomerCategoryStatus
    {
        Nomarl = 30,
        Important = 15,
        Necessary = 7,
        Invalid = 100000,
        Cooperation = 99999
    }

    public enum CustomerCompanyStatus
    {
        Default = 0,
        Delete = -1
    }

    public enum CustomerStatus
    {
        Default = 0,
        Delete = -1
    }


    public enum MemberCurrentStatus
    {
        Default = 0,
        Delete = -1
    }
}