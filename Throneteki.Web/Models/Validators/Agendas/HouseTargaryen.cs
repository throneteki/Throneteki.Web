namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "01204", Faction = "House Targaryen")]
    public class HouseTargaryen : BannerValidator
    {
        public HouseTargaryen(string factionName)
            : base(factionName)
        {
        }
    }
}
