namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01200", Faction = "House Lannister")]
    public class HouseLannister : BannerValidator
    {
        public HouseLannister(string factionName)
            : base(factionName)
        {
        }
    }
}
