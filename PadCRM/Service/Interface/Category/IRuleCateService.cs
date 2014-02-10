using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IRuleCateService
    {
        IQueryable<RuleCate> GetALL();

        IQueryable<RuleCate> GetKendoALL();

        void Create(RuleCate model);

        void Update(RuleCate model);

        void Delete(RuleCate model);

        RuleCate Find(int ID);
    }
}