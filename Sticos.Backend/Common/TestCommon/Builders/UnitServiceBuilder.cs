using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Domain.Services;
using FakeItEasy;

namespace TestCommon.Builders
{
    public class UnitServiceBuilder
    {
        private IUnitRepository _unitRepository = A.Fake<IUnitRepository>();
        

        public UnitServiceBuilder WithCompanyRepository(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
            return this;
        }

        public IUnitService Build()
        {
            return new UnitService(_unitRepository);
        }
 
    }
}