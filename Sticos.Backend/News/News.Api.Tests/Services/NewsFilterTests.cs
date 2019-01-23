using FakeItEasy;
using Microsoft.Extensions.Logging;
using News.Api.Repository;
using News.Api.Services;
using NUnit.Framework;
using Shared.Domain.Enums;
using Shared.Domain.ValueObjects.Queries;
using Shared.Interfaces;
using Shared.Services.Models;
using Shared.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Api.Tests.Services
{
    public class NewsFilterTests
    {

        [TestFixture]
        public class GetBaseFilter_NoDateRestrictions : TestBase
        {
            [OneTimeSetUp]
            public async Task Init()
            {
                var models = new List<Models.News> {
                    new Models.News { Id = 1, FromDate = null, ToDate = null, UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 2, FromDate = null, ToDate = null, UnitId = 9999, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 3, FromDate = null, ToDate = null, UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" }
                };

                await _db.Set<Models.News>().AddRangeAsync(models);
                await _db.SaveChangesAsync();
            }

            [TestCase(false, false, ExpectedResult = 0)]
            [TestCase(true, false, ExpectedResult = 1)]
            [TestCase(false, true, ExpectedResult = 1)]
            [TestCase(true, true, ExpectedResult = 1)]
            public async Task<int> NoDateRestrictions_ReturnsCorrect(bool isReader, bool isEditor)
            {
                var unitId = 50;
                var userId = 100;

                var unitPermissions = new List<UnitPermission>();
                if (isReader)
                {
                    unitPermissions.Add(new UnitPermission { UnitId = unitId, PermissionType = PermissionType.LesNyheter });
                }
                if (isEditor)
                {
                    unitPermissions.Add(new UnitPermission { UnitId = unitId, PermissionType = PermissionType.SkrivNyheter });
                }

                A.CallTo(() => CurrentUserContext.Get()).Returns(new UserContext { UserId = userId });
                A.CallTo(() => AuthorizationService.GetUnitPermissions(A<int>.Ignored, A<int?>.Ignored, A<PermissionType[]>.Ignored)).Returns(unitPermissions);

                var result = await NewsFilterService.GetBaseFilter();

                return result.Count();
            }

            [OneTimeTearDown]
            public async Task Teardown()
            {
                await _db.Database.EnsureDeletedAsync();
            }
        }

        public class GetBaseFilter_DateRestrictions : TestBase
        {
            [OneTimeSetUp]
            public async Task Init()
            {
                var models = new List<Models.News> {
                    new Models.News { Id = 1, FromDate = DateTime.Today.AddDays(-1), ToDate = DateTime.Today.AddDays(1), UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 2, FromDate = DateTime.Today.AddDays(-3), ToDate = DateTime.Today.AddDays(-2), UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 3, FromDate = DateTime.Today.AddDays(2), ToDate = DateTime.Today.AddDays(3), UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 4, FromDate = null, ToDate = DateTime.Today.AddDays(-1), UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 5, FromDate = null, ToDate = DateTime.Today.AddDays(1), UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 6, FromDate = DateTime.Today.AddDays(-1), ToDate = null, UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 7, FromDate = DateTime.Today.AddDays(1), ToDate = null, UnitId = 50, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 8, FromDate = null, ToDate = null, UnitId = 9999, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 9, FromDate = null, ToDate = null, UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" }
                };

                await _db.Set<Models.News>().AddRangeAsync(models);
                await _db.SaveChangesAsync();
            }

            [TestCase(false, false, ExpectedResult = 0)]
            [TestCase(true, false, ExpectedResult = 3)]
            [TestCase(false, true, ExpectedResult = 7)]
            [TestCase(true, true, ExpectedResult = 7)]
            public async Task<int> NoDateRestrictions_ReturnsCorrect(bool isReader, bool isEditor)
            {
                var unitId = 50;
                var userId = 100;

                var unitPermissions = new List<UnitPermission>();
                if (isReader)
                {
                    unitPermissions.Add(new UnitPermission { UnitId = unitId, PermissionType = PermissionType.LesNyheter });
                }
                if (isEditor)
                {
                    unitPermissions.Add(new UnitPermission { UnitId = unitId, PermissionType = PermissionType.SkrivNyheter });
                }

                A.CallTo(() => CurrentUserContext.Get()).Returns(new UserContext { UserId = userId });
                A.CallTo(() => AuthorizationService.GetUnitPermissions(A<int>.Ignored, A<int?>.Ignored, A<PermissionType[]>.Ignored)).Returns(unitPermissions);

                var result = await NewsFilterService.GetBaseFilter();

                return result.Count();
            }

            [OneTimeTearDown]
            public async Task Teardown()
            {
                await _db.Database.EnsureDeletedAsync();
            }
        }

        public class GetBaseFilter_DateRestrictions_Master : TestBase
        {
            [OneTimeSetUp]
            public async Task Init()
            {
                var models = new List<Models.News> {
                    new Models.News { Id = 1, FromDate = DateTime.Today.AddDays(-1), ToDate = DateTime.Today.AddDays(1), UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 2, FromDate = DateTime.Today.AddDays(-3), ToDate = DateTime.Today.AddDays(-2), UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 3, FromDate = DateTime.Today.AddDays(2), ToDate = DateTime.Today.AddDays(3), UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 4, FromDate = null, ToDate = DateTime.Today.AddDays(-1), UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 5, FromDate = null, ToDate = DateTime.Today.AddDays(1), UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 6, FromDate = DateTime.Today.AddDays(-1), ToDate = null, UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 7, FromDate = DateTime.Today.AddDays(1), ToDate = null, UnitId = null, Title = "Title", Text = "Text", Email = "email@email.com" },
                    new Models.News { Id = 8, FromDate = null, ToDate = null, UnitId = 1, Title = "Title", Text = "Text", Email = "email@email.com" }
                };

                await _db.Set<Models.News>().AddRangeAsync(models);
                await _db.SaveChangesAsync();
            }

            [Ignore("Can't get these up and running")]
            [TestCase(false, false, ExpectedResult = 0)]
            [TestCase(true, false, ExpectedResult = 3)]
            [TestCase(false, true, ExpectedResult = 7)]
            [TestCase(true, true, ExpectedResult = 7)]
            public async Task<int> NoDateRestrictions_ReturnsCorrect(bool isReader, bool isEditor)
            {
                var unitId = -1;
                var userId = 100;

                var unitPermissions = new List<UnitPermission>();
                if (isReader)
                {
                    unitPermissions.Add(new UnitPermission { UnitId = unitId, PermissionType = PermissionType.LesNyheter });
                }
                if (isEditor)
                {
                    unitPermissions.Add(new UnitPermission { UnitId = unitId, PermissionType = PermissionType.SkrivNyheter });
                }

                A.CallTo(() => CurrentUserContext.Get()).Returns(new UserContext { UserId = userId });
                A.CallTo(() => AuthorizationService.GetUnitPermissions(A<int>.Ignored, A<int?>.Ignored, A<PermissionType[]>.Ignored)).Returns(unitPermissions);

                var result = await NewsFilterService.GetBaseFilter();

                return result.Count();
            }

            [OneTimeTearDown]
            public async Task Teardown()
            {
                await _db.Database.EnsureDeletedAsync();
            }
        }

        public class TestBase : IDisposable
        {
            protected NewsContext _db;

            protected ILogger<NewsFilterService> LogService { get; set; }
            protected IDbContextFactory<NewsContext> DbContextFactory { get; set; }
            protected ICurrentUserContext CurrentUserContext { get; set; }
            protected IAuthorizationService AuthorizationService { get; set; }

            protected NewsFilterService NewsFilterService { get; set; }


            [OneTimeSetUp]
            protected virtual async Task Setup()
            {
                LogService = A.Fake<ILogger<NewsFilterService>>();
                DbContextFactory = A.Fake<IDbContextFactory<NewsContext>>();
                CurrentUserContext = A.Fake<ICurrentUserContext>();
                AuthorizationService = A.Fake<IAuthorizationService>();

                _db = await (new InMemoryContextFactory<NewsContext>()).CreateDbContext();
                A.CallTo(() => DbContextFactory.CreateDbContext()).Returns(_db);

                NewsFilterService = new NewsFilterService(LogService, DbContextFactory, CurrentUserContext, AuthorizationService);
            }

            [OneTimeTearDown]
            public async Task TearDown()
            {
                await _db.Database.EnsureDeletedAsync();
            }

            public void Dispose()
            {
            }
        }
    }
}
