namespace CrimsonDev.Throneteki.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using CrimsonDev.Throneteki.Helpers;
    using Newtonsoft.Json;
    using StackExchange.Redis;

    public class RedisManager
    {
        private readonly ICardService cardService;
        private readonly IDatabase database;

        public RedisManager(ICardService cardService, IConnectionMultiplexer redisConnection)
        {
            this.cardService = cardService;

            database = redisConnection.GetDatabase();
        }

        public async Task PopulateCards()
        {
            var cards = (await cardService.GetAllCardsAsync()).ToDictionary(k => k.Key, v => v.Value.ToApiCard());

            await database.StringSetAsync("cards", JsonConvert.SerializeObject(cards));
        }
    }
}
