using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class AttractionsCommand : IRequest
    {

    }

    public class AttractionsCommandHandler : IRequestHandler<AttractionsCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public AttractionsCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(AttractionsCommand request, CancellationToken cancellationToken)
        {
            var text = "📍 *Увидеть главное в Хабаровске*\n\n" +
        "Добро пожаловать в Хабаровск — город на берегу великой Амур-реки!\n\n" +
        "В этом разделе вы найдёте подборку интересных мест, которые стоит обязательно посетить:\n" +
        "🏛 Достопримечательности\n" +
        "🖼 Музеи и культурные объекты\n" +
        "🌳 Прогулочные зоны и набережные\n" +
        "☕ Уютные кофейни и места для перекуса\n" +
        "🍽 Локальные заведения с дальневосточной кухней\n\n" +
        "\n\n" +
        "Погрузитесь в атмосферу города и открывайте новые места!\n\n" +
        "🔗 [Подробнее на сайте](https://tdv.life/attractions)";

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithUrl(HandlerConstant.SeeMain, "https://tdv.life/attractions") },
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
