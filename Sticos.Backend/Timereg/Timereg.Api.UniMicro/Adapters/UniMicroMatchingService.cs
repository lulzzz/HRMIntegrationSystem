using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using commonContracts = Common.Api.Contracts;
using Common.Api.Contracts.Employees;
using Common.Api.Contracts.Services;
using Integrations.Api.Contracts;
using Timereg.Api.Domain.Interfaces;
using Shared.Contracts;
using Timereg.Api.Unimicro.Models;
using Sticos.Personal.MessageContracts.Enums;

namespace Timereg.Api.Unimicro.Adapters
{
    public class UniMicroMatchingService : IExternalMatchingService
    {
        private readonly IMapper _mapper;
        private readonly IExternalDataService _externalDataService;
        private readonly IEmployeeService _employeeService;
        private readonly IUnitService _unitService;
        private readonly IAbsenceTypeService _absenceTypeService;

        public UniMicroMatchingService(IMapper mapper, UniMicroExternalDataService externalDataService,
            IEmployeeService employeeService,
            IUnitService unitService,
            IAbsenceTypeService absenceTypeService)
        {
            _mapper = mapper;
            _externalDataService = externalDataService;
            _employeeService = employeeService;
            _unitService = unitService;
            _absenceTypeService = absenceTypeService;
        }
        public async Task<IEnumerable<EntityMatch>> MatchEmployeeData(int unitId, int[] ids)
        {
            var externalEmployees = await _externalDataService.GetExternalEmployeeData(unitId);
            var externalEmployeesMapped = _mapper.Map<IEnumerable<ExternalData>>(externalEmployees);
            var internalEmployees = await _employeeService.SearchEmployee(new SearchQueryEmployee { EmployeesIds = ids.ToList() });
            var factorRank = 10.0 / 4.0;
            var matchedData = new List<EntityMatch>();

            foreach (var employee in internalEmployees)
            {

                var bestFactor = 0.0;
                var bestMatch = new ExternalData();
                var matchEntity = new EntityMatch()
                {
                    EntityId = employee.Id,
                    EntityMap = EntityType.Employee.ToString(),
                };
                foreach (var externalEmployee in externalEmployeesMapped)
                {
                    var factor = 0.0;
                    if (externalEmployee.DataSet.Any(x => !string.IsNullOrWhiteSpace(x.Value)
                                                            && !string.IsNullOrWhiteSpace(employee.FirstName)
                                                            && x.Value.Contains(employee.FirstName)))
                        factor += factorRank;
                    if (externalEmployee.DataSet.Any(x => !string.IsNullOrWhiteSpace(x.Value)
                                                          && !string.IsNullOrWhiteSpace(employee.Email)
                                                          && x.Value.Contains(employee.Email)))
                        factor += factorRank;
                    if (externalEmployee.DataSet.Any(x => !string.IsNullOrWhiteSpace(x.Value)
                                                          && !string.IsNullOrWhiteSpace(employee.LastName)
                                                          && x.Value.Contains(employee.LastName)))
                        factor += factorRank;
                    if (externalEmployee.DataSet.Any(x => !string.IsNullOrWhiteSpace(x.Value)
                                                          && !string.IsNullOrWhiteSpace(employee.JobTitle)
                                                          && x.Value.Contains(employee.JobTitle)))
                        factor += factorRank;

                    if (factor > bestFactor)
                    {
                        bestFactor = factor;
                        bestMatch = externalEmployee;
                    }
                }
                if (bestFactor != 0)
                {
                    matchEntity.ExternalData = bestMatch;
                    matchEntity.MatchFactor = bestFactor;
                }
                matchedData.Add(matchEntity);
            }
            return matchedData;
        }

