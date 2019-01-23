using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using domain = Common.Api.Domain.Interfaces.Employees;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Models;
using Shared.Interfaces;


namespace Common.Api.Repositories.Legacy.Repositories
{
    public class EmployeeLegacyDbEfRepository : domain.IEmployeeRepository
    {
        private readonly IDbContextFactory<PersonalLegacyContext> _dbContextFactory;
        private readonly IDbContextFactory<PersonalCommonLegacyContext> _dbCommonContextFactory;

        public EmployeeLegacyDbEfRepository(IDbContextFactory<PersonalLegacyContext> dbContextFactory, IDbContextFactory<PersonalCommonLegacyContext> dbCommonContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbCommonContextFactory = dbCommonContextFactory;
        }
        
        public async Task<IList<domain.IEmployee>> Search(domain.ISearchQueryEmployee searchQuery)
        {
            using (var context = await _dbContextFactory.CreateDbContext())
            {
                var query = context.Employees
                    .Where(u => !u.IsDeleted && u.EmployeeEndDate > DateTime.Today);
                query = ApplyQueryParameters(searchQuery, query);

                var employees = query
                    .Skip(searchQuery.Skip ?? SearchConstants.DEFAULT_SKIP)
                    .Take(searchQuery.Take ?? SearchConstants.DEFAULT_TAKE)
                    .OrderBy(x => x.FirstName).ThenBy(x=>x.LastName)
                    .ToList();

                await AttachPropertiesFromUserEntity(employees);
                return employees.ToList<domain.IEmployee>();
            }
        }

        private static IQueryable<Employee> ApplyQueryParameters(domain.ISearchQueryEmployee searchQuery, IQueryable<Employee> query)
        {
            if(searchQuery.UnitIds.Count >= 1)
            {
                query = query.Where(e => searchQuery.UnitIds.Contains((int)e.UnitId));
            }
            if (!string.IsNullOrEmpty(searchQuery.FirstName))
            {
                query = query.Where(e =>e.FirstName !=null && e.FirstName.Contains(searchQuery.FirstName, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(searchQuery.LastName))
            {
                query = query.Where(e =>e.LastName !=null && e.LastName.Contains(searchQuery.LastName, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(searchQuery.JobTitle))
            {
                query = query.Where(e =>e.JobTitle !=null && e.JobTitle.Contains(searchQuery.JobTitle, StringComparison.InvariantCultureIgnoreCase));
            }
            if (!string.IsNullOrEmpty(searchQuery.SocialSecurityNumber))
            {
                //todo: implement search for ssn
            }
            if(searchQuery.EmployeesIds.Count > 0)
            {
                query = query.Where(e => searchQuery.EmployeesIds.Contains(e.Id));
            }
            if(searchQuery.UserIds.Count > 0)
            {
                query = query.Where(e =>e.UserId.HasValue && searchQuery.UserIds.Contains(e.UserId.Value));
            }
            return query;
        }

        private async Task AttachPropertiesFromUserEntity(List<Employee> employees)
        {
            var employeeDictionary = employees.Where(e => e.UserId.HasValue && e.UserId.Value > 0)
                .ToDictionary(e => e.UserId, e => e);

            var userIds = employeeDictionary.Keys;

            if (userIds.Any())
            {
                using (var commonContext = await _dbCommonContextFactory.CreateDbContext())
                {
                    //todo: possible more than 2000 userids. add batch-logic
                    var userDictionary = commonContext.Users.Where(u => userIds.Contains(u.UserId))
                        .GroupBy(user=>user.UserId)
                        .ToDictionary(g=>g.Key, g=>g.FirstOrDefault());
                    foreach (var user in userDictionary)
                    {
                        employeeDictionary[user.Key].Email = user.Value.Email;
                        employeeDictionary[user.Key].Phone = user.Value.Mobilephone;
                    }
                }
            }
        }
    }
}