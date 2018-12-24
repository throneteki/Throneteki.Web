namespace CrimsonDev.Throneteki.Models.Api.Response
{
    using System.Collections.Generic;
    using CrimsonDev.Gameteki.Api.Models.Api.Response;

    public class GetDecksResponse : ApiResponse
    {
        public List<ApiDeck> Decks { get; set; }
    }
}
