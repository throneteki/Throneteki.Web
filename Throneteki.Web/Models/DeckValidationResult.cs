namespace CrimsonDev.Throneteki.Models
{
    using System.Collections.Generic;

    public class DeckValidationResult
    {
        public DeckValidationResult()
        {
            ExtendedStatus = new List<string>();
        }

        public bool BasicRules { get; set; }
        public bool FaqJoustRules { get; set; }
        public string FaqVersion { get; set; }
        public bool NoUnreleasedCards { get; set; }
        public int PlotCount { get; set; }
        public int DrawCount { get; set; }
        public List<string> ExtendedStatus { get; set; }

        public void AddError(string error)
        {
            ExtendedStatus.Add(error);
        }
    }
}
