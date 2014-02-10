using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IFileCateService
    {
        IQueryable<FileCate> GetALL();

        IQueryable<FileCate> GetKendoALL();

        void Create(FileCate model);

        void Update(FileCate model);

        void Delete(FileCate model);

        FileCate Find(int ID);
    }
}