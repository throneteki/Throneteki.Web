namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using CrimsonDev.Throneteki.Data.GameData;

    [AgendaValidator(Code = "00004")]
    public class UnitingTheSevenKingdoms : DraftAgenda
    {
        public override bool MayInclude(Card card)
        {
            return card.IsDrawCard();
        }
    }
}
