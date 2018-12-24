namespace CrimsonDev.Throneteki.Models.Api.Response
{
    using System.Collections.Generic;
    using CrimsonDev.Gameteki.Api.Models.Api;
    using CrimsonDev.Gameteki.Api.Models.Api.Response;

    public class GetRestrictedListResponse : ApiResponse
    {
        public List<ApiRestrictedListEntry> RestrictedList { get; set; }
    }
}