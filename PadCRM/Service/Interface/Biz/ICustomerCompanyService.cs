using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
using PadCRM.Utils;
namespace PadCRM.Service.Interface
{
    public interface ICustomerCompanyService
    {
        IQueryable<CustomerCompany> GetALL();

        IQueryable<CustomerCompany> GetKendoALL();

        void Create(CustomerCompany model);

        CustomerCompany Create(CustomerCompanyViewModel model);

        CustomerCompany Update(CustomerCompanyViewModel model);

        void Update(CustomerCompany model);

        void Delete(CustomerCompany model);

        void Replace(int CompanyID, int MemberID);

        CustomerCompany Find(int ID);

        List<int> GetMemberCompanyIDs(int MemberID);

        bool IsReplaceable(int ID);

        void ChangeStatus(string ids, CustomerCompanyStatus status);

        void ChangeRelation(int ID, int RelationID);

    }
}