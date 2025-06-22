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
            var text = @$"
📞 **Контакты**

По всем вопросам, связанным с общей организацией мероприятия, вы можете обратиться к следующим ответственным лицам:

Начальник отдела социального развития Афанасьев Андрей Алексеевич
+7 (914) 422-40-33
AfanasievAA@dmn.transneft.ru

Начальник службы общественных коммуникаций Кривко Екатерина Сергеевна
+7 (924) 216-77-25
KrivkoES@dmn.transneft.ru
";

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
