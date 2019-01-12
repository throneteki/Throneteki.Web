namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System.Linq;

    [AgendaValidator(Code = "00001")]
    public class ThePowerOfWealth : DraftAgenda
    {
        public ThePowerOfWealth()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Cannot include cards from more than 1 outside faction",
                Condition = deck => deck.GetNormalCards().Count(dc => dc.Card.Faction.Code != deck.Faction.Code && dc.Card.Faction.Code != "neutral") <= 1
            });
        }
    }
}
