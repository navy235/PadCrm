using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface ITcNoticeService
    {
        IQueryable<TcNotice> GetALL();

        IQueryable<TcNotice> GetKendoALL();

        void Create(TcNotice model);

        TcNotice Create(TcNoticeViewModel model);

        TcNotice Update(TcNoticeViewModel model);

        void Update(TcNotice model);

        void Delete(TcNotice model);

        TcNotice Find(int ID);
    }
}