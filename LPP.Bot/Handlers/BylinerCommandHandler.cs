using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using LPP.Bot.Core;
using LPP.DAL.Context;
using Telegram.Bot;

namespace LPP.Bot.Handlers
{
    public class BylinerCommand : IRequest
    {
        
    }

    public class BylinerCommandHandler : IRequestHandler<BylinerCommand>
    {
        private readonly LPPContext context;

        private readonly KeyboardHandler keyboardHandler;

        private readonly CurrentUserState userState;

        public BylinerCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }

        public async Task Handle(BylinerCommand request, CancellationToken cancellationToken)
        {

            string hello = $@"
Уважаемые коллеги!

Приветствую вас на региональном этапе ежегодного корпоративного конкурса профессионального мастерства «Лучший по профессии», который в 2025 году проходит на площадках «Транснефть – Дальний Восток».

Представители 13 рабочих профессий, более 100 человек, на этой неделе будут состязаться в своих дисциплинах, чтобы определить лучших специалистов среди трех предприятий: «Транснефть - Восток», «Транснефть – Дальний Восток» и «Транснефть – Порт Козьмино». Победители отправятся на финальный этап конкурса в Тюмень.

Высокая квалификация наших сотрудников – обязательный элемент безопасной и бесперебойной транспортировки нефти в приоритетном сейчас восточном направлении. Поэтому я уверен, что все вы готовы к честной борьбе за звание «Лучшего».

Я желаю всем вам продемонстрировать отличные практические навыки и теоретические знания, достойно представить свое предприятие. Успехов вам!

";

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

            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
               {

                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("🏆 О конкурсе", HandlerConstant.AboutCompetition),
                    }
               });

            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: hello,
                    replyMarkup: kbrd,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: CancellationToken.None);

            this.userState.User.IsBylinerReaded = true;

            this.context.Users.Update(this.userState.User);

            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
