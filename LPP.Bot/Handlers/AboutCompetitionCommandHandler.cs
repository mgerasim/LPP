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
            string hello = @$"
🏆 **Об ООО «Транснефть - Дальний Восток»**

Транснефть это движение!

Основные приоритеты ПАО «Транснефть» на ближайшие годы — это инновационное развитие производственной деятельности и повышение уровня надежности, промышленной и экологической безопасности, энергоэффективности системы магистральных трубопроводов.
Важнейшими направлениями развития определены цифровизация и импортозамещение. Компания переходит на использование отечественного программного обеспечения и аппаратных средств во всех сферах деятельности. Особенное внимание уделяется системам цифрового диспетчерского управления и мониторинга инфраструктуры, информационной безопасности, киберзащиты.
";
            /*
            await userState.BotClient.SendSticker(
                chatId: this.userState.ChatId,
                sticker: "CAACAgIAAxkBAAIHymgmsmu42ozei3DcnpviKoqDyPYWAAJFaAAC1Bk4ScQExV2943heNgQ"
            );
            */

            var kbrd = new InlineKeyboardMarkup();

            kbrd.AddNewRow(new[]
                {
                    InlineKeyboardButton.WithCallbackData("🎤 Приветственное слово", HandlerConstant.Byliner)
                });
            kbrd.AddNewRow(new[]
            {
                    InlineKeyboardButton.WithUrl("🌐 О нас", "https://tdv.life/about")
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
