using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class ParticipantsCommand : IRequest
    {

    }

    public class ParticipantsCommandHandler : IRequestHandler<ParticipantsCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public ParticipantsCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(ParticipantsCommand request, CancellationToken cancellationToken)
        {
            var text = "👥 *Участники*\n\n" +
               "В 2025 году на региональном этапе соревнуются 119 сотрудников трех предприятий системы *«Транснефть»*:\n\n" +
               "Ознакомиться с ними вы можете на сайте конкурса";

            var keyboard = new InlineKeyboardMarkup(new[]
            {                
                new[] { InlineKeyboardButton.WithUrl("👥 Открыть", "https://tdv.life/participants") }
            });

            await this.userState.BotClient.SendMessage(
                chatId: this.userState.ChatId,
                text: text,
                parseMode: ParseMode.Markdown,
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }
}
