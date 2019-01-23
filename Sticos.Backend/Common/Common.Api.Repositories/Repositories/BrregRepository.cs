using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Domain.Entities;
using Common.Api.Domain.HttpClientHelper;
using Common.Api.Domain.Interfaces;
using Newtonsoft.Json;

namespace Common.Api.Repositories.Repositories
{
    public class BrregRepository : IBrregRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public BrregRepository(IHttpClientFactory clientFactory, IMapper mapper)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
        }

        public async Task<BrregEntity> LookupBrregEntity(int orgNumber)
        {
            var client = _clientFactory.CreateClient(HttpClientConfiguration.BrregClient);
            
            var res = await client.GetAsync($"enheter/{orgNumber}");
            if (res.StatusCode == HttpStatusCode.NotFound) return null;

            var str = await res.Content.ReadAsStringAsync();

            var brregOrg = JsonConvert.DeserializeObject<BrregOrganization>(str);

            var brregEntity = _mapper.Map<BrregEntity>(brregOrg);
            brregEntity.Type = BrregEntityType.Parent;
            return brregEntity;
        }

        public async Task<BrregEntity> LookupBrregChildEntity(int orgNumber)
        {
            var client = _clientFactory.CreateClient(HttpClientConfiguration.BrregClient);

            var res = await client.GetAsync($"underenheter/{orgNumber}");
            if (res.StatusCode == HttpStatusCode.NotFound) return null;

            var str = await res.Content.ReadAsStringAsync();

            var brregOrg = JsonConvert.DeserializeObject<BrregOrganization>(str);

            var brregEntity = _mapper.Map<BrregEntity>(brregOrg);
            brregEntity.Type = BrregEntityType.Child;
            return brregEntity;
        }

        public async Task<List<BrregEntity>> LookupBrregChildren(int organizationNumber)
        {
            var client = _clientFactory.CreateClient(HttpClientConfiguration.BrregClient);

            var children = new List<BrregEntity>();
            var page = 0;
            int totalPages;
            do
            {
                var res = await client.GetAsync($"underenheter?overordnetEnhet={organizationNumber}&page={page}");

                var str = await res.Content.ReadAsStringAsync();
                var pagedRes = JsonConvert.DeserializeObject<PagedBrregOrganizationResult>(str);

                totalPages = pagedRes.Page.TotalPages;
                children.AddRange(_mapper.Map<List<BrregEntity>>(pagedRes.EmbeddedOrgList.Underenheter));

                page++;
            } while (page <= totalPages - 1);

            return children;
        }
    }

    public class BrregOrganization
    {
        public int OrganisasjonsNummer { get; set; }
        public string Navn { get; set; }
    }

    public class PagedBrregOrganizationResult
    {
        [JsonProperty("_embedded")] public EmbeddedBrregOrgList EmbeddedOrgList { get; set; }
        public Page Page { get; set; }
    }

    public class EmbeddedBrregOrgList
    {
        public List<BrregOrganization> Underenheter { get; set; }
    }

    public class Page
    {
        public int TotalPages { get; set; }
    }
}