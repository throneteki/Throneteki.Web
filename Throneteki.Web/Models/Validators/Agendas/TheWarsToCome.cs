namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    [AgendaValidator(Code = "10045")]
    public class TheWarsToCome : BaseValidator
    {
        public override int? RequiredPlots => 10;
        public override int? MaxDoublePlots => 2;
    }
}
