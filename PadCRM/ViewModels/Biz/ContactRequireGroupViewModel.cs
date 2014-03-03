using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PadCRM.ViewModels
{
    public class ContactRequireGroupViewModel
    {
        public ContactRequireGroupViewModel()
        {
            this.ContactRequires = new List<ContactRequireItemViewModel>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int SenderID { get; set; }

        public int CompanyID { get; set; }

        public int AddUser { get; set; }

        public int Status { get; set; }

        public int IsRoot { get; set; }

        public DateTime AddTime { get; set; }

        public int ContractInfoID { get; set; }

        public List<ContactRequireItemViewModel> ContactRequires { get; set; }
    }

    public class ContactRequireItemViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }



        public string Description { get; set; }

        public int SenderID { get; set; }

        public int ResolveID { get; set; }

        public int AddUser { get; set; }

        public int CompanyID { get; set; }

        public string AttachmentPath { get; set; }

        public int Status { get; set; }

        public DateTime AddTime { get; set; }
    }


}
