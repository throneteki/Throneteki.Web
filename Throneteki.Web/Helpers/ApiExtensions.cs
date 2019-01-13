namespace CrimsonDev.Throneteki.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CrimsonDev.Throneteki.Data.GameData;
    using CrimsonDev.Throneteki.Data.Models;
    using CrimsonDev.Throneteki.Data.Models.Api;

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
                LastUpdated = deck.LastModified,
                Cards = new List<ApiDeckCard>(),
                RookeryCards = new List<ApiDeckCard>()
            };

            foreach (var deckCard in deck.DeckCards.Where(dc => dc.CardType == DeckCardType.Normal))
            {
                apiDeck.Cards.Add(new ApiDeckCard { Code = deckCard.Card.Code, Count = deckCard.Count });
            }

            foreach (var deckCard in deck.DeckCards.Where(dc => dc.CardType == DeckCardType.Rookery))
            {
                apiDeck.RookeryCards.Add(new ApiDeckCard { Code = deckCard.Card.Code, Count = deckCard.Count });
            }

            return apiDeck;
        }

        public static ApiCard ToApiCard(this Card card)
        {
            return new ApiCard
            {
                Code = card.Code,
                Name = card.Name,
                Cost = card.Cost,
                DeckLimit = card.DeckLimit,
                Faction = card.Faction.Code,
                Icons = card.Icons,
                Label = card.Label,
                Loyal = card.Loyal,
                PackCode = card.Pack.Code,
                PlotStats = card.PlotStats,
                Strength = card.Strength,
                Text = card.Text,
                Traits = card.Traits.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList(),
                Type = card.Type,
                Unique = card.Unique
            };
        }
    }
}
