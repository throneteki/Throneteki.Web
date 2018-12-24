namespace CrimsonDev.Throneteki.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CrimsonDev.Throneteki.Data.GameData;

    public interface ICardService
    {
        Task<Dictionary<string, Card>> GetAllCardsAsync();
        Task<List<Pack>> GetAllPacksAsync();
        Task<List<Faction>> GetAllFactionsAsync();
        Task<List<RestrictedListEntry>> GetRestrictedListAsync();
    }
}