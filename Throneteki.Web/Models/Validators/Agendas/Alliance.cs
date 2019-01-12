namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.Models;

    [AgendaValidator(Code = "06018")]
    public class Alliance : BaseValidator
    {
        public Alliance()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Alliance cannot have more than 2 Banner agendas",
                Condition = deck => deck.CountCards(DeckCardType.Normal, card => card.HasTrait("Banner")) <= 2
            });
        }

        public override int? RequiredDraw => 75;
    }
}
