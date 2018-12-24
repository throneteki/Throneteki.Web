namespace CrimsonDev.Throneteki.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CrimsonDev.Throneteki.Data;
    using CrimsonDev.Throneteki.Data.GameData;
    using Microsoft.EntityFrameworkCore;

    public class CardService : ICardService
    {
        private readonly ThronetekiDbContext context;

        public CardService(ThronetekiDbContext context)
        {
            this.context = context;
        }

        public async Task<Dictionary<string, Card>> GetAllCardsAsync()
        {
            var allCards = await context.Card.Include(c => c.Faction).Include(c => c.Pack).ToListAsync();

            return allCards.ToDictionary(key => key.Code, value => value);
        }

        public Task<List<Pack>> GetAllPacksAsync()
        {
            return context.Pack.ToListAsync();
        }

        public Task<List<Faction>> GetAllFactionsAsync()
        {
            return context.Faction.ToListAsync();
        }

        public Task<List<RestrictedListEntry>> GetRestrictedListAsync()
        {
            return context.RestrictedListEntry.Include(rl => rl.JoustCards).Include("JoustCards.Card").Include("JoustCards.Card.Faction")
                .Include(rl => rl.MeleeCards).Include("MeleeCards.Card").Include("MeleeCards.Card.Faction").ToListAsync();
        }
    }
}