using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class ProgramCommand : IRequest
    {

    }

    public class ProgramCommandHandler : IRequestHandler<ProgramCommand>
    {
        private readonly LPPContext context;

        private readonly KeyboardHandler keyboardHandler;

        private readonly CurrentUserState userState;

        public ProgramCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }

        public async Task Handle(ProgramCommand request, CancellationToken cancellationToken)
        {
            string programText = "📅 **Программа конкурса** \n\n" +
                "Актуальное расписание и полное содержание программы мероприятия доступны на официальном сайте.\n\n" +
"🔗 Перейти к программе: https://tdv.life/Events";


            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithUrl("📅 Программа", "https://tdv.life/Events") }
            });

            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: programText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    replyMarkup: keyboard,
                    cancellationToken: CancellationToken.None);
        }
    }
}