        public async Task<IEnumerable<EntityMatch>> MatchUnitData(int unitId, int[] ids)
        {
            var externalUnits = await _externalDataService.GetExternalUnitData(unitId);
            var externalUnitsMapped = _mapper.Map<IEnumerable<ExternalData>>(externalUnits);
            var internalUnits = await _unitService.SearchUnits(new commonContracts.SearchQueryUnit { UnitIds = ids.ToList() });
            var factorRank = 10.0 / 2.0;
            var matchedData = new List<EntityMatch>();

            foreach (var unit in internalUnits)
            {

                var bestFactor = 0.0;
                var bestMatch = new ExternalData();
                var matchEntity = new EntityMatch()
                {
                    EntityId = unit.Id,
                    EntityMap = EntityType.Unit.ToString(),
                };
                foreach (var externalEmployee in externalUnitsMapped)
                {
                    var factor = 0.0;
                    if (externalEmployee.DataSet.Any(x => x.Value.Contains(unit.Name)))
                        factor += factorRank;
                    if (!string.IsNullOrWhiteSpace(unit.BusinessOrganizationNumber) && externalEmployee.DataSet.Any(x => x.Value.Contains(unit.BusinessOrganizationNumber)))
                        factor += factorRank;

                    if (factor > bestFactor)
                    {
                        bestFactor = factor;
                        bestMatch = externalEmployee;
                    }
                }
                if (bestFactor != 0)
                {
                    matchEntity.ExternalData = bestMatch;
                    matchEntity.MatchFactor = bestFactor;
                }
                matchedData.Add(matchEntity);
            }
            return matchedData;
        }

        public async Task<IEnumerable<EntityMatch>> MatchAbsenceCodeData(int unitId, int[] ids)
        {

            var externalAbsenceCodes = await _externalDataService.GetExternalAbsenceCodeData(unitId);
            var externalAbsenceCodesMapped = _mapper.Map<IEnumerable<ExternalData>>(externalAbsenceCodes);

            var matchedData = GetPremappedAbsenceType(externalAbsenceCodes);

            matchedData = matchedData.Where(x => ids.Contains(x.EntityId)).ToList();
            return matchedData;
        }

        private List<EntityMatch> GetPremappedAbsenceType(IEnumerable<ExternalData> externalAbsenceCodes)
        {
            var matchedAbsenceTypes = new List<EntityMatch>();

            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "6", 200);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "3", 201);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "1", 202);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "5", 203);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "1", 204);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "1", 205);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "4", 206);
            FindAndAddExternalLeaveType(matchedAbsenceTypes, externalAbsenceCodes, "6", 207);

            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Ferie", 100);
            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Fri", 101);
            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Diverse", 300);
            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Møte", 301);
            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Møte", 302);
            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Diverse", 303);
            FindAndAddExternalAbsenceType(matchedAbsenceTypes, externalAbsenceCodes, "Fri", 400);

            return matchedAbsenceTypes;
        }

        private void FindAndAddExternalAbsenceType(List<EntityMatch> matchedAbsenceTypes, IEnumerable<ExternalData> externalAbsenceCodes,string name, int entityId)
        {
            var absenceType = EntityType.AbsenceType.ToString();
            var externalData = externalAbsenceCodes.Where(x => x.DataSet.Any(y => y.Value == name)).ToList();

            if(externalData.Count == 1)
            {
                matchedAbsenceTypes.Add(new EntityMatch { EntityId = entityId, EntityMap = absenceType, ExternalData = externalData.First() });
            }
        }

        private void FindAndAddExternalLeaveType(List<EntityMatch> matchedAbsenceTypes, IEnumerable<ExternalData> externalAbsenceCodes, string id, int entityId)
        {
            var absenceType = EntityType.AbsenceType.ToString();
            var externalData = externalAbsenceCodes.Where(x => x.Identifiers.Any(y => y.Entity == IdentifierEntity.EmploymentLeaveType.ToString() && y.Value == id));
            matchedAbsenceTypes.Add(new EntityMatch { EntityId = entityId, EntityMap = absenceType, ExternalData = externalData.First() });
        }
    }
}