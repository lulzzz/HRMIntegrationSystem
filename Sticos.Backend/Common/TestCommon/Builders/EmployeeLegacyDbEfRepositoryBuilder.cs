using AutoMapper;
using Common.Api.Domain.Interfaces.Employees;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Repositories;
using FakeItEasy;
using Shared.Interfaces;


namespace TestCommon.Builders
{
    public class EmployeeLegacyDbEfRepositoryBuilder
    {
        private IDbContextFactory<PersonalLegacyContext> _dbFactory = A.Fake<IDbContextFactory<PersonalLegacyContext>>();
        private IDbContextFactory<PersonalCommonLegacyContext> _dbCommonFactory = A.Fake<IDbContextFactory<PersonalCommonLegacyContext>>();

        public EmployeeLegacyDbEfRepositoryBuilder WithDbFactory(IDbContextFactory<PersonalLegacyContext> dbFactory)
        {
            _dbFactory = dbFactory;
            return this;
        }
        public EmployeeLegacyDbEfRepositoryBuilder WithDbCommonFactory(IDbContextFactory<PersonalCommonLegacyContext> dbCommonFactory)
        {
            _dbCommonFactory = dbCommonFactory;
            return this;
        }

        public IEmployeeRepository Build()
        {
            return new EmployeeLegacyDbEfRepository(_dbFactory,_dbCommonFactory);
        }
    }
}