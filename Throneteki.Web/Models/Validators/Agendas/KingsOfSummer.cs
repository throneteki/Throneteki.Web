namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System.Linq;

    [AgendaValidator(Code = "04037")]
    public class KingsOfSummer : BaseValidator
    {
        public KingsOfSummer()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Kings of Summer cannot include Winter plot cards",
                Condition = deck => !deck.DeckCards.Any(dc => dc.Card.IsPlotCard() && dc.Card.HasTrait("Winter"))
            });
        }
    }
}
