using AutoMapper;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Repositories;
using FakeItEasy;
using Shared.Interfaces;


namespace TestCommon.Builders
{
    public class UnitCompanyLegacyDbEfRepositoryBuilder
    {
        private IDbContextFactory<PersonalLegacyContext> _dbFactory = A.Fake<IDbContextFactory<PersonalLegacyContext>>();
        private IMapper _mapper = A.Fake<IMapper>();


        public UnitCompanyLegacyDbEfRepositoryBuilder WithMapper(IMapper mapper)
        {
            _mapper = mapper;
            return this;
        }
        public UnitCompanyLegacyDbEfRepositoryBuilder WithDbFactory(IDbContextFactory<PersonalLegacyContext> dbFactory)
        {
            _dbFactory = dbFactory;
            return this;
        }

        public IUnitRepository Build()
        {
            return new UnitLegacyDbEFRepository(_dbFactory,_mapper);
        }
    }
}