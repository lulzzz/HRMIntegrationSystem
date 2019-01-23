using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Interfaces;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Repositories.Context;
using entities = Timereg.Api.Repositories.Models;
using Shared.Services.Services;

namespace Timereg.Api.Repositories.Repositories
{
    public class AbsenceExportRepository : IAbsenceExportRepository
    {
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<TimeregDbContext> _contextFactory;
        private readonly IAuthorizationContextService _authorizationContextService;

        public AbsenceExportRepository(IMapper mapper, 
            IDbContextFactory<TimeregDbContext> contextFactory,
            IAuthorizationContextService authorizationContextService
                                         )
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _authorizationContextService = authorizationContextService;
        }

        public async Task<AbsenceExport> Create(AbsenceExport entity)
        {
            var mappedEntity = _mapper.Map<AbsenceExport, entities.AbsenceExport>(entity);
            mappedEntity.CreatedAt = DateTimeOffset.Now;
            mappedEntity.CreatedBy = _authorizationContextService.GetUserIdFromClaims().ToString();
            using (var context = await _contextFactory.CreateDbContext())
            {
                context.AbsenceExports.Add(mappedEntity);
                await context.SaveChangesAsync();
                return entity;
            }
        }

        public async Task Delete(int unitId)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var absencesForDelete = await context.AbsenceExports.Where(x => x.UnitId == unitId).ToListAsync();
                context.AbsenceExports.RemoveRange(absencesForDelete);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployee(EmployeeDeleted message)
        {
            using(var context = await _contextFactory.CreateDbContext())
            {
                var absences = await context.AbsenceExports.Where(x => x.EmployeeId == message.EmployeeId 
                                                                  && x.UnitId == message.OrgUnitId).ToListAsync();
                if (!absences.Any()) return;
                context.AbsenceExports.RemoveRange(absences);
                await context.SaveChangesAsync();
            }
        }

        public async Task<AbsenceExport> GetSingle(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AbsenceExport>> Search(SearchQueryAbsenceExport searchQuery)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var query = context.AbsenceExports.AsQueryable();

                if(searchQuery.UnitId != 0 || searchQuery.UnitIds.Any())
                {
                     query = context.AbsenceExports.Where(x => (searchQuery.UnitId != 0 && x.UnitId == searchQuery.UnitId) || searchQuery.UnitIds.Contains(x.UnitId));
                }

                if (searchQuery.LocalId != 0)
                {
                    query = query.Where(x => x.LocalAbsenceId == searchQuery.LocalId);
                }

                if(!string.IsNullOrWhiteSpace(searchQuery.Id))
                {
                    query = query.Where(x => x.Id == searchQuery.Id);
                }

                var filterResult = await query.ToListAsync();
                var mappedResult = _mapper.Map<IEnumerable<entities.AbsenceExport>, IEnumerable<AbsenceExport>>(filterResult);
                return mappedResult;
            }
        }

        public async Task<AbsenceExport> Update(AbsenceExport entity)
        {
            var mappedEntity = _mapper.Map<AbsenceExport, entities.AbsenceExport>(entity);
            mappedEntity.UpdateAt = DateTimeOffset.Now;
            mappedEntity.UpdatedBy = this._authorizationContextService.GetUserIdFromClaims().ToString();
            using (var context = await _contextFactory.CreateDbContext())
            {
                var updatedEntity = context.AbsenceExports.Update(mappedEntity);
                await context.SaveChangesAsync();
                return entity;
            }
        }
    }
}
