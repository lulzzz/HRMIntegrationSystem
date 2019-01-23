﻿namespace Altinn.Api.Domain.Entities
{
    public class SearchQuery
    {
        private const int DEFAULT_SKIP = 0;
        private const int DEFAULT_TAKE = 50;

        public int Skip { get; set; } = DEFAULT_SKIP;
        public int Take { get; set; } = DEFAULT_TAKE;
    }
}
