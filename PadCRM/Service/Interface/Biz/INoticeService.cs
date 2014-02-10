using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface INoticeService
    {
        IQueryable<Notice> GetALL();

        IQueryable<Notice> GetKendoALL();

        void Create(Notice model);

        Notice Create(NoticeViewModel model);

        Notice Update(NoticeViewModel model);

        void Update(Notice model);

        void Delete(Notice model);

        Notice Find(int ID);
    }
}