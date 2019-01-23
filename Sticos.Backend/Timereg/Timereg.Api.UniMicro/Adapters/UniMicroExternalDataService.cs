using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Api.Contracts;
using Common.Api.Contracts.Services;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Unimicro.HttpClients;
using Timereg.Api.Unimicro.Models;
using unimicro = Timereg.Api.Unimicro.Models;
using Shared.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Timereg.Api.Unimicro.Adapters
{
    public class UniMicroExternalDataService : IExternalDataService
    {
        private IUnitService _unitService;
        private IUnimicroClient _unimicroClient;
        public ILogger<UniMicroExternalDataService> Logger { get; set; }

        public UniMicroExternalDataService(IUnitService unitService, IUnimicroClient unimicroClient)
        {
            _unitService = unitService;
            _unimicroClient = unimicroClient;
            Logger = NullLogger<UniMicroExternalDataService>.Instance;
        }

        public async Task<IEnumerable<ExternalData>> GetExternalEmployeeData(int unitId)
        {
            var unit = await SignIn(unitId);

            var externalDataList = new List<ExternalData>();

            var employees = await _unimicroClient.GetEmployees(unit.BusinessOrganizationNumber);
            var employeeIds = employees.Select(e => e.Id).ToList();
            var employments = _unimicroClient.GetEmployments(employeeIds).Result
                .Where(e => !e.Deleted && (!e.EndDate.HasValue || e.EndDate.Value > DateTime.Today));
            var workers = await _unimicroClient.GetWorkers(employeeIds);
            var workerIds = workers.Select(w => w.Id).ToList();
            var workRelations = await _unimicroClient.GetWorkRelations(workerIds);
            var userIds = workers.Where(w => w.UserId.HasValue).Select(w => w.UserId.Value);
            var users = await _unimicroClient.GetUsers(userIds);

            foreach (var employee in employees)
            {
                var employment = employments?.FirstOrDefault(e => e.EmployeeId == employee.Id);
                var worker =  workers?.FirstOrDefault(w => w.EmployeeId == employee.Id);
                var workRelation = worker != null ? workRelations.FirstOrDefault(wr => wr.WorkerID == worker.Id) : null;
                var user = worker != null ? users?.FirstOrDefault(u => u.Id == worker.UserId) : null;

                var externalData = new ExternalData();
                CheckIfIsSupported(externalData, workRelation, employment, worker, employee);

                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.WorkRelation.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = workRelation?.Id.ToString() });
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.Worker.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = worker?.Id.ToString() });
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.User.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = worker?.UserId.ToString() });
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.Employee.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = employee?.Id.ToString() });
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.Employee.ToString(), Property = unimicro.IdentifierProperty.SocialSecurityNumber.ToString(), Value = employee?.SocialSecurityNumber });
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.Employment.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = employment?.Id.ToString() });


                externalData.DataSet.Add(new Data { Code = PropertyName.Name.ToString(), Value = employee?.BusinessRelationInfo?.Name });
                externalData.DataSet.Add(new Data { Code = PropertyName.Email.ToString(), Value = employee?.DefaultEmail?.EmailAddress });
                externalData.DataSet.Add(new Data { Code = PropertyName.JobDescription.ToString(), Value = employment?.JobName });
                externalData.DataSet.Add(new Data { Code = PropertyName.Name.ToString(), Value = user?.DisplayName });
                externalData.DataSet.Add(new Data { Code = PropertyName.UserName.ToString(), Value = user?.UserName });
                externalData.DataSet.Add(new Data { Code = PropertyName.Email.ToString(), Value = user?.Email });

                if (!externalData.ValidForUse)
                {
                    var infoText = "Employee is not valid for mapping. Reason: " + string.Join(",", externalData.NotValidReasons);
                    Logger.LogInformation(infoText);
                }
                externalDataList.Add(externalData);
            }

            RemoveNullData(externalDataList);
            return externalDataList;
        }

        public async Task<IEnumerable<ExternalData>> GetExternalUnitData(int unitId)
        {
            await SignIn(unitId);

            var externalDataList = new List<ExternalData>();

            var subEntities = await _unimicroClient.GetSubEntities();
            foreach (var subentity in subEntities.Where(se => !se.Deleted))
            {
                var externalData = new ExternalData();

                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.SubEntity.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = subentity.Id.ToString() });
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.SubEntity.ToString(), Property = unimicro.IdentifierProperty.OrganizationNumber.ToString(), Value = subentity?.OrgNumber });

                externalData.DataSet.Add(new Data { Code = PropertyName.Name.ToString(), Value = subentity?.BusinessRelationInfo?.Name });
                externalData.DataSet.Add(new Data { Code = PropertyName.OrganizationNumber.ToString(), Value = subentity.OrgNumber });

                externalDataList.Add(externalData);
            }

            RemoveNullData(externalDataList);
            return externalDataList;
        }

        public async Task<IEnumerable<ExternalData>> GetExternalAbsenceCodeData(int unitId)
        {
            await SignIn(unitId);

            var externalDataList = new List<ExternalData>();
            var workTypes = _unimicroClient.GetWorkTypes().Result.Where(wt => !wt.Deleted);

            foreach (var workType in workTypes)
            {
                var externalData = new ExternalData();
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.WorkType.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = workType.Id.ToString() });
                externalData.DataSet.Add(new Data { Code = PropertyName.Name.ToString(), Value = workType.Name });
                externalDataList.Add(externalData);
            }

            RemoveNullData(externalDataList);

            AddExternalLeaveTypes(externalDataList);

            return externalDataList;
        }

        private void AddExternalLeaveTypes(List<ExternalData> externalDataList)
        {
            var typeOfEnum = typeof(LeaveType);
            var enumDictionary = Enum.GetValues(typeOfEnum).Cast<int>().ToDictionary(e => e, e => Enum.GetName(typeOfEnum, e));

            foreach (var leaveTypeEnum in enumDictionary)
            {
                var externalData = new ExternalData();
                externalData.Identifiers.Add(new Identifier { Entity = unimicro.IdentifierEntity.EmploymentLeaveType.ToString(), Property = unimicro.IdentifierProperty.Id.ToString(), Value = leaveTypeEnum.Key.ToString() });
                externalData.DataSet.Add(new Data { Code = PropertyName.Name.ToString(), Value = leaveTypeEnum.Value });
                externalDataList.Add(externalData);
            }
        }

        private async Task<Unit> SignIn(int unitId)
        {
            var unit = await _unitService.GetUnit(unitId);

            await _unimicroClient.SignIn();
            await _unimicroClient.GetAndSetCompanyAuthorizationInfo(unit.LegalOrganizationNumber);

            return unit;
        }

        private void CheckIfIsSupported(ExternalData externalData, unimicro.WorkRelation wr, unimicro.Employment employment, unimicro.Worker worker, unimicro.Employee employee)
        {
            externalData.ValidForUse = true;
            var supportedWorkPercent = 100d;
            var hoursInAWeek = 37.5d;
            var minutesInAnOur = 60.0d;
            var minutesInANormalWeek = (int)(hoursInAWeek * minutesInAnOur);

            if (wr == null)
            {
                externalData.ValidForUse = false;
                externalData.NotValidReasons.Add($"WorkRelation is not found");
                externalData.NotValidReasonsEnums.Add(((int)EmployeeError.WorkrelationMissingError).ToString());
            }
            else
            {
                if (wr.WorkPercentage != supportedWorkPercent)
                {
                    externalData.ValidForUse = false;
                    externalData.NotValidReasons.Add("Only a workpercentage of 100 is supported on workrelation");
                    externalData.NotValidReasonsEnums.Add(((int)EmployeeError.WorkPercentageError).ToString());
                }

                if (wr.WorkProfile?.MinutesPerWeek != minutesInANormalWeek)
                {
                    externalData.ValidForUse = false;
                    externalData.NotValidReasons.Add($"Only a normal week of {minutesInANormalWeek} minutes a week is supported");
                    externalData.NotValidReasonsEnums.Add(((int)EmployeeError.HoursOfWeekError).ToString());
                }
            }

            if (employment == null)
            {
                externalData.ValidForUse = false;
                externalData.NotValidReasons.Add($"Employment is not found");
                externalData.NotValidReasonsEnums.Add(((int)EmployeeError.EmploymentMissingError).ToString());
            }
            else
            {
                if (employment.WorkPercent != supportedWorkPercent)
                {
                    externalData.ValidForUse = false;
                    externalData.NotValidReasons.Add("Only a workpercentage of 100 is supported on employement");
                    externalData.NotValidReasonsEnums.Add(((int)EmployeeError.WorkPercentageError).ToString());
                }
                if (employment?.HoursPerWeek != hoursInAWeek)
                {
                    externalData.ValidForUse = false;
                    externalData.NotValidReasons.Add($"Only a normal week of {hoursInAWeek} hours a week is supported");
                    externalData.NotValidReasonsEnums.Add(((int)EmployeeError.HoursOfWeekError).ToString());
                }
            }

            if (worker == null)
            {
                externalData.ValidForUse = false;
                externalData.NotValidReasons.Add($"Worker related to Employee is not found");
                externalData.NotValidReasonsEnums.Add(((int)EmployeeError.WorkerMissingError).ToString());
            }
        }
        private void RemoveNullData(List<ExternalData> externalDataList)
        {
            foreach (var externalData in externalDataList)
            {
                externalData.Identifiers.RemoveAll(x => string.IsNullOrWhiteSpace(x.Value) || x.Value.Equals("0"));
                externalData.DataSet.RemoveAll(x => string.IsNullOrWhiteSpace(x.Value) || x.Value.Equals("0"));
            }
        }
    }
}