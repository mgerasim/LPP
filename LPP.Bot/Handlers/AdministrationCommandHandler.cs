using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class AdministrationCommand : IRequest
    {

    }

    public class AdministrationCommandHandler : IRequestHandler<AdministrationCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;

        public AdministrationCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(AdministrationCommand request, CancellationToken cancellationToken)
        {
            this.userState.UserState.Clear();

            var keyboardInline = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Список пользователей", HandlerConstant.GetUsersCmd),
                    },
                    new []  
                    {
                        InlineKeyboardButton.WithCallbackData("📢 Отправить сообщение участникам 🔔", HandlerConstant.SendMessageToSpeakers),
                    },
                });

            await this.userState.BotClient.DeleteMessage(this.userState.ChatId, this.userState.Update.Message.Id);

            Message sentMessage = await this.userState.BotClient.SendMessage(
                chatId: this.userState.ChatId,
                text: "🛠️ Выберите действие 👇",
                replyMarkup: keyboardInline,
                cancellationToken: CancellationToken.None);
        }
    }
}
