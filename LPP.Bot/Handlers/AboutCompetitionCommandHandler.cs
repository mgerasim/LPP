using MediatR;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using LPP.Bot.Core;
using LPP.DAL.Context;
using Telegram.Bot;

namespace LPP.Bot.Handlers
{
    public class AboutCompetitionCommand : IRequest
    {

    }
    public class AboutCompetitionCommandHandler : IRequestHandler<AboutCompetitionCommand>
    {
        private readonly LPPContext context;

        private readonly KeyboardHandler keyboardHandler;

        private readonly CurrentUserState userState;

        public AboutCompetitionCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }

        public async Task Handle(AboutCompetitionCommand request, CancellationToken cancellationToken)
        {
            string hello = "🏆 **О конкурсе**\n\n" +
            "Региональный конкурс профессионального мастерства на звание «Лучший по профессии» между рабочими основных профессий организаций системы «Транснефть» проходит с 2019 года. В конкурсе принимают участие ООО «Транснефть - Восток», ООО «Транснефть - Дальний Восток» и ООО «Транснефть - Порт Козьмино». В нём участвуют сотрудники, представляющие ключевые профессии трубопроводной отрасли. Цель конкурса — повышение профессионального мастерства, обмен опытом и развитие творческого подхода к труду. Победители корпоративного этапа представляют компанию на уровне ПАО «Транснефть»." +
            "" +
"В 2025 году региональный конкурс вновь проходит в ООО «Транснефть - Дальний Восток». Участники будут состязаться в 13 профессиях.";

            // Путь к файлу изображения
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", "LOGO1.png");

            if (File.Exists(imagePath))
            {
                using var stream = File.OpenRead(imagePath);

                var photo = InputFile.FromStream(stream, "LOGO1.png");

                await this.userState.BotClient.SendPhoto(
                    chatId: this.userState.ChatId,
                    photo,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                );
            }

            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
               {

                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🎤 Приветственное слово", HandlerConstant.Byliner),
                    }
               });

            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: hello,
                    replyMarkup: kbrd,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: CancellationToken.None);
        }
    }
}
