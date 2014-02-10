﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface IRolesService
    {
        IQueryable<Roles> GetALL();

        IQueryable<Roles> GetKendoALL();

        void Create(Roles model);

        void Update(Roles model);

        void Delete(Roles model);

        Roles Find(int ID);
    }
}