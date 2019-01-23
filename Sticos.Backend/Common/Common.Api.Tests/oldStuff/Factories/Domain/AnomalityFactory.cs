using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Domain.Entities;

namespace Common.Api.Tests.Factories.Domain
{
    public static class AnomalityFactory
    {
        private static readonly List<string> Location = new List<string>
        {
            "Toalett",
            "Room",
            "Hallway"
        };

        private static readonly List<string> Status = new List<string>
        {
            "Open",
            "OnGoing",
            "Closed"
        };

        public static Faker<Anomaly> GetFactory(int startId = 1, int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Anomaly>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.Date, f => f.Date.Past())
                .RuleFor(o => o.Location, f => f.PickRandom(Location))
                .RuleFor(o => o.Description, f => f.Lorem.Sentences(f.Random.Number(1, 5)))
                .RuleFor(o => o.Responsible, f => f.Name.FullName())
                .RuleFor(o => o.Deadline, f => f.Date.Future())
                .RuleFor(o => o.Status, f => f.PickRandom(Status));
        }

        public static Faker<Anomaly> GetFactory(Dictionary<string, object> overrides, int startId = 1,
            int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Anomaly>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.Date,
                    f => overrides.ContainsKey("Date") ? overrides["Date"] as DateTimeOffset? : f.Date.Past())
                .RuleFor(o => o.Location,
                    f => overrides.ContainsKey("Location") ? overrides["Location"] as string : f.PickRandom(Location))
                .RuleFor(o => o.Description,
                    f => overrides.ContainsKey("Description")
                        ? overrides["Description"] as string
                        : f.Lorem.Sentences(f.Random.Number(1, 5)))
                .RuleFor(o => o.Responsible,
                    f => overrides.ContainsKey("Responsible") ? overrides["Responsible"] as string : f.Name.FullName())
                .RuleFor(o => o.Deadline,
                    f => overrides.ContainsKey("Deadline") ? overrides["Deadline"] as DateTimeOffset? : f.Date.Future())
                .RuleFor(o => o.Status,
                    f => overrides.ContainsKey("Status") ? overrides["Status"] as string : f.PickRandom(Status));
        }
    }
}