namespace CrimsonDev.Throneteki.Models.Validators
{
    using System.Collections.Generic;
    using CrimsonDev.Throneteki.Data.GameData;

    public abstract class BaseValidator : IDeckValidator
    {
        protected BaseValidator()
        {
            Rules = new List<ValidationRule>();
        }

        public virtual int? RequiredPlots => null;
        public virtual int? RequiredDraw => null;
        public virtual int? MaxDoublePlots => null;
        public List<ValidationRule> Rules { get; set; }

        public virtual bool MayInclude(Card card)
        {
            return true;
        }

        public virtual bool CannotInclude(Card card)
        {
            return false;
        }
    }
}