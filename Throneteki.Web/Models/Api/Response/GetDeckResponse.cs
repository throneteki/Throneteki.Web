namespace CrimsonDev.Throneteki.Models.Api.Response
{
    using CrimsonDev.Gameteki.Api.Models.Api.Response;

    public class GetDeckResponse : ApiResponse
    {
        public ApiDeck Deck { get; set; }
    }
}
