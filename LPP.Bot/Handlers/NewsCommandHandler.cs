using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LPP.Bot.Handlers
{
    public class NewsCommand : IRequest
    {

    }

    public class NewsCommandHandler : IRequestHandler<NewsCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public NewsCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(NewsCommand request, CancellationToken cancellationToken)
        {
            string newsText = "📰 **Новости конкурса**\n\n" +
            "1️⃣ *Регистрация участников закрыта!* Мы завершили прием заявок. Спасибо всем, кто подал заявки. Ожидайте расписания.\n\n" +
            "2️⃣ *Подтверждены даты проведения соревнований!* Конкурс состоится с 12 по 14 мая 2025 года. Мы рады сообщить, что в этом году примут участие 13 профессий.\n\n" +
            "3️⃣ *Подготовка площадок завершена!* Все места для соревнований готовы. Технические специалисты провели окончательную проверку оборудования.\n\n" +
            "_Следите за новыми обновлениями и новостями конкурса!_";


            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: newsText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: CancellationToken.None);
        }
    }
}
