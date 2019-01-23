using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Repositories.Legacy.Models;

namespace TestCommon.DataFactories
{
    public static class DbUserFactory
    {
        [Obsolete]
        public static Faker<User> GetFactory(int customerId, int startId = 1, int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<User>()
                .RuleFor(o => o.UserId, f => startId++)
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.Mobilephone, f => f.Phone.PhoneNumber())
                .RuleFor(o => o.UnitId, f => f.Random.Number(0, 1000))
                .RuleFor(o => o.CustomerId, customerId);
        }

        public static Faker<User> GetFactory(int customerId, List<int> potentialUnitIds, int userId,int seedId = 8675309)
        {
            var generationId = Guid.NewGuid().ToString("N");
            Randomizer.Seed = new Random(seedId);
            return new Faker<User>()
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                .RuleFor(o => o.FirstName, f =>"GenerationId: "+generationId)
                .RuleFor(o => o.LastName, f => f.Name.LastName())
                .RuleFor(o => o.CustomerId, customerId) 
                .RuleFor(o => o.IsActive, true)
                .RuleFor(o => o.Mobilephone, f => f.Phone.PhoneNumber($"{f.Random.Int(90,99)}######"))
                .RuleFor(o => o.UnitId, f => f.Random.ArrayElement(potentialUnitIds.ToArray()))
                .RuleFor(o => o.UserId, f=> userId)
                ;
        }
    }
}