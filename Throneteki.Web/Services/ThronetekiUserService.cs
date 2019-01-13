namespace CrimsonDev.Throneteki.Services
{
    using System.Threading.Tasks;
    using CrimsonDev.Gameteki.Api.Services;
    using CrimsonDev.Gameteki.Data;
    using CrimsonDev.Gameteki.Data.Models;
    using CrimsonDev.Gameteki.Data.Models.Config;
    using CrimsonDev.Throneteki.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class ThronetekiUserService : UserService
    {
        public ThronetekiUserService(IGametekiDbContext context, UserManager<GametekiUser> userManager, IOptions<AuthTokenOptions> optionsAccessor, IOptions<GametekiApiOptions> apiOptions, IEmailSender emailSender, IViewRenderService viewRenderService, ILogger<UserService> logger)
            : base(context, userManager, optionsAccessor, apiOptions, emailSender, viewRenderService, logger)
        {
        }

        public override async Task<GametekiUser> GetUserFromUsernameAsync(string username)
        {
            var user = await base.GetUserFromUsernameAsync(username);
            if (user == null)
            {
                return null;
            }

            var settings = user.CustomData == null ? new ThronetekiSettings() : JsonConvert.DeserializeObject<ThronetekiSettings>(user.CustomData);

            SetDefaultSettings(settings);
            user.CustomData = JsonConvert.SerializeObject(settings, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            return user;
        }

        private static void SetDefaultSettings(ThronetekiSettings settings)
        {
            if (!settings.PromptDupes.HasValue)
            {
                settings.PromptDupes = false;
            }

            if (settings.KeywordSettings == null)
            {
                settings.KeywordSettings = new KeywordSettings();
            }

            settings.KeywordSettings.ChooseCards = settings.KeywordSettings.ChooseCards.GetValueOrDefault(false);
            settings.KeywordSettings.ChooseOrder = settings.KeywordSettings.ChooseOrder.GetValueOrDefault(false);

            if (settings.TimerSettings == null)
            {
                settings.TimerSettings = new TimerSettings();
            }

            settings.TimerSettings.Abilities = settings.TimerSettings.Abilities.GetValueOrDefault(false);
            settings.TimerSettings.Events = settings.TimerSettings.Events.GetValueOrDefault(true);
            settings.TimerSettings.Duration = settings.TimerSettings.Duration.GetValueOrDefault(10);

            if (settings.PromptedActionWindows == null)
            {
                settings.PromptedActionWindows = new ActionWindowSettings();
            }

            settings.PromptedActionWindows.PlotPhase = settings.PromptedActionWindows.PlotPhase.GetValueOrDefault(false);
            settings.PromptedActionWindows.DrawPhase = settings.PromptedActionWindows.DrawPhase.GetValueOrDefault(false);
            settings.PromptedActionWindows.ChallengeBegin = settings.PromptedActionWindows.ChallengeBegin.GetValueOrDefault(false);
            settings.PromptedActionWindows.AttackersDeclared = settings.PromptedActionWindows.AttackersDeclared.GetValueOrDefault(true);
            settings.PromptedActionWindows.DefendersDeclared = settings.PromptedActionWindows.DefendersDeclared.GetValueOrDefault(true);
            settings.PromptedActionWindows.DominancePhase = settings.PromptedActionWindows.DominancePhase.GetValueOrDefault(false);
            settings.PromptedActionWindows.StandingPhase = settings.PromptedActionWindows.StandingPhase.GetValueOrDefault(false);
            settings.PromptedActionWindows.TaxationPhase = settings.PromptedActionWindows.TaxationPhase.GetValueOrDefault(false);
        }
    }
}
