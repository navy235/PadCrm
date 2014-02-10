using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface IMediaRequireService
    {
        IQueryable<MediaRequire> GetALL();

        IQueryable<MediaRequire> GetKendoALL();

        void Create(MediaRequire model);

        MediaRequire Create(MediaRequireViewModel model);

        MediaRequire Update(MediaRequireViewModel model);

        void Update(MediaRequire model);

        void Delete(MediaRequire model);

        MediaRequire Find(int ID);
    }
}