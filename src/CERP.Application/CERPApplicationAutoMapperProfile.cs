﻿using AutoMapper;
using CERP.App;
using CERP.ApplicationContracts.HR.OrganizationalManagement.OrganizationStructure;
using CERP.ApplicationContracts.HR.OrganizationalManagement.PayrollStructure;
using CERP.CERP.HR.Documents;
using CERP.FM;
using CERP.FM.COA;
using CERP.FM.COA.DTOs;
using CERP.FM.COA.UV_DTOs;
using CERP.FM.DTOs;
using CERP.FM.UV_DTOs;
using CERP.HR.Attendance;
using CERP.HR.Documents;
using CERP.HR.EmployeeCentral.DTOs;
using CERP.HR.EMPLOYEE.DTOs;
using CERP.HR.EmployeeCentral.Employee;
using CERP.HR.EmployeeCentral.DTOs.Employee;
using CERP.HR.Employees.UV_DTOs;
using CERP.HR.Holidays;
using CERP.HR.Leaves;
using CERP.HR.Loans;
using CERP.HR.OrganizationalManagement.OrganizationStructure;
using CERP.HR.OrganizationalManagement.PayrollStructure;
using CERP.HR.Timesheets;
using CERP.HR.Workshift.DTOs;
using CERP.HR.Workshifts;
using CERP.Payroll.DTOs;
using CERP.Payroll.Payrun;
using CERP.Setup;
using CERP.Setup.DTOs;
using CERP.Users;
using System.Linq;
using CERP.HR.EmployeeCentral.DTOs.Attributes;

namespace CERP
{
    public class CERPApplicationAutoMapperProfile : Profile
    {
        public CERPApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<ApprovalRouteTemplate, ApprovalRouteTemplate_Dto>();
            CreateMap<ApprovalRouteTemplate_Dto, ApprovalRouteTemplate>();

            CreateMap<ApprovalRouteTemplateItem, ApprovalRouteTemplateItem_Dto>().ForMember(d => d.ApprovalRouteTemplate, opt => opt.Ignore()).AfterMap((x, y) => y.Initialize());
            CreateMap<ApprovalRouteTemplateItem_Dto, ApprovalRouteTemplateItem>();

            CreateMap<ApprovalRouteTemplateItemEmployee, ApprovalRouteTemplateItemEmployee_Dto>().ForMember(d => d.ApprovalRouteTemplateItem, opt => opt.Ignore());
            CreateMap<ApprovalRouteTemplateItemEmployee_Dto, ApprovalRouteTemplateItemEmployee>();

            CreateMap<TaskTemplate, TaskTemplate_Dto>();
            CreateMap<TaskTemplate_Dto, TaskTemplate>();

            CreateMap<TaskTemplateItem, TaskTemplateItem_Dto>().ForMember(d => d.TaskTemplate, opt => opt.Ignore()).AfterMap((x, y) => y.Initialize());
            CreateMap<TaskTemplateItem_Dto, TaskTemplateItem>();

            CreateMap<TaskTemplateItemEmployee, TaskTemplateItemEmployee_Dto>().ForMember(d => d.TaskTemplateItem, opt => opt.Ignore());
            CreateMap<TaskTemplateItemEmployee_Dto, TaskTemplateItemEmployee>();

            CreateMap<COA_Account, COA_Account_Dto>();
            CreateMap<COA_Account_Dto, COA_Account>();
            CreateMap<COA_Account_Dto, COA_Account_UV_Dto>();
            CreateMap<COA_Account_UV_Dto, COA_Account>();

            CreateMap<COA_AccountSubCategory, COA_AccountSubCategory_Dto>();
            CreateMap<COA_AccountSubCategory_Dto, COA_AccountSubCategory>();
            CreateMap<COA_AccountSubCategory_UV_Dto, COA_AccountSubCategory>();

            CreateMap<COA_HeadAccount, COA_HeadAccount_Dto>();
            CreateMap<COA_HeadAccount_Dto, COA_HeadAccount>();
            CreateMap<COA_HeadAccount_UV_Dto, COA_HeadAccount>();

