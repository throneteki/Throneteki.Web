namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    public abstract class DraftAgenda : BaseValidator
    {
        public override int? RequiredDraw => 40;
        public override int? RequiredPlots => 5;
    }
}
