using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Repositories.Legacy.Models;

namespace TestCommon.DataFactories
{
    public static class UnitFactory
    {
        public static Faker<Unit> GetCompanyFactory(int parentRegionId)
        {
            var generationId = Guid.NewGuid().ToString("N");
            int seedId = 8675309;
            Randomizer.Seed = new Random(seedId);
            return new Faker<Unit>()
                    .RuleFor(o => o.Type, f => UnitType.Enhet)
                    .RuleFor(o => o.IsDeleted, f => f.Random.Bool(0.2f))
                    .RuleFor(o => o.PhysicalAndChemicalEnvironment, f =>"GenerationId: "+generationId)
                    .RuleFor(o => o.ParentId, f => parentRegionId)
                    .RuleFor(o => o.Name, f => f.Commerce.Department())
                ;
        }
        public static Faker<Unit> GetDepartmentFactory(List<int> potentialParents)
        {
            var generationId = Guid.NewGuid().ToString("N");
            int seedId = 8675309;
            Randomizer.Seed = new Random(seedId);
            return new Faker<Unit>()
                    .RuleFor(o => o.Type, f => UnitType.Avdeling)
                    .RuleFor(o => o.IsDeleted, f => f.Random.Bool(0.2f))
                    .RuleFor(o => o.Name, f => f.Commerce.Department())
                    .RuleFor(o => o.PhysicalAndChemicalEnvironment, f =>"GenerationId: "+generationId)
                    .RuleFor(o => o.ParentId, f => f.Random.ArrayElement(potentialParents.ToArray()))
                ;
        }
    }
}