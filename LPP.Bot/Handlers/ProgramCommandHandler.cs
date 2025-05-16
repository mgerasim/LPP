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
            /*
            // Путь к файлу изображения
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "_stepanov_vitaliy_valerevich.jpg");

            if (File.Exists(imagePath))
            {
                using var stream = File.OpenRead(imagePath);

                var photo = InputFile.FromStream(stream, "_stepanov_vitaliy_valerevich.jpg");

                await this.userState.BotClient.SendPhoto(
                    chatId: this.userState.ChatId,
                    photo,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                );
            }
            */

            string programText = @"📅 **Программа конкурса** 

Актуальное расписание и полное содержание программы мероприятия доступны:

📎 **Во вложении** — удобно для скачивания и просмотра прямо в боте
🔗 **На сайте** — по ссылке: https://tdv.life/Events
";


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


            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "ПРОГРАММА ЛПП_инет версия.pdf");

            using var stream = File.OpenRead(filePath);

            var file = InputFile.FromStream(stream, "ПРОГРАММА ЛПП_инет версия.pdf");

            await this.userState.BotClient.SendDocument(
                    chatId: this.userState.ChatId,
                    document: file,
                    caption: "Программа конкурса ЛПП 2025 также доступна на сайте: https://tdv.life/Events");
        }
    }
}
