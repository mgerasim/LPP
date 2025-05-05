using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LPP.Bot.Core
{
    public class CurrentUserState
    {
        private readonly BotConfiguration botConfiguration;
        public CurrentUserState(IOptions<BotConfiguration> options)
        {
            this.botConfiguration = options.Value;
        }

        public UserState UserState { get; set; } = new UserState();

        public long ChatId { get; set; }

        public Models.Entities.User User { get; set; }

        public Update Update { get; set; }

        public bool IsAdmin => this.botConfiguration.AdminIds.Contains(this.ChatId) || this.User.IsAdmin;

        public ITelegramBotClient BotClient { get; set; }
        public string BotName { get; internal set; }
    }
}
