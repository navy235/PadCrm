[assembly: WebActivator.PreApplicationStartMethod(typeof(PadCRM.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(PadCRM.App_Start.NinjectWebCommon), "Stop")]

namespace PadCRM.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Maitonn.Core;
    using PadCRM.Models;
    using PadCRM.Service.Interface;
    using PadCRM.Service;
    using WebBackgrounder;
    using WebBackgrounder.Jobs;
    using PadCRM.Jobs;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        private static JobManager _jobManager;
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
            BackgroundJobsPostStart();
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
            BackgroundJobsStop();
        }


        private static void BackgroundJobsPostStart()
        {
            var jobs = new IJob[]
            {
                new WorkItemCleanJob(TimeSpan.FromDays(1), () => new EntitiesContext(),  TimeSpan.FromDays(4)),
                new CustomerCompanySetCommonJob(TimeSpan.FromDays(1),() => new EntitiesContext(), timeout: TimeSpan.FromMinutes(2))
            };

            var coordinator = new WebFarmJobCoordinator(new EntityWorkItemRepository(() => new EntitiesContext()));
            _jobManager = new JobManager(jobs, coordinator)
            {
                RestartSchedulerOnFailure = true
            };
            _jobManager.Fail(ex => LogHelper.WriteLog("后台服务出错", ex));
            _jobManager.Start();
        }


        private static void BackgroundJobsStop()
        {
            _jobManager.Dispose();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUnitOfWork>().To<EntitiesContext>().InRequestScope();
            kernel.Bind<IDepartmentService>().To<DepartmentService>().InRequestScope();
            kernel.Bind<IPermissionsService>().To<PermissionsService>().InRequestScope();
            kernel.Bind<IRolesService>().To<RolesService>().InRequestScope();
            kernel.Bind<IGroupService>().To<GroupService>().InRequestScope();
            kernel.Bind<IMemberService>().To<MemberService>().InRequestScope();
            kernel.Bind<IMessageService>().To<MessageService>().InRequestScope();

            #region category
            kernel.Bind<ICityCateService>().To<CityCateService>().InRequestScope();
            kernel.Bind<IFileCateService>().To<FileCateService>().InRequestScope();
            kernel.Bind<IRuleCateService>().To<RuleCateService>().InRequestScope();
            kernel.Bind<IJobCateService>().To<JobCateService>().InRequestScope();
            kernel.Bind<IContractCateService>().To<ContractCateService>().InRequestScope();
            kernel.Bind<IJobTitleCateService>().To<JobTitleCateService>().InRequestScope();
            kernel.Bind<IRelationCateService>().To<RelationCateService>().InRequestScope();
            kernel.Bind<IIndustryCateService>().To<IndustryCateService>().InRequestScope();
            kernel.Bind<ICustomerCateService>().To<CustomerCateService>().InRequestScope();
            #endregion

            #region  biz
            kernel.Bind<ICustomerService>().To<CustomerService>().InRequestScope();
            kernel.Bind<ICustomerCompanyService>().To<CustomerCompanyService>().InRequestScope();
            kernel.Bind<ICustomerShareService>().To<CustomerShareService>().InRequestScope();
            kernel.Bind<ITraceLogService>().To<TraceLogService>().InRequestScope();
            kernel.Bind<IPlanLogService>().To<PlanLogService>().InRequestScope();
            kernel.Bind<INoticeService>().To<NoticeService>().InRequestScope();
            kernel.Bind<IPunishService>().To<PunishService>().InRequestScope();
            kernel.Bind<ITaskService>().To<TaskService>().InRequestScope();
            kernel.Bind<IFileShareService>().To<FileShareService>().InRequestScope();
            kernel.Bind<IContactRequireService>().To<ContactRequireService>().InRequestScope();
            kernel.Bind<IMediaRequireService>().To<MediaRequireService>().InRequestScope();
            kernel.Bind<IContractInfoService>().To<ContractInfoService>().InRequestScope();
            kernel.Bind<ITcNoticeService>().To<TcNoticeService>().InRequestScope();
            #endregion
        }
    }
}
