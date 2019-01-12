namespace CrimsonDev.Throneteki.Models.Validators.Agendas
{
    using System;

    public class AgendaValidatorAttribute : Attribute
    {
        public string Code { get; set; }
        public string Faction { get; set; }
    }
}