            CreateMap<Company, Company_Dto>();
            CreateMap<Company_Dto, Company>();
            CreateMap<Company_UV_Dto, Company>();

            CreateMap<CompanyCurrency, CompanyCurrency_Dto>();
            CreateMap<CompanyCurrency_Dto, CompanyCurrency>();

            CreateMap<CompanyLocation, CompanyLocation_Dto>();
            CreateMap<CompanyLocation_Dto, CompanyLocation>();

            CreateMap<CompanyDocument, CompanyDocument_Dto>();
            CreateMap<CompanyDocument_Dto, CompanyDocument>();

            CreateMap<CompanyPrintSize, CompanyPrintSize_Dto>();
            CreateMap<CompanyPrintSize_Dto, CompanyPrintSize>();

            CreateMap<LocationTemplate, LocationTemplate_Dto>().AfterMap((x, y) => y.Initialize());
            CreateMap<LocationTemplate_Dto, LocationTemplate>();

            CreateMap<Currency, Currency_Dto>();
            CreateMap<Currency_Dto, Currency>();
            
            CreateMap<Document, Document_Dto>();
            CreateMap<Document_Dto, Document>();

            CreateMap<Department, Department_Dto>().MaxDepth(1)
                .ForMember(d => d.Positions, opt => opt.Ignore());
            CreateMap<Department_Dto, Department>().MaxDepth(1)
                .ForMember(d => d.Positions, opt => opt.Ignore());
            CreateMap<Department_UV_Dto, Department>()
                .ForMember(d => d.Positions, opt => opt.Ignore());

            CreateMap<Position, Position_Dto>().MaxDepth(1)
                .ForMember(d => d.Employee, opt => opt.Ignore());
                //.ForMember(d => d.Department, opt => opt.Ignore());
            CreateMap<Position_Dto, Position>().MaxDepth(1)
                .ForMember(d => d.Employee, opt => opt.Ignore())
                .ForMember(d => d.Department, opt => opt.Ignore());
            CreateMap<Position_UV_Dto, Position>().MaxDepth(1)
                .ForMember(d => d.Employee, opt => opt.Ignore())
                .ForMember(d => d.Department, opt => opt.Ignore());

            CreateMap<Branch, Branch_Dto>();
            CreateMap<Branch_Dto, Branch>();
            CreateMap<Branch_UV_Dto, Branch>();

            CreateMap<COA_SubLedgerRequirement, COA_SubLedgerRequirement_Dto>().ForMember(d => d.SubLedgerRequirementAccounts, opt => opt.Ignore());
            CreateMap<COA_SubLedgerRequirement_Dto, COA_SubLedgerRequirement>().ForMember(d => d.SubLedgerRequirementAccounts, opt => opt.Ignore());
            CreateMap<COA_SubLedgerRequirement_UV_Dto, COA_SubLedgerRequirement>().ForMember(d => d.SubLedgerRequirementAccounts, opt => opt.Ignore());

            CreateMap<COA_SubLedgerRequirement_Account, COA_SubLedgerRequirement_Account_Dto>().ForMember(d => d.Account, opt => opt.Ignore());
            CreateMap<COA_SubLedgerRequirement_Account_Dto, COA_SubLedgerRequirement_Account>().ForMember(d => d.Account, opt => opt.Ignore());
            CreateMap<COA_SubLedgerRequirement_Account_UV_Dto, COA_SubLedgerRequirement_Account>().ForMember(d => d.Account, opt => opt.Ignore());

            CreateMap<AccountStatementType, AccountStatementType_Dto>();
            CreateMap<AccountStatementType_Dto, AccountStatementType>();
            CreateMap<AccountStatementType_UV_Dto, AccountStatementType>();

            CreateMap<DictionaryValue, DictionaryValue_Dto>();
            CreateMap<DictionaryValue_Dto, DictionaryValue>();

            CreateMap<DictionaryValueType, DictionaryValueType_Dto>().ForMember(d => d.Values, opt => opt.Ignore());
            CreateMap<DictionaryValueType_Dto, DictionaryValueType>().ForMember(d => d.Values, opt => opt.Ignore());


