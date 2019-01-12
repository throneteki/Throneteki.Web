namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.GameData;
    using CrimsonDev.Throneteki.Data.Models;

    [AgendaValidator(Code = "09045")]
    public class TheConclave : BaseValidator
    {
        public TheConclave()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Must contain 12 or more Maester characters",
                Condition = deck => deck.CountCards(DeckCardType.Normal, card => card.IsCharacter() && card.HasTrait("Maester")) >= 12
            });
        }

        public override bool MayInclude(Card card)
        {
            return card.IsCharacter() && card.HasTrait("Maester") && !card.Loyal;
        }
    }
}
