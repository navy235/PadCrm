using System;
using System.Threading.Tasks;
using System.Linq;
using System.Data.Entity;
using PadCRM.Models;
using Maitonn.Core;
using WebBackgrounder;

namespace PadCRM.Jobs
{
    public class CustomerCompanySetCommonJob : Job
    {
        private readonly Func<IUnitOfWork> _contextThunk;

        public CustomerCompanySetCommonJob(TimeSpan frequence, Func<IUnitOfWork> contextThunk, TimeSpan timeout)
            : base("CustomerCompany SetCommon", frequence, timeout)
        {

            if (contextThunk == null)
            {
                throw new ArgumentNullException("contextThunk");
            }
            _contextThunk = contextThunk;

        }

        public override System.Threading.Tasks.Task Execute()
        {
            return new System.Threading.Tasks.Task(SetCommon);
        }

        private void SetCommon()
        {
            using (var db = _contextThunk())
            {
                var companys = db.Set<CustomerCompany>().Include(x => x.CustomerCate).ToList();
                foreach (var entity in companys)
                {
                    var editable = false;
                    var customeCateCode = entity.CustomerCate.OrderIndex;
                    var lastUser = entity.LastUser;
                    var lastTime = DateTime.Now;
                    var hasLog = db.Set<TraceLog>()
                        .Where(x => x.AddUser == lastUser
                            && x.CompanyID == entity.ID).Any();
                    if (hasLog)
                    {
                        var logs = db.Set<TraceLog>()
                            .Where(x => x.AddUser == lastUser
                                && x.CompanyID == entity.ID)
                            .OrderByDescending(x => x.AddTime).Take(1).ToList();
                        lastTime = logs[0].AddTime;
                    }
                    else
                    {
                        lastTime = entity.LastTime;
                    }
                    var dayCount = (DateTime.Now - lastTime).Days;
                    editable = dayCount > customeCateCode;
                    if (editable)
                    {
                        db.Attach<CustomerCompany>(entity);
                        entity.IsCommon = true;
                        db.Commit();
                    }
                }
            }
        }
    }
}