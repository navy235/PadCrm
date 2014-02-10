using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IPermissionsService
    {
        IQueryable<Permissions> GetALL();

        IQueryable<Permissions> GetKendoALL();

        void Create(Permissions model);

        void Update(Permissions model);

        void Delete(Permissions model);

        Permissions Find(int ID);

        bool CheckPermission(string controller, string action, int MemberID);
    }
}