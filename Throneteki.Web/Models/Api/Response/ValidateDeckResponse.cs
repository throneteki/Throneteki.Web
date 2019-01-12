namespace CrimsonDev.Throneteki.Models.Api.Response
{
    using CrimsonDev.Gameteki.Api.Models.Api.Response;

    public class ValidateDeckResponse : ApiResponse
    {
        public DeckValidationResult Result { get; set; }
    }
}
