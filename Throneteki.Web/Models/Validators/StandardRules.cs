namespace CrimsonDev.Throneteki.Models.Validators
{
    public class StandardRules : BaseValidator
    {
        public override int? RequiredDraw => 60;
        public override int? RequiredPlots => 7;
        public override int? MaxDoublePlots => 1;
    }
}
