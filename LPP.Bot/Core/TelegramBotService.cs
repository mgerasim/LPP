using LPP.Bot.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using static System.Formats.Asn1.AsnWriter;

namespace LPP.Bot.Core
{
    public class TelegramBotService : BackgroundService
    {
        private readonly ITelegramBotClient _botClient;

        private readonly UserStateManager _userStateManager;

        private readonly ILogger<TelegramBotService> _logger;

        public TelegramBotService(UserStateManager userStateManager, IOptions<BotConfiguration> botConfigure, ILogger<TelegramBotService> logger)
        {
            _botClient = new TelegramBotClient(botConfigure.Value.BotToken);

            _userStateManager = userStateManager;

            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, stoppingToken);

            _logger.LogInformation("Bot started"); while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var userState = await _userStateManager.StartAsync(update, botClient);

            if (update.Message is { } message)
            {
                if (message.Text == "/start")
                {
                    userState.ServiceProvider.GetRequiredService<CurrentUserState>().UserState.Clear();

                    await userState.ServiceProvider.GetRequiredService<IMediator>().Send(new StartCommand(), cancellationToken);
                }
                else if (message.Text == "/program" || message.Text == HandlerConstant.ShowProgram)
                {
                    await userState.ServiceProvider.GetRequiredService<IMediator>().Send(new ProgramCommand(), cancellationToken);
                }
                else if (message.Text == "/news" || message.Text == HandlerConstant.ShowNews)
                {
                    await userState.ServiceProvider.GetRequiredService<IMediator>().Send(new ProgramCommand(), cancellationToken);
                }
            }
            else if (update.CallbackQuery is { } callbackQuery)
            {
                await botClient.AnswerCallbackQuery(update.CallbackQuery.Id);

                if (update.CallbackQuery.Data == HandlerConstant.Byliner)
                {
                    await userState.ServiceProvider.GetRequiredService<IMediator>().Send(new BylinerCommand(), cancellationToken);
                }
                else if (update.CallbackQuery.Data == HandlerConstant.AboutCompetition)
                {
                    await userState.ServiceProvider.GetRequiredService<IMediator>().Send(new AboutCompetitionCommand(), cancellationToken);
                }
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
