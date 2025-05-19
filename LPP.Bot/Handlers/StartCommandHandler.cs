using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class StartCommand: IRequest
    {

    }

    public class StartCommandHandler : IRequestHandler<StartCommand>
    {
        private readonly LPPContext context;

        private readonly KeyboardHandler keyboardHandler;

        private readonly CurrentUserState userState;

        public StartCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }

        public async Task Handle(StartCommand request, CancellationToken cancellationToken)
        {
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.TelegramId == this.userState.ChatId);

            if (user is null)
            {
                user = new Models.Entities.User
                {
                    TelegramId = this.userState.ChatId,

                    Username = this.userState.Update.Message.Chat.Username,

                    LastName = this.userState.Update.Message.Chat.LastName,

                    FirstName = this.userState.Update.Message.Chat.FirstName,

                    CreatedAt = DateTime.UtcNow,

                    UpdatedAt = DateTime.UtcNow,
                };

                await this.context.AddAsync(user);

                await this.context.SaveChangesAsync();
            }
            else
            {
                user.Username = this.userState.Update.Message.Chat.Username;

                user.LastName = this.userState.Update.Message.Chat.LastName;

                user.FirstName = this.userState.Update.Message.Chat.FirstName;

                user.UpdatedAt = DateTime.UtcNow;

                this.context.Update(user);

                await this.context.SaveChangesAsync();
            }

            this.context.Entry(user).State = EntityState.Detached;

            this.userState.User = user;

            this.userState.UserState.Clear();

            var keyboard = this.keyboardHandler.GetKeaboard();

            string hello = $@"
** 👋 Добро пожаловать **  

Здесь Вы найдёте полезную информацию о региональном этапе конкурса профессионального мастерства «Лучший по профессии» и сможете ознакомиться с актуальными новостями состязаний.

";


            await userState.BotClient.SendSticker(
                chatId: this.userState.ChatId,
                replyMarkup: keyboard,
                sticker: "CAACAgIAAxkBAAIHymgmsmu42ozei3DcnpviKoqDyPYWAAJFaAAC1Bk4ScQExV2943heNgQ"
            );

            var kbrd = new InlineKeyboardMarkup();

            if (!this.userState.User.IsBylinerReaded)
            {
                kbrd.AddNewRow(new[]
                {
                    InlineKeyboardButton.WithCallbackData("🎤 Приветственное слово", HandlerConstant.Byliner)
                });
            }

            kbrd.AddNewRow(new[]
                {
                    InlineKeyboardButton.WithCallbackData("🏆 О конкурсе", HandlerConstant.AboutCompetition),
                });

            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: hello,
                    replyMarkup: kbrd,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: CancellationToken.None);
            
        }
    }
}