            CreateMap<AppUser, AppUser_Dto>();
            CreateMap<AppUser_Dto, AppUser>();

            //CreateMap<PhysicalID, PhysicalID_Dto>().ForMember(d => d.Employee, opt => opt.Ignore());
            //CreateMap<PhysicalID_Dto, PhysicalID>().ForMember(d => d.Employee, opt => opt.Ignore());
            //CreateMap<PhysicalID_UV_Dto, PhysicalID_Dto>().ForMember(d => d.Employee, opt => opt.Ignore());
            //CreateMap<PhysicalID_Dto, PhysicalID_UV_Dto>().ForMember(d => d.Employee, opt => opt.Ignore());
            //CreateMap<PhysicalID_UV_Dto, PhysicalID>().ForMember(d => d.Employee, opt => opt.Ignore());
            //CreateMap<DictionaryValue_UV_Dto, DictionaryValue>();

            #region HR
            #region Organizational Management
            #region Organization Structure
            CreateMap<OS_OrganizationStructureTemplate, OS_OrganizationStructureTemplate_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplate_Dto, OS_OrganizationStructureTemplate>().MaxDepth(1);

            CreateMap<OS_OrganizationStructureTemplateBusinessUnit, OS_OrganizationStructureTemplateBusinessUnit_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateBusinessUnit_Dto, OS_OrganizationStructureTemplateBusinessUnit>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateBusinessUnitPosition, OS_OrganizationStructureTemplateBusinessUnitPosition_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateBusinessUnitPosition_Dto, OS_OrganizationStructureTemplateBusinessUnitPosition>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateBusinessUnitCostCenter, OS_OrganizationStructureTemplateBusinessUnitCostCenter_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateBusinessUnitCostCenter_Dto, OS_OrganizationStructureTemplateBusinessUnitCostCenter>().MaxDepth(1);

            CreateMap<OS_OrganizationStructureTemplateDivision, OS_OrganizationStructureTemplateDivision_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDivision_Dto, OS_OrganizationStructureTemplateDivision>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDivisionPosition, OS_OrganizationStructureTemplateDivisionPosition_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDivisionPosition_Dto, OS_OrganizationStructureTemplateDivisionPosition>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDivisionCostCenter, OS_OrganizationStructureTemplateDivisionCostCenter_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDivisionCostCenter_Dto, OS_OrganizationStructureTemplateDivisionCostCenter>().MaxDepth(1);

            CreateMap<OS_OrganizationStructureTemplateDepartment, OS_OrganizationStructureTemplateDepartment_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDepartment_Dto, OS_OrganizationStructureTemplateDepartment>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDepartmentPosition, OS_OrganizationStructureTemplateDepartmentPosition_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDepartmentPosition_Dto, OS_OrganizationStructureTemplateDepartmentPosition>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDepartmentCostCenter, OS_OrganizationStructureTemplateDepartmentCostCenter_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplateDepartmentCostCenter_Dto, OS_OrganizationStructureTemplateDepartmentCostCenter>().MaxDepth(1);

            CreateMap<OS_OrganizationStructureTemplatePosition, OS_OrganizationStructureTemplatePosition_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplatePosition_Dto, OS_OrganizationStructureTemplatePosition>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplatePositionJob, OS_OrganizationStructureTemplatePositionJob_Dto>().MaxDepth(1);
            CreateMap<OS_OrganizationStructureTemplatePositionJob_Dto, OS_OrganizationStructureTemplatePositionJob>().MaxDepth(1);


            CreateMap<OS_DivisionTemplate, OS_DivisionTemplate_Dto>();
            CreateMap<OS_DivisionTemplate_Dto, OS_DivisionTemplate>();

            CreateMap<OS_BusinessUnitTemplate, OS_BusinessUnitTemplate_Dto>();
            CreateMap<OS_BusinessUnitTemplate_Dto, OS_BusinessUnitTemplate>();
            
