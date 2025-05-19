using LPP.Bot.Core;
using LPP.DAL.Context;
using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class MediaCommand : IRequest
    {

    }

    public class MediaCommandHandler : IRequestHandler<MediaCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public MediaCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(MediaCommand request, CancellationToken cancellationToken)
        {
            var text = "📹 *Медиа*\n\n" +
               "Добро пожаловать в раздел *«Медиа»*! Здесь вы можете:\n" +
               "• 📷 Посмотреть *фотографии*\n" +
               "• 🔴 Подключиться к *онлайн трансляции* (21-22 мая)\n" +
               "• 🌐 Перейти в *медиараздел* на сайте";

            var keyboard = new InlineKeyboardMarkup(new[]
            {
                new[] { InlineKeyboardButton.WithUrl("📷 Фотографии", "https://disk.yandex.ru/d/do0H4xF41Ie7qA") },
                new[] { InlineKeyboardButton.WithUrl("🔴 Онлайн трансляция (21-22 мая)", "https://tdv.life/media") },
                new[] { InlineKeyboardButton.WithUrl("🌐 Медиа на сайте", "https://tdv.life/media") }
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
