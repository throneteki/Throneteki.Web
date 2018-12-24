namespace CrimsonDev.Throneteki.Models.Api.Response
{
    using System.Collections.Generic;
    using CrimsonDev.Gameteki.Api.Models.Api.Response;
    using CrimsonDev.Throneteki.Data.GameData;

    public class GetFactionsResponse : ApiResponse
    {
        public List<Faction> Factions { get; set; }
    }
}