            CreateMap<OS_DepartmentTemplate, OS_DepartmentTemplate_Dto>();
            CreateMap<OS_DepartmentTemplate_Dto, OS_DepartmentTemplate>();
            CreateMap<OS_DepartmentSubDepartmentTemplate, OS_DepartmentSubDepartmentTemplate_Dto>();
            CreateMap<OS_DepartmentSubDepartmentTemplate_Dto, OS_DepartmentSubDepartmentTemplate>();
            //CreateMap<OS_DepartmentPositionTemplate, OS_DepartmentPositionTemplate_Dto>();
            //CreateMap<OS_DepartmentPositionTemplate_Dto, OS_DepartmentPositionTemplate>();
            CreateMap<OS_DepartmentCostCenterTemplate, OS_DepartmentCostCenterTemplate_Dto>();
            CreateMap<OS_DepartmentCostCenterTemplate_Dto, OS_DepartmentCostCenterTemplate>();

            CreateMap<OS_PositionTemplate, OS_PositionTemplate_Dto>();
            CreateMap<OS_PositionTemplate_Dto, OS_PositionTemplate>();
            CreateMap<OS_PositionJobTemplate, OS_PositionJobTemplate_Dto>();
            CreateMap<OS_PositionJobTemplate_Dto, OS_PositionJobTemplate>();
            //CreateMap<OS_PositionTaskTemplate, OS_PositionTaskTemplate_Dto>();
            //CreateMap<OS_PositionTaskTemplate_Dto, OS_PositionTaskTemplate>();
            CreateMap<OS_PositionCostCenterTemplate, OS_PositionCostCenterTemplate_Dto>();
            CreateMap<OS_PositionCostCenterTemplate_Dto, OS_PositionCostCenterTemplate>();

            CreateMap<OS_JobTemplate, OS_JobTemplate_Dto>();
            CreateMap<OS_JobTemplate_Dto, OS_JobTemplate>();
            CreateMap<OS_JobTaskTemplate, OS_JobTaskTemplate_Dto>();
            CreateMap<OS_JobTaskTemplate_Dto, OS_JobTaskTemplate>();
            CreateMap<OS_JobFunctionTemplate, OS_JobFunctionTemplate_Dto>();
            CreateMap<OS_JobFunctionTemplate_Dto, OS_JobFunctionTemplate>();
            CreateMap<OS_JobSkillTemplate, OS_JobSkillTemplate_Dto>();
            CreateMap<OS_JobSkillTemplate_Dto, OS_JobSkillTemplate>();
            CreateMap<OS_JobAcademiaTemplate, OS_JobAcademiaTemplate_Dto>();
            CreateMap<OS_JobAcademiaTemplate_Dto, OS_JobAcademiaTemplate>();
            CreateMap<OS_JobWorkshiftTemplate, OS_JobWorkshiftTemplate_Dto>();
            CreateMap<OS_JobWorkshiftTemplate_Dto, OS_JobWorkshiftTemplate>();

            CreateMap<OS_TaskTemplate, OS_TaskTemplate_Dto>();
            CreateMap<OS_TaskTemplate_Dto, OS_TaskTemplate>();
            CreateMap<OS_TaskSkillTemplate, OS_TaskSkillTemplate_Dto>();
            CreateMap<OS_TaskSkillTemplate_Dto, OS_TaskSkillTemplate>();
            CreateMap<OS_TaskAcademiaTemplate, OS_TaskAcademiaTemplate_Dto>();
            CreateMap<OS_TaskAcademiaTemplate_Dto, OS_TaskAcademiaTemplate>();
            
            CreateMap<OS_FunctionTemplate, OS_FunctionTemplate_Dto>();
            CreateMap<OS_FunctionTemplate_Dto, OS_FunctionTemplate>();
            CreateMap<OS_FunctionSkillTemplate, OS_FunctionSkillTemplate_Dto>();
            CreateMap<OS_FunctionSkillTemplate_Dto, OS_FunctionSkillTemplate>();
            CreateMap<OS_FunctionAcademiaTemplate, OS_FunctionAcademiaTemplate_Dto>();
            CreateMap<OS_FunctionAcademiaTemplate_Dto, OS_FunctionAcademiaTemplate>();

