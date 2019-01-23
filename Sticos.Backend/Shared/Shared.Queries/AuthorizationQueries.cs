using Dapper;
using Shared.Domain.Enums;
using Shared.Domain.ValueObjects.Queries;
using Shared.Interfaces;
using Shared.Interfaces.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared.Queries
{
    public class AuthorizationQueries : IAuthorizationQueries
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public AuthorizationQueries(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public IEnumerable<UnitPermission> GetUnitPermissionsByUserId(int userId, int? unitId = null, params PermissionType[] permissionTypes)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            if (unitId.HasValue && (unitId.Value == 0 || unitId.Value < UnitConstants.MasterUnitId)) throw new ArgumentOutOfRangeException(nameof(unitId));

            if (permissionTypes == null)
            {
                permissionTypes = new PermissionType[] { };
            }

            var whereClauses = new List<string> { "AR.UserId = @userId" };
            if (permissionTypes.Any())
            {
                whereClauses.Add("RR.RettighetId in @permissionTypes");
            }
            if (unitId.GetValueOrDefault(0) != 0)
            {
                whereClauses.Add("AR.EnhetId = @unitId");
            }

            using (var conn = _dbConnectionFactory.GetTenant())
            {
                var sql = $@"select distinct AR.EnhetId [UnitId]
                                  , RR.RettighetId [PermissionType]
                               from AnsattRolle AR
                               left join {_dbConnectionFactory.GetCoreDbName()}.dbo.RolleRettighet RR on AR.RolleId = RR.RolleId
                              where { string.Join(" AND ", whereClauses) }";

                return conn.Query<UnitPermission>(sql, new { userId, unitId = unitId.GetValueOrDefault(0), permissionTypes = permissionTypes.Select(x => (int)x) });
            }
        }
    }
}
