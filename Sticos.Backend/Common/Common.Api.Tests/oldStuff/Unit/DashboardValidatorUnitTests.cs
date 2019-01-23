using Bogus;
using Common.Api.Domain.Entities;
using Common.Api.Domain.Validators;
using NUnit.Framework;

namespace Common.Api.Tests.Unit
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    internal class DashboardValidatorUnitTests
    {
        [SetUp]
        public void SetUp()
        {
            _dashboardValidator = new DashboardValidator();
        }

        private DashboardValidator _dashboardValidator;

        [TestCase(0)]
        [TestCase(-1)]
        public void DashboardValidateCreateOwnerTypeId(int ownerTypeId)
        {
            var validationResult = _dashboardValidator.ValidateCreate(new Dashboard
            {
                OwnerTypeId = ownerTypeId
            });

            Assert.IsTrue(validationResult.Errors.Contains("OwnerTypeId - OwnerTypeId is required."));
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void DashboardValidateCreateModelIdNotAllowed(int id)
        {
            var validationResult = _dashboardValidator.ValidateCreate(new Dashboard
            {
                Id = id
            });

            Assert.IsTrue(validationResult.Errors.Contains("Id - Id not allowed."));
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void DashboardValidateUpdateOwnerTypeId(int ownerTypeId)
        {
            var validationResult = _dashboardValidator.ValidateUpdate(new Dashboard
            {
                OwnerTypeId = ownerTypeId
            });

            Assert.IsTrue(validationResult.Errors.Contains("OwnerTypeId - OwnerTypeId is required."));
        }

        [Test]
        public void DashboardValidateCreateDashboardConfig()
        {
            var validationResult1 = _dashboardValidator.ValidateCreate(new Dashboard());
            var validationResult2 = _dashboardValidator.ValidateCreate(new Dashboard
            {
                DashboardConfig = ""
            });
            var validationResult3 = _dashboardValidator.ValidateCreate(new Dashboard
            {
                DashboardConfig = "  "
            });

            Assert.IsTrue(validationResult1.Errors.Contains("DashboardConfig - DashboardConfig is required."));
            Assert.IsTrue(validationResult2.Errors.Contains("DashboardConfig - DashboardConfig is required."));
            Assert.IsTrue(validationResult3.Errors.Contains("DashboardConfig - DashboardConfig is required."));
        }

        [Test]
        public void DashboardValidateCreateNullEntity()
        {
            var validationResult = _dashboardValidator.ValidateCreate(null);

            Assert.IsTrue(validationResult.Errors.Contains("Invalid request."));
        }

        [Test]
        public void DashboardValidateCreateTitle()
        {
            var faker = new Faker();
            var longTitle = faker.Random.String(256);

            var validationResult1 = _dashboardValidator.ValidateCreate(new Dashboard());
            var validationResult2 = _dashboardValidator.ValidateCreate(new Dashboard
            {
                Title = ""
            });
            var validationResult3 = _dashboardValidator.ValidateCreate(new Dashboard
            {
                Title = "  "
            });
            var validationResult4 = _dashboardValidator.ValidateCreate(new Dashboard
            {
                Title = longTitle
            });

            Assert.IsTrue(validationResult1.Errors.Contains("Title - Title is required."));
            Assert.IsTrue(validationResult2.Errors.Contains("Title - Title is required."));
            Assert.IsTrue(validationResult3.Errors.Contains("Title - Title is required."));
            Assert.IsTrue(validationResult4.Errors.Contains("Title - Title is limited to maximum of 255 characters."));
        }

        [Test]
        public void DashboardValidateDeleteId()
        {
            var validationResult1 = _dashboardValidator.ValidateDelete(null);
            var validationResult2 = _dashboardValidator.ValidateDelete(0);

            Assert.IsTrue(validationResult1.Errors.Contains("Id - Id is missing."));
            Assert.IsTrue(validationResult2.Errors.Contains("Id - Id should be greater than 0."));
        }

        [Test]
        public void DashboardValidateUpdateDashboardConfig()
        {
            var validationResult1 = _dashboardValidator.ValidateUpdate(new Dashboard());
            var validationResult2 = _dashboardValidator.ValidateUpdate(new Dashboard
            {
                DashboardConfig = ""
            });
            var validationResult3 = _dashboardValidator.ValidateUpdate(new Dashboard
            {
                DashboardConfig = "  "
            });

            Assert.IsTrue(validationResult1.Errors.Contains("DashboardConfig - DashboardConfig is required."));
            Assert.IsTrue(validationResult2.Errors.Contains("DashboardConfig - DashboardConfig is required."));
            Assert.IsTrue(validationResult3.Errors.Contains("DashboardConfig - DashboardConfig is required."));
        }

        [Test]
        public void DashboardValidateUpdateModelIdProvided()
        {
            var validationResult = _dashboardValidator.ValidateUpdate(new Dashboard());

            Assert.IsTrue(validationResult.Errors.Contains("Id - Id is missing."));
        }

        [Test]
        public void DashboardValidateUpdateNullEntity()
        {
            var validationResult = _dashboardValidator.ValidateUpdate(null);

            Assert.IsTrue(validationResult.Errors.Contains("Invalid request."));
        }

        [Test]
        public void DashboardValidateUpdateTitle()
        {
            var faker = new Faker();
            var longTitle = faker.Random.String(256);

            var validationResult1 = _dashboardValidator.ValidateUpdate(new Dashboard());
            var validationResult2 = _dashboardValidator.ValidateUpdate(new Dashboard
            {
                Title = ""
            });
            var validationResult3 = _dashboardValidator.ValidateUpdate(new Dashboard
            {
                Title = "  "
            });
            var validationResult4 = _dashboardValidator.ValidateUpdate(new Dashboard
            {
                Title = longTitle
            });

            Assert.IsTrue(validationResult1.Errors.Contains("Title - Title is required."));
            Assert.IsTrue(validationResult2.Errors.Contains("Title - Title is required."));
            Assert.IsTrue(validationResult3.Errors.Contains("Title - Title is required."));
            Assert.IsTrue(validationResult4.Errors.Contains("Title - Title is limited to maximum of 255 characters."));
        }
    }
}