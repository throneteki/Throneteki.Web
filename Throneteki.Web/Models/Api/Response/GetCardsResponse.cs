namespace CrimsonDev.Throneteki.Models.Api.Response
{
    using System.Collections.Generic;
    using CrimsonDev.Gameteki.Api.Models.Api.Response;

    public class GetCardsResponse : ApiResponse
    {
        public Dictionary<string, ApiCard> Cards { get; set; }
    }
}