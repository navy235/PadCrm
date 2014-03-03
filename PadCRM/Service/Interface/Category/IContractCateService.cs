using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IContractCateService
    {
        IQueryable<ContractCate> GetALL();

        IQueryable<ContractCate> GetKendoALL();

        void Create(ContractCate model);

        void Update(ContractCate model);

        void Delete(ContractCate model);

        ContractCate Find(int ID);
    }
}