            CreateMap<OS_SkillTemplate, OS_SkillTemplate_Dto>();
            CreateMap<OS_SkillTemplate_Dto, OS_SkillTemplate>();
            CreateMap<OS_AcademiaTemplate, OS_AcademiaTemplate_Dto>();
            CreateMap<OS_AcademiaTemplate_Dto, OS_AcademiaTemplate>();

            CreateMap<OS_CompensationMatrixTemplate, OS_CompensationMatrixTemplate_Dto>();
            CreateMap<OS_CompensationMatrixTemplate_Dto, OS_CompensationMatrixTemplate>();
            #endregion
            #region Payroll Structure
            CreateMap<PS_PayGroup, PS_PayGroup_Dto>();
            CreateMap<PS_PayGroup_Dto, PS_PayGroup>();
            CreateMap<PS_PaySubGroup, PS_PaySubGroup_Dto>();
            CreateMap<PS_PaySubGroup_Dto, PS_PaySubGroup>();
            CreateMap<PS_PaymentBank, PS_PaymentBank_Dto>();
            CreateMap<PS_PaymentBank_Dto, PS_PaymentBank>();

            CreateMap<PS_PaymentBankFile, PS_PaymentBankFile_Dto>();
            CreateMap<PS_PaymentBankFile_Dto, PS_PaymentBankFile>();

            CreateMap<PS_PaymentBankFile, PS_PaymentBankFile_Dto>();
            CreateMap<PS_PaymentBankFile_Dto, PS_PaymentBankFile>();
            CreateMap<PS_PaymentBankFileBank, PS_PaymentBankFileBank_Dto>();
            CreateMap<PS_PaymentBankFileBank_Dto, PS_PaymentBankFileBank>();

            CreateMap<PS_PaymentCashFile, PS_PaymentCashFile_Dto>();
            CreateMap<PS_PaymentCashFile_Dto, PS_PaymentCashFile>();
            CreateMap<PS_PaymentCashFileColumn, PS_PaymentCashFileColumn_Dto>();
            CreateMap<PS_PaymentCashFileColumn_Dto, PS_PaymentCashFileColumn>();
            CreateMap<PS_PaymentChequeFile, PS_PaymentChequeFile_Dto>();
            CreateMap<PS_PaymentChequeFile_Dto, PS_PaymentChequeFile>();
            CreateMap<PS_PaymentChequeFileColumn, PS_PaymentChequeFileColumn_Dto>();
            CreateMap<PS_PaymentChequeFileColumn_Dto, PS_PaymentChequeFileColumn>();

            CreateMap<PS_PaySubGroupBank, PS_PaySubGroupBank_Dto>();
            CreateMap<PS_PaySubGroupBank_Dto, PS_PaySubGroupBank>();
            CreateMap<PS_PaySubGroupBusinessUnit, PS_PaySubGroupBusinessUnit_Dto>();
            CreateMap<PS_PaySubGroupBusinessUnit_Dto, PS_PaySubGroupBusinessUnit>();
            CreateMap<PS_PaySubGroupBusinessUnitDivision, PS_PaySubGroupBusinessUnitDivision_Dto>();
            CreateMap<PS_PaySubGroupBusinessUnitDivision_Dto, PS_PaySubGroupBusinessUnitDivision>();
            CreateMap<PS_PaySubGroupBusinessUnitDivisionDepartment, PS_PaySubGroupBusinessUnitDivisionDepartment_Dto>();
            CreateMap<PS_PaySubGroupBusinessUnitDivisionDepartment_Dto, PS_PaySubGroupBusinessUnitDivisionDepartment>();

            CreateMap<PS_PayRange, PS_PayRange_Dto>();
            CreateMap<PS_PayRange_Dto, PS_PayRange>();

            CreateMap<PS_PayGrade, PS_PayGrade_Dto>();
            CreateMap<PS_PayGrade_Dto, PS_PayGrade>();
            CreateMap<PS_PayGradeComponent, PS_PayGradeComponent_Dto>();
            CreateMap<PS_PayGradeComponent_Dto, PS_PayGradeComponent>();

