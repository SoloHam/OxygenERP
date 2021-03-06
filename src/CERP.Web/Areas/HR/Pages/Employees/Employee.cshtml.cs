using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Helpers;
using CERP.App;
using CERP.App.CustomEntityHistorySystem;
using CERP.AppServices.HR.DepartmentService;
using CERP.AppServices.HR.EmployeeService;
using CERP.AppServices.HR.WorkShiftService;
using CERP.AppServices.Identity;
using CERP.AppServices.Setup.DepartmentSetup;
using CERP.AppServices.Setup.PositionSetup;
using CERP.Attributes;
using CERP.CERP.HR.Documents;
using CERP.Helpers;
using CERP.HR.Documents;
using CERP.HR.EMPLOYEE.DTOs;
using CERP.HR.EMPLOYEE.RougeDTOs;
using CERP.HR.EmployeeCentral.Employee;
using CERP.HR.Employees.DTOs;
using CERP.HR.Employees.UV_DTOs;
using CERP.Identity;
using CERP.Setup;
using CERP.Setup.DTOs;
using CERP.Web.Pages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using Syncfusion.EJ2.Base;
using Volo.Abp;
using Volo.Abp.Account.Settings;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Json;
using Volo.Abp.Settings;
using Contact = CERP.HR.EMPLOYEE.RougeDTOs.Contact;
using Dependant = CERP.HR.EMPLOYEE.RougeDTOs.Dependant;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace CERP.Web.Areas.HR.Pages.Employees
{
    public class EmployeeModel : CERPPageModel
    {
        protected AccountAppService AccountAppService { get; }

        protected IdentityUserManager UserManager { get; }

        [BindProperty(SupportsGet = true)]
        public Guid EmployeeId { get; set; }
        public bool IsEditing { get; set; }

        public Employee_Dto EmployeeToEdit { get; set; }
        public IJsonSerializer JsonSerializer { get; set; }

        public EmployeeAppService employeeAppService;
        public DepartmentAppService departmentAppService;
        public PositionAppService positionAppService;
        public WorkshiftAppService workShiftsAppService;
        public documentAppService documentAppService;

        public IRepository<DictionaryValue, Guid> DictionaryValuesRepo;

        //public List<PhysicalID_Dto> PhysicalIds = new List<PhysicalID_Dto>();
        public List<PhysicalID_Dto> BasicSalaries = new List<PhysicalID_Dto>();

        public List<Department_Dto> Departments = new List<Department_Dto>();

        public GeneralInfo GeneralInfo { get; set; }
        public FinancialDetails FinancialDetails { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public QualificationDetail QualificationDetail { get; set; }
        public ExperienceDetail ExperiencesDetail { get; set; }
        public DependantsDetail DependantsDetail { get; set; }
        public WorkShiftDetail WorkShiftDetail { get; set; }

        public IWebHostEnvironment webHostEnvironment { get; set; }

        public IAuditingManager AuditingManager { get; set; }
        public IRepository<CustomEntityChange> CustomEntityChangesRepo { get; set; }

        protected async Task<bool> CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                return false;
            }

            return true;
        }

        public EmployeeModel(IRepository<DictionaryValue, Guid> dictionaryValuesRepo,
            EmployeeAppService employeeAppService, DepartmentAppService departmentAppService,
            PositionAppService positionAppService, WorkshiftAppService workShiftsAppService,
            IJsonSerializer jsonSerializer, IWebHostEnvironment webHostEnvironment,
            documentAppService documentAppService, AccountAppService accountAppService,
            IdentityUserManager userManager, IRepository<CustomEntityChange> customEntityChangesRepo, IAuditingManager auditingManager)
        {
            DictionaryValuesRepo = dictionaryValuesRepo;
            this.employeeAppService = employeeAppService;
            this.departmentAppService = departmentAppService;
            this.positionAppService = positionAppService;
            this.workShiftsAppService = workShiftsAppService;
            JsonSerializer = jsonSerializer;
            this.webHostEnvironment = webHostEnvironment;
            this.documentAppService = documentAppService;
            AccountAppService = accountAppService;
            UserManager = userManager;
            CustomEntityChangesRepo = customEntityChangesRepo;
            AuditingManager = auditingManager;
        }

        public JsonResult OnGetPositions(Guid departmentId)
        {
            List<Position_Dto> result =  positionAppService.GetPositionByDepartmentId(departmentId);
            return new JsonResult(result);
        }
        //public IActionResult OnGetWorkshifts(Guid departmentId)
        //{
        //    return new JsonResult(workShiftsAppService.Repository.Where(x => x.Department.Id == departmentId).ToList());
        //}
        public async Task<IActionResult> OnGet()
        {
            IsEditing = false;
            if (EmployeeId != null && EmployeeId != Guid.Empty)
            {
                //EmployeeToEdit = await employeeAppService.GetAsync(EmployeeId);

                GeneralInfo = JsonSerializer.Deserialize<GeneralInfo>(EmployeeToEdit.ExtraProperties["generalInfo"].ToString());
                GeneralInfo.Initialize(DictionaryValuesRepo, documentAppService);
                FinancialDetails = JsonSerializer.Deserialize<FinancialDetails>(EmployeeToEdit.ExtraProperties["financialDetails"].ToString());
                FinancialDetails.Initialize(DictionaryValuesRepo);
                ContactInformation = JsonSerializer.Deserialize<ContactInformation>(EmployeeToEdit.ExtraProperties["contactInformation"].ToString());
                ContactInformation.Initialize(DictionaryValuesRepo);
                DependantsDetail = JsonSerializer.Deserialize<DependantsDetail>(EmployeeToEdit.ExtraProperties["dependantsDetail"].ToString());
                DependantsDetail.Initialize(DictionaryValuesRepo, documentAppService);
                QualificationDetail = JsonSerializer.Deserialize<QualificationDetail>(EmployeeToEdit.ExtraProperties["qualificationDetail"].ToString());
                QualificationDetail.Initialize(DictionaryValuesRepo);
                ExperiencesDetail = EmployeeToEdit.ExtraProperties.ContainsKey("experienceDetail")? JsonSerializer.Deserialize<ExperienceDetail>(EmployeeToEdit.ExtraProperties["experienceDetail"].ToString()) : new ExperienceDetail();
                WorkShiftDetail = JsonSerializer.Deserialize<WorkShiftDetail>(EmployeeToEdit.ExtraProperties["workShiftDetail"].ToString());
                WorkShiftDetail.Initialize(workShiftsAppService.Repository);

                IsEditing = true;
            }

            Departments = departmentAppService.GetDepartments();

            ViewData["IsEditing"] = IsEditing;
            return Page();
        }
        public async Task<IActionResult> OnPost(Employee_UV_Dto employee, 
                                                IList<PhysicalId<Guid>> physicalIDs, 
                                                IList<BasicSalaryRDTO> basicSalaries, 
                                                IList<AllowanceRDTO> allowances, 
                                                IList<BanksRDTO> banks,
                                                Contact primaryContact, 
                                                NationalAddress nationalAddress, 
                                                IList<Contact> secondaryContacts, 
                                                IList<Qualification> qualifications, 
                                                IList<Experience> experiences, 
                                                IList<Dependant> dependants, 
                                                IList<PhysicalId<int>> dependantsIds, 
                                                IList<WorkShiftRDto> workShifts,
                                                SelfPortalDetail selfPortalDetail)
        {
            try
            {
                //CustomEntityChange customEntityChange = new CustomEntityChange(GuidGenerator.Create());
                
                var FormData = Request.Form;

                employee = JsonSerializer.Deserialize<Employee_UV_Dto>(FormData["employee"]);
                physicalIDs = JsonSerializer.Deserialize<IList<PhysicalId<Guid>>>(FormData["physicalIDs"]);
                basicSalaries = JsonSerializer.Deserialize<IList<BasicSalaryRDTO>>(FormData["basicSalaries"]);
                allowances = JsonSerializer.Deserialize<IList<AllowanceRDTO>>(FormData["allowances"]);
                banks = JsonSerializer.Deserialize<IList<BanksRDTO>>(FormData["banks"]);
                primaryContact = JsonSerializer.Deserialize<Contact>(FormData["primaryContact"]);
                nationalAddress = JsonSerializer.Deserialize<NationalAddress>(FormData["nationalAddress"]);
                secondaryContacts = JsonSerializer.Deserialize<IList<Contact>>(FormData["secondaryContacts"]);
                qualifications = JsonSerializer.Deserialize<IList<Qualification>>(FormData["qualifications"]);
                experiences = JsonSerializer.Deserialize<IList<Experience>>(FormData["experiences"]);
                dependants = JsonSerializer.Deserialize<IList<Dependant>>(FormData["dependants"]);
                dependantsIds = JsonSerializer.Deserialize<IList<PhysicalId<int>>>(FormData["dependantsIds"]);
                workShifts = JsonSerializer.Deserialize<IList<WorkShiftRDto>>(FormData["workShifts"]);
                selfPortalDetail = JsonSerializer.Deserialize<SelfPortalDetail>(FormData["selfPortalDetail"]);
                //for (int i = 0; i < physicalIDs.Count; i++)
                //{
                //    //physicalIDs[i].SetId(GuidGenerator.Create());
                //    physicalIDs[i].IDTypeId = physicalIDs[i].IDType.Id;
                //    physicalIDs[i].IDType = null;
                //    physicalIDs[i].IssuedFromId = physicalIDs[i].IssuedFrom.Id;
                //    physicalIDs[i].IssuedFrom = null;
                //}

                //var empIds = new List<PhysicalID_UV_Dto>();

                //empIds.AddRange(physicalIDs);
                //employee.PhysicalIDs = empIds;
                employee.WorkShiftId = workShifts.Last().WorkShiftId;
                //Employee_UV_Dto employeeObj = JsonSerializer.Deserialize<Employee_UV_Dto>(employee);

                GeneralInfo generalInfo = new GeneralInfo(physicalIDs);
                FinancialDetails financialDetails = new FinancialDetails(basicSalaries, allowances, banks);
                ContactInformation contactInformation = new ContactInformation(primaryContact, nationalAddress, secondaryContacts);
                QualificationDetail qualificationDetail = new QualificationDetail(qualifications);
                ExperienceDetail experienceDetail = new ExperienceDetail(experiences);
                DependantsDetail dependantsDetail = new DependantsDetail(dependants, dependantsIds);
                WorkShiftDetail workShiftDetail = new WorkShiftDetail(workShifts);

                employee.ExtraProperties.Add("generalInfo", generalInfo);
                employee.ExtraProperties.Add("financialDetails", financialDetails);
                employee.ExtraProperties.Add("contactInformation", contactInformation);
                employee.ExtraProperties.Add("qualificationDetail", qualificationDetail);
                employee.ExtraProperties.Add("experienceDetail", experienceDetail);
                employee.ExtraProperties.Add("dependantsDetail", dependantsDetail);
                employee.ExtraProperties.Add("workShiftDetail", workShiftDetail);

                if (FormData.Files.Count > 0 && FormData.Files.Any(x => x.Name == "ProfilePicture"))
                {
                    IFormFile formFile = FormData.Files.First(x => x.Name == "ProfilePicture");

                    string uploadedFileName = UploadedFile(formFile);
                    employee.ProfilePic = uploadedFileName;
                }

                IsEditing = employee.Id != Guid.Empty;
                if(!IsEditing) employee.Id = GuidGenerator.Create();

                bool selfRegistration = await CheckSelfRegistrationAsync();
                if (selfRegistration && !selfPortalDetail.CreatePortal)
                {
                    string employeeUsername = selfPortalDetail.IsUsernameEmpId ? employee.GetReferenceId : selfPortalDetail.Username;

                    var registerDto = new RegisterDto
                    {
                        AppName = "OxygenERP",
                        EmailAddress = contactInformation.PrimaryContact.Email,
                        Password = "EMP_" + employee.GetReferenceId,
                        UserName = employeeUsername,
                        EmployeeId = employee.Id
                    };

                    var userDto = await AccountAppService.RegisterAsync(registerDto);
                    var user = await UserManager.GetByIdAsync(userDto.Id);

                    await UserManager.SetEmailAsync(user, registerDto.EmailAddress);

                    employee.PortalId = user.Id;
                }

                Employee_Dto empAdded = null;

                //selfPortalDetail.
                //employee.

                if (!IsEditing)
                {

                    List<Document_Dto> documentsToAdd = new List<Document_Dto>();
                    if (FormData.Files.Count > 0)
                    {
                        for (int i = 0; i < physicalIDs.Count; i++)
                        {
                            PhysicalId<Guid> phId = physicalIDs[i];
                            string curFileName = phId.SoftCopy;
                            if (FormData.Files.Any(x => x.Name == curFileName))
                            {
                                var curDoc = phId.GetDocument;
                                if (curDoc.Id == Guid.Empty)
                                {
                                    IFormFile physicalId = FormData.Files.First(x => x.Name == curFileName);
                                    string uploadedFileName = UploadedFile(physicalId);

                                    Document_Dto doc = new Document_Dto(GuidGenerator.Create());
                                    doc.ReferenceNo = (new Random()).Next(10, 90) * (new Random()).Next(10000, 90000);
                                    doc.Name = employee.FirstName + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.NameLocalized = employee.FirstNameLocalized + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.Description = $"Soft copy of {phId.GetIDTypeValue}";
                                    doc.OwnerId = employee.Id;
                                    doc.OwnerTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "Employee").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.OwnerType).Id;
                                    doc.DocumentTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "ID").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.DocumentType).Id;
                                    doc.IssuedFromId = phId.IssuedFromId;
                                    doc.IssueDate = phId.IssuedDate.ToShortDateString();
                                    doc.ExpiryDate = phId.EndDate.ToShortDateString();
                                    doc.FileName = uploadedFileName;
                                    documentsToAdd.Add(doc);

                                    phId.SoftCopy = doc.Id.ToString();
                                }
                            }
                        }

                        for (int i = 0; i < dependantsIds.Count; i++)
                        {
                            PhysicalId<int> phId = dependantsIds[i];
                            string curFileName = phId.SoftCopy;
                            if (FormData.Files.Any(x => x.Name == curFileName))
                            {
                                var curDoc = phId.GetDocument;
                                if (curDoc.Id == Guid.Empty)
                                {
                                    IFormFile physicalId = FormData.Files.First(x => x.Name == curFileName);
                                    string uploadedFileName = UploadedFile(physicalId);

                                    Document_Dto doc = new Document_Dto(GuidGenerator.Create());
                                    doc.ReferenceNo = (new Random()).Next(10, 90) * (new Random()).Next(10000, 90000);
                                    doc.Name = employee.FirstName + "_" + dependants.First(x => x.Id == phId.ParentId).Name + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.NameLocalized = employee.FirstNameLocalized + "_" + dependants.First(x => x.Id == phId.ParentId).Name + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.Description = $"Soft copy of {phId.GetIDTypeValue} for dependant";
                                    doc.OwnerId = employee.Id;
                                    doc.OwnerTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "Dependant").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.OwnerType).Id;
                                    doc.DocumentTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "ID").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.DocumentType).Id;
                                    doc.IssuedFromId = phId.IssuedFromId;
                                    doc.IssueDate = phId.IssuedDate.ToShortDateString();
                                    doc.ExpiryDate = phId.EndDate.ToShortDateString();
                                    doc.FileName = uploadedFileName;
                                    documentsToAdd.Add(doc);

                                    phId.SoftCopy = doc.Id.ToString();
                                }
                            }
                        }

                    }

                    if (employee.ProfilePic == null)
                    {
                        employee.ProfilePic = "noimage.jpg";
                    }
                    //empAdded = await employeeAppService.CreateEmployee(employee);

                    //customEntityChange.EntityId = empAdded.Id.ToString();
                    //customEntityChange.TenantId = CurrentUser.TenantId.ToString();
                    //customEntityChange.EntityTenantId = empAdded.TenantId.ToString();
                    //customEntityChange.ChanegeTime = DateTime.Now;
                    //customEntityChange.ChangeType = EntityChangeType.Created;
                    //customEntityChange.EntityTypeFullName = typeof(Employee).FullName;

                    //await CustomEntityChangesRepo.InsertAsync(customEntityChange);

                    //customEntityChange.PropertyChanges = new List<CustomEntityPropertyChange>();
                    //List<CustomEntityPropertyChange> customEntityPropertyChanges = new List<CustomEntityPropertyChange>();
                    //var auditProps = empAdded.GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(AuditedAttribute)));
                    //foreach (var prop in auditProps)
                    //{
                    //    CustomEntityPropertyChange propertyChange = new CustomEntityPropertyChange();
                    //    propertyChange.EntityChangeId = customEntityChange.Id;
                    //    propertyChange.TenantId = CurrentTenant.Id;
                    //    propertyChange.PropertyName = prop.Name;
                    //    propertyChange.OriginalValue = "";
                    //    propertyChange.NewValue = prop.GetValue(empAdded).ToString();

                    //    customEntityChange.PropertyChanges.Add(propertyChange);
                    //}

                    if (AuditingManager.Current != null)
                    {
                        EntityChangeInfo entityChangeInfo = new EntityChangeInfo();
                        entityChangeInfo.EntityId = empAdded.Id.ToString();
                        entityChangeInfo.EntityTenantId = empAdded.TenantId;
                        entityChangeInfo.ChangeTime = DateTime.Now;
                        entityChangeInfo.ChangeType = EntityChangeType.Created;
                        //entityChangeInfo.EntityTypeFullName = typeof(Employee).FullName;

                        AuditingManager.Current.Log.EntityChanges.Add(entityChangeInfo);
                    }

                    for (int i = 0; i < documentsToAdd.Count; i++)
                    {
                        Document_Dto curDoc = documentsToAdd[i];

                        await documentAppService.CreateAsync(curDoc);
                    }
                }
                else
                {
                    List<Document_Dto> documentsToAdd = new List<Document_Dto>();
                    if (FormData.Files.Count > 0)
                    {
                        for (int i = 0; i < physicalIDs.Count; i++)
                        {
                            PhysicalId<Guid> phId = physicalIDs[i];
                            string curFileName = phId.SoftCopy;
                            if (FormData.Files.Any(x => x.Name == curFileName))
                            {
                                var curDoc = phId.GetDocument;
                                if (curDoc.Id == Guid.Empty)
                                {
                                    IFormFile physicalId = FormData.Files.First(x => x.Name == curFileName);
                                    string uploadedFileName = UploadedFile(physicalId);

                                    Document_Dto doc = new Document_Dto(GuidGenerator.Create());
                                    doc.ReferenceNo = (new Random()).Next(10, 90) * (new Random()).Next(10000, 90000);
                                    doc.Name = employee.FirstName + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.NameLocalized = employee.FirstNameLocalized + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.Description = $"Soft copy of {phId.GetIDTypeValue}";
                                    doc.OwnerId = employee.Id;
                                    doc.OwnerTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "Employee").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.OwnerType).Id;
                                    doc.DocumentTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "ID").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.DocumentType).Id;
                                    doc.IssuedFromId = phId.IssuedFromId;
                                    doc.IssueDate = phId.IssuedDate.ToShortDateString();
                                    doc.ExpiryDate = phId.EndDate.ToShortDateString();
                                    doc.FileName = uploadedFileName;
                                    documentsToAdd.Add(doc);

                                    phId.SoftCopy = doc.Id.ToString();
                                }
                            }
                        }
                        for (int i = 0; i < dependantsIds.Count; i++)
                        {
                            PhysicalId<int> phId = dependantsIds[i];
                            string curFileName = phId.SoftCopy;
                            if (FormData.Files.Any(x => x.Name == curFileName))
                            {
                                var curDoc = phId.GetDocument;
                                if (curDoc.Id == Guid.Empty)
                                {
                                    IFormFile physicalId = FormData.Files.First(x => x.Name == curFileName);
                                    string uploadedFileName = UploadedFile(physicalId);

                                    Document_Dto doc = new Document_Dto(GuidGenerator.Create());
                                    doc.ReferenceNo = (new Random()).Next(10, 90) * (new Random()).Next(10000, 90000);
                                    doc.Name = employee.FirstName + "_" + dependants.First(x => x.Id == phId.ParentId).Name + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.NameLocalized = employee.FirstNameLocalized + "_" + dependants.First(x => x.Id == phId.ParentId).Name + "_" + phId.GetIDTypeValue + "_" + phId.Id;
                                    doc.Description = $"Soft copy of {phId.GetIDTypeValue} for dependant";
                                    doc.OwnerId = employee.Id;
                                    doc.OwnerTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "Dependant").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.OwnerType).Id;
                                    doc.DocumentTypeId = DictionaryValuesRepo.WithDetails().Where(x => x.Value == "ID").First(x => x.ValueType.ValueTypeFor == ValueTypeModules.DocumentType).Id;
                                    doc.IssuedFromId = phId.IssuedFromId;
                                    doc.IssueDate = phId.IssuedDate.ToShortDateString();
                                    doc.ExpiryDate = phId.EndDate.ToShortDateString();
                                    doc.FileName = uploadedFileName;
                                    documentsToAdd.Add(doc);

                                    phId.SoftCopy = doc.Id.ToString();
                                }
                            }
                        }
                    }
                    for (int i = 0; i < documentsToAdd.Count; i++)
                    {
                        Document_Dto curDoc = documentsToAdd[i];

                        await documentAppService.CreateAsync(curDoc);
                    }

                    var emp = await employeeAppService.Repository.GetAsync(employee.Id);
                    var curGeneralInfo = JsonSerializer.Deserialize<GeneralInfo>(emp.ExtraProperties["generalInfo"].ToString());
                    curGeneralInfo.Initialize(DictionaryValuesRepo, documentAppService);
                    for (int i = 0; i < curGeneralInfo.PhysicalIds.Count; i++)
                    {
                        PhysicalId<Guid> physicalId = curGeneralInfo.PhysicalIds[i];
                        if(physicalId.SoftCopy != "" && !physicalId.SoftCopy.Contains("undefined") && !physicalIDs.Any(x => x.SoftCopy.Contains("undefined") || x.SoftCopy == physicalId.SoftCopy))
                        {
                            try
                            {
                                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                                string filePath = Path.Combine(uploadsFolder, physicalId.GetDocument.FileName);

                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }

                                await documentAppService.Repository.DeleteAsync(physicalId.GetDocument);
                            }
                            catch(Exception ex)
                            { }
                        }
                    }

                    var DependantsDetail = JsonSerializer.Deserialize<DependantsDetail>(emp.ExtraProperties["dependantsDetail"].ToString());
                    DependantsDetail.Initialize(DictionaryValuesRepo, documentAppService);
                    for (int i = 0; i < DependantsDetail.PhysicalIds.Count; i++)
                    {
                        PhysicalId<int> physicalId = DependantsDetail.PhysicalIds[i];
                        if (physicalId.SoftCopy != null && physicalId.SoftCopy != "" && !physicalId.SoftCopy.Contains("undefined") && !dependantsIds.Any(x => x.SoftCopy.Contains("undefined") || x.SoftCopy == physicalId.SoftCopy))
                        {
                            try
                            {
                                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                                string filePath = Path.Combine(uploadsFolder, physicalId.GetDocument.FileName);

                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }

                                await documentAppService.Repository.DeleteAsync(physicalId.GetDocument);
                            }
                            catch (Exception ex)
                            { }
                        }
                    }

                    if (AuditingManager.Current != null)
                    {
                        EntityChangeInfo entityChangeInfo = new EntityChangeInfo();

                        entityChangeInfo.EntityId = emp.Id.ToString();
                        entityChangeInfo.EntityTenantId = emp.TenantId;
                        entityChangeInfo.ChangeTime = DateTime.Now;
                        entityChangeInfo.ChangeType = EntityChangeType.Updated;
                        //entityChangeInfo.EntityTypeFullName = typeof(Employee).FullName;
                        
                        entityChangeInfo.PropertyChanges = new List<EntityPropertyChangeInfo>();
                        List<EntityPropertyChangeInfo> entityPropertyChanges = new List<EntityPropertyChangeInfo>();
                        //var auditProps = typeof(Employee).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();

                        //Employee mappedInput = ObjectMapper.Map<Employee_UV_Dto, Employee>(employee);
                        //foreach (var prop in auditProps)
                        //{
                        //    EntityPropertyChangeInfo propertyChange = new EntityPropertyChangeInfo();
                        //    object origVal = prop.GetValue(emp);
                        //    propertyChange.OriginalValue = origVal == null ? "" : origVal.ToString();
                        //    object newVal = prop.GetValue(mappedInput);
                        //    propertyChange.NewValue = newVal == null ? "" : newVal.ToString();
                        //    if (propertyChange.OriginalValue == propertyChange.NewValue) continue;

                        //    propertyChange.PropertyName = prop.Name;

                        //    if(prop.Name.EndsWith("Id"))
                        //    {
                        //        try
                        //        {
                        //            string valuePropName = prop.Name.TrimEnd('d', 'I');
                        //            propertyChange.PropertyName = valuePropName;

                        //            var valueProp = typeof(Employee).GetProperty(valuePropName);

                        //            DictionaryValue _origValObj = (DictionaryValue)valueProp.GetValue(emp);
                        //            if (_origValObj == null) _origValObj = await DictionaryValuesRepo.GetAsync((Guid)origVal);
                        //            string _origVal = _origValObj.Value;
                        //            propertyChange.OriginalValue = origVal == null ? "" : _origVal;
                        //            DictionaryValue _newValObj = (DictionaryValue)valueProp.GetValue(mappedInput);
                        //            if (_newValObj == null) _newValObj = await DictionaryValuesRepo.GetAsync((Guid)newVal);
                        //            string _newVal = _newValObj.Value;
                        //            propertyChange.NewValue = _newValObj == null ? "" : _newVal;
                        //        }
                        //        catch(Exception ex)
                        //        {

                        //        }
                        //    }

                        //    propertyChange.PropertyTypeFullName = prop.Name.GetType().FullName;

                        //    entityChangeInfo.PropertyChanges.Add(propertyChange);
                        //}

                        //#region ExtraProperties
                        //List<EmployeeExtraPropertyHistory> allExtraPropertyHistories = new List<EmployeeExtraPropertyHistory>();
                        //if (emp.ExtraProperties != employee.ExtraProperties)
                        //{
                        //    GeneralInfo oldGeneralInfo = emp.GetProperty<GeneralInfo>("generalInfo");
                        //    List<EmployeeExtraPropertyHistory> physicalIdsHistory = new List<EmployeeExtraPropertyHistory>();
                        //    var generalInfoPhysicalIdAuditProps = typeof(PhysicalID).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    List<PhysicalId<Guid>> NewPhysicalIds = generalInfo.PhysicalIds.Where(x => !oldGeneralInfo.PhysicalIds.Any(y => y.Id == x.Id)).ToList();
                        //    List<PhysicalId<Guid>> UpdatedPhysicalIds = generalInfo.PhysicalIds.Where(x => oldGeneralInfo.PhysicalIds.Any(y => y.Id == x.Id)).ToList();
                        //    List<PhysicalId<Guid>> DeletedPhysicalIds = oldGeneralInfo.PhysicalIds.Where(x => !generalInfo.PhysicalIds.Any(y => y.Id == x.Id)).ToList();
                        //    for (int i = 0; i < NewPhysicalIds.Count; i++)
                        //    {
                        //        PhysicalId<Guid> curPhId = generalInfo.PhysicalIds[i];

                        //        EmployeeExtraPropertyHistory newPhIdHistory = new EmployeeExtraPropertyHistory(2, "Physical Id", curPhId.IDNumber, "Created");
                        //        physicalIdsHistory.Add(newPhIdHistory);
                        //    }
                        //    for (int i = 0; i < UpdatedPhysicalIds.Count; i++)
                        //    {
                        //        PhysicalId<Guid> curPhId = generalInfo.PhysicalIds[i];
                        //        PhysicalId<Guid> oldPhId = oldGeneralInfo.PhysicalIds.First(x => x.Id == curPhId.Id);
   
                        //        EmployeeExtraPropertyHistory updatedPhIdHistory = new EmployeeExtraPropertyHistory(2, "Physical Id", curPhId.IDNumber, "Updated");
                        //        foreach (var prop in generalInfoPhysicalIdAuditProps)
                        //        {
                        //            updatedPhIdHistory.PropertyChanges = new List<EmployeeTypePropertyChange>();

                        //            EmployeeTypePropertyChange propertyChange = new EmployeeTypePropertyChange();

                        //            object origVal = prop.GetValue(oldPhId);
                        //            propertyChange.OriginalValue = origVal == null ? "" : origVal.ToString();
                        //            object newVal = prop.GetValue(curPhId);
                        //            propertyChange.NewValue = newVal == null ? "" : newVal.ToString();
                        //            if (propertyChange.OriginalValue == propertyChange.NewValue) continue;

                        //            propertyChange.PropertyName = prop.Name;

                        //            if (prop.Name.EndsWith("Id"))
                        //            {
                        //                try
                        //                {
                        //                    string valuePropName = prop.Name.TrimEnd('d', 'I');
                        //                    propertyChange.PropertyName = valuePropName;

                        //                    var valueProp = typeof(PhysicalID).GetProperty(valuePropName);

                        //                    DictionaryValue _origValObj = (DictionaryValue)valueProp.GetValue(oldPhId);
                        //                    if (_origValObj == null) _origValObj = await DictionaryValuesRepo.GetAsync((Guid)origVal);
                        //                    string _origVal = _origValObj.Value;
                        //                    propertyChange.OriginalValue = origVal == null ? "" : _origVal;
                        //                    DictionaryValue _newValObj = (DictionaryValue)valueProp.GetValue(curPhId);
                        //                    if (_newValObj == null) _newValObj = await DictionaryValuesRepo.GetAsync((Guid)newVal);
                        //                    string _newVal = _newValObj.Value;
                        //                    propertyChange.NewValue = _newValObj == null ? "" : _newVal;
                        //                }
                        //                catch (Exception ex)
                        //                {

                        //                }
                        //            }

                        //            propertyChange.PropertyTypeFullName = prop.Name.GetType().FullName;

                        //            updatedPhIdHistory.PropertyChanges.Add(propertyChange);
                        //        }
                        //        physicalIdsHistory.Add(updatedPhIdHistory);
                        //    }
                        //    for (int i = 0; i < DeletedPhysicalIds.Count; i++)
                        //    {
                        //        PhysicalId<Guid> curPhId = generalInfo.PhysicalIds[i];

                        //        EmployeeExtraPropertyHistory deletedPhIdHistory = new EmployeeExtraPropertyHistory(2, "Physical Id", curPhId.IDNumber, "Deleted");
                        //        physicalIdsHistory.Add(deletedPhIdHistory);
                        //    }

                        //    allExtraPropertyHistories.AddRange(physicalIdsHistory);

                        //    var financialDetailsAuditProps = typeof(FinancialDetails).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    FinancialDetails oldFinancialDetails = emp.GetProperty<FinancialDetails>("financialDetails");

                        //    var contactInformationAuditProps = typeof(ContactInformation).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    ContactInformation oldContactInformation = emp.GetProperty<ContactInformation>("contactInformation");

                        //    var qualificationDetailAuditProps = typeof(QualificationDetail).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    QualificationDetail oldQualificationDetail = emp.GetProperty<QualificationDetail>("qualificationDetail");

                        //    var experienceDetailAuditProps = typeof(ExperienceDetail).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    ExperienceDetail oldExperienceDetail = emp.GetProperty<ExperienceDetail>("experienceDetail");

                        //    var dependantsDetailAuditProps = typeof(DependantsDetail).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    DependantsDetail oldDependantsDetail = emp.GetProperty<DependantsDetail>("dependantsDetail");

                        //    var workShiftDetailAuditProps = typeof(WorkShiftDetail).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                        //    WorkShiftDetail oldWorkShiftDetail = emp.GetProperty<WorkShiftDetail>("workShiftDetail");

                        //    entityChangeInfo.SetProperty("extraPropertiesHistory", allExtraPropertyHistories);
                        //}
                        //#endregion

                        AuditingManager.Current.Log.EntityChanges.Add(entityChangeInfo);
                    }

                    //customEntityChange.EntityId = emp.Id.ToString();
                    //customEntityChange.TenantId = CurrentUser.TenantId.ToString();
                    //customEntityChange.EntityTenantId = emp.TenantId.ToString();
                    //customEntityChange.ChanegeTime = DateTime.Now;
                    //customEntityChange.ChangeType = EntityChangeType.Updated;
                    //customEntityChange.EntityTypeFullName = typeof(Employee).FullName;

                    //customEntityChange.PropertyChanges = new List<CustomEntityPropertyChange>();
                    //List<CustomEntityPropertyChange> customEntityPropertyChanges = new List<CustomEntityPropertyChange>();
                    //var auditProps = typeof(Employee).GetProperties().Where(x => Attribute.IsDefined(x, typeof(CustomAuditedAttribute))).ToList();
                    //Employee mappedInput = ObjectMapper.Map<Employee_UV_Dto, Employee>(employee);
                    //foreach (var prop in auditProps)
                    //{
                    //    CustomEntityPropertyChange propertyChange = new CustomEntityPropertyChange(GuidGenerator.Create());
                    //    object origVal = prop.GetValue(emp);
                    //    propertyChange.OriginalValue = origVal == null? "" : origVal.ToString();
                    //    object newVal = prop.GetValue(mappedInput);
                    //    propertyChange.NewValue = newVal == null? "" : newVal.ToString();
                    //    if (propertyChange.OriginalValue == propertyChange.NewValue) continue;

                    //    propertyChange.EntityChangeId = customEntityChange.Id;
                    //    propertyChange.TenantId = CurrentTenant.Id;
                    //    propertyChange.PropertyName = prop.Name;

                    //    customEntityChange.PropertyChanges.Add(propertyChange);
                    //}

                    //await CustomEntityChangesRepo.InsertAsync(customEntityChange);

                    emp.UpdateExtraProperties(employee.ExtraProperties);

                    //emp.DepartmentId = employee.DepartmentId;
                    //emp.PositionId = employee.PositionId;
                    //emp.EmployeeStatusId = employee.EmployeeStatusId;
                    //emp.ContractStatusId = employee.ContractStatusId;
                    //emp.ContractTypeId = employee.ContractTypeId;
                    //emp.ReligionId = employee.ReligionId;
                    //emp.BloodGroupId = employee.BloodGroupId;
                    //emp.MaritalStatusId = employee.MaritalStatusId;
                    //emp.GenderId = employee.GenderId;
                    //emp.NationalityId = employee.NationalityId;
                    //emp.POB_ID = employee.POB_ID;
                    //emp.SocialInsuranceId = employee.SocialInsuranceId;
                    //emp.SITypeId = employee.SITypeId;
                    //emp.IndemnityTypeId = employee.IndemnityTypeId;

                    //emp.JoiningDate = employee.JoiningDate;
                    //emp.JoiningHDate = employee.JoiningHDate;
                    //emp.ContractStartDate = employee.ContractStartDate;
                    //emp.ContractStartHDate = employee.ContractStartHDate;
                    //emp.ContractEndDate = employee.ContractEndDate;
                    //emp.ContractEndHDate = employee.ContractEndHDate;

                    //emp.VacationDays = employee.VacationDays;
                    //emp.NoOfDependents = employee.NoOfDependents;
                    //emp.DOB = employee.DOB;
                    //emp.DOB_H = employee.DOB_H;

                    //emp.FirstName = employee.FirstName;
                    //emp.FirstNameLocalized = employee.FirstNameLocalized;
                    //emp.MiddleName = employee.MiddleName;
                    //emp.MiddleNameLocalized = employee.MiddleNameLocalized;
                    //emp.LastName = employee.LastName;
                    //emp.LastNameLocalized = employee.LastNameLocalized;
                    //emp.FamilyName = employee.FamilyName;
                    //emp.FamilyNameLocalized = employee.FamilyNameLocalized;

                    if (emp.ProfilePic != null && emp.ProfilePic != "noimage.jpg" && employee.ProfilePic != emp.ProfilePic)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                        string filePath = Path.Combine(uploadsFolder, emp.ProfilePic);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    emp.ProfilePic = employee.ProfilePic;

                    //emp.Position = null;
                    //emp.EmployeeStatus = null;
                    //emp.ContractStatus = null;
                    //emp.ContractType = null;
                    //emp.Religion = null;
                    //emp.BloodGroup = null;
                    //emp.MaritalStatus = null;
                    //emp.Gender = null;
                    //emp.Nationality = null;
                    //emp.POB = null;

                    //var empBack = await employeeAppService.Repository.UpdateAsync(emp);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500);
            }
            return NoContent();
        }
        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = ((new Random()).Next(1, 9) * (new Random()).Next(10000, 900000)).ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
