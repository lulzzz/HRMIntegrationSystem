using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Controllers;
using Common.Api.Domain.HttpClientHelper;
using Common.Api.Domain.Services;
using Common.Api.Repositories.Repositories;
using Common.Api.Tests.Helpers;
using shared = Shared.Exceptions;
using NUnit.Framework;
using Shared.TestCommon;

namespace Common.Api.Tests.Unit
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class BrregControllerUnitTests
    {
        [Test]
        public async Task WhenAskingForParentWithNoChildren_ParentOrgIsReturned()
        {
            var organizationNumber = 934228391;

            var controller = new BrregControllerBuilder()
                .AddExpectedHttpResponse($"enheter/{organizationNumber}", SticosEntityJson, HttpStatusCode.OK)
                .Build();

            var ret = await controller.GetBrregEntity(organizationNumber, false);

            Assert.AreEqual(organizationNumber, ret.Value.OrganizationNumber);
            Assert.AreEqual("STICOS AS", ret.Value.Name);
            Assert.AreEqual(Contracts.BrregEntityType.Parent, ret.Value.Type);
            Assert.AreEqual(0, ret.Value.Children.Count);
        }

        [Test]
        public async Task WhenAskingForParentWithChildren_ParentOrgHasChildren()
        {
            var organizationNumber = 934228391;

            var controller = new BrregControllerBuilder()
                .AddExpectedHttpResponse($"enheter/{organizationNumber}", SticosEntityJson, HttpStatusCode.OK)
                .AddExpectedHttpResponse($"underenheter?overordnetEnhet={organizationNumber}&page={0}", SticosEntityWithChildentities, HttpStatusCode.OK)
                .Build();

            var ret = await controller.GetBrregEntity(organizationNumber, true);

            Assert.AreEqual(organizationNumber, ret.Value.OrganizationNumber);
            Assert.AreEqual("STICOS AS", ret.Value.Name);
            Assert.AreEqual(Contracts.BrregEntityType.Parent, ret.Value.Type);
            Assert.AreEqual(1, ret.Value.Children.Count);
        }

        [Test]
        public async Task WhenAskingForChild_ChildLookupIsCalled()
        {
            var organizationNumber = 971998016;

            var controller = new BrregControllerBuilder()
                .AddExpectedHttpResponse($"underenheter/{organizationNumber}", SticosChildentityJson, HttpStatusCode.OK)
                .AddExpectedHttpResponse($"enheter/{organizationNumber}", "", HttpStatusCode.NotFound)
                .Build();

            var ret = await controller.GetBrregEntity(organizationNumber, true);

            Assert.AreEqual(organizationNumber, ret.Value.OrganizationNumber);
            Assert.AreEqual("STICOS AS", ret.Value.Name);
            Assert.AreEqual(Contracts.BrregEntityType.Child, ret.Value.Type);
            Assert.AreEqual(0, ret.Value.Children.Count);
        }

        [Test]
        public void WhenAskingForInvalidOrgNumber_ThrowNotFound()
        {
            var organizationNumber = 971998011;

            var controller = new BrregControllerBuilder()
                .AddExpectedHttpResponse($"underenheter/{organizationNumber}", "", HttpStatusCode.NotFound)
                .AddExpectedHttpResponse($"enheter/{organizationNumber}", "", HttpStatusCode.NotFound)
                .Build();

            var ex = Assert.ThrowsAsync<shared.NotFoundException>(async () => await controller.GetBrregEntity(organizationNumber, true));
            Assert.AreEqual("Organisasjonen ble ikke funnet", ex.Message);
        }
        
        private const string SticosEntityJson =
            "{\"organisasjonsnummer\": \"934228391\",\r\n    \"navn\": \"STICOS AS\",\r\n    \"organisasjonsform\": {\r\n        \"kode\": \"AS\",\r\n        \"beskrivelse\": \"Aksjeselskap\",\r\n        \"_links\": {\r\n            \"self\": {\r\n                \"href\": \"https://data.brreg.no/enhetsregisteret/api/organisasjonsformer/AS\"\r\n            }\r\n        }\r\n    },\r\n    \"hjemmeside\": \"www.sticos.no\",\r\n    \"postadresse\": {\r\n        \"land\": \"Norge\",\r\n        \"landkode\": \"NO\",\r\n        \"postnummer\": \"7438\",\r\n        \"poststed\": \"TRONDHEIM\",\r\n        \"adresse\": [\r\n            \"Postboks 2934 Torgard\"\r\n        ],\r\n        \"kommune\": \"TRONDHEIM\",\r\n        \"kommunenummer\": \"5001\"\r\n    },\r\n    \"registreringsdatoEnhetsregisteret\": \"1995-02-19\",\r\n    \"registrertIMvaregisteret\": true,\r\n    \"naeringskode1\": {\r\n        \"beskrivelse\": \"Utgivelse av annen programvare\",\r\n        \"kode\": \"58.290\"\r\n    },\r\n    \"antallAnsatte\": 112,\r\n    \"forretningsadresse\": {\r\n        \"land\": \"Norge\",\r\n        \"landkode\": \"NO\",\r\n        \"postnummer\": \"7041\",\r\n        \"poststed\": \"TRONDHEIM\",\r\n        \"adresse\": [\r\n            \"Håkon Magnussons gate 1A\"\r\n        ],\r\n        \"kommune\": \"TRONDHEIM\",\r\n        \"kommunenummer\": \"5001\"\r\n    },\r\n    \"stiftelsesdato\": \"1983-10-06\",\r\n    \"institusjonellSektorkode\": {\r\n        \"kode\": \"2100\",\r\n        \"beskrivelse\": \"Private aksjeselskaper mv.\"\r\n    },\r\n    \"registrertIForetaksregisteret\": true,\r\n    \"registrertIStiftelsesregisteret\": false,\r\n    \"registrertIFrivillighetsregisteret\": false,\r\n    \"sisteInnsendteAarsregnskap\": \"2017\",\r\n    \"konkurs\": false,\r\n    \"underAvvikling\": false,\r\n    \"underTvangsavviklingEllerTvangsopplosning\": false,\r\n    \"maalform\": \"Bokmål\",\r\n    \"_links\": {\r\n        \"self\": {\r\n            \"href\": \"https://data.brreg.no/enhetsregisteret/api/enheter/934228391\"\r\n        }\r\n    }\r\n}";

        private const string SticosEntityWithChildentities =
            "{\r\n    \"_embedded\": {\r\n        \"underenheter\": [\r\n            {\r\n                \"organisasjonsnummer\": \"971998016\",\r\n                \"navn\": \"STICOS AS\",\r\n                \"organisasjonsform\": {\r\n                    \"kode\": \"BEDR\",\r\n                    \"beskrivelse\": \"Bedrift\",\r\n                    \"_links\": {\r\n                        \"self\": {\r\n                            \"href\": \"https://data.brreg.no/enhetsregisteret/api/organisasjonsformer/BEDR\"\r\n                        }\r\n                    }\r\n                },\r\n                \"hjemmeside\": \"www.sticos.no\",\r\n                \"postadresse\": {\r\n                    \"land\": \"Norge\",\r\n                    \"landkode\": \"NO\",\r\n                    \"postnummer\": \"7438\",\r\n                    \"poststed\": \"TRONDHEIM\",\r\n                    \"adresse\": [\r\n                        \"Postboks 2934 Torgard\"\r\n                    ],\r\n                    \"kommune\": \"TRONDHEIM\",\r\n                    \"kommunenummer\": \"5001\"\r\n                },\r\n                \"registreringsdatoEnhetsregisteret\": \"1995-02-23\",\r\n                \"registrertIMvaregisteret\": false,\r\n                \"naeringskode1\": {\r\n                    \"beskrivelse\": \"Utgivelse av annen programvare\",\r\n                    \"kode\": \"58.290\"\r\n                },\r\n                \"antallAnsatte\": 112,\r\n                \"overordnetEnhet\": \"934228391\",\r\n                \"oppstartsdato\": \"1983-10-16\",\r\n                \"beliggenhetsadresse\": {\r\n                    \"land\": \"Norge\",\r\n                    \"landkode\": \"NO\",\r\n                    \"postnummer\": \"7041\",\r\n                    \"poststed\": \"TRONDHEIM\",\r\n                    \"adresse\": [\r\n                        \"Håkon Magnussons gate 1A\"\r\n                    ],\r\n                    \"kommune\": \"TRONDHEIM\",\r\n                    \"kommunenummer\": \"5001\"\r\n                },\r\n                \"_links\": {\r\n                    \"self\": {\r\n                        \"href\": \"https://data.brreg.no/enhetsregisteret/api/underenheter/971998016\"\r\n                    },\r\n                    \"overordnetEnhet\": {\r\n                        \"href\": \"https://data.brreg.no/enhetsregisteret/api/enheter/934228391\"\r\n                    }\r\n                }\r\n            }\r\n        ]\r\n    },\r\n    \"_links\": {\r\n        \"self\": {\r\n            \"href\": \"https://data.brreg.no/enhetsregisteret/api/underenheter/?overordnetEnhet=934228391&page=0\"\r\n        }\r\n    },\r\n    \"page\": {\r\n        \"size\": 20,\r\n        \"totalElements\": 1,\r\n        \"totalPages\": 1,\r\n        \"number\": 0\r\n    }\r\n}";
        private const string SticosChildentityJson =
            "{\r\n    \"organisasjonsnummer\": \"971998016\",\r\n    \"navn\": \"STICOS AS\",\r\n    \"organisasjonsform\": {\r\n        \"kode\": \"BEDR\",\r\n        \"beskrivelse\": \"Bedrift\",\r\n        \"_links\": {\r\n            \"self\": {\r\n                \"href\": \"https://data.brreg.no/enhetsregisteret/api/organisasjonsformer/BEDR\"\r\n            }\r\n        }\r\n    },\r\n    \"hjemmeside\": \"www.sticos.no\",\r\n    \"postadresse\": {\r\n        \"land\": \"Norge\",\r\n        \"landkode\": \"NO\",\r\n        \"postnummer\": \"7438\",\r\n        \"poststed\": \"TRONDHEIM\",\r\n        \"adresse\": [\r\n            \"Postboks 2934 Torgard\"\r\n        ],\r\n        \"kommune\": \"TRONDHEIM\",\r\n        \"kommunenummer\": \"5001\"\r\n    },\r\n    \"registreringsdatoEnhetsregisteret\": \"1995-02-23\",\r\n    \"registrertIMvaregisteret\": false,\r\n    \"naeringskode1\": {\r\n        \"beskrivelse\": \"Utgivelse av annen programvare\",\r\n        \"kode\": \"58.290\"\r\n    },\r\n    \"antallAnsatte\": 112,\r\n    \"overordnetEnhet\": \"934228391\",\r\n    \"oppstartsdato\": \"1983-10-16\",\r\n    \"beliggenhetsadresse\": {\r\n        \"land\": \"Norge\",\r\n        \"landkode\": \"NO\",\r\n        \"postnummer\": \"7041\",\r\n        \"poststed\": \"TRONDHEIM\",\r\n        \"adresse\": [\r\n            \"Håkon Magnussons gate 1A\"\r\n        ],\r\n        \"kommune\": \"TRONDHEIM\",\r\n        \"kommunenummer\": \"5001\"\r\n    },\r\n    \"_links\": {\r\n        \"self\": {\r\n            \"href\": \"https://data.brreg.no/enhetsregisteret/api/underenheter/971998016\"\r\n        },\r\n        \"overordnetEnhet\": {\r\n            \"href\": \"https://data.brreg.no/enhetsregisteret/api/enheter/934228391\"\r\n        }\r\n    }\r\n}";
    }

    public class BrregControllerBuilder
    {
        private readonly MockHttpClientBuilder _mockHttpClientBuilder;

        public BrregControllerBuilder()
        {
            _mockHttpClientBuilder = new MockHttpClientBuilder(HttpClientConfiguration.BrregClient, HttpClientConfiguration.BrregUrl);
        }

        public BrregEntityController Build()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles("Common.Api.Mapping");
                cfg.CreateMissingTypeMaps = false;
            }));
            
            var repository = new BrregRepository(_mockHttpClientBuilder.Build(), mapper);
            var service = new BrregService(repository);
            return new BrregEntityController(service, mapper);
        }

        public BrregControllerBuilder AddExpectedHttpResponse(string url, string content, HttpStatusCode statusCode)
        {
            _mockHttpClientBuilder.WithExpectedGetResponse(url, content, statusCode);
            return this;
        }
    }
}