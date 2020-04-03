﻿using CERP.App;
using CERP.FM.COA;
using CERP.HR.Documents;
using CERP.HR.Employees;
using CERP.HR.Timesheets;
using CERP.HR.Workshifts;
using CERP.Payroll.Payrun;
using CERP.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace CERP.EntityFrameworkCore
{
    [DependsOn(
        typeof(CERPDomainModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityServerEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule),
        typeof(AbpFeatureManagementEntityFrameworkCoreModule)
        )]
    public class CERPEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<CERPDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                options.AddDefaultRepositories(includeAllEntities: true);

                options.Entity<COA_Account>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Company)
                                                       .Include(p => p.Branch)
                                                       .Include(p => p.HeadAccount)
                                                       .Include(p => p.AccountSubCategory)
                                                       .Include(p => p.AccountGroupCategory)
                                                       .Include(p => p.AccountStatementType)
                                                       .Include(p => p.AccountStatementDetailType)
                                                       .Include(p => p.CashFlowStatementType)
                                                       .Include(p => p.SubLedgerAccount)
                                                       .Include(p => p.SubLedgerRequirementAccounts)
                                                       .ThenInclude(y => (y as COA_SubLedgerRequirement_Account).SubLedgerRequirement);
                });

                options.Entity<COA_AccountSubCategory>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Company)
                                                       .Include(p => p.HeadAccount);
                });

                options.Entity<Employee>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Gender)
                                                       .Include(p => p.BloodGroup)
                                                       .Include(p => p.Nationality)
                                                       .Include(p => p.Religion)
                                                       .Include(p => p.POB)
                                                       .Include(p => p.MaritalStatus)
                                                       .Include(p => p.ContractStatus)
                                                       .Include(p => p.WorkShift)
                                                       .Include(p => p.EmployeeStatus)
                                                       .Include(p => p.EmployeeType)
                                                       .Include(p => p.Position).ThenInclude(x => x.Department)
                                                       .Include(p => p.Portal)
                                                       .Include(p => p.SIType)
                                                       .Include(p => p.IndemnityType);
                });

                options.Entity<AppUser>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Employee);
                });

                    options.Entity<PhysicalID>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.IssuedFrom)
                                                       .Include(p => p.IDType);
                });

                options.Entity<DictionaryValueType>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Values).ThenInclude(x => x.ValueType);
                });
                options.Entity<DictionaryValue>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.ValueType);
                });

                options.Entity<WorkShift>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Employees).ThenInclude(p => p.Position);
                });
                options.Entity<Document>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.OwnerType)
                                                       .Include(p => p.DocumentType)
                                                       .Include(p => p.Owner)
                                                       .Include(p => p.IssuedFrom);
                });

                options.Entity<Timesheet>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.WeeklySummaries).ThenInclude(x => (x as TimesheetWeekSummary).WeeklyJobSummaries)
                                                       .Include(p => p.Employee).ThenInclude(p => p.Department);
                });

                options.Entity<Payrun>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Company)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.PayrunAllowancesSummaries)
                                                        .ThenInclude(p => p.AllowanceType)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(x => x.Department)
                                                        .ThenInclude(x => x.Company)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.Position)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.Nationality)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.SIType)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.IndemnityType)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(x => x.EmployeeTimesheet)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Indemnity)
                                                       .Include(p => p.PostedBy);
                });
                options.Entity<PayrunDetail>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Employee)
                                                        .ThenInclude(x => x.Department)
                                                        .ThenInclude(x => x.Company)
                                                       .Include(p => p.Employee)
                                                        .ThenInclude(p => p.Position)
                                                       .Include(p => p.Employee)
                                                        .ThenInclude(p => p.SIType)
                                                       .Include(p => p.Employee)
                                                        .ThenInclude(p => p.IndemnityType)
                                                       .Include(p => p.PayrunAllowancesSummaries)
                                                        .ThenInclude(x => x.AllowanceType)
                                                       .Include(p => p.EmployeeTimesheet)
                                                       .Include(p => p.Indemnity);
                });
                options.Entity<SISetup>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.ContributionCategories).ThenInclude(x => x.SIContributions);
                });
                options.Entity<SIContributionCategory>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SIContributions);
                });
                options.Entity<SIContribution>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SICategory);
                });
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also CERPMigrationsDbContextFactory for EF Core tooling. */
                options.UseSqlServer();
            });
        }
    }
}
