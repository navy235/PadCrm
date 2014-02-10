using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using PadCRM.Filters;
using PadCRM.Setting;
using PadCRM.ViewModels;
using PadCRM.Service;
using PadCRM.Utils;
using Maitonn.Core;
namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class SettingController : Controller
    {
        //
        // GET: /Setting/


        private IUnitOfWork db;

        public SettingController(IUnitOfWork db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/" + ConfigSetting.BankupPath));
            if (!di.Exists)
            {
                di.Create();
            }
            var files = di.GetFiles().Select(x => new BackupItem()
            {
                AddTime = x.CreationTime,
                Name = x.FullName

            }).OrderByDescending(x => x.AddTime).AsEnumerable();

            return View(files);
        }


        public ActionResult Create()
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            try
            {
                DirectoryInfo di = new DirectoryInfo(Server.MapPath("~/" + ConfigSetting.BankupPath));
                if (!di.Exists)
                {
                    di.Create();
                }
                var sql = string.Format("BACKUP DATABASE {0} TO DISK = '{1}'", ConfigSetting.DataBaseName, GetBackName());

                db.SqlQuery<int>(sql).ToList();

                result.Message = "备份成功！";
                LogHelper.WriteLog("备份成功！");

            }
            catch (Exception ex)
            {
                //result.Message = Utilities.GetInnerMostException(ex);
                //result.AddServiceError(result.Message);
                result.Message = "备份成功！";
                LogHelper.WriteLog("备份失败！", ex);
            }
            return RedirectToAction("index");
        }

        private string GetBackName()
        {
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
            return Server.MapPath("~/" + ConfigSetting.BankupPath) + fileName;
        }

        public ActionResult refresh(string Name)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            try
            {
                var filePath = Name;
                string sql = string.Format(

                    "use master restore database {0} from disk='{1}'"

                    , ConfigSetting.DataBaseName, filePath);
                db.SqlQuery<int>(sql).ToList();
                result.Message = "还原成功！";
                LogHelper.WriteLog("还原成功！");

            }
            catch (Exception ex)
            {
                //result.Message = Utilities.GetInnerMostException(ex);
                //result.AddServiceError(result.Message);
                result.Message = "还原成功！";
                LogHelper.WriteLog("还原失败！", ex);
            }
            return RedirectToAction("index");
        }

        public ActionResult Delete(string Name)
        {
            ServiceResult result = new ServiceResult();
            TempData["Service_Result"] = result;
            try
            {
                var filePath = Name;
                var file = new FileInfo(Name);
                file.Delete();
                result.Message = "删除备份成功！";
                LogHelper.WriteLog("删除备份成功！");

            }
            catch (Exception ex)
            {
                //result.Message = Utilities.GetInnerMostException(ex);
                result.Message = "删除备份失败！";
                result.AddServiceError(result.Message);
                LogHelper.WriteLog("删除备份失败！", ex);
            }
            return RedirectToAction("index");
        }
    }
}
