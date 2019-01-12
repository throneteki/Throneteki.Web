namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01205", Faction = "House Tyrell")]
    public class HouseTyrell : BannerValidator
    {
        public HouseTyrell(string factionName)
            : base(factionName)
        {
        }
    }
}
