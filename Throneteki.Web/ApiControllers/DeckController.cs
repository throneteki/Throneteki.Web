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
    using CrimsonDev.Throneteki.Helpers;
    using CrimsonDev.Throneteki.Models.Api;
    using CrimsonDev.Throneteki.Models.Api.Request;
    using CrimsonDev.Throneteki.Models.Api.Response;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    public class DeckController : Controller
    {
        private readonly IThronetekiDbContext context;
        private readonly UserManager<GametekiUser> userManager;

        public DeckController(IThronetekiDbContext context, UserManager<GametekiUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        [Route("api/decks")]
        public async Task<IActionResult> GetDecks()
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var decks = await context.Deck.Include(d => d.Agenda).Include(d => d.Faction).Include(d => d.Owner).Include(d => d.DeckCards).ThenInclude(dc => dc.Card).Where(d => d.OwnerId == currentUser.Id).ToListAsync();

            return Json(new GetDecksResponse { Success = true, Decks = decks.Select(d => d.ToApiDeck()).ToList() });
        }

        [Authorize]
        [HttpGet]
        [Route("api/decks/{deckId}")]
        public async Task<IActionResult> GetDeck(int deckId)
        {
            var currentUser = await userManager.FindByNameAsync(User.Identity.Name);

            var deck = await context.Deck.Include(d => d.Agenda).Include(d => d.Faction).Include(d => d.Owner).Include(d => d.DeckCards).ThenInclude(dc => dc.Card).SingleOrDefaultAsync(d => d.Id == deckId);
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
        public async Task<IActionResult> AddDeck(AddDeckRequest request)
        {
            var cardsByCode = (await context.Card.ToListAsync()).ToDictionary(key => key.Code, value => value);

            var owner = await userManager.FindByNameAsync(User.Identity.Name);

            var newDeck = new Deck
            {
                AgendaId = cardsByCode[request.Deck.Agenda].Id,
                Name = request.Deck.Name,
                CreatedDate = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                OwnerId = owner.Id,
                FactionId = (await context.Faction.SingleOrDefaultAsync(f => f.Code == request.Deck.Faction.Code)).Id,
                DeckCards = new Collection<DeckCard>()
            };

            AddDeckCards(newDeck, request.Deck.PlotCards, DeckCardType.Plot, cardsByCode);
            AddDeckCards(newDeck, request.Deck.DrawCards, DeckCardType.Draw, cardsByCode);
            AddDeckCards(newDeck, request.Deck.RookeryCards, DeckCardType.Rookery, cardsByCode);

            foreach (var card in request.Deck.BannerCards)
            {
                newDeck.DeckCards.Add(new DeckCard { CardId = cardsByCode[card].Id, CardType = DeckCardType.Banner, Count = 1, Deck = newDeck });
            }

            context.Deck.Add(newDeck);

            await context.SaveChangesAsync();

            return this.SuccessResponse();
        }

        [Authorize]
        [HttpPut]
        [Route("api/decks/{deckId}")]
        public async Task<IActionResult> UpdateDeck(int deckId, AddDeckRequest request)
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

            var cardsByCode = (await context.Card.ToListAsync()).ToDictionary(key => key.Code, value => value);

            deck.AgendaId = cardsByCode[request.Deck.Agenda].Id;
            deck.Name = request.Deck.Name;
            deck.LastModified = DateTime.UtcNow;
            deck.FactionId = (await context.Faction.SingleOrDefaultAsync(f => f.Code == request.Deck.Faction.Code)).Id;

            UpdateDeckCards(deck, request.Deck.PlotCards, DeckCardType.Plot, cardsByCode);
            UpdateDeckCards(deck, request.Deck.DrawCards, DeckCardType.Draw, cardsByCode);
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

        private static void AddDeckCards(Deck newDeck, List<ApiDeckCard> cards, DeckCardType cardType, Dictionary<string, Card> cardsByCode)
        {
            foreach (var card in cards)
            {
                newDeck.DeckCards.Add(
                    new DeckCard
                    {
                        CardId = cardsByCode[card.Code].Id,
                        CardType = cardType,
                        Count = card.Count,
                        Deck = newDeck
                    });
            }
        }

        private void UpdateDeckCards(Deck deck, List<ApiDeckCard> cards, DeckCardType cardType, Dictionary<string, Card> cardsByCode)
        {
            var existingCodes = deck.DeckCards.Select(dc => dc.Card.Code).ToList();
            var updateCodes = cards.Select(c => c.Code).ToList();

            var newCodes = updateCodes.Except(existingCodes);
            var removedCodes = existingCodes.Except(updateCodes);

            var toRemove = deck.DeckCards.Where(dc => removedCodes.Contains(dc.Card.Code));
            foreach (var deckCard in toRemove)
            {
                deck.DeckCards.Remove(deckCard);
            }

            var toAdd = cards.Where(dc => newCodes.Contains(dc.Code)).ToList();
            AddDeckCards(deck, toAdd, cardType, cardsByCode);
        }
    }
}
