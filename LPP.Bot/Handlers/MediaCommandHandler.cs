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
📞 *Контакты*

По всем вопросам, связанным с общей организацией мероприятия, вы можете обратиться к следующим ответственным лицам:

*Голышев Дмитрий Евгеньевич*
Заместитель генерального директора по управлению персоналом и общим вопросам ООО «Транснефть - Дальний Восток»
+7 (913) 853-70-39
 
*ТЕХНИЧЕСКОЕ СОПРОВОЖДЕНИЕ:*
*Донской Михаил Николаевич*
Заместитель главного инженера по автоматизации и  информационной безопасности ООО «Транснефть - Дальний Восток»
+7 (914) 935-92-79

*ОРГАНИЗАЦИОННЫЕ ВОПРОСЫ, ПРОЖИВАНИЕ, МЕДИЦИНСКОЕ ОБЕСПЕЧЕНИЕ:*
*Афанасьев Андрей Александрович*
Начальник отдела социального развития ООО «Транснефть - Дальний Восток»
+7 (914) 422-40-33

*ТРАНСПОРТНАЯ ЛОГИСТИКА:*
*Гопко Александр Владимирович*
Начальник отдела транспортных средств и специальной техники ООО «Транснефть - Дальний Восток»
+7 (914) 567-34-52

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
