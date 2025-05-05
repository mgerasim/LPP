using LPP.Bot.Core;
using LPP.Bot.Core.Handler;
using LPP.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LPP.Bot.Handlers
{
    public class StartHandler : BaseHandler
    {
        private readonly LPPContext context;

        private readonly KeyboardHandler keyboardHandler;
        public StartHandler(LPPContext context, CurrentUserState userState, KeyboardHandler keyboardHandler) : base(userState)
        {
            this.context = context;

            this.keyboardHandler = keyboardHandler;
        }

        public override async Task RunAsync()
        {
            var user = await this.context.Users.SingleOrDefaultAsync(x => x.TelegramId == this.userState.ChatId);

            if (user is null)
            {
                user = new Models.Entities.User
                {
                    TelegramId = this.userState.ChatId,

                    Username = this.userState.Update.Message.Chat.Username,

                    LastName = this.userState.Update.Message.Chat.LastName,

                    FirstName = this.userState.Update.Message.Chat.FirstName,

                    CreatedAt = DateTime.UtcNow,

                    UpdatedAt = DateTime.UtcNow,
                };

                await this.context.AddAsync(user);

                await this.context.SaveChangesAsync();
            }
            else
            {
                user.Username = this.userState.Update.Message.Chat.Username;

                user.LastName = this.userState.Update.Message.Chat.LastName;

                user.FirstName = this.userState.Update.Message.Chat.FirstName;

                user.UpdatedAt = DateTime.UtcNow;

                this.context.Update(user);

                await this.context.SaveChangesAsync();
            }

            this.context.Entry(user).State = EntityState.Detached;

            this.userState.User = user;

            this.userState.UserState.Clear();

            var keyboard = this.keyboardHandler.GetKeaboard();

            string hello = $@"
**📚✨ Добро пожаловать в Читательский клуб! 📖🎉**  

Рады видеть вас в нашем уютном книжном сообществе! Здесь вас ждёт:  
📖 **Огромная библиотека** – находите и скачивайте книги, которые вас заинтересуют.  
⭐ **Рейтинги и отзывы** – оценивайте книги и делитесь впечатлениями с другими читателями.  
🔍 **Поиск по ключевым словам** – легко находите нужные книги.  
📌 **Ваш личный список чтения** – отмечайте, что читаете сейчас или уже закончили.  
🎭 **Живые обсуждения** – участвуйте в книговоротах и обмене мнениями.  
🎉 **Викторины и встречи** – регулярно проводим интеллектуальные игры и обсуждения книг!  


⚠ **Правила использования материалов** ⚠  
📌 Все книги в клубе предназначены **исключительно для личного ознакомления**.  
🚫 Распространение и коммерческое использование запрещено!  
✅ Загружая книгу, вы **соглашаетесь** использовать её только для себя.  

📅 **Следите за анонсами викторин и встреч!** Будем рады вашему активному участию! 🎭💡  
📩 Если у вас есть вопросы – пишите администрации.  

✨ **Приятного чтения и незабываемых книжных приключений!** 🚀📚
";

            Message sentMessage = await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: hello,
                    replyMarkup: keyboard,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    cancellationToken: CancellationToken.None);

        }
    }
}
