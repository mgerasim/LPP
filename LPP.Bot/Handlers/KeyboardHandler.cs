using LPP.Bot.Core;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class KeyboardHandler
    {
        private readonly CurrentUserState currentUserState;
        public KeyboardHandler(CurrentUserState currentUserState)
        {
            this.currentUserState = currentUserState;
        }

        public ReplyKeyboardMarkup GetKeaboard()
        {
            var buttons = new List<KeyboardButton[]>()
                                    {
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton(HandlerConstant.ShowProgram),
                                            new KeyboardButton(HandlerConstant.ShowNews),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton(HandlerConstant.ShowMedia),
                                            new KeyboardButton(HandlerConstant.ShowContacts),
                                        },
                                        new KeyboardButton[]
                                        {
                                            new KeyboardButton(HandlerConstant.SeeMain),
                                        },
                                    };

            if (this.currentUserState.IsAdmin)
            {
                buttons.Add(new KeyboardButton[]
                                    {
                                            new KeyboardButton(HandlerConstant.AdminMenu),
                                    });
            }

            var keyboard = new ReplyKeyboardMarkup(buttons)
            {
                ResizeKeyboard = true,
            };

            return keyboard;
        }
    }
}
