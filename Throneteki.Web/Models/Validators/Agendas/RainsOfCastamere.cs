namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System.Linq;
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.Models;

    [AgendaValidator(Code = "05045")]
    public class RainsOfCastamere : BaseValidator
    {
        public RainsOfCastamere()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Rains of Castamere must contain exactly 5 different Scheme plots",
                Condition = deck =>
                {
                    var schemePlots = deck.NormalCards.Where(dc => dc.Card.IsPlotCard() && dc.Card.HasTrait("Scheme"));

                    return schemePlots.Count() == 5 && deck.CountCards(DeckCardType.Normal, card => card.IsPlotCard() && card.HasTrait("Scheme")) == 5;
                }
            });
        }

        public override int? RequiredPlots => 12;
    }
}
