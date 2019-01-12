namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.Models;

    [AgendaValidator(Code = "01027")]
    public class Fealty : BaseValidator
    {
        public Fealty()
        {
            Rules.Add(new ValidationRule
            {
                Message = "You cannot include more than 15 neutral cards in a deck with Fealty",
                Condition = deck => deck.CountCards(DeckCardType.Normal, card => card.IsDrawCard() && card.Faction.Code == "neutral") <= 15
            });
        }
    }
}
