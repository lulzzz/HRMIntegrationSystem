using System;
using System.Linq;
using System.Threading.Tasks;
using Timereg.Api.Domain.Exceptions;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Integrations.Api.Contracts.Services;
using Integrations.Api.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Timereg.Api.Domain.Validators.Interfaces;
using Timereg.Api.Services.Extensions;
using Newtonsoft.Json;
using Shared.Exceptions;
using Common.Api.Contracts.Services;

namespace Timereg.Api.Services.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IExternalSystemFactory _externalSystemFactory;
        private readonly IAbsenceExportService _absenceExportService;
        private readonly IIntegrationService _integrationService;
        private readonly IExternalSystemValidatorFactory _externalSystemValidatorFactory;
        private readonly IUnitService _unitService;

        private readonly TimeregIntegrationValidator _integrationValidator;

        public ILogger<AbsenceService> Logger { get; set; }

        public AbsenceService(IExternalSystemFactory externalSystemFactory,
            IIntegrationService integrationService,
            IAbsenceExportService absenceExportService,
            IExternalSystemValidatorFactory externalSystemValidatorFactory,
            IUnitService unitService)
        {
            _externalSystemFactory = externalSystemFactory;
            _integrationService = integrationService;
            _absenceExportService = absenceExportService;
            _externalSystemValidatorFactory = externalSystemValidatorFactory;
            _unitService = unitService;
            Logger = NullLogger<AbsenceService>.Instance;

            _integrationValidator = new TimeregIntegrationValidator();
        }

        public async Task DeleteAbsence(Absence absence)
        {
            var integration = await GetIntegrationRecursive(absence.UnitId);
            var validationResult = await Validate(absence, integration);
            if (validationResult.SkipFurtherProcessing)
            {
                Logger.LogInformation($"{validationResult.Message}. Absence will not be processed. UnitId:{absence?.UnitId}");
                return;
            }

            var externalSystem = (ExternalEconomySystem)integration.ExternalSystem;
            var absenceExport = await GetExistingAbsenceExport(absence.UnitId, absence.LocalId);
            if (absenceExport == null)
            {
                return;
            }
            try
            {
                var externalSystemAdapter = _externalSystemFactory.CreateSystemAdapter(externalSystem);
                absenceExport.Action = AbsenceExportAction.Delete;
                await externalSystemAdapter.DeleteAbsence(integration.UnitId, absenceExport.Absence);
                absenceExport.Status = AbsenceExportStatus.Success;
            }
            catch (ExternalSystemCommunicationException ex)
            {
                absenceExport.Status = AbsenceExportStatus.Failed;
                absenceExport.Message = $"Communication with external system failed: {ex.Message}";
                Logger.LogInformation($"Communication with external system failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                absenceExport.Status = AbsenceExportStatus.Failed;
                absenceExport.Message = ex.Message;
                Logger.LogInformation(ex.Message);
            }
            finally
            {
                await _absenceExportService.UpdateAbsenceExport(absenceExport);
            }
        }

        public async Task ExportAbsence(Absence absence)
        {
            var integration = await GetIntegrationRecursive(absence.UnitId);
            
            var validationResult = await Validate(absence, integration);
            if (validationResult.SkipFurtherProcessing)
            {
                Logger.LogInformation($"{validationResult.Message}. Absence will not be processed. UnitId:{absence?.UnitId}");
                return;
            }

            var externalSystem = (ExternalEconomySystem)integration.ExternalSystem;
            var absenceExport = new AbsenceExport(absence);

            try
            {
                if (!validationResult.IsValid)
                {
                    absenceExport.Status = AbsenceExportStatus.Failed;
                    absenceExport.Message = validationResult.Message;
                    return;
                }

                var externalSystemAdapter = _externalSystemFactory.CreateSystemAdapter(externalSystem);

                var existingAbsenceExport = await GetExistingAbsenceExport(absence.UnitId, absence.LocalId);
                if (existingAbsenceExport != null)
                {
                    existingAbsenceExport.Status = AbsenceExportStatus.Obsolete;
                    await _absenceExportService.UpdateAbsenceExport(existingAbsenceExport);
                    absenceExport.Action = AbsenceExportAction.Update;
                    await externalSystemAdapter.DeleteAbsence(integration.UnitId, existingAbsenceExport.Absence);
                }

                absenceExport.Absence = await externalSystemAdapter.ExportAbsence(integration.UnitId,absence);
                absenceExport.Status = AbsenceExportStatus.Success;
                absenceExport.Message = "Absence successfully exported";
            }
            catch (ExternalSystemCommunicationException ex)
            {
                absenceExport.Status = AbsenceExportStatus.Failed;
                absenceExport.Message = $"Communication with external system failed: {ex.Message}";
                Logger.LogInformation($"Communication with external system failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                absenceExport.Status = AbsenceExportStatus.Failed;
                absenceExport.Message = ex.Message;
                Logger.LogInformation(ex.Message);
            }
            finally
            {
                await _absenceExportService.CreateAbsenceExport(absenceExport);
            }
        }

        private async Task<Integration> GetIntegrationRecursive(int unitId)
        {
            var integration = await _integrationService.GetTimeregIntegration(unitId);
            if(integration == null)
            {
                var unit = await _unitService.GetUnit(unitId);
                if (unit != null && unit.ParentId.HasValue && unit.ParentId.Value > 0)
                {
                    integration = await GetIntegrationRecursive(unit.ParentId.Value);
                }
            }
            return integration;
        }

        public async Task Resend(string absenceExportId)
        {
            var query = new SearchQueryAbsenceExport()
            {
                 Id = absenceExportId
            };
            var absenceExport = (await _absenceExportService.Search(query)).FirstOrDefault();

            if (absenceExport == null)
            {
                throw new NotFoundException("Absence doesn't exist");
            }
            
            var absenceResult = JsonConvert.DeserializeObject<Absence>(absenceExport.AbsenceJson);

            var integration = await GetIntegrationRecursive(absenceResult.UnitId);
            var validationResult = await Validate(absenceResult, integration);

            var externalSystem = (ExternalEconomySystem)integration.ExternalSystem;
            var externalSystemAdapter = _externalSystemFactory.CreateSystemAdapter(externalSystem);

            try
            {
                if (!validationResult.IsValid)
                {
                    absenceExport.Status = AbsenceExportStatus.Failed;
                    absenceExport.Message = validationResult.Message;
                    return;
                }

                absenceExport.Absence = await externalSystemAdapter.ResendAbsence(integration.UnitId, absenceResult);
                absenceExport.Status = AbsenceExportStatus.Success;
                absenceExport.Message = "Absence successfully exported";
              
            }catch(ExternalSystemCommunicationException ex )
            {
                absenceExport.Status = AbsenceExportStatus.Failed;
                absenceExport.Message = $"Communication with external system failed: {ex.Message}";

            }
            catch (Exception ex)
            {
                absenceExport.Status = AbsenceExportStatus.Failed;
                absenceExport.Message = ex.Message;
            }
            finally
            {
                await _absenceExportService.UpdateAbsenceExport(absenceExport);
            }
        }


        private async Task<ITimeregValidationResult> Validate(Absence absence, Integration integration)
        {
            var validationResult = _integrationValidator.Validate(integration);

            if (validationResult.SkipFurtherProcessing)
            {
                return validationResult;
            }
            else if(validationResult.IsValid)
            {
                var validatorFactory = _externalSystemValidatorFactory.Create(integration.ExternalSystem);
                validationResult = await validatorFactory.ValidateExportAbsence(integration.UnitId, absence);
            }
            return validationResult;
        }

        private async Task<AbsenceExport> GetExistingAbsenceExport(int unitId, int localId)
        {
            var searchQueryAbsenceExport = new SearchQueryAbsenceExport(unitId) { LocalId = localId };
            var existingAbsenceExports = await _absenceExportService.Search(searchQueryAbsenceExport);

            return existingAbsenceExports.OrderByDescending(arg => arg.UpdateAt ?? arg.CreatedAt).FirstOrDefault();
        }
    }
}
