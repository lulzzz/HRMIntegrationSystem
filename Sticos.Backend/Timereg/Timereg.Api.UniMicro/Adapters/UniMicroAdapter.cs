using AutoMapper;
using Common.Api.Contracts.Services;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Unimicro.Adapters.UniMicro;
using Timereg.Api.Unimicro.HttpClients;
using Timereg.Api.Unimicro.Models;
using contracts = Common.Api.Contracts;
using domain = Timereg.Api.Domain.Models;
using unimicro = Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Unimicro.Adapters
{
    public class UniMicroAdapter : IExternalSystemAdapter
    {
        private readonly IUnitService _unitService;
        private readonly IUnimicroClient _unimicroClient;
        private readonly IEntityMapService _entityMapService;
        private readonly IMapper _mapper;

        public UniMicroAdapter(
            IMapper mapper,
            IUnimicroClient unimicroClient,
            IUnitService unitService,
            IEntityMapService entityMapService)
        {
            _mapper = mapper;
            _unimicroClient = unimicroClient;
            _unitService = unitService;
            _entityMapService = entityMapService;
        }

        public async Task<Absence> ResendAbsence(int integrationUnitId, Absence absence)
        {
            await SignIn(integrationUnitId);

            foreach (var absenceEntry  in absence.AbsenceEntries)
            {
                var workTypeId = await _entityMapService.GetWorkTypeId(absenceEntry.LocalAbsenceCode, integrationUnitId);

                var isWorkItem = workTypeId != null;

                if (isWorkItem)
                {
                    var workRelationId = await _entityMapService.GetWorkRelationId(absence.EmployeeId, integrationUnitId);
                    absenceEntry.ExternalEntityId = workRelationId.ToString();
                    absenceEntry.ExternalAbsenceCode = workTypeId.ToString();

                    var workItem = new WorkItem
                    {
                        // From AbsenceDay 
                        StartTime = absenceEntry.StartTime,
                        EndTime = absenceEntry.EndTime,
                        WorkTypeId = workTypeId.Value,

                        // From Absence
                        UnitId = absence.UnitId,
                        WorkRelationId = workRelationId.Value,
                        Description = absence.Description,
                        LunchInMinutes = absence.LunchInMinutes
                    };
                    if(string.IsNullOrWhiteSpace(absenceEntry.ExternalId))
                    {
                        var externalId = await _unimicroClient.PostWorkItem(workItem);
                        absenceEntry.ExternalId = externalId.ToString();
                    }
                    else
                    {
                        workItem.Id = int.Parse(absenceEntry.ExternalId);
                        await _unimicroClient.PutWorkItem(workItem);
                    }
                } 
                else
                {
                    var employementLeaveId = await _entityMapService.GetEmploymentLeaveId(absenceEntry.LocalAbsenceCode, absence.UnitId);
                    var isEmploymentLeave = employementLeaveId != null;

                    if(isEmploymentLeave)
                    {
                        var employmentId = await _entityMapService.GetEmploymentId(absence.EmployeeId, integrationUnitId);
                        absenceEntry.ExternalEntityId = employmentId.ToString();
                        absenceEntry.ExternalAbsenceCode = employementLeaveId.ToString();

                        var employmentLeave = new EmploymentLeave()
                        {
                            EmploymentID = employmentId.Value,
                            LeaveType = Enum.Parse<LeaveType>(employementLeaveId.Value.ToString()),
                            FromDate = absenceEntry.StartTime.Date,
                            ToDate = absenceEntry.StartTime.Date,
                            LeavePercent = 100
                        };

                        if(string.IsNullOrWhiteSpace(absenceEntry.ExternalId))
                        {
                            var externalId = await _unimicroClient.PostEmploymentLeave(employmentLeave);
                            absenceEntry.ExternalId = externalId.ToString();
                        }
                        else
                        {
                            employmentLeave.Id = int.Parse(absenceEntry.ExternalId);
                            await _unimicroClient.PutEmploymentLeave(employmentLeave);
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Mapping is not found for absencecode={absenceEntry.LocalAbsenceCode}. This error should have been taken care of in pre-validation");
                    }
                }
            }
            return absence;
        }

        public async Task<Absence> ExportAbsence(int integrationUnitId, Absence absence)
        {
            await SignIn(integrationUnitId);

            foreach (var absenceEntry in absence.AbsenceEntries)
            {
                var workTypeId = await _entityMapService.GetWorkTypeId(absenceEntry.LocalAbsenceCode, integrationUnitId);

                var isWorkItem = workTypeId != null;
                
                if (isWorkItem)
                {
                    var workRelationId = await _entityMapService.GetWorkRelationId(absence.EmployeeId, integrationUnitId);
                    absenceEntry.ExternalEntityId = workRelationId.ToString();
                    absenceEntry.ExternalAbsenceCode = workTypeId.ToString();

                    var workItem = new WorkItem
                    {
                        // From AbsenceDay
                        StartTime = absenceEntry.StartTime,
                        EndTime = absenceEntry.EndTime,
                        WorkTypeId = workTypeId.Value,

                        // From Absence
                        UnitId = absence.UnitId,
                        WorkRelationId = workRelationId.Value,
                        Description = absence.Description,
                        LunchInMinutes = absence.LunchInMinutes,
                    };
                    var externalId = await _unimicroClient.PostWorkItem(workItem);
                    absenceEntry.ExternalId = externalId.ToString();
                }
                else
                {
                    var employmentLeaveId = await _entityMapService.GetEmploymentLeaveId(absenceEntry.LocalAbsenceCode, integrationUnitId);
                    var isEmploymentLeave = employmentLeaveId!=null;

                    if(isEmploymentLeave)
                    {
                        var employmentId = await _entityMapService.GetEmploymentId(absence.EmployeeId, integrationUnitId);
                        absenceEntry.ExternalEntityId = employmentId.ToString();
                        absenceEntry.ExternalAbsenceCode = employmentLeaveId.ToString();

                        var employmentLeave = new EmploymentLeave()
                        {
                            EmploymentID = employmentId.Value,
                            LeaveType = Enum.Parse<LeaveType>(employmentLeaveId.Value.ToString()),
                            FromDate = absenceEntry.StartTime.Date,
                            ToDate = absenceEntry.EndTime.Date,
                            LeavePercent = 100
                        };
                        var externalId = await _unimicroClient.PostEmploymentLeave(employmentLeave);
                        absenceEntry.ExternalId = externalId.ToString();
                    }
                    else
                    {
                        throw new ArgumentException($"Mapping is not found for absencecode={absenceEntry.LocalAbsenceCode}. This error should have been taken care of in pre-validation");
                    }
                }

                
                

            }
            return absence;
        }

        public async Task DeleteAbsence(int integrationUnitId, Absence absence)
        {
            await SignIn(integrationUnitId);
            foreach (var absenceEntry in absence.AbsenceEntries)
            {
                var workTypeId = await _entityMapService.GetWorkTypeId(absenceEntry.LocalAbsenceCode, integrationUnitId);
                var isWorkItem = workTypeId != null;
                
                if (isWorkItem)
                {
                    await _unimicroClient.DeleteWorkItem(absenceEntry.ExternalId);
                }
                else
                {
                    var employmentLeaveId = await _entityMapService.GetEmploymentLeaveId(absenceEntry.LocalAbsenceCode, integrationUnitId);
                    var isEmploymentLeave = employmentLeaveId!=null;

                    if (isEmploymentLeave)
                    {
                        await _unimicroClient.DeleteEmploymentLeave(absenceEntry.ExternalId);
                    }
                    else
                    {
                        throw new ArgumentException($"Mapping is not found for absencecode={absenceEntry.LocalAbsenceCode}. This error should have been taken care of in prevalidation");
                    }
                }
               
            }
        }
      
        public async Task<domain.HourBalance> GetHourBalance(int unitId, int employeeId)
        {
            await SignIn(unitId);
            var workRelationId = await _entityMapService.GetWorkRelationId(employeeId, unitId);
            var hourBalance = await _unimicroClient.GetHourBalance(workRelationId.Value);

            return _mapper.Map<unimicro.HourBalance, domain.HourBalance>(hourBalance);
        }

        private async Task SignIn(int unitId)
        {
            var unit = await _unitService.GetUnit(unitId);

            await _unimicroClient.SignIn();
            await _unimicroClient.GetAndSetCompanyAuthorizationInfo(unit.LegalOrganizationNumber);
        }
    }
}