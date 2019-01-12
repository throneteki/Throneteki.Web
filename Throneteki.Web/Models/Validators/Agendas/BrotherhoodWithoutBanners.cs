namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System.Linq;
    using CrimsonDev.Throneteki.Data.GameData;

    [AgendaValidator(Code = "06119")]
    public class BrotherhoodWithoutBanners : BaseValidator
    {
        public BrotherhoodWithoutBanners()
        {
            Rules.Add(new ValidationRule
            {
                Message = "The Brotherhood Without Banners cannot include loyal characters",
                Condition = deck => !deck.GetNormalCards().Any(dc => dc.Card.IsCharacter() && dc.Card.Loyal)
            });
        }

        public override bool CannotInclude(Card card)
        {
            return card.Loyal && card.IsCharacter();
        }
    }
}
