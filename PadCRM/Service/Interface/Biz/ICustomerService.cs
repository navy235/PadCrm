using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;

namespace PadCRM.Service.Interface
{
    public interface ICustomerService
    {
        IQueryable<Customer> GetALL();

        IQueryable<Customer> GetKendoALL();

        void Create(Customer model);

        Customer Create(CustomerViewModel model);

        Customer Update(CustomerViewModel model);

        IQueryable<Customer> GetBirthCustomerInDays(int day,List<int> companyIds);

        void Update(Customer model);

        void Delete(Customer model);

        Customer Find(int ID);
    }
}