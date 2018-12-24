namespace CrimsonDev.Throneteki.Models.Api.Request
{
    using System.ComponentModel.DataAnnotations;

    public class AddDeckRequest
    {
        [Required]
        public ApiDeck Deck { get; set; }
    }
}
