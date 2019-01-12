namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01202", Faction = "The Night's Watch")]
    public class TheNightsWatch : BannerValidator
    {
        public TheNightsWatch(string factionName)
            : base(factionName)
        {
        }
    }
}
