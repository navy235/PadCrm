using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface IContractInfoService
    {
        IQueryable<ContractInfo> GetALL();

        IQueryable<ContractInfo> GetKendoALL();

        void Create(ContractInfo model);

        ContractInfo Create(ContractInfoViewModel model);

        ContractInfo Update(ContractInfoViewModel model);

        void Update(ContractInfo model);

        void Delete(ContractInfo model);

        ContractInfo Find(int ID);
    }
}