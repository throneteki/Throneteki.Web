namespace CrimsonDev.Throneteki.Models
{
    using System.Collections.Generic;
    using CrimsonDev.Throneteki.Data.GameData;

    public class ApiCard
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int DeckLimit { get; set; }
        public string Cost { get; set; }
        public PlotStats PlotStats { get; set; }
        public string Strength { get; set; }
        public bool Unique { get; set; }
        public bool Loyal { get; set; }
        public CardIcons Icons { get; set; }
        public string Label { get; set; }
        public string Faction { get; set; }
        public string PackCode { get; set; }
        public List<string> Traits { get; set; } = new List<string>();
    }
}
