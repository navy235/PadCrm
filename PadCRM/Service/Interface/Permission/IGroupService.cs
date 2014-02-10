using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IGroupService
    {
        IQueryable<Group> GetALL();

        IQueryable<Group> GetKendoALL();

        void Create(Group model);

        void Update(Group model);

        void Delete(Group model);

        Group Find(int ID);
    }
}