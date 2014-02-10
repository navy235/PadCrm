using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface IFileShareService
    {
        IQueryable<FileShare> GetALL();

        IQueryable<FileShare> GetKendoALL();

        void Create(FileShare model);

        FileShare Create(FileShareViewModel model);

        FileShare Update(FileShareViewModel model);

        void Update(FileShare model);

        void Delete(FileShare model);

        FileShare Find(int ID);
    }
}