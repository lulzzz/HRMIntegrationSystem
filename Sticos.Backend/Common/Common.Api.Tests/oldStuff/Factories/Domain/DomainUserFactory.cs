using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Domain.Entities;

namespace Common.Api.Tests.Factories.Domain
{
    public static class DomainUserFactory
    {
        public static Faker<User> GetFactory(int startId = 1, int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<User>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                .RuleFor(o => o.LastName, f => f.Name.LastName())
                .RuleFor(o => o.Email, f => f.Internet.Email())
                .RuleFor(o => o.Location, f => f.Address.City())
                .RuleFor(o => o.JobPosition, f => f.Name.JobTitle())
                .RuleFor(o => o.Image, f => f.Image.Business());
        }

        public static Faker<User> GetFactory(Dictionary<string, object> overrides, int startId = 1,
            int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<User>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.FirstName,
                    f => overrides.ContainsKey("FirstName") ? overrides["FirstName"] as string : f.Name.FirstName())
                .RuleFor(o => o.LastName,
                    f => overrides.ContainsKey("LastName") ? overrides["LastName"] as string : f.Name.LastName())
                .RuleFor(o => o.Email,
                    f => overrides.ContainsKey("Email") ? overrides["Email"] as string : f.Internet.Email())
                .RuleFor(o => o.Location,
                    f => overrides.ContainsKey("Location") ? overrides["Location"] as string : f.Address.City())
                .RuleFor(o => o.JobPosition,
                    f => overrides.ContainsKey("JobPosition") ? overrides["JobPosition"] as string : f.Name.JobTitle())
                .RuleFor(o => o.Image,
                    f => overrides.ContainsKey("Image") ? overrides["Image"] as string : f.Image.Business());
        }
    }
}