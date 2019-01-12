namespace CrimsonDev.Throneteki.Models.Validators
{
    using CrimsonDev.Throneteki.Data.GameData;

    public class FactionValidator : BaseValidator
    {
        private readonly string factionCode;

        public FactionValidator(string factionCode)
        {
            this.factionCode = factionCode;
        }

        public override bool MayInclude(Card card)
        {
            return card.Faction.Code == factionCode || card.Faction.Code == "neutral";
        }
    }
}
