using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface IContactRequireService
    {
        IQueryable<ContactRequire> GetALL();

        IQueryable<ContactRequire> GetKendoALL();

        void Create(ContactRequire model);

        ContactRequire Create(ContactRequireViewModel model);

        ContactRequire Update(ContactRequireViewModel model);

        void Update(ContactRequire model);

        void Delete(ContactRequire model);

        ContactRequire Find(int ID);
    }
}