            CreateMap<PS_PayComponent, PS_PayComponent_Dto>();
            CreateMap<PS_PayComponent_Dto, PS_PayComponent>();

            CreateMap<PS_PayComponentType, PS_PayComponentType_Dto>();
            CreateMap<PS_PayComponentType_Dto, PS_PayComponentType>();

            CreateMap<PS_PayFrequency, PS_PayFrequency_Dto>();
            CreateMap<PS_PayFrequency_Dto, PS_PayFrequency>();

            CreateMap<PS_PayrollPeriod, PS_PayrollPeriod_Dto>();
            CreateMap<PS_PayrollPeriod_Dto, PS_PayrollPeriod>();

            CreateMap<PS_PayPeriod, PS_PayPeriod_Dto>();
            CreateMap<PS_PayPeriod_Dto, PS_PayPeriod>();
            #endregion
            #endregion
            #region Employee Central

            #region Objects
            CreateMap<PrimaryValidityAttachment, PrimaryValidityAttachment_Dto>();
            CreateMap<PrimaryValidityAttachment_Dto, PrimaryValidityAttachment>();
            #endregion
            #region Employee
            CreateMap<Employee, Employee_Dto>();
            CreateMap<Employee_Dto, Employee>();

            CreateMap<Dependant, Dependant_Dto>();
            CreateMap<Dependant_Dto, Dependant>();

            CreateMap<EmailAddress, EmailAddress_Dto>();
            CreateMap<EmailAddress_Dto, EmailAddress>();
            CreateMap<EmployeeEmailAddress, EmployeeEmailAddress_Dto>();
            CreateMap<EmployeeEmailAddress_Dto, EmployeeEmailAddress>();

            CreateMap<HomeAddress, HomeAddress_Dto>();
            CreateMap<HomeAddress_Dto, HomeAddress>();
            CreateMap<EmployeeHomeAddress, EmployeeHomeAddress_Dto>();
            CreateMap<EmployeeHomeAddress_Dto, EmployeeHomeAddress>();

            CreateMap<PhoneAddress, PhoneAddress_Dto>();
            CreateMap<PhoneAddress_Dto, PhoneAddress>();
            CreateMap<EmployeePhoneAddress, EmployeePhoneAddress_Dto>();
            CreateMap<EmployeePhoneAddress_Dto, EmployeePhoneAddress>();
            #endregion
            #region Bio
            CreateMap<Disability, Disability_Dto>();
            CreateMap<Disability_Dto, Disability>();
            CreateMap<EmployeeDisability, EmployeeDisability_Dto>();
            CreateMap<EmployeeDisability_Dto, EmployeeDisability>();
            #endregion
            #region Contact
            CreateMap<Contact, Contact_Dto>();
            CreateMap<Contact_Dto, Contact>();
            CreateMap<EmployeeContact, EmployeeContact_Dto>();
            CreateMap<EmployeeContact_Dto, EmployeeContact>();
            #endregion

            #region Identities
            CreateMap<NationalIdentity, NationalIdentity_Dto>();
            CreateMap<NationalIdentity_Dto, NationalIdentity>();
            CreateMap<EmployeePrimaryValidityAttachment, EmployeePrimaryValidityAttachment_Dto>();
            CreateMap<EmployeePrimaryValidityAttachment_Dto, EmployeePrimaryValidityAttachment>();
            CreateMap<DependantNationalIdentity, DependantNationalIdentity_Dto>();
            CreateMap<DependantNationalIdentity_Dto, DependantNationalIdentity>();

            CreateMap<PassportTravelDocument, PassportTravelDocument_Dto>();
            CreateMap<PassportTravelDocument_Dto, PassportTravelDocument>();
            CreateMap<EmployeePassportTravelDocument, EmployeePassportTravelDocument_Dto>();
            CreateMap<EmployeePassportTravelDocument_Dto, EmployeePassportTravelDocument>();
            CreateMap<DependantPassportTravelDocument, DependantPassportTravelDocument_Dto>();
            CreateMap<DependantPassportTravelDocument_Dto, DependantPassportTravelDocument>();

