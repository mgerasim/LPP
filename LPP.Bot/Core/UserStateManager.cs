using LPP.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LPP.Bot.Core
{
    public class UserStateManager
    {
        private readonly IServiceScopeFactory scopeFactory;

        private readonly ConcurrentDictionary<long, IServiceScope> userStates = new();

        public UserStateManager(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public async Task<IServiceScope> StartAsync(Update update, ITelegramBotClient botClient)
        {

            long chatId = update.Type switch
            {
                UpdateType.Message => update.Message.Chat.Id,

                UpdateType.CallbackQuery => update.CallbackQuery.From.Id,

                _ => throw new NotSupportedException(),
            };

            this.userStates.TryAdd(
                    chatId,
                    this.scopeFactory.CreateScope()
                    );

            this.userStates[chatId]
                    .ServiceProvider
                    .GetRequiredService<CurrentUserState>()
                    .Update = update;

            this.userStates[chatId]
                    .ServiceProvider
                    .GetRequiredService<CurrentUserState>()
                    .ChatId = chatId;

            this.userStates[chatId]
                    .ServiceProvider
                    .GetRequiredService<CurrentUserState>()
                    .BotClient = botClient;

            var botName = await botClient.GetMe();

            this.userStates[chatId]
                    .ServiceProvider
                    .GetRequiredService<CurrentUserState>()
                    .BotName = botName.Username;

            var user = await this.userStates[chatId].ServiceProvider.GetRequiredService<LPPContext>()
                .Users.SingleOrDefaultAsync(x => x.TelegramId == chatId);

            if (user != null)
            {
                this.userStates[chatId].ServiceProvider.GetRequiredService<LPPContext>().Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                this.userStates[chatId]
                        .ServiceProvider
                        .GetRequiredService<CurrentUserState>()
                        .User = user;

                user.UpdatedAt = DateTime.UtcNow;

                this.userStates[chatId].ServiceProvider.GetRequiredService<LPPContext>().Update(user);

                await this.userStates[chatId].ServiceProvider.GetRequiredService<LPPContext>().SaveChangesAsync();

                this.userStates[chatId].ServiceProvider.GetRequiredService<LPPContext>().Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }

            return userStates[chatId];
        }

        public void SetState(long userId, UserState state) => this.userStates[userId]
                    .ServiceProvider
                    .GetRequiredService<CurrentUserState>()
            .UserState = state;

        public void ClearState(long userId) => this.userStates[userId].ServiceProvider
                    .GetRequiredService<CurrentUserState>()
                    .UserState.Clear();
    }
}
