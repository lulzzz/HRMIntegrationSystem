using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Domain.Entities;

namespace Common.Api.Tests.Factories.Domain
{
    public static class NotificationFactory
    {
        public static Faker<Notification> GetFactory(int startId = 1, int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Notification>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.Title, f => f.Company.CatchPhrase())
                .RuleFor(o => o.Body, f => f.Lorem.Paragraphs(f.Random.Int(1, 5)))
                .RuleFor(o => o.Date, f => f.Date.Past());
        }

        public static Faker<Notification> GetFactory(Dictionary<string, object> overrides, int startId = 1,
            int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Notification>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.Title,
                    f => overrides.ContainsKey("Title") ? overrides["Title"] as string : f.Company.CatchPhrase())
                .RuleFor(o => o.Body,
                    f => overrides.ContainsKey("Body")
                        ? overrides["Body"] as string
                        : f.Lorem.Paragraphs(f.Random.Int(1, 5)))
                .RuleFor(o => o.Date,
                    f => overrides.ContainsKey("Date") ? overrides["Body"] as DateTimeOffset? : f.Date.Past());
        }
    }
}