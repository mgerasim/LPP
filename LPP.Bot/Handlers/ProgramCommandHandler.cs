using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot.Types;
using Telegram.Bot;

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
            string programText = "📅 **Программа конкурса**\n\n" +
                "📍 *День 1 – Заезд и регистрация участников*\n" +
                "• Приветствие\n" +
                "• Ознакомление с площадками\n\n" +
                "📍 *День 2 – Практический этап*\n" +
                "• Соревнования по профессиям\n" +
                "• Работа экспертных комиссий\n\n" +
                "📍 *День 3 – Подведение итогов и награждение*\n" +
                "• Итоговое заседание жюри\n" +
                "• Торжественная церемония\n\n" +
                "_Следите за обновлениями — возможны изменения в расписании._";

            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: programText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: CancellationToken.None);
        }
    }
}
