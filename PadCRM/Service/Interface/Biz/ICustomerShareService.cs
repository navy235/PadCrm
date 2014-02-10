using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface ICustomerShareService
    {
        IQueryable<CustomerShare> GetALL();

        IQueryable<CustomerShare> GetKendoALL();

        void Create(CustomerShare model);

        CustomerShare Create(CustomerShareViewModel model);

        List<int> GetMemberShareCompanyIDs(int MemberID);

        void Update(CustomerShare model);

        void Delete(CustomerShare model);

        CustomerShare Find(int ID);
    }
}