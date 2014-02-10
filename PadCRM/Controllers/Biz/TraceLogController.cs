using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Maitonn.Core;
using PadCRM.Service.Interface;
using PadCRM.Models;
using PadCRM.ViewModels;
using PadCRM.Utils;
using System.Data.Objects.SqlClient;

using PadCRM.Filters;

namespace PadCRM.Controllers
{
    [PermissionAuthorize]
    public class TraceLogController : Controller
    {
        private ICustomerCompanyService CustomerCompanyService;
        private IRelationCateService RelationCateService;
        private ICustomerCateService CustomerCateService;
        private ICityCateService CityCateService;
        private IIndustryCateService IndustryCateService;
        private ICustomerService CustomerService;
        private ITraceLogService TraceLogService;
        private ICustomerShareService CustomerShareService;
        public TraceLogController(
            ICustomerCompanyService CustomerCompanyService
            , IRelationCateService RelationCateService
            , ICustomerCateService CustomerCateService
            , ICityCateService CityCateService
            , IIndustryCateService IndustryCateService
            , ICustomerService CustomerService
            , ITraceLogService TraceLogService
            , ICustomerShareService CustomerShareService
            )
        {
            this.CustomerCompanyService = CustomerCompanyService;
            this.RelationCateService = RelationCateService;
            this.CustomerCateService = CustomerCateService;
            this.CityCateService = CityCateService;
            this.IndustryCateService = IndustryCateService;
            this.CustomerService = CustomerService;
            this.TraceLogService = TraceLogService;
            this.CustomerShareService = CustomerShareService;
        }


        /// <summary>
        /// 客户详细页面的跟单日志显示
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(int ID, int page = 1)
        {
            const int pageSize = 20;
            var logs = TraceLogService.GetALL()
                .Include(x => x.RelationCate)
                .Where(x => x.CompanyID == ID)
                .OrderByDescending(x => x.AddTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();
            var totalCount = TraceLogService.GetALL()
                .Count(x => x.CompanyID == ID);
            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };
            ViewBag.CompanyID = ID;
            return View(logs);
        }

        /// <summary>
        /// 登录首页显示在我的跟单
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult My(int page = 1)
        {
            const int pageSize = 1;
            var model = CustomerCompanyService.GetALL().Include(x => x.AddMember)
                .Include(x => x.TraceLog)
                .Where(x => x.AddUser == CookieHelper.MemberID)
                .Select(x => new TraceLogGroupViewModel()
                {
                    ID = x.ID,
                    AddTime = x.AddTime,
                    Name = x.Name,
                    MemberID = x.AddUser,
                    UserName = x.AddMember.NickName,

                }).OrderByDescending(x => x.AddTime).ToList();

            var gxmodels = CustomerShareService.GetALL()
                .Include(x => x.CustomerCompany)
                .Include(x => x.CustomerCompany.AddMember)
                .Where(x => x.MemberID == CookieHelper.MemberID)
                .Select(x => new TraceLogGroupViewModel()
                {
                    ID = x.CompanyID,
                    AddTime = x.CustomerCompany.AddTime,
                    Name = x.CustomerCompany.Name,
                    MemberID = x.CustomerCompany.AddUser,
                    UserName = x.CustomerCompany.AddMember.NickName
                });

            model.AddRange(gxmodels);

            model = model.Distinct(x => x.ID).ToList();

            var totalCount = model.Count;
            model = model.Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();



            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            foreach (var item in model)
            {
                var traceLogs = TraceLogService.GetALL()
                    .Include(x => x.AddMember)
                    .Where(x => x.CompanyID == item.ID)
                    .Select(o => new TraceLogItemViewModel()
                    {
                        CompanyID = o.CompanyID,
                        Content = o.Content,
                        ID = o.ID,
                        AddTime = o.AddTime,
                        UserName = o.AddMember.NickName
                    }).OrderByDescending(o => o.AddTime).ToList();
                item.TraceLogs = traceLogs;
            }

            return View(model);
        }

        public ActionResult ViewTrace(int ID, int page = 1)
        {
            const int pageSize = 1;
            var model = CustomerCompanyService.GetALL().Include(x => x.AddMember)
                .Include(x => x.TraceLog)
                .Where(x => x.AddUser == ID)
                .Select(x => new TraceLogGroupViewModel()
                {
                    ID = x.ID,
                    AddTime = x.AddTime,
                    Name = x.Name,
                    MemberID = x.AddUser,
                    UserName = x.AddMember.NickName,

                }).OrderByDescending(x => x.AddTime).ToList();

            var gxmodels = CustomerShareService.GetALL()
                .Include(x => x.CustomerCompany)
                .Include(x => x.CustomerCompany.AddMember)
                .Where(x => x.MemberID == ID)
                .Select(x => new TraceLogGroupViewModel()
                {
                    ID = x.CompanyID,
                    AddTime = x.CustomerCompany.AddTime,
                    Name = x.CustomerCompany.Name,
                    MemberID = x.CustomerCompany.AddUser,
                    UserName = x.CustomerCompany.AddMember.NickName
                });

            model.AddRange(gxmodels);

            model = model.Distinct(x => x.ID).ToList();

            var totalCount = model.Count;
            model = model.Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();

            ViewBag.PageInfo = new PagingInfo()
            {
                TotalItems = totalCount,
                CurrentPage = page,
                ItemsPerPage = pageSize
            };

            foreach (var item in model)
            {
                var traceLogs = TraceLogService.GetALL()
                    .Include(x => x.AddMember)
                    .Where(x => x.CompanyID == item.ID
                  )
                    .Select(o => new TraceLogItemViewModel()
                    {
                        CompanyID = o.CompanyID,
                        Content = o.Content,
                        ID = o.ID,
                        AddTime = o.AddTime,
                        UserName = o.AddMember.NickName

                    }).OrderByDescending(o => o.AddTime).ToList();
                item.TraceLogs = traceLogs;
            }

            return View(model);
        }
        //
        // GET: /Customer/

