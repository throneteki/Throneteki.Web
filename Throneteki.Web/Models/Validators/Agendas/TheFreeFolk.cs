namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using CrimsonDev.Throneteki.Data.GameData;

    [AgendaValidator(Code = "11079")]
    public class TheFreeFolk : BaseValidator
    {
        public override bool CannotInclude(Card card)
        {
            return card.Faction.Code != "neutral";
        }
    }
}
