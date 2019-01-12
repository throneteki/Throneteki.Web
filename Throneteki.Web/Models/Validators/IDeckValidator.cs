namespace CrimsonDev.Throneteki.Models.Validators
{
    using System.Collections.Generic;
    using CrimsonDev.Throneteki.Data.GameData;

    public interface IDeckValidator
    {
        int? RequiredPlots { get; }
        int? RequiredDraw { get; }
        int? MaxDoublePlots { get; }
        List<ValidationRule> Rules { get; set; }
        bool MayInclude(Card card);
        bool CannotInclude(Card card);
    }
}
