using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IDepartmentService
    {
        IQueryable<Department> GetALL();

        IQueryable<Department> GetKendoALL();

        void Create(Department model);

        void Update(Department model);

        void Delete(Department model);

        Department Find(int ID);

        Department GetRoot(int ID);
    }
}