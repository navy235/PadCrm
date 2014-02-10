﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
namespace PadCRM.Service.Interface
{
    public interface ICityCateService
    {
        IQueryable<CityCate> GetALL();

        IQueryable<CityCate> GetKendoALL();

        void Create(CityCate model);

        void Update(CityCate model);

        void Delete(CityCate model);

        CityCate Find(int ID);
    }
}