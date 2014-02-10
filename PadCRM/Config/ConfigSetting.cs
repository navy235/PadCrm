using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Maitonn.Core;

namespace PadCRM.Setting
{
    public static class ConfigSetting
    {
        public static string Default_AvtarUrl { get; set; }

        public static string DomainUrl { get; set; }

        public static string BankupPath { get; set; }

        public static string DataBaseName { get; set; }


        static ConfigSetting()
        {
            Default_AvtarUrl = ConfigurationManager.AppSettings["Default_AvtarUrl"];

            DomainUrl = ConfigurationManager.AppSettings["LocalDomain"];

            BankupPath = ConfigurationManager.AppSettings["BankupPath"];

            DataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
        }
    }
}