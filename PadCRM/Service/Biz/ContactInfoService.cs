using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PadCRM.Models;
using PadCRM.Service.Interface;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using Maitonn.Core;
using PadCRM.Utils;

namespace PadCRM.Service
{
    public class ContractInfoService : IContractInfoService
    {
        private readonly IUnitOfWork db;

        public ContractInfoService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IQueryable<ContractInfo> GetALL()
        {
            return db.Set<ContractInfo>();
        }

        public IQueryable<ContractInfo> GetKendoALL()
        {
            db.SetProxyCreationEnabledFlase();
            return db.Set<ContractInfo>();
        }

        public void Create(ContractInfo model)
        {
            db.Add<ContractInfo>(model);
            db.Commit();
        }

        public void Update(ContractInfo model)
        {
            var target = Find(model.ID);
            db.Attach<ContractInfo>(target);
            target.AttachmentPath = model.AttachmentPath;
            target.ContractCateID = model.ContractCateID;
            target.ExpiryTime = model.ExpiryTime;
            target.FinanceFax = model.FinanceFax;
            target.FinancePhone = model.FinancePhone;
            target.LastTime = DateTime.Now;
            target.LastUser = CookieHelper.MemberID;
            target.NextTime = model.NextTime;
            target.Payment = model.Payment;
            target.PlayTime = model.PlayTime;
            target.Price = model.Price;
            target.SignerID = model.SignerID;
            target.SigningTime = model.SigningTime;
            target.SubscribeTime = model.SubscribeTime;
            db.Commit();
        }

        public void Delete(ContractInfo model)
        {
            var target = Find(model.ID);
            db.Remove<ContractInfo>(target);
            db.Commit();
        }

        public ContractInfo Find(int ID)
        {
            return db.Set<ContractInfo>().Single(x => x.ID == ID);
        }


        public ContractInfo Create(ViewModels.ContractInfoViewModel model)
        {
            ContractInfo entity = new ContractInfo();

            var todayCount = GetALL().Count(x => SqlFunctions.DateDiff("day", x.AddTime, DateTime.Now) == 0);

            var key = DateTime.Now.ToString("yyyyMMdd");

            if (todayCount == 0)
            {
                key = key + "0001";
            }
            else if (todayCount < 10)
            {

                key = key + "000" + (todayCount + 1).ToString();
            }
            else
            {
                var length = todayCount.ToString().Length;

                var toadyStr = todayCount.ToString();

                for (var i = length; i <= 4; i++)
                {
                    toadyStr = "0" + toadyStr;
                }
                key = key + toadyStr;
            }
            entity.Key = key;
            entity.AddTime = DateTime.Now;
            entity.AddUser = CookieHelper.MemberID;
            entity.CompanyID = model.CompanyID;
            entity.SenderID = model.SenderID;
            entity.AttachmentPath = model.AttachmentPath;
            entity.ContractCateID = model.ContractCateID;
            entity.ExpiryTime = model.ExpiryTime;
            entity.FinanceFax = model.FinanceFax;
            entity.FinancePhone = model.FinancePhone;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.NextTime = model.NextTime;
            entity.Payment = model.Payment;
            entity.PlayTime = model.PlayTime;
            entity.Price = model.Price;
            entity.SignerID = model.SignerID;
            entity.SigningTime = model.SigningTime;
            entity.SubscribeTime = model.SubscribeTime;
            db.Add<ContractInfo>(entity);
            db.Commit();
            return entity;
        }

        public ContractInfo Update(ViewModels.ContractInfoViewModel model)
        {
            ContractInfo entity = Find(model.ID);
            db.Attach<ContractInfo>(entity);
            entity.AttachmentPath = model.AttachmentPath;
            entity.CompanyID = model.CompanyID;
            entity.SenderID = model.SenderID;
            entity.ContractCateID = model.ContractCateID;
            entity.ExpiryTime = model.ExpiryTime;
            entity.FinanceFax = model.FinanceFax;
            entity.FinancePhone = model.FinancePhone;
            entity.LastTime = DateTime.Now;
            entity.LastUser = CookieHelper.MemberID;
            entity.NextTime = model.NextTime;
            entity.Payment = model.Payment;
            entity.PlayTime = model.PlayTime;
            entity.Price = model.Price;
            entity.SignerID = model.SignerID;
            entity.SigningTime = model.SigningTime;
            entity.SubscribeTime = model.SubscribeTime;
            db.Commit();
            return entity;
        }
    }
}