namespace CrimsonDev.Throneteki.Models.Validators
{
    using System;
    using CrimsonDev.Throneteki.Data.Models;

    public class ValidationRule
    {
        public string Message { get; set; }
        public Func<Deck, bool> Condition { get; set; }
    }
}
