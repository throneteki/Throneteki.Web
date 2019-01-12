namespace CrimsonDev.Throneteki.ApiControllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using CrimsonDev.Gameteki.Api.Models.Api;
    using CrimsonDev.Throneteki.Helpers;
    using CrimsonDev.Throneteki.Models.Api.Response;
    using CrimsonDev.Throneteki.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public class CardController : Controller
    {
        private readonly ICardService cardService;

        public CardController(ICardService cardService)
        {
            this.cardService = cardService;
        }

        [Route("api/cards")]
        public async Task<IActionResult> GetCardsAsync()
        {
            var cards = await cardService.GetAllCardsAsync();

            return Json(new GetCardsResponse { Success = true, Cards = cards.ToDictionary(k => k.Key, v => v.Value.ToApiCard()) });
        }

        [Route("api/packs")]
        public async Task<IActionResult> GetPacksAsync()
        {
            var packs = await cardService.GetAllPacksAsync();

            return Json(new GetPacksResponse { Success = true, Packs = packs });
        }

        [Route("api/factions")]
        public async Task<IActionResult> GetFactionsAsync()
        {
            var factions = await cardService.GetAllFactionsAsync();

            return Json(new GetFactionsResponse { Success = true, Factions = factions });
        }

        [Route("api/restricted-list")]
        public async Task<IActionResult> GetRestrictedListAsync()
        {
            var restrictedList = await cardService.GetRestrictedListAsync();
            var response = new GetRestrictedListResponse
            {
                Success = true,
                RestrictedList = restrictedList.Select(
                    rl => new ApiRestrictedListEntry
                    {
                        Id = rl.Id,
                        Version = rl.Version,
                        Date = rl.Date,
                        JoustCards = rl.JoustCards.Select(c => c.Card.Code).ToList(),
                        MeleeCards = rl.MeleeCards.Select(c => c.Card.Code).ToList()
                    }).ToList()
            };

            return Json(response);
        }
    }
}
