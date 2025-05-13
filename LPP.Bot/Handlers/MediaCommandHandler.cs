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
                  "👤 *Начальник отдела кадров*\n" +
                  "📞 +7 (XXX) XXX-XX-XX\n" +
                  "✉️ email1@domain.ru\n\n" +

                  "👤 *Начальник отдела социального развития*\n" +
                  "📞 +7 (XXX) XXX-XX-XX\n" +
                  "✉️ email2@domain.ru\n\n" +

                  "👤 *Начальник отдела транспортных средств и спецтехники*\n" +
                  "📞 +7 (XXX) XXX-XX-XX\n" +
                  "✉️ email3@domain.ru\n\n" +

                  "👤 *Главный волонтёр*\n" +
                  "📞 +7 (XXX) XXX-XX-XX\n" +
                  "✉️ email4@domain.ru";

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
