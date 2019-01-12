namespace CrimsonDev.Throneteki.Models.Validators
{
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.GameData;
    using CrimsonDev.Throneteki.Data.Models;

    public abstract class BannerValidator : BaseValidator
    {
        private readonly string factionName;

        protected BannerValidator(string factionName)
        {
            this.factionName = factionName;

            Rules.Add(new ValidationRule
            {
                Message = $"Must contain 12 or more {factionName} cards",
                Condition = deck => deck.CountCards(DeckCardType.Normal, card => card.Faction.Name == factionName && card.IsDrawCard()) >= 12
            });
        }

        public override bool MayInclude(Card card)
        {
            return card.Faction.Name == factionName && !card.Loyal && !card.IsPlotCard();
        }
    }
}
