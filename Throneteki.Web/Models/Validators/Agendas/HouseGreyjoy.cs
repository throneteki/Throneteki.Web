namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01199", Faction = "House Greyjoy")]
    public class HouseGreyjoy : BannerValidator
    {
        public HouseGreyjoy(string factionName)
            : base(factionName)
        {
        }
    }
}
