using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface IPlanLogService
    {
        IQueryable<PlanLog> GetALL();

        IQueryable<PlanLog> GetKendoALL();

        void Create(PlanLog model);

        PlanLog Create(PlanLogViewModel model);

        void Update(PlanLog model);

        PlanLog Update(PlanLogViewModel model);

        PlanLog Update(PlanLogEditViewModel model);

        void Delete(PlanLog model);

        PlanLog Find(int ID);
    }
}