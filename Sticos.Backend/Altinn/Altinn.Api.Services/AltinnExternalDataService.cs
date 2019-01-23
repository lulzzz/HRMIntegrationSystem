using Altinn.Api.Client.HttpClients;
using Altinn.Api.Client.Models;
using Altinn.Api.Domain.Interfaces;
using Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altinn.Api.Services
{
    public class AltinnExternalDataService : IExternalDataService
    {
        private IAltinnClient _altinnClient;

        public AltinnExternalDataService(IAltinnClient altinnService)
        {
            _altinnClient = altinnService;
        }

        public async Task<IEnumerable<ExternalData>> GetExternalReportees()
        {
            var externalDataList = new List<ExternalData>();

            var reportees = await _altinnClient.GetReportees();
            foreach (var reportee in reportees)
            {
                var externalData = new ExternalData();

                externalData.Identifiers.Add(new Identifier { Entity = IdentifierEntity.Reportee.ToString(), Property = IdentifierProperty.ReporteeId.ToString(), Value = reportee.ReporteeId });

                externalData.DataSet.Add(new Data { Code = PropertyName.Name.ToString(), Value = reportee.Name });
                externalData.DataSet.Add(new Data { Code = PropertyName.Type.ToString(), Value = reportee.Type });
                externalData.DataSet.Add(new Data { Code = PropertyName.Status.ToString(), Value = reportee.Status });
                externalData.DataSet.Add(new Data { Code = PropertyName.OrganizationNumber.ToString(), Value = reportee.OrganizationNumber });

                externalDataList.Add(externalData);
            }
            RemoveNullData(externalDataList);
            return externalDataList;
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
