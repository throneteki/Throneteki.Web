namespace CrimsonDev.Throneteki.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using CrimsonDev.Throneteki.Data.GameData;

    public class RestrictedListValidationResult
    {
        public RestrictedListValidationResult()
        {
            Errors = new List<string>();
        }

        public string Version { get; set; }
        public List<string> Errors { get; set; }
        public List<Card> JoustCardsOnList { get; set; }

        public bool ValidForJoust => JoustCardsOnList.Count <= 1;
        public bool Valid => !Errors.Any();
    }
}
