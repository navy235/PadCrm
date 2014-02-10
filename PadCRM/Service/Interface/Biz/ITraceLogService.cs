using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface ITraceLogService
    {
        IQueryable<TraceLog> GetALL();

        IQueryable<TraceLog> GetKendoALL();

        void Create(TraceLog model);

        TraceLog Create(TraceLogViewModel model);

        TraceLog Update(TraceLogViewModel model);

        void Update(TraceLog model);

        void Delete(TraceLog model);

        TraceLog Find(int ID);
    }
}