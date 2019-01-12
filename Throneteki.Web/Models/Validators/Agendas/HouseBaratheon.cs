namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01198", Faction="House Baratheon")]
    public class HouseBaratheon : BannerValidator
    {
        public HouseBaratheon(string factionName)
            : base(factionName)
        {
        }
    }
}
