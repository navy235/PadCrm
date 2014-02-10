using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Maitonn.Core;
using WebBackgrounder;

namespace PadCRM.Models
{
    public class EntitiesContext : UnitOfWork, IWorkItemsContext
    {
        public EntitiesContext()
            : base("padcrm_db")
        {

        }

        public IDbSet<WorkItem> WorkItems { get; set; }

        public IDbSet<SolarData> SolarData { get; set; }

        public IDbSet<LunarCalenderContrastTable> LunarCalenderContrastTable { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            #region Category

            modelBuilder.Entity<CityCate>()
             .HasOptional(c => c.PCate)
             .WithMany(pc => pc.ChildCates)
             .HasForeignKey(c => c.PID)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerCate>()
            .HasOptional(c => c.PCate)
            .WithMany(pc => pc.ChildCates)
            .HasForeignKey(c => c.PID)
            .WillCascadeOnDelete(false);


            modelBuilder.Entity<IndustryCate>()
            .HasOptional(c => c.PCate)
            .WithMany(pc => pc.ChildCates)
            .HasForeignKey(c => c.PID)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<RelationCate>()
            .HasOptional(c => c.PCate)
            .WithMany(pc => pc.ChildCates)
            .HasForeignKey(c => c.PID)
            .WillCascadeOnDelete(false);

            #endregion

            #region biz

            modelBuilder.Entity<Customer>()
                .HasRequired(m => m.CustomerCompany)
                .WithMany(c => c.Customer)
                .HasForeignKey(o => o.CompanyID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                 .HasRequired(m => m.JobCate)
                 .WithMany(c => c.Customer)
                 .HasForeignKey(o => o.JobID)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                 .HasRequired(m => m.AddMember)
                 .WithMany(c => c.Customer)
                 .HasForeignKey(o => o.AddUser)
                 .WillCascadeOnDelete(false);


            modelBuilder.Entity<PlanLog>()
                 .HasRequired(m => m.CustomerCompany)
                 .WithMany(c => c.PlanLog)
                 .HasForeignKey(o => o.CompanyID)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<PlanLog>()
                .HasRequired(m => m.AddMember)
                .WithMany(c => c.PlanLog)
                .HasForeignKey(o => o.AddUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TraceLog>()
              .HasRequired(m => m.CustomerCompany)
              .WithMany(c => c.TraceLog)
              .HasForeignKey(o => o.CompanyID)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<TraceLog>()
              .HasRequired(m => m.RelationCate)
              .WithMany(c => c.TraceLog)
              .HasForeignKey(o => o.RelationID)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<TraceLog>()
            .HasRequired(m => m.AddMember)
            .WithMany(c => c.TraceLog)
            .HasForeignKey(o => o.AddUser)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerCompany>()
                .HasRequired(m => m.RelationCate)
                .WithMany(c => c.CustomerCompany)
                .HasForeignKey(o => o.RelationID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerCompany>()
                .HasRequired(m => m.CityCate)
                .WithMany(c => c.CustomerCompany)
                .HasForeignKey(o => o.CityID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerCompany>()
                .HasRequired(m => m.IndustryCate)
                .WithMany(c => c.CustomerCompany)
                .HasForeignKey(o => o.IndustryID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerCompany>()
                  .HasRequired(m => m.CustomerCate)
                  .WithMany(c => c.CustomerCompany)
                  .HasForeignKey(o => o.CustomerCateID)
                  .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerCompany>()
              .HasRequired(m => m.AddMember)
              .WithMany(c => c.CustomerCompany)
              .HasForeignKey(o => o.AddUser)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerShare>()
                .HasRequired(m => m.Member)
                .WithMany(c => c.CustomerShare)
                .HasForeignKey(o => o.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerShare>()
                 .HasRequired(m => m.CustomerCompany)
                 .WithMany(c => c.CustomerShare)
                 .HasForeignKey(o => o.CompanyID)
                 .WillCascadeOnDelete(false);

            #endregion

            #region permission
            modelBuilder.Entity<Member>()
              .HasRequired(c => c.Department)
              .WithMany(pc => pc.Member)
              .HasForeignKey(c => c.DepartmentID)
              .WillCascadeOnDelete(false);




            modelBuilder.Entity<Member>()
             .HasRequired(c => c.JobTitleCate)
             .WithMany(pc => pc.Member)
             .HasForeignKey(c => c.JobTitleID)
             .WillCascadeOnDelete(false);

            modelBuilder.Entity<Punish>()
                .HasRequired(c => c.Member)
                .WithMany(pc => pc.Punish)
                .HasForeignKey(c => c.MemberID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                  .HasRequired(c => c.Member)
                  .WithMany(pc => pc.Task)
                  .HasForeignKey(c => c.MemberID)
                  .WillCascadeOnDelete(false);

            modelBuilder.Entity<Punish>()
               .HasRequired(c => c.RuleCate)
               .WithMany(pc => pc.Punish)
               .HasForeignKey(c => c.RuleID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Notice>()
                .HasRequired(c => c.Department)
                .WithMany(pc => pc.Notice)
                .HasForeignKey(c => c.DepartmentID)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<FileShare>()
                .HasRequired(c => c.FileCate)
                .WithMany(pc => pc.FileShare)
                .HasForeignKey(c => c.FileCateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FileShare>()
              .HasRequired(c => c.Member)
              .WithMany(pc => pc.FileShare)
              .HasForeignKey(c => c.AddUser)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<MediaRequire>()
               .HasRequired(c => c.Department)
               .WithMany(pc => pc.MediaRequire)
               .HasForeignKey(c => c.DepartmentID)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContactRequire>()
              .HasRequired(c => c.Department)
              .WithMany(pc => pc.ContactRequire)
              .HasForeignKey(c => c.DepartmentID)
              .WillCascadeOnDelete(false);


            modelBuilder.Entity<Department>()
              .HasOptional(c => c.PDepartment)
              .WithMany(pc => pc.ChildDepartments)
              .HasForeignKey(c => c.PID)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Permissions>()
                .HasRequired(p => p.Department)
                .WithMany(d => d.Permissions)
                .HasForeignKey(p => p.DepartmentID);

            modelBuilder.Entity<Roles>()
                .HasMany(b => b.Permissions)
                .WithMany(c => c.Roles)
                .Map
                (
                    m =>
                    {
                        m.MapLeftKey("RoleID");
                        m.MapRightKey("PermissionID");
                        m.ToTable("Role_Permissions");
                    }
                );
            modelBuilder.Entity<Group>()
               .HasMany(g => g.Roles)
               .WithMany(r => r.Group)
               .Map
               (
                   m =>
                   {
                       m.MapLeftKey("GroupID");
                       m.MapRightKey("RoleID");
                       m.ToTable("Group_Roles");
                   }
               );

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}