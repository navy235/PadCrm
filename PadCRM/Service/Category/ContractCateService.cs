using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using Maitonn.Core;
namespace PadCRM.Service
{
    public class ContractCateService : IContractCateService
    {
        private readonly IUnitOfWork db;

        public ContractCateService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<ContractCate> GetALL()
        {
            return db.Set<ContractCate>();
        }

        public IQueryable<ContractCate> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<ContractCate>();
        }

        public void Create(ContractCate model)
        {
            db.Add<ContractCate>(model);
            db.Commit();
        }

        public void Update(ContractCate model)
        {
            var target = Find(model.ID);
            db.Attach<ContractCate>(target);
            target.CateName = model.CateName;
            target.PID = model.PID;
            target.Level = model.Level;
            target.Code = model.Code;
            target.OrderIndex = model.OrderIndex;
            db.Commit();
        }

        public void Delete(ContractCate model)
        {
            var target = Find(model.ID);
            db.Remove<ContractCate>(target);
            db.Commit();
        }

        public ContractCate Find(int ID)
        {
            return db.Set<ContractCate>().Single(x => x.ID == ID);
        }
    }
}