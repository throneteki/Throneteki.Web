namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System.Linq;

    [AgendaValidator(Code = "04038")]
    public class KingsOfWinter : BaseValidator
    {
        public KingsOfWinter()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Kings of Winter cannot include Summer plot cards",
                Condition = deck => !deck.DeckCards.Any(dc => dc.Card.IsPlotCard() && dc.Card.HasTrait("Summer"))
            });
        }
    }
}
