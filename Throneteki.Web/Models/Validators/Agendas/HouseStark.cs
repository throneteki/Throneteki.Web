namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01203", Faction = "House Stark")]
    public class HouseStark : BannerValidator
    {
        public HouseStark(string factionName)
            : base(factionName)
        {
        }
    }
}
