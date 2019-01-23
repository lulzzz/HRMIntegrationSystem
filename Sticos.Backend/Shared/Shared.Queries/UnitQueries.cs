using Dapper;
using Shared.Domain.ValueObjects.Queries;
using Shared.Interfaces;
using Shared.Interfaces.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Queries
{
    public class UnitQueries : IUnitQueries
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public UnitQueries(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public async Task<IEnumerable<UnitWithParent>> GetHierarchyUp(int fromUnitId)
        {
            using (var conn = _dbConnectionFactory.GetTenant())
            {
                var sql = $@"
                            WITH Hierarchy as
                            (
	                            SELECT 1 AS relationLevel, child.Id, child.Navn, child.Parent, child.[Type]
	                            FROM  Enhet child
	                            WHERE Id = {fromUnitId}
	                            AND (child.ErSlettet is null OR child.ErSlettet = 0)
	                            UNION ALL
	                            SELECT relationLevel+1, parent.Id, parent.Navn, parent.Parent, parent.[Type]
	                            FROM  Enhet parent
	                            INNER JOIN Hierarchy OUList ON OUList.Parent = parent.Id
	                            AND parent.ErSlettet = 0
                            )
                            select Id, Navn as Name, Parent as ParentId, [Type] 
                            from  Hierarchy
                            order by relationLevel,Id
                            ";



                return await conn.QueryAsync<UnitWithParent>(sql, new { unitId = fromUnitId });
            }
        }
        public async Task<IEnumerable<UnitWithParent>> GetHierarchyDown(int fromUnitId)
        {
            using (var conn = _dbConnectionFactory.GetTenant())
            {
                var sql = $@"
                            WITH Hierarchy AS
                            (
                              SELECT 1 AS relationLevel, parent.Id, parent.Navn, parent.Parent, parent.[Type]
                              FROM  [Enhet] parent
                              WHERE parent.Id = {fromUnitId}
                              AND (parent.ErSlettet is null OR parent.ErSlettet = 0)
                              UNION ALL
                              SELECT relationLevel+1, child.Id, child.Navn, child.Parent, child.[Type]
                              FROM Hierarchy parent
                              INNER JOIN [Enhet] child ON child.parent = parent.Id
                              WHERE child.ErSlettet = 0
                            )
                            select Id, Navn as Name, Parent as ParentId, [Type] 
                            from  Hierarchy
                            order by relationLevel,Id
                            OPTION(MAXRECURSION 100)
                            ";



                return await conn.QueryAsync<UnitWithParent>(sql, new { unitId = fromUnitId });
            }
        }

        public async Task<IEnumerable<UnitWithParent>> GetByIdList(IEnumerable<int> idList)
        {
            using (var conn = _dbConnectionFactory.GetTenant())
            {
                var sql = $@"select Id
                                  , Parent as [ParentId]
                                  , [Type] from Enhet E 
                              where Id in @idList 
                                and ErSlettet = 0";

                return await conn.QueryAsync<UnitWithParent>(sql, new { idList });
            }
        }
    }
}
