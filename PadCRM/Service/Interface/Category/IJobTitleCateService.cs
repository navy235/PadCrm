using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IJobTitleCateService
    {
        IQueryable<JobTitleCate> GetALL();

        IQueryable<JobTitleCate> GetKendoALL();

        void Create(JobTitleCate model);

        void Update(JobTitleCate model);

        void Delete(JobTitleCate model);

        JobTitleCate Find(int ID);
    }
}