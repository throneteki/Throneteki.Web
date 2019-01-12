namespace CrimsonDev.Throneteki.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using CrimsonDev.Throneteki.Data.Extensions;
    using CrimsonDev.Throneteki.Data.GameData;
    using CrimsonDev.Throneteki.Data.Models;
    using CrimsonDev.Throneteki.Models;
    using CrimsonDev.Throneteki.Models.Validators;
    using CrimsonDev.Throneteki.Models.Validators.Agendas;

    public class DeckValidationService : IDeckValidationService
    {
        private readonly ICardService cardService;
        private readonly Dictionary<string, IDeckValidator> agendaValidators;
        private List<IDeckValidator> validators;

        public DeckValidationService(ICardService cardService)
        {
            this.cardService = cardService;
            validators = new List<IDeckValidator>();
            agendaValidators = new Dictionary<string, IDeckValidator>();

            var agendaValidatorTypes = GetTypesWithHelpAttribute(Assembly.GetEntryAssembly());
            foreach (var validatorType in agendaValidatorTypes)
            {
                var attribute = validatorType.GetCustomAttribute<AgendaValidatorAttribute>();
                BaseValidator validator;
                if (attribute.Faction != null)
                {
                    validator = Activator.CreateInstance(validatorType, attribute.Faction) as BaseValidator;
                }
                else
                {
                    validator = Activator.CreateInstance(validatorType) as BaseValidator;
                }

                agendaValidators[attribute.Code] = validator;
            }
        }

        public async Task<DeckValidationResult> ValidateDeckAsync(Deck deck)
        {
            validators = new List<IDeckValidator> { new FactionValidator(deck.Faction.Code) };

            if (deck.Agenda != null && agendaValidators.ContainsKey(deck.Agenda.Code))
            {
                validators.Add(agendaValidators[deck.Agenda.Code]);
            }

            var result = ValidateBasicRules(deck);

            var rules = validators.SelectMany(v => v.Rules);
            foreach (var rule in rules)
            {
                if (!rule.Condition(deck))
                {
                    result.AddError(rule.Message);
                }
            }

            result.NoUnreleasedCards = true;

            foreach (var deckCard in deck.DeckCards)
            {
                if (!validators.Any(v => v.MayInclude(deckCard.Card)) || validators.Any(v => v.CannotInclude(deckCard.Card)))
                {
                    result.AddError($"{deckCard.Card.Label} is not allowed by faction or agenda");
                }

                if (!IsCardInReleasedPack(deckCard.Card))
                {
                    result.NoUnreleasedCards = false;
                    result.AddError($"{deckCard.Card.Label} is not yet released");
                }

                if (deckCard.CardType == DeckCardType.Normal && deckCard.Count > deckCard.Card.DeckLimit)
                {
                    result.AddError($"{deckCard.Card.Label} has limit {deckCard.Card.DeckLimit}");
                }
            }

            if (deck.DeckCards.Any(dc => dc.Card.IsDraftCard()))
            {
                result.AddError("You cannot include Draft cards in a normal deck");
            }

            if (result.ExtendedStatus.Any())
            {
                result.BasicRules = false;
            }

            var restrictedListResult = await ValidateRestrictedList(deck);
            result.ExtendedStatus.AddRange(restrictedListResult.Errors);

            result.FaqJoustRules = restrictedListResult.ValidForJoust;
            result.FaqVersion = restrictedListResult.Version;

            return result;
        }

        private static IEnumerable<Type> GetTypesWithHelpAttribute(Assembly assembly)
        {
            return assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(AgendaValidatorAttribute), true).Length > 0);
        }

        private static bool IsCardInReleasedPack(Card card)
        {
            if (card.Pack.ReleaseDate == null)
            {
                return false;
            }

            return card.Pack.ReleaseDate <= DateTime.UtcNow;
        }

        private async Task<RestrictedListValidationResult> ValidateRestrictedList(Deck deck)
        {
            var result = new RestrictedListValidationResult();

            var restrictedList = await cardService.GetRestrictedListAsync();
            var currentRules = restrictedList.OrderByDescending(rl => rl.Date).First();

            var joustCardsOnList = deck.DeckCards.Where(dc => currentRules.JoustCards.Any(jc => jc.CardId == dc.CardId)).Select(dc => dc.Card).ToList();

            if (joustCardsOnList.Count > 1)
            {
                result.Errors.Add($"Contains more than 1 card on the FAQ v{currentRules.Version} Joust restricted list: {string.Join(',', joustCardsOnList.Select(card => card.Name))}");
            }

            result.Version = currentRules.Version;
            result.JoustCardsOnList = joustCardsOnList;

            return result;
        }

        private DeckValidationResult ValidateBasicRules(Deck deck)
        {
            var standardRules = new StandardRules();

            var result = new DeckValidationResult();

            var requiredPlots = validators.SingleOrDefault(v => v.RequiredPlots.HasValue)?.RequiredPlots ?? standardRules.RequiredPlots;
            var requiredDraw = validators.SingleOrDefault(v => v.RequiredDraw.HasValue)?.RequiredDraw ?? standardRules.RequiredDraw;
            var maxDoublePlots = validators.SingleOrDefault(v => v.MaxDoublePlots.HasValue)?.MaxDoublePlots ?? standardRules.MaxDoublePlots;

            var plotCount = deck.CountCards(DeckCardType.Normal, card => card.IsPlotCard());
            var drawCount = deck.CountCards(DeckCardType.Normal, card => card.IsDrawCard());

            if (plotCount < requiredPlots)
            {
                result.AddError("Too few plot cards");
            }
            else if (plotCount > requiredPlots)
            {
                result.AddError("Too many plot cards");
            }

            if (drawCount < requiredDraw)
            {
                result.AddError("Too few draw cards");
            }

            if (deck.NormalCards.Count(dc => dc.Card.IsPlotCard() && dc.Count == 2) > maxDoublePlots)
            {
                result.AddError($"Maximum allowed number of doubled plots: {maxDoublePlots}");
            }

            result.PlotCount = plotCount;
            result.DrawCount = drawCount;

            result.BasicRules = true;

            return result;
        }
    }
}
