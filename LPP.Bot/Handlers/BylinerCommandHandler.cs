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

Приветствую вас на совете департамента АСУТП ПАО «Транснефть» с заместителями главных инженеров по автоматизации и информационной безопасности, который в 2025 году проходит на площадках «Транснефть – Дальний Восток».

Вопросы обеспечения отказоустойчивости и эффективной работы оборудования АСУТП, диспетчерских систем, а также информационная безопасность в рамках производственного процесса организаций системы «Транснефть» в современных реалиях санкционного давления и нестабильной политической ситуации приобретают еще большее значение.

Кроме того, в свете активного внедрения стандартов СРТ «ОПТИМУМ» на первый план выходит оптимизация самого производственного процесса, модернизация подходов, межличностных отношений в коллективе для повышения эффективности и безопасности работы всех подразделений.

Для «Транснефть – Дальний Восток» очень ценна возможность выступить в качестве организатора и площадки для этого мероприятия, ознакомиться с опытом коллег из других ОСТ и поделиться своим опытом внедрения передовых разработок.

Желаю всем участникам совещания продуктивной работы!

Генеральный директор
ООО «Транснефть – Дальний Восток»
В. В. Степанов

";

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

            var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
               {

                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Об ООО «Транснефть - Дальний Восток»", HandlerConstant.AboutCompetition),
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
