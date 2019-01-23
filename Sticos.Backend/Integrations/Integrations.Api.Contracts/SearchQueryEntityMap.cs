using System;
using System.Collections.Generic;
using System.Text;

namespace Integrations.Api.Contracts
{
    public class SearchQueryEntityMap : SearchQueryBase
    {
        public int IntegrationId { get; set; }

        public int LocalId { get; set; }
        public string EntityName { get; set; }
        public string ExternalProperty { get; set; }
        public string ExternalEntity { get; set; }

        public int UnitId { get; set; }
        public int ExternalSystemId { get; set; }

        public int IntegrationCategory { get; set; }

        public SearchQueryEntityMap() { }
        public SearchQueryEntityMap(int localId, string entityName, int integrationId)
        {
            LocalId = localId;
            EntityName = entityName;
            IntegrationId = integrationId;
        }
        public SearchQueryEntityMap(int localId, string entityName, int unitId, int externalSystemId, int integrationCategory)
        {
            LocalId = localId;
            EntityName = entityName;
            UnitId = unitId;
            ExternalSystemId = externalSystemId;
            IntegrationCategory = integrationCategory;
        }
    }

    public enum EntityType
    {
        Unknown = 0,
        Employee = 1,
        Unit = 2,
        AbsenceType = 3,
    }
}
