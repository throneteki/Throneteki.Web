namespace CrimsonDev.Throneteki.Services
{
    using System.Threading.Tasks;
    using CrimsonDev.Throneteki.Data.Models;
    using CrimsonDev.Throneteki.Data.Models.Validators;

    public interface IDeckValidationService
    {
        Task<DeckValidationResult> ValidateDeckAsync(Deck deck);
    }
}
