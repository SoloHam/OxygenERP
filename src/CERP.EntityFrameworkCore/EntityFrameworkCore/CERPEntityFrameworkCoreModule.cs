﻿using CERP.App;
using CERP.FM.COA;
using CERP.HR.Documents;
using CERP.HR.EmployeeCentral.Employee;
using CERP.HR.Holidays;
using CERP.HR.Leaves;
using CERP.HR.Loans;
using CERP.HR.OrganizationalManagement.OrganizationStructure;
using CERP.HR.OrganizationalManagement.PayrollStructure;
using CERP.HR.Timesheets;
using CERP.HR.Workshifts;
using CERP.Payroll.Payrun;
using CERP.Setup;
using CERP.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

                options.Entity<Company>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.CompanyLocations)
                                                        .ThenInclude(x => x.Location)
                                                        .ThenInclude(x => x.LocationCountry)
                                                       .Include(x => x.CompanyCurrencies)
                                                        .ThenInclude(x => x.Currency)
                                                       .Include(x => x.CompanyDocuments)
                                                        .ThenInclude(x => x.Document)
                                                       .Include(x => x.CompanyDocuments)
                                                        .ThenInclude(x => x.DocumentType)
                                                       .Include(x => x.CompanyPrintSizes);
                });
                options.Entity<LocationTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.LocationCountry);
                });
                options.Entity<CompanyLocation>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.Location);
                });
                options.Entity<CompanyCurrency>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.Currency);
                });
                options.Entity<CompanyDocument>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.Document);
                });
                options.Entity<CompanyPrintSize>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.PrintSize);
                });

                options.Entity<ApprovalRouteTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(x => x.ApprovalRouteItemEmployees)
                                                        .ThenInclude(x => x.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(x => x.ApprovalRouteItemEmployees)
                                                        .ThenInclude(x => x.Employee)
                                                       //.ThenInclude(x => x.Position)
                                                       .Include(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(x => x.TaskTemplate)
                                                        .ThenInclude(x => x.TaskTemplateItems)
                                                        .ThenInclude(x => x.TaskEmployees)
                                                        .ThenInclude(x => x.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(x => x.TaskTemplate)
                                                        .ThenInclude(x => x.TaskTemplateItems)
                                                        .ThenInclude(x => x.TaskEmployees)
                                                        .ThenInclude(x => x.Employee);
                                                        //.ThenInclude(x => x.Position);
                });
                options.Entity<ApprovalRouteTemplateItem>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.ApprovalRouteItemEmployees)
                                                        .ThenInclude(x => x.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                       .Include(x => x.ApprovalRouteItemEmployees)
                                                        .ThenInclude(x => x.Employee);
                                                        //.ThenInclude(x => x.Position);
                });
                options.Entity<ApprovalRouteTemplateItemEmployee>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                       .Include(x => x.Employee);
                                                        //.ThenInclude(x => x.Position);
                });

                options.Entity<TaskTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.TaskTemplateItems)
                                                        .ThenInclude(x => x.TaskEmployees)
                                                        .ThenInclude(x => x.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.TaskTemplateItems)
                                                        .ThenInclude(x => x.TaskEmployees)
                                                        .ThenInclude(x => x.Employee);
                                                        //.ThenInclude(x => x.Position);
                });
                options.Entity<TaskTemplateItem>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(x => x.TaskEmployees)
                                                        .ThenInclude(x => x.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                       .Include(x => x.TaskEmployees)
                                                        .ThenInclude(x => x.Employee);
                                                        //.ThenInclude(x => x.Position);
                });

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
                                                       .Include(p => p.Title)
                                                       .Include(p => p.MaritalStatus)
                                                       .Include(p => p.PreferredLanguage)
                                                       .Include(p => p.Nationality)
                                                       .Include(p => p.BirthCountry)
                                                       .Include(p => p.CostCenter)
                                                       .Include(p => p.PaySubGroup)
                                                       .Include(p => p.PayGrade)
                                                       .Include(p => p.EmployeeSubGroup)
                                                       .Include(p => p.EmployeeGroup)
                                                       .Include(p => p.EmploymentType)
                                                       .Include(p => p.Timezone)
                                                       .Include(p => p.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.Portal);

                                                       //.Include(x => x.NationalIdentities)
                                                       //     .ThenInclude(x => x.NationalIdentity)
                                                       // .Include(x => x.PassportTravelDocuments)
                                                       //     .ThenInclude(x => x.PassportTravelDocument)

                                                       // .Include(x => x.Dependants)
                                                       //     .ThenInclude(x => x.NationalIdentities)
                                                       //     .ThenInclude(x => x.NationalIdentity)
                                                       // .Include(x => x.Dependants)
                                                       //     .ThenInclude(x => x.PassportTravelDocuments)
                                                       //     .ThenInclude(x => x.PassportTravelDocument)

                                                       // .Include(x => x.OrganizationStructureTemplateDepartment)
                                                       //     .ThenInclude(x => x.DepartmentTemplate)

                                                       // .Include(x => x.EmailAddresses)
                                                       //     .ThenInclude(x => x.EmailAddress)
                                                       // .Include(x => x.PhoneAddresses)
                                                       //     .ThenInclude(x => x.PhoneAddress)
                                                       // .Include(x => x.HomeAddresses)
                                                       //     .ThenInclude(x => x.HomeAddress)
                                                       // .Include(x => x.Contacts)
                                                       //     .ThenInclude(x => x.Contact)

                                                       // .Include(x => x.EmployeeBenefits)

                                                       // .Include(x => x.CashPaymentTypes)
                                                       // .Include(x => x.ChequePaymentTypes)
                                                       // .Include(x => x.BankPaymentTypes)

                                                       // .Include(x => x.AcademiaProfile)
                                                       // .Include(x => x.SkillsProfile)

                                                       // .Include(x => x.EmployeeLoans);
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

                #region HR
                #region Organizational Management
                #region Organization Structure

                options.Entity<OS_OrganizationStructureTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.OrganizationStructureTemplateBusinessUnits)
                                                        .ThenInclude(p => p.BusinessUnitTemplate)
                                                       .Include(p => p.OrganizationStructureTemplateBusinessUnits)
                                                        .ThenInclude(p => p.Location)
                                                       .Include(p => p.OrganizationStructureTemplateBusinessUnits)
                                                        .ThenInclude(p => p.OrganizationStructureTemplateBusinessUnitAssociatedPositions)
                                                        .ThenInclude(p => p.PositionTemplate)
                                                       .Include(p => p.OrganizationStructureTemplateBusinessUnits)
                                                        .ThenInclude(p => p.OrganizationStructureTemplateBusinessUnitPositions)
                                                        .ThenInclude(p => p.PositionTemplate);

                                                       //.Include(p => p.OrganizationStructureTemplateDivisions)
                                                       // .ThenInclude(p => p.DivisionTemplate)
                                                       //.Include(p => p.OrganizationStructureTemplateDivisions)
                                                        //.ThenInclude(p => p.Location)
                                                       //.Include(p => p.OrganizationStructureTemplateDivisions)
                                                       // .ThenInclude(p => p.OrganizationStructureTemplateDivisionPositions)
                                                       // .ThenInclude(p => p.PositionTemplate)

                                                       //.Include(p => p.OrganizationStructureTemplateDepartments)
                                                       // .ThenInclude(p => p.DepartmentTemplate)
                                                       //.Include(p => p.OrganizationStructureTemplateDepartments)
                                                        //.ThenInclude(p => p.Location)
                                                       //.Include(p => p.OrganizationStructureTemplateDepartments)
                                                       // .ThenInclude(p => p.OrganizationStructureTemplateDepartmentPositions)
                                                       // .ThenInclude(p => p.PositionTemplate);
                });

                options.Entity<OS_OrganizationStructureTemplateBusinessUnit>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.BusinessUnitTemplate)
                                                        .Include(p => p.OrganizationStructureTemplateDivisions)
                                                        .ThenInclude(p => p.DivisionTemplate)
                                                       .Include(p => p.OrganizationStructureTemplateDepartments)
                                                        .ThenInclude(p => p.DepartmentTemplate)
                                                       .Include(p => p.OrganizationStructureTemplateBusinessUnitPositions)
                                                        .ThenInclude(p => p.PositionTemplate)
                                                       .Include(p => p.Location);
                });
                options.Entity<OS_OrganizationStructureTemplateDivision>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.DivisionTemplate)
                                                        //.Include(p => p.OrganizationStructureTemplateDivisions)
                                                        //.ThenInclude(p => p.DivisionTemplate)
                                                       .Include(p => p.OrganizationStructureTemplateDepartments)
                                                        .ThenInclude(p => p.DepartmentTemplate)
                                                       //.Include(p => p.Location)
                                                       .Include(p => p.OrganizationStructureTemplateDivisionPositions)
                                                        .ThenInclude(p => p.PositionTemplate);
                });
                options.Entity<OS_OrganizationStructureTemplateDepartment>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.DepartmentTemplate)
                                                       //.Include(p => p.Location)
                                                       .Include(p => p.OrganizationStructureTemplateDepartmentPositions)
                                                        .ThenInclude(p => p.PositionTemplate);
                });

                options.Entity<OS_DepartmentTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SubDepartmentTemplates)
                                                        .ThenInclude(p => p.SubDepartmentTemplate)
                                                       //.Include(p => p.PositionTemplates)
                                                       //.Include(p => p.DepartmentHead)
                                                       // .ThenInclude(p => p.PositionTemplate)
                                                       .Include(p => p.DepartmentCostCenterTemplates)
                                                        .ThenInclude(p => p.CostCenter);
                                                        
                });
                options.Entity<OS_PositionTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.PositionJobTemplates)
                                                        .ThenInclude(p => p.JobTemplate)
                                                       //.Include(p => p.PositionTaskTemplates)
                                                       //.ThenInclude(p => p.TaskTemplate)
                                                       .Include(p => p.PositionCostCenterTemplates)
                                                        .ThenInclude(p => p.CostCenter);
                                                       //.Include(p => p.DepartmentTemplate);
                });

                options.Entity<OS_JobTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.JobSkillTemplates)
                                                        .ThenInclude(p => p.SkillTemplate)
                                                        .ThenInclude(p => p.SkillSubType)
                                                       .Include(p => p.JobAcademiaTemplates)
                                                        .ThenInclude(p => p.AcademiaTemplate)
                                                        .ThenInclude(p => p.AcademiaCertificateSubType)
                                                       .Include(p => p.JobFunctionTemplates)
                                                        .ThenInclude(p => p.FunctionTemplate)
                                                       .Include(p => p.JobTaskTemplates)
                                                        .ThenInclude(p => p.TaskTemplate)
                                                       .Include(p => p.JobWorkshiftTemplates)
                                                        .ThenInclude(p => p.Workshift)
                                                       .Include(p => p.CompensationMatrix);
                });
                options.Entity<OS_JobTaskTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.TaskSkillTemplates)
                                                        .ThenInclude(p => p.SkillTemplate)
                                                       .Include(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.TaskAcademiaTemplates)
                                                        .ThenInclude(p => p.AcademiaTemplate)
                                                       .Include(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.CompensationMatrix);
                });
                options.Entity<OS_JobFunctionTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.FunctionTemplate)
                                                        .ThenInclude(p => p.FunctionSkillTemplates)
                                                        .ThenInclude(p => p.SkillTemplate)
                                                       .Include(p => p.FunctionTemplate)
                                                        .ThenInclude(p => p.FunctionAcademiaTemplates)
                                                        .ThenInclude(p => p.AcademiaTemplate)
                                                       .Include(p => p.FunctionTemplate)
                                                        .ThenInclude(p => p.CompensationMatrix);
                });
                options.Entity<OS_JobSkillTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SkillTemplate)
                                                       .ThenInclude(p => p.SkillSubType);
                });
                options.Entity<OS_JobAcademiaTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.AcademiaTemplate)
                                                       .ThenInclude(p => p.AcademiaCertificateSubType);
                });
                options.Entity<OS_JobWorkshiftTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Workshift)
                                                        .ThenInclude(p => p.DeductionMethod);
                });

                options.Entity<OS_AcademiaTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.AcademiaCertificateSubType)
                                                        .Include(p => p.CompensationMatrix);
                });
                options.Entity<OS_SkillTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SkillSubType)
                                                        .Include(p => p.CompensationMatrix);
                });

                options.Entity<OS_TaskTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.TaskSkillTemplates)
                                                        .ThenInclude(p => p.SkillTemplate)
                                                       .Include(p => p.TaskAcademiaTemplates)
                                                        .ThenInclude(p => p.AcademiaTemplate)
                                                       .Include(p => p.CompensationMatrix);
                });
                options.Entity<OS_TaskSkillTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SkillTemplate)
                                                       .ThenInclude(p => p.SkillSubType);
                });
                options.Entity<OS_TaskAcademiaTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.AcademiaTemplate)
                                                       .ThenInclude(p => p.AcademiaCertificateSubType);
                });

                options.Entity<OS_FunctionTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.FunctionSkillTemplates)
                                                        .ThenInclude(p => p.SkillTemplate)
                                                       .Include(p => p.FunctionAcademiaTemplates)
                                                        .ThenInclude(p => p.AcademiaTemplate)
                                                       .Include(p => p.CompensationMatrix);
                });
                options.Entity<OS_FunctionSkillTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.SkillTemplate)
                                                       .ThenInclude(p => p.SkillSubType);
                });
                options.Entity<OS_FunctionAcademiaTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.AcademiaTemplate)
                                                       .ThenInclude(p => p.AcademiaCertificateSubType);
                });

                //options.Entity<OS_TaskQualificationTemplate>(opt =>
                //{
                //    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Degree)
                //                                       .Include(p => p.Institute);
                //});
                //options.Entity<OS_JobQualificationTemplate>(opt =>
                //{
                //    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Degree)
                //                                       .Include(p => p.Institute);
                //});
                #endregion
                #region Payroll Structure
                options.Entity<PS_PaySubGroup>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Frequency)
                                                       .Include(p => p.PayrollPeriod)
                                                       .Include(p => p.LegalEntity)
                                                       .Include(p => p.PayGroup)
                                                       .Include(p => p.AllowedBanks);
                });
                options.Entity<PS_PayrollPeriod>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.PayPeriods);
                });
                options.Entity<PS_PayGrade>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.PayRange)
                                                       .Include(p => p.PayGradeComponents)
                                                        .ThenInclude(p => p.PayComponent)
                                                        .ThenInclude(p => p.PayComponentType);
                });
                options.Entity<PS_PayComponent>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.PayComponentType)
                                                       .Include(p => p.Currency)
                                                       .Include(p => p.PayFrequency);
                });
                #endregion
                #endregion
                #endregion

                options.Entity<WorkShift>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.DeductionMethod);
                });
                options.Entity<Document>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.OwnerType)
                                                       .Include(p => p.DocumentType)
                                                       .Include(p => p.IssuedFrom);
                });

                options.Entity<Timesheet>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.WeeklySummaries).ThenInclude(x => (x as TimesheetWeekSummary).WeeklyJobSummaries);
                                                       //.Include(p => p.Employee).ThenInclude(p => p.Department);
                });

                options.Entity<Payrun>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Company)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.PayrunAllowancesSummaries)
                                                        .ThenInclude(p => p.AllowanceType)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                        //.ThenInclude(x => x.Company)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.Position)
                                                        //.ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.Nationality)
                                                       .Include(p => p.PayrunDetails)
                                                        //.ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.SIType)
                                                       //.Include(p => p.PayrunDetails)
                                                        //.ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.IndemnityType)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(x => x.EmployeeTimesheet)
                                                       .Include(p => p.PayrunDetails)
                                                        .ThenInclude(p => p.Indemnity)
                                                       .Include(p => p.PostedBy);
                });
                options.Entity<PayrunDetail>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.Employee)
                                                        .ThenInclude(x => x.OrganizationStructureTemplateDepartment)
                                                        //.ThenInclude(x => x.Company)
                                                       .Include(p => p.Employee)
                                                        //.ThenInclude(p => p.Position)
                                                       //.Include(p => p.Employee)
                                                        //.ThenInclude(p => p.SIType)
                                                       .Include(p => p.Employee)
                                                        //.ThenInclude(p => p.IndemnityType)
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

                options.Entity<LeaveRequestTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.ApprovalRouteItemEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.ApprovalRouteItemEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.Position)
                                                       .Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.TaskTemplateItems)
                                                        .ThenInclude(p => p.TaskEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.TaskTemplateItems)
                                                        .ThenInclude(p => p.TaskEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.Position)
                                                       .Include(p => p.LeaveType)
                                                       .Include(p => p.Departments)
                                                        .ThenInclude(p => p.Department)
                                                       .Include(p => p.Positions)
                                                        .ThenInclude(p => p.Position)
                                                       .Include(p => p.EmployeeStatuses)
                                                       .Include(p => p.EmploymentTypes)
                                                       .Include(p => p.Holidays)
                                                        .ThenInclude(p => p.Holiday);
                });

                options.Entity<LoanRequestTemplate>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.ApprovalRouteItemEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.ApprovalRouteItemEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.Position)
                                                       .Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.TaskTemplateItems)
                                                        .ThenInclude(p => p.TaskEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        .ThenInclude(p => p.OrganizationStructureTemplateDepartment)
                                                       .Include(p => p.ApprovalRouteTemplate)
                                                        .ThenInclude(p => p.ApprovalRouteTemplateItems)
                                                        .ThenInclude(p => p.TaskTemplate)
                                                        .ThenInclude(p => p.TaskTemplateItems)
                                                        .ThenInclude(p => p.TaskEmployees)
                                                        .ThenInclude(p => p.Employee)
                                                        //.ThenInclude(p => p.Position)
                                                       .Include(p => p.LoanType)
                                                       .Include(p => p.Departments)
                                                        .ThenInclude(p => p.Department)
                                                       .Include(p => p.Positions)
                                                        .ThenInclude(p => p.Position)
                                                       .Include(p => p.EmployeeStatuses)
                                                       .Include(p => p.EmploymentTypes);
                });

                options.Entity<Holiday>(opt =>
                {
                    opt.DefaultWithDetailsFunc = q => q.Include(p => p.HolidayType)
                                                       .Include(p => p.ReligiousDenomination);

                });

                //options.Entity<Attendance>(opt =>
                //{
                //    opt.DefaultWithDetailsFunc = q => q.Include(p => p.HolidayType)
                //                                       .Include(p => p.ReligiousDenomination);

                //});

                Configure<AbpDbContextOptions>(options =>
                {
                    /* The main point to change your DBMS.
                     * See also CERPMigrationsDbContextFactory for EF Core tooling. */
                    options.UseSqlServer();
                });
            });
        }
    }
}