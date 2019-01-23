using System;
using System.Collections.Generic;
using Bogus;
using Common.Api.Repositories.Legacy.Models;

namespace TestCommon.DataFactories
{
    public static class EmployeeFactory
    {
        public static Faker<Employee> GetFactory(bool randomIsDeleted=false)
        {
            return GetFactory(new List<int> {1}, randomIsDeleted);

        }
        public static Faker<Employee> GetFactory(List<int> potentialUnitIds, bool randomIsDeleted=true)
        {
            var generationId = Guid.NewGuid().ToString("N");
            var seedId = 8675309;
            Randomizer.Seed = new Random(seedId);
            var userId = 1;
            return new Faker<Employee>()
                    .RuleFor(o => o.UserId, f => userId++)
                    .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                    .RuleFor(o => o.LastName, f => f.Name.LastName())
                    .RuleFor(o => o.UnitId, f => f.Random.ArrayElement(potentialUnitIds.ToArray()))
                    .RuleFor(o => o.JobTitle, f => f.Name.JobTitle())
                    .RuleFor(o => o.Phone, f => f.Phone.PhoneNumber($"{f.Random.Int(90,99)}######"))
                    .RuleFor(o => o.Email, f => f.Internet.Email())
                    .RuleFor(o => o.IsDeleted, f => randomIsDeleted ? f.Random.Bool(0.2f) : false)
                    .RuleFor(o => o.Sex, f => f.Random.Int(1,2))
                    .RuleFor(o => o.Image, f => f.Image.Business())
                    .RuleFor(o => o.EmployeeStartDate, f => f.Date.Past(5))
                    .RuleFor(o => o.EmployeeEndDate, f => f.Date.Future(15, f.Date.Future(1)))
                    .RuleFor(o => o.Address, f => "GenerationId: "+generationId)
                ;
        }


        public static Faker<Employee> GetFactory(Dictionary<string, object> overrides, int startId = 1,
            int seedId = 8675309)
        {
            Randomizer.Seed = new Random(seedId);
            return new Faker<Employee>()
                .RuleFor(o => o.Id, f => startId++)
                .RuleFor(o => o.FirstName,
                    f => overrides.ContainsKey("FirstName") ? overrides["FirstName"] as string : f.Name.FirstName())
                .RuleFor(o => o.LastName,
                    f => overrides.ContainsKey("LastName") ? overrides["LastName"] as string : f.Name.LastName())
                .RuleFor(o => o.JobTitle,
                    f => overrides.ContainsKey("JobTitle") ? overrides["JobTitle"] as string : f.Name.JobTitle())
                .RuleFor(o => o.Phone,
                    f => overrides.ContainsKey("Phone") ? overrides["Phone"] as string : f.Phone.PhoneNumber())
                .RuleFor(o => o.Email,
                    f => overrides.ContainsKey("Email") ? overrides["Email"] as string : f.Internet.Email())
                .RuleFor(o => o.Image,
                    f => overrides.ContainsKey("Image") ? overrides["Image"] as string : f.Image.Business());
        }
    }
}