using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;


namespace Common.Api.Repositories.Repositories
{
    public class OwnerTypeRepository : IRepository<OwnerType, SearchQueryOwnerType>
    {
        private readonly IDbContextFactory<SticosWidgetDbContext> _contextFactory;
        private readonly IMapper _mapper;

        public OwnerTypeRepository(IDbContextFactory<SticosWidgetDbContext> contextFactory, IMapper mapper)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
        }

        public async Task<OwnerType> Create(OwnerType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerType> Update(OwnerType entity)
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerType> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OwnerType> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<OwnerType>> Search(SearchQueryOwnerType searchQuery)
        {
            using (var context = await _contextFactory.CreateDbContext())
            {
                var query = context.OwnerTypes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(searchQuery.Name))
                    query = query.Where(x => x.Name.ToLower().Contains(searchQuery.Name.ToLower()));

                if (searchQuery.MinPriority.HasValue)
                    query = query.Where(x => x.Priority >= searchQuery.MinPriority.Value);

                if (searchQuery.MaxPriority.HasValue)
                    query = query.Where(x => x.Priority <= searchQuery.MaxPriority.Value);

                query = query
                    .Skip(searchQuery.Skip ?? SearchConstants.DEFAULT_SKIP)
                    .Take(searchQuery.Take ?? SearchConstants.DEFAULT_TAKE);

                var filteredDbOwnerTypes = await query.ToListAsync();
                var result = _mapper.Map<IEnumerable<OwnerType>>(filteredDbOwnerTypes);
                return result.ToList();
            }
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }
    }
}