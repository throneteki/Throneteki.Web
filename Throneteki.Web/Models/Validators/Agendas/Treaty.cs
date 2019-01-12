namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System.Linq;

    public class Treaty : DraftAgenda
    {
        public Treaty()
        {
            Rules.Add(new ValidationRule
            {
                Message = "Cannot include cards from more than 2 outside factions",
                Condition = deck => deck.GetNormalCards().Count(dc => dc.Card.Faction.Code != deck.Faction.Code && dc.Card.Faction.Code != "neutral") <= 2
            });
        }
    }
}