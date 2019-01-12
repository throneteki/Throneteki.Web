namespace CrimsonDev.Throneteki.Models.Validators
{
    using System.Linq;
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.Models;

    public class RookeryValidator : BaseValidator
    {
        public RookeryValidator()
        {
            Rules.Add(new ValidationRule
            {
                Message = "More than 2 plot cards in rookery",
                Condition = deck => deck.DeckCards.All(dc => dc.CardType != DeckCardType.Rookery) || deck.CountCards(DeckCardType.Rookery, card => card.IsPlotCard()) <= 2
            });

            Rules.Add(new ValidationRule
            {
                Message = "More than 10 draw cards in rookery",
                Condition = deck => deck.DeckCards.All(dc => dc.CardType != DeckCardType.Rookery) || deck.CountCards(DeckCardType.Rookery, card => card.IsDrawCard()) <= 10
            });
        }
    }
}
