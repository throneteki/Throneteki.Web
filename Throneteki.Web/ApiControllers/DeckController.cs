namespace CrimsonDev.Throneteki.ApiControllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using CrimsonDev.Gameteki.Api.Helpers;
    using CrimsonDev.Gameteki.Data.Models;
    using CrimsonDev.Throneteki.Data;
    using CrimsonDev.Throneteki.Data.GameData;
    using CrimsonDev.Throneteki.Data.Models;
    using CrimsonDev.Throneteki.Data.Models.Api;
    using CrimsonDev.Throneteki.Data.Models.Validators;
    using CrimsonDev.Throneteki.Helpers;
    using CrimsonDev.Throneteki.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class DeckController : Controller
    {
        private readonly IThronetekiDbContext context;
        private readonly UserManager<GametekiUser> userManager;
        private readonly IDeckValidationService deckValidationService;

        public DeckController(IThronetekiDbContext context, UserManager<GametekiUser> userManager, IDeckValidationService deckValidationService)
        {
            this.context = context;
            this.userManager = userManager;
            this.deckValidationService = deckValidationService;
        }

        [Authorize]
        [HttpGet]
        [Route("api/decks")]
        public async Task<IActionResult> GetDecks()
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var decks = await context.Deck
                .Include(d => d.Faction)
                .Include(d => d.Owner)
                .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                .ThenInclude(c => c.Faction)
                .Include("DeckCards.Card.Pack")
                .Where(d => d.OwnerId == currentUser.Id).ToListAsync();

            var apiDecks = new List<ApiDeck>();
            foreach (var deck in decks)
            {
                var apiDeck = deck.ToApiDeck();

                apiDeck.ValidationResult = await deckValidationService.ValidateDeckAsync(deck);

                apiDecks.Add(apiDeck);
            }

            return Json(new GetDecksResponse { Success = true, Decks = apiDecks });
        }

        [Authorize]
        [HttpGet]
        [Route("api/decks/{deckId}")]
        public async Task<IActionResult> GetDeck(int deckId)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var deck = await context.Deck.Include(d => d.Faction).Include(d => d.Owner).Include(d => d.DeckCards).ThenInclude(dc => dc.Card).SingleOrDefaultAsync(d => d.Id == deckId);
            if (deck == null)
            {
                return NotFound();
            }

            if (deck.OwnerId != currentUser.Id)
            {
                return Forbid();
            }

            return Json(new GetDeckResponse { Success = true, Deck = deck.ToApiDeck() });
        }

        [Authorize]
        [HttpPost]
        [Route("api/decks")]
        public async Task<IActionResult> AddDeck([FromBody]AddDeckRequest request)
        {
            var newDeck = await GetDeckFromApiDeckAsync(request.Deck);

            context.Deck.Add(newDeck);

            await context.SaveChangesAsync();

            return this.SuccessResponse();
        }

        [Authorize]
        [HttpPut]
        [Route("api/decks/{deckId}")]
        public async Task<IActionResult> UpdateDeck(int deckId, AddDeckRequest request)
        {
            var deck = await context.Deck.Include(d => d.Faction).Include(d => d.DeckCards).SingleOrDefaultAsync(d => d.Id == deckId);

            if (deck == null)
            {
                return NotFound();
            }

            var owner = await userManager.FindByNameAsync(User.Identity.Name);
            if (owner == null)
            {
                return NotFound();
            }

            var cardsByCode = (await context.Card.ToListAsync()).ToDictionary(key => key.Code, value => value);

            deck.Name = request.Deck.Name;
            deck.LastModified = DateTime.UtcNow;
            deck.FactionId = (await context.Faction.SingleOrDefaultAsync(f => f.Code == request.Deck.Faction.Code)).Id;

            UpdateDeckCards(deck, request.Deck.Cards, DeckCardType.Normal, cardsByCode);
            UpdateDeckCards(deck, request.Deck.RookeryCards, DeckCardType.Rookery, cardsByCode);

            return this.SuccessResponse();
        }

        [Authorize]
        [HttpDelete]
        [Route("api/decks/{deckId}")]
        public async Task<IActionResult> DeleteDeck(int deckId)
        {
            var deck = await context.Deck.SingleOrDefaultAsync(d => d.Id == deckId);

            if (deck == null)
            {
                return NotFound();
            }

            var owner = await userManager.FindByNameAsync(User.Identity.Name);
            if (owner == null)
            {
                return NotFound();
            }

            if (owner.Id != deck.OwnerId)
            {
                return Forbid();
            }

            context.Deck.Remove(deck);

            await context.SaveChangesAsync();

            return Json(new DeleteDeckResponse { Success = true, DeckId = deck.Id });
        }

        [Authorize]
        [HttpPost]
        [Route("api/decks/validate")]
        public async Task<IActionResult> ValidateDeck(AddDeckRequest request)
        {
            var deck = await GetDeckFromApiDeckAsync(request.Deck);

            var validationResult = await deckValidationService.ValidateDeckAsync(deck);

            return Json(new ValidateDeckResponse { Success = true, Result = validationResult });
        }

        [HttpGet]
        [Route("api/decks/{deckId:int}/validate/{userId:guid}")]
        public async Task<IActionResult> ValidateDeckForUser(int deckId, string userId)
        {
            var deck = await context.Deck
                .Include(d => d.Faction)
                .Include(d => d.Owner)
                .Include(d => d.DeckCards)
                .ThenInclude(dc => dc.Card)
                .Include("DeckCards.Card.Faction")
                .Include("DeckCards.Card.Pack")
                .SingleOrDefaultAsync(d => d.Id == deckId);

            if (deck == null)
            {
                return NotFound();
            }

            if (deck.OwnerId != userId)
            {
                return NotFound();
            }

            var validationResult = await deckValidationService.ValidateDeckAsync(deck);

            return Json(new ValidateDeckForUserResponse
            {
                Success = true,
                Result = new DeckValidationResultShort
                {
                    BasicRules = validationResult.BasicRules,
                    NoUnreleasedCards = validationResult.NoUnreleasedCards,
                    FaqJoustRules = validationResult.FaqJoustRules
                }
            });
        }

        private static void AddDeckCards(Deck newDeck, List<ApiDeckCard> cards, DeckCardType cardType, Dictionary<string, Card> cardsByCode)
        {
            foreach (var card in cards)
            {
                newDeck.DeckCards.Add(
                    new DeckCard
                    {
                        CardId = cardsByCode[card.Code].Id,
                        Card = cardsByCode[card.Code],
                        CardType = cardType,
                        Count = card.Count,
                        Deck = newDeck
                    });
            }
        }

        private async Task<Deck> GetDeckFromApiDeckAsync(ApiDeck apiDeck)
        {
            var cardsByCode = (await context.Card.Include(c => c.Faction).Include(c => c.Pack).ToListAsync()).ToDictionary(key => key.Code, value => value);

            var owner = await userManager.FindByNameAsync(User.Identity.Name);
            var faction = await context.Faction.SingleOrDefaultAsync(f => f.Code == apiDeck.Faction.Code);
            var deck = new Deck
            {
                Name = apiDeck.Name,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                OwnerId = owner.Id,
                FactionId = faction.Id,
                Faction = faction,
                DeckCards = new Collection<DeckCard>()
            };

            AddDeckCards(deck, apiDeck.Cards, DeckCardType.Normal, cardsByCode);
            AddDeckCards(deck, apiDeck.RookeryCards, DeckCardType.Rookery, cardsByCode);

            return deck;
        }

        private void UpdateDeckCards(Deck deck, List<ApiDeckCard> cards, DeckCardType cardType, Dictionary<string, Card> cardsByCode)
        {
            var existingCodes = deck.DeckCards.Where(dc => dc.CardType == cardType).Select(dc => dc.Card.Code).ToList();
            var updateCodes = cards.Select(c => c.Code).ToList();

            var newCodes = updateCodes.Except(existingCodes);
            var removedCodes = existingCodes.Except(updateCodes);

            var toRemove = deck.DeckCards.Where(dc => dc.CardType == cardType && removedCodes.Contains(dc.Card.Code)).ToList();
            foreach (var deckCard in toRemove)
            {
                deck.DeckCards.Remove(deckCard);
            }

            var toAdd = cards.Where(dc => newCodes.Contains(dc.Code)).ToList();
            AddDeckCards(deck, toAdd, cardType, cardsByCode);
        }
    }
}