            CreateMap<EmployeeSponsorLegalEntity, EmployeeSponsorLegalEntity_Dto>();
            CreateMap<EmployeeSponsorLegalEntity_Dto, EmployeeSponsorLegalEntity>();
            #endregion

            #region Benefits Info
            CreateMap<Benefit, Benefit_Dto>();
            CreateMap<Benefit_Dto, Benefit>();
            #endregion

            #region Payment Types
            CreateMap<BankPaymentType, BankPaymentType_Dto>();
            CreateMap<BankPaymentType_Dto, BankPaymentType>();

            CreateMap<CashPaymentType, CashPaymentType_Dto>();
            CreateMap<CashPaymentType_Dto, CashPaymentType>();

            CreateMap<ChequePaymentType, ChequePaymentType_Dto>();
            CreateMap<ChequePaymentType_Dto, ChequePaymentType>();
            #endregion

            #region Academia & Skills Profile
            CreateMap<EC_AcademiaTemplate, EC_AcademiaTemplate_Dto>();
            CreateMap<EC_AcademiaTemplate_Dto, EC_AcademiaTemplate>();

            CreateMap<EC_SkillTemplate, EC_SkillTemplate_Dto>();
            CreateMap<EC_SkillTemplate_Dto, EC_SkillTemplate>();
            #endregion

            #region Loans List
            CreateMap<EmployeeLoan, EmployeeLoan_Dto>();
            CreateMap<EmployeeLoan_Dto, EmployeeLoan>();
            #endregion

            #endregion
            #endregion

            CreateMap<WorkShift, WorkShift_Dto>();
            CreateMap<WorkShift_Dto, WorkShift>();

            CreateMap<DeductionMethod, DeductionMethod_Dto>().ForMember(d => d.WorkShifts, opt => opt.Ignore());
            CreateMap<DeductionMethod_Dto, DeductionMethod>();

            CreateMap<Timesheet, Timesheet_Dto>().AfterMap((ts, tsDto) => tsDto.Initialize());
            CreateMap<Timesheet_Dto, Timesheet>();
            CreateMap<TimesheetWeekSummary, TimesheetWeekSummary_Dto>().ForMember(d => d.Timesheet, opt => opt.Ignore()).AfterMap((tws, twsDto) => twsDto.Initialize());
            CreateMap<TimesheetWeekSummary_Dto, TimesheetWeekSummary>().ForMember(d => d.Timesheet, opt => opt.Ignore());
            CreateMap<TimesheetWeekJobSummary, TimesheetWeekJobSummary_Dto>().ForMember(d => d.WeekSheet, opt => opt.Ignore());
            CreateMap<TimesheetWeekJobSummary_Dto, TimesheetWeekJobSummary>().ForMember(d => d.WeekSheet, opt => opt.Ignore());

            CreateMap<Payrun, Payrun_Dto>();
            CreateMap<Payrun_Dto, Payrun>();
            CreateMap<PayrunDetail, PayrunDetail_Dto>().ForMember(d => d.Payrun, opt => opt.Ignore());
            CreateMap<PayrunDetail_Dto, PayrunDetail>().ForMember(d => d.Payrun, opt => opt.Ignore());
            CreateMap<PayrunAllowanceSummary, PayrunAllowanceSummary_Dto>();
            CreateMap<PayrunAllowanceSummary_Dto, PayrunAllowanceSummary>();
            //CreateMap<Payslip, Payslip_Dto>().ForMember(d => d.Payrun, opt => opt.Ignore());
            //CreateMap<Payslip_Dto, Payslip>().ForMember(d => d.Payrun, opt => opt.Ignore());

            CreateMap<SISetup, SISetup_Dto>();
            CreateMap<SISetup_Dto, SISetup>();
            CreateMap<SIContributionCategory, SIContributionCategory_Dto>().ForMember(d => d.Setup, opt => opt.Ignore());
            CreateMap<SIContributionCategory_Dto, SIContributionCategory>().ForMember(d => d.Setup, opt => opt.Ignore());
            CreateMap<SIContribution, SIContribution_Dto>().ForMember(d => d.SICategory, opt => opt.Ignore());
            CreateMap<SIContribution_Dto, SIContribution>().ForMember(d => d.SICategory, opt => opt.Ignore());

