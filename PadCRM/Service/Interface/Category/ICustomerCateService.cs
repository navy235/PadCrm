using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface ICustomerCateService
    {
        IQueryable<CustomerCate> GetALL();

        IQueryable<CustomerCate> GetKendoALL();

        void Create(CustomerCate model);

        void Update(CustomerCate model);

        void Delete(CustomerCate model);

        CustomerCate Find(int ID);
    }
}