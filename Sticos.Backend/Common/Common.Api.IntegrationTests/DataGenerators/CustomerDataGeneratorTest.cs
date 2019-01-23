using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Factories;
using Common.Api.Repositories.Legacy.Models;
using NUnit.Framework;
using TestCommon.Builders;
using TestCommon.DataFactories;

namespace Common.Api.IntegrationTests.DataGenerators
{
    [TestFixture]
    public class CustomerDataGeneratorTest
    {
        static int _regionIdHardcoded = 1;
        static int _customerId = 400423;

        private readonly PersonalLegacyContextFactory _customerDbFactory;
        private static PersonalCommonLegacyContextFactory _commonDbFactory;

        public CustomerDataGeneratorTest()
        {
            _customerDbFactory = new PersonalLegacyContextFactoryBuilder()
                .SetConnectionstringFormat($"Data Source=localhost;Initial Catalog=SticosPersonalKunde_{_customerId};Integrated Security=True;")
                .Build();

            _commonDbFactory = new PersonalCommonLegacyContextFactoryBuilder()
                .SetConnectionstringFormat($"Data Source=localhost;Initial Catalog=SticosPersonalFelles;Integrated Security=True;")
                .Build();
        }

        [Ignore("only for manual execution")]
        [Test]
        public async Task Generate_LotsOfVarious_Data()
        {
            var averageNumberOfAbsencesPerEmployeePerYear = 5;
            var numberOfCompanies = 5;
            var numberOfDepartments = 15;
            var numberOfEmployees = 1000;
            var yearForAbsences = DateTime.Now.Year;

            using (var context = await _customerDbFactory.CreateDbContext())
            {
                
                var companies = GenerateAndAddCompanies(numberOfCompanies, context);
                var potentiaCompanyIds = companies.Select(c => c.Id).ToList();
                var departments = GenerateAndAddDepartments(potentiaCompanyIds, numberOfDepartments, context);
                var potentialDepartmentIds = departments.Select(c => c.Id).ToList();
                var potentialUnitIds = potentiaCompanyIds.Union(potentialDepartmentIds).ToList();
                var employees = GenerateAndAddEmployees(potentialUnitIds, numberOfEmployees, context);
                var employeeIds = employees.Select(e => e.Id).ToList();
                GenerateAndAddAbsences(employeeIds,averageNumberOfAbsencesPerEmployeePerYear,yearForAbsences, context);
            }
        }

        [Ignore("only for manual execution")]
        [Test]
        public async Task GenerateAbsences()
        {
            var yearForAbsences = DateTime.Now.Year;
            var averageNumberOfAbsencesPerEmployeePerYear = 5;

            using (var context = await _customerDbFactory.CreateDbContext())
            {
                var employeeIds = context.Employees.Select(e => e.Id).ToList();
                GenerateAndAddAbsences(employeeIds,averageNumberOfAbsencesPerEmployeePerYear,yearForAbsences, context);
            }
        }
        
        [Ignore("only for manual execution")]
        [Test]
        public async Task GenerateCompanies()
        {
            var numberOfCompanies = 5;
            var numberOfDepartments = 15;
            using (var context = await _customerDbFactory.CreateDbContext())
            {
                var companies = GenerateAndAddCompanies(numberOfCompanies, context);
                var potentiaCompanyIds = companies.Select(c => c.Id).ToList();
                var departments = GenerateAndAddDepartments(potentiaCompanyIds, numberOfDepartments, context);
            }
        }
        
        [Ignore("only for manual execution")]
        [Test]
        public async Task GenerateEmployees()
        {
            var numberOfEmployees = 100;
            using (var context = await _customerDbFactory.CreateDbContext())
            {
                var potentialUnitIds = context.Units
                    .Where(u => u.Type == UnitType.Enhet || u.Type == UnitType.Avdeling)
                    .Select(u => u.Id).ToList();
                var employees = GenerateAndAddEmployees(potentialUnitIds, numberOfEmployees, context);
            }
        }


        private static List<Unit> GenerateAndAddCompanies(int numberOfCompanies, PersonalLegacyContext context)
        {
            var companies = UnitFactory.GetCompanyFactory(_regionIdHardcoded)
                .Generate(numberOfCompanies);

            context.Units.AddRange(companies);
            context.SaveChanges();
            var unitContentBooks = companies.SelectMany(c => new List<UnitContentBook>
            {
                new UnitContentBook {UnitId = c.Id, ContentBookId = 1},
                new UnitContentBook {UnitId = c.Id, ContentBookId = 2},
                new UnitContentBook {UnitId = c.Id, ContentBookId = 8},
            });

            context.UnitContentBooks.AddRange(unitContentBooks);
            context.SaveChanges();
            return companies;
        }

        private static List<Unit> GenerateAndAddDepartments(List<int> potentialParentIds, int numberOfDepartments, PersonalLegacyContext context)
        {
            var departments = UnitFactory.GetDepartmentFactory(potentialParentIds)
                .Generate(numberOfDepartments);
            context.Units.AddRange(departments);
            context.SaveChanges();
            return departments;
        }

        private static void GenerateAndAddAbsences(List<int> employeeIds,int averageNumberOfAbsencesPerEmployeePerYear, int year, PersonalLegacyContext context)
        {
            var absences = AbsenceFactory
                .GetFactory(employeeIds, year)
                .Generate(employeeIds.Count * averageNumberOfAbsencesPerEmployeePerYear);

            context.Absences.AddRange(absences);
            context.SaveChanges();

            //while (date.Year <=year)
            //{
            //    var absences = AbsenceFactory
            //        .GetFactory(employeeIds, date, date.AddDays(3),seedId++)
            //        .Generate(2);
            //    absences.ForEach(a => a.FromAndToHasTime = a.From > a.From.Date && a.To >a.To.Date);

            //    context.Absences.AddRange(absences);
            //    context.SaveChanges();

            //    date = date.AddDays(4);
            //}
        }

        private static List<Employee> GenerateAndAddEmployees(List<int> potentialUnitIds, int numberOfEmployees, PersonalLegacyContext context)
        {
            List<int> userIds = null;
            using (var commonContext = _commonDbFactory.CreateDbContext().Result)
            {
                var currentMaxId = commonContext.Users.Max(u=>u.UserId.Value);
                userIds = Enumerable.Range(currentMaxId+1000, numberOfEmployees).ToList();
                var dbId = commonContext.Customers.First(c => c.SticosCustomerId == _customerId).Id;
                
                foreach (var userId in userIds)
                {
                    var users = DbUserFactory
                        .GetFactory(dbId, potentialUnitIds,userId,8675309+userId)
                        .Generate();
                    commonContext.Users.AddRange(users);
                }
                commonContext.SaveChanges();
            }

            var employees = EmployeeFactory
                .GetFactory(potentialUnitIds)
                .Generate(numberOfEmployees);
            for (int i = 0; i < employees.Count; i++)
            {
                employees[i].UserId = userIds.ElementAt(i);
            }
            
            context.Employees.AddRange(employees);
            context.SaveChanges();

            var employments = employees.Select(e=>new Employment{EmployeeId = e.Id, Percentage = 100,StartDate = e.EmployeeStartDate});
            context.Employments.AddRange(employments);
            context.SaveChanges();

            

            return employees;
        }

    }
}
