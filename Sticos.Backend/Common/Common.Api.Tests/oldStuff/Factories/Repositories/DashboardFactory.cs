using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Repositories.Models;

namespace Common.Api.Tests.Factories.Repositories
{
    public static class DashboardFactory
    {
        public static Faker<Dashboard> GetFactory(int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Dashboard>()
                .RuleFor(o => o.Title, f => f.Name.JobArea())
                .RuleFor(o => o.DashboardConfig, f => f.Lorem.Paragraph())
                .RuleFor(o => o.OwnerTypeId, f => f.Random.Number(1, 3));
        }

        public static Faker<Dashboard> GetFactory(Dictionary<string, object> overrides, int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Dashboard>()
                .RuleFor(o => o.Title,
                    f => overrides.ContainsKey("Title") ? overrides["Title"] as string : f.Name.JobArea())
                .RuleFor(o => o.DashboardConfig,
                    f => overrides.ContainsKey("DashboardConfig")
                        ? overrides["DashboardConfig"] as string
                        : f.Lorem.Paragraph())
                .RuleFor(o => o.OwnerTypeId,
                    f => overrides.ContainsKey("OwnerTypeId")
                        ? overrides["OwnerTypeId"] as int?
                        : f.Random.Number(1, 3))
                .RuleFor(o => o.OwnerId,
                    f => overrides.ContainsKey("OwnerId")
                        ? overrides["OwnerId"]
                        : f.Random.Number());
        }
    }
}