            CreateMap<PayrunDetailIndemnity, PayrunDetailIndemnity_Dto>().ForMember(d => d.PayrunDetail, opt => opt.Ignore());
            CreateMap<PayrunDetailIndemnity_Dto, PayrunDetailIndemnity>().ForMember(d => d.PayrunDetail, opt => opt.Ignore());

            CreateMap<LeaveRequestTemplate, LeaveRequestTemplate_Dto>();
            CreateMap<LeaveRequestTemplate_Dto, LeaveRequestTemplate>();

            CreateMap<LeaveRequestTemplateDepartment, LeaveRequestTemplateDepartment_Dto>().ForMember(d => d.LeaveRequestTemplate, opt => opt.Ignore());
            CreateMap<LeaveRequestTemplateDepartment_Dto, LeaveRequestTemplateDepartment>();

            CreateMap<LeaveRequestTemplatePosition, LeaveRequestTemplatePosition_Dto>().ForMember(d => d.LeaveRequestTemplate, opt => opt.Ignore());
            CreateMap<LeaveRequestTemplatePosition_Dto, LeaveRequestTemplatePosition>();

            CreateMap<LeaveRequestTemplateEmploymentType, LeaveRequestTemplateEmploymentType_Dto>().ForMember(d => d.LeaveRequestTemplate, opt => opt.Ignore());
            CreateMap<LeaveRequestTemplateEmploymentType_Dto, LeaveRequestTemplateEmploymentType>();

            CreateMap<LeaveRequestTemplateEmployeeStatus, LeaveRequestTemplateEmployeeStatus_Dto>().ForMember(d => d.LeaveRequestTemplate, opt => opt.Ignore());
            CreateMap<LeaveRequestTemplateEmployeeStatus_Dto, LeaveRequestTemplateEmployeeStatus>();

            CreateMap<LeaveRequestTemplateHoliday, LeaveRequestTemplateHoliday_Dto>().ForMember(d => d.LeaveRequestTemplate, opt => opt.Ignore());
            CreateMap<LeaveRequestTemplateHoliday_Dto, LeaveRequestTemplateHoliday>();

            CreateMap<LoanRequestTemplate, LoanRequestTemplate_Dto>();
            CreateMap<LoanRequestTemplate_Dto, LoanRequestTemplate>();

            CreateMap<LoanRequestTemplateDepartment, LoanRequestTemplateDepartment_Dto>().ForMember(d => d.LoanRequestTemplate, opt => opt.Ignore());
            CreateMap<LoanRequestTemplateDepartment_Dto, LoanRequestTemplateDepartment>();

            CreateMap<LoanRequestTemplatePosition, LoanRequestTemplatePosition_Dto>().ForMember(d => d.LoanRequestTemplate, opt => opt.Ignore());
            CreateMap<LoanRequestTemplatePosition_Dto, LoanRequestTemplatePosition>();

            CreateMap<LoanRequestTemplateEmploymentType, LoanRequestTemplateEmploymentType_Dto>().ForMember(d => d.LoanRequestTemplate, opt => opt.Ignore());
            CreateMap<LoanRequestTemplateEmploymentType_Dto, LoanRequestTemplateEmploymentType>();

            CreateMap<LoanRequestTemplateEmployeeStatus, LoanRequestTemplateEmployeeStatus_Dto>().ForMember(d => d.LoanRequestTemplate, opt => opt.Ignore());
            CreateMap<LoanRequestTemplateEmployeeStatus_Dto, LoanRequestTemplateEmployeeStatus>();

            CreateMap<Holiday, Holiday_Dto>();
            CreateMap<Holiday_Dto, Holiday>();

            CreateMap<Attendance, Attendance_Dto>();
            CreateMap<Attendance_Dto, Attendance>();
        }
    }
}