        #region nomarl form

        public ActionResult Create(int ID)
        {
            var model = new TraceLogViewModel()
            {
                CompanyID = ID
            };
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TraceLogViewModel model)
        {
            ServiceResult result = new ServiceResult();

            TempData["Service_Result"] = result;
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    TraceLogService.Create(model);

                    CustomerCompanyService.ChangeRelation(model.CompanyID, model.RelationID);
                    result.Message = "添加跟单日志成功！";
                    return RedirectToAction("details", "customercompany", new { id = model.CompanyID });
                }
                catch (Exception ex)
                {
                    result.Message = "添加跟单日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加跟单日志失败!", ex);
                }
            }
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            var entity = TraceLogService.Find(ID);
            var model = new TraceLogViewModel()
            {
                CompanyID = entity.CompanyID,

                ID = entity.ID,
                RelationID = entity.RelationID,
                Content = entity.Content
            };
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(),
                x => x.ID, x => x.CateName,
                model.RelationID,
                true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TraceLogViewModel model)
        {
            ServiceResult result = new ServiceResult();

            TempData["Service_Result"] = result;
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    TraceLogService.Update(model);
                    result.Message = "编辑跟单日志成功！";
                    return RedirectToAction("details", "customercompany", new { id = model.CompanyID });
                }
                catch (Exception ex)
                {
                    result.Message = "编辑跟单日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑跟单日志失败!", ex);
                }

            }
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(),
                x => x.ID, x => x.CateName,
                model.RelationID,
                true);
            return View(model);
        }


        #endregion



        #region ajaxform

        public ActionResult AjaxCreate(int ID)
        {
            var model = new TraceLogViewModel()
            {
                CompanyID = ID
            };
            ViewBag.Data_CompanyID = ID;
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxCreate(TraceLogViewModel model)
        {

            ServiceResult result = new ServiceResult();
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    TraceLogService.Create(model);
                    CustomerCompanyService.ChangeRelation(model.CompanyID, model.RelationID);
                    result.Message = "添加跟单日志成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "添加跟单日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "添加跟单日志失败!", ex);
                }
            }
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(), x => x.ID, x => x.CateName, true);
            return Json(result);
        }


        public ActionResult AjaxEdit(int ID)
        {
            var entity = TraceLogService.Find(ID);
            var model = new TraceLogViewModel()
            {
                CompanyID = entity.CompanyID,

                ID = entity.ID,
                RelationID = entity.RelationID,
                Content = entity.Content
            };
            ViewBag.Data_CompanyID = ID;
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(),
                x => x.ID, x => x.CateName,
                model.RelationID,
                true);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AjaxEdit(TraceLogViewModel model)
        {
            ServiceResult result = new ServiceResult();
            if (!ModelState.IsValid)
            {
                result.Message = "表单输入有误,请仔细填写表单！";
                result.AddServiceError("表单输入有误,请仔细填写表单！");
            }
            else
            {
                try
                {
                    TraceLogService.Update(model);
                    result.Message = "编辑跟单日志成功！";
                }
                catch (Exception ex)
                {
                    result.Message = "编辑跟单日志失败!";
                    result.AddServiceError(Utilities.GetInnerMostException(ex));
                    LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "编辑跟单日志失败!", ex);
                }
            }
            ViewBag.Data_RelationID = Utilities.GetSelectListData(RelationCateService.GetALL(),
                x => x.ID, x => x.CateName,
                model.RelationID,
                true);
            return Json(result);
        }

        #endregion



        public ActionResult isEditable(int ID)
        {
            var entity = TraceLogService.Find(ID);
            return Json(entity.AddUser == CookieHelper.MemberID, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                var entity = TraceLogService.Find(ID);
                TraceLogService.Delete(entity);
                result.Message = "删除跟单日志成功！";
            }
            catch (Exception ex)
            {
                result.Message = "删除跟单日志失败!";
                result.AddServiceError(Utilities.GetInnerMostException(ex));
                LogHelper.WriteLog("用户:" + CookieHelper.MemberID + "删除跟单日志失败!", ex);
            }
            return Json(result);
        }

    }
}
