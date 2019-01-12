namespace CrimsonDev.Throneteki.Models.Api
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CrimsonDev.Throneteki.Data.GameData;

    public class ApiDeck
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTime LastUpdated { get; set; }
        [Required]
        public Faction Faction { get; set; }
        public List<ApiDeckCard> Cards { get; set; }
        public List<ApiDeckCard> RookeryCards { get; set; }
        public DeckValidationResult ValidationResult { get; set; }
    }
}
