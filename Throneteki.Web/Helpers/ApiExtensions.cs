namespace CrimsonDev.Throneteki.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using CrimsonDev.Throneteki.Data.Models;
    using CrimsonDev.Throneteki.Models.Api;

    public static class ApiExtensions
    {
        public static ApiDeck ToApiDeck(this Deck deck)
        {
            var apiDeck = new ApiDeck
            {
                Id = deck.Id,
                Name = deck.Name,
                Username = deck.Owner.UserName,
                Faction = deck.Faction,
                Agenda = deck.Agenda.Code,
                LastUpdated = deck.LastModified,
                PlotCards = new List<ApiDeckCard>(),
                BannerCards = new List<string>(),
                DrawCards = new List<ApiDeckCard>(),
                RookeryCards = new List<ApiDeckCard>()
            };

            foreach (var deckCard in deck.DeckCards.Where(dc => dc.CardType == DeckCardType.Plot))
            {
                apiDeck.PlotCards.Add(new ApiDeckCard { Code = deckCard.Card.Code, Count = deckCard.Count });
            }

            foreach (var deckCard in deck.DeckCards.Where(dc => dc.CardType == DeckCardType.Draw))
            {
                apiDeck.DrawCards.Add(new ApiDeckCard { Code = deckCard.Card.Code, Count = deckCard.Count });
            }

            foreach (var deckCard in deck.DeckCards.Where(dc => dc.CardType == DeckCardType.Rookery))
            {
                apiDeck.RookeryCards.Add(new ApiDeckCard { Code = deckCard.Card.Code, Count = deckCard.Count });
            }

            foreach (var deckCard in deck.DeckCards.Where(dc => dc.CardType == DeckCardType.Banner))
            {
                apiDeck.BannerCards.Add(deckCard.Card.Code);
            }

            return apiDeck;
        }
    }
}
