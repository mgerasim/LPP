using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class ContactsCommand : IRequest
    {

    }

    public class ContactsCommandHandler : IRequestHandler<ContactsCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public ContactsCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(ContactsCommand request, CancellationToken cancellationToken)
        {
            var text = "📋 *Контакты*\n\n" +
                  "Общая организация:\n" +
                  "👤 *Начальник отдела кадров Юдаева Екатерина Сергеевна*\n" +
                  "📞 +7 (914) 150-22-23\n" +
                  "✉️ YudaevaES@dmn.transneft.ru\n\n" +

                  "По вопросам проживания:\n" +
                  "👤 *Начальник отдела социального развития Афанасьев Андрей Алексеевич*\n" +
                  "📞 +7 (914) 422-40-33\n" +
                  "✉️ AfanasievAA@dmn.transneft.ru\n\n"
;

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithUrl("📋 Открыть раздел на сайте", "https://tdv.life/contacts") },
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
