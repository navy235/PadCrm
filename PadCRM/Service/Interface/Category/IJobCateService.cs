using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IJobCateService
    {
        IQueryable<JobCate> GetALL();

        IQueryable<JobCate> GetKendoALL();

        void Create(JobCate model);

        void Update(JobCate model);

        void Delete(JobCate model);

        JobCate Find(int ID);
    }
}