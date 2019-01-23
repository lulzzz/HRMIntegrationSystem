using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;

namespace Common.Api.Repositories.Legacy.Repositories
{
    public class UnitLegacyDbEFRepository : IUnitRepository
    {
        private readonly IDbContextFactory<PersonalLegacyContext> _dbContextFactory;
        private readonly IMapper _mapper;

        public UnitLegacyDbEFRepository(IDbContextFactory<PersonalLegacyContext> dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }
        public async Task<IList<Domain.Entities.Unit>> Search(SearchQueryUnit searchQuery)
        {
            using (var context = await _dbContextFactory.CreateDbContext())
            {
                var query = context.Units
                    .Include(unit => unit.OrgUnitVerifications)
                    .Where(u => !u.IsDeleted);
                    

                if (searchQuery.UnitTypes.Any())
                {
                    query = query.Where(u => searchQuery.UnitTypes.Contains((int) u.Type));
                }

                if (searchQuery.UnitIds.Any())
                {
                    query = query.Where(u => searchQuery.UnitIds.Contains(u.Id));
                }

                var units = query
                    .OrderBy(u => u.Name)
                    .Skip(searchQuery.Skip ?? SearchConstants.DEFAULT_SKIP)
                    .Take(searchQuery.Take ?? SearchConstants.DEFAULT_TAKE)
                    .AsNoTracking()
                    .ToList();
                
                return _mapper.Map<IList<Domain.Entities.Unit>>(units);
            }
        }

        public async Task<Domain.Entities.Unit> GetUnit(int id)
        {
            using (var context = await _dbContextFactory.CreateDbContext())
            {
                IQueryable<Models.Unit> query =  context.Units
                    .Include(u => u.OrgUnitVerifications);

                var unit = query
                    .Where(u => !u.IsDeleted)
                    .AsNoTracking()
                    .FirstOrDefault(u=>u.Id==id);

                if (unit == null) return null;
                return _mapper.Map<Domain.Entities.Unit>(unit);
            }
        }
    }
}