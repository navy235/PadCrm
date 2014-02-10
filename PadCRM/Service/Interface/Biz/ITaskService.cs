using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;

namespace PadCRM.Service.Interface
{
    public interface ITaskService
    {
        IQueryable<Task> GetALL();

        IQueryable<Task> GetKendoALL();

        void Create(Task model);

        void Update(Task model);

        void Delete(Task model);

        Task Create(TaskViewModel model);

        Task Update(TaskViewModel model);

        Task Find(int ID);
    }
}