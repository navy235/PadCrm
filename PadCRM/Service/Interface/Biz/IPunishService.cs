using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.ViewModels;
namespace PadCRM.Service.Interface
{
    public interface IPunishService
    {
        IQueryable<Punish> GetALL();

        IQueryable<Punish> GetKendoALL();

        void Create(Punish model);

        Punish Create(PunishViewModel model);

        Punish Update(PunishViewModel model);

        void Update(Punish model);

        void Delete(Punish model);

        Punish Find(int ID);
    }
}