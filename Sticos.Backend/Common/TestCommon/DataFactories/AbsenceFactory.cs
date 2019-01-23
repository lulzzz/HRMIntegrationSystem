using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Common.Api.Repositories.Legacy.Models;

namespace TestCommon.DataFactories
{
    public static class AbsenceFactory
    {
        public static Faker<Absence> GetFactory(List<int> potentialEmployeeIds, int year, int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);

            var typeIds = new Dictionary<int, List<int>>
            {
                {0, new List<int> {3}},
                {1, new List<int> {100, 101}},
                {2, new List<int> {200, 203}},
                {3, new List<int> {300, 301}}
            };

            var typeIndex = new Random().Next(0, typeIds.Count);

            var startOfYear = new DateTime(year, 1, 1);
            var endofYear = new DateTime(year,12,31);

            return new Faker<Absence>()
                    .RuleFor(o => o.EmployeeId, f => f.Random.ArrayElement(potentialEmployeeIds.ToArray()))
                    .RuleFor(o => o.From, f => f.Random.Bool(0.9f) ? f.Date.Between(startOfYear,endofYear).Date: f.Date.Between(startOfYear,endofYear))
                    .RuleFor(o => o.Status, f => f.Random.Bool(0.8f) ? 1 : 0)
                    .RuleFor(o => o.Type, f => typeIds.ElementAt(typeIndex).Key)
                    .RuleFor(o => o.SubType, f => f.Random.ArrayElement(typeIds.ElementAt(typeIndex).Value.ToArray()))
                    .FinishWith((f, t) =>
                    {
                        t.To = t.From.AddDays(f.Random.Int(1, 7));
                        t.CreatedAt = t.From.AddDays(-1);
                        t.FromAndToHasTime = t.From > t.From.Date && t.To > t.To.Date;
                    })
                ;
        }
    }
}