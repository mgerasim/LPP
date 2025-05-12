using LPP.Bot.Core;
using LPP.DAL.Context;
using LPP.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Message = LPP.Models.Entities.Message;

namespace LPP.Bot.Handlers
{
    public class AdministrationSendMessageCommand : IRequest
    {

    }

    public class AdministrationSendMessageHandler : IRequestHandler<AdministrationSendMessageCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;
        public AdministrationSendMessageHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }
        public async Task Handle(AdministrationSendMessageCommand request, CancellationToken cancellationToken)
        {
            if (userState.UserState.State == String.Empty)
            {
                this.userState.UserState.State = HandlerConstant.AdministrationSendMessageCmd;
                this.userState.UserState.CurrentStep = HandlerConstant.AdministrationSendMessageCmd;

                await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: "Укажите сообщение участникам 👇 и нажмите кнопку отправить ⬆️",
                    replyMarkup: null,
                    cancellationToken: CancellationToken.None);
            } 
            else if (userState.UserState.State == HandlerConstant.AdministrationSendMessageCmd) {
                this.userState.UserState.Clear();

                await this.SendMessageAsync();

                await this.userState.BotClient.SendMessage(
                    chatId: this.userState.ChatId,
                    text: "⭐ Сообщение участникам успешно отправлено!",
                    replyMarkup: null,                    
                    cancellationToken: CancellationToken.None);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private async Task SendMessageAsync()
        {
            var users = await this.context.Users.AsNoTracking().ToListAsync();

            string text = "";

            MessageType messageType = MessageType.None;

            if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            {
                text = this.userState.Update.Message.Caption;

                messageType = MessageType.Photo;
            }
            else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                text = this.userState.Update.Message.Text;

                messageType = MessageType.Text;
            }
            else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Video)
            {
                text = this.userState.Update.Message.Caption;

                messageType = MessageType.Video;
            }
            else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                text = this.userState.Update.Message.Caption;

                messageType = MessageType.Document;
            }
            else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Animation)
            {
                text = this.userState.Update.Message.Caption;

                messageType = MessageType.Document;
            }
            else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Sticker)
            {
                text = this.userState.Update.Message.Caption;

                messageType = MessageType.Document;
            }

            Message message = null;

            if (!String.IsNullOrEmpty(text))
            {
                message = new Message
                {
                    Text = text,

                    TelegramMessageId = this.userState.Update.Message.MessageId,

                    MessageType = messageType,
                };

                await this.context.AddAsync(message);

                await this.context.SaveChangesAsync();
            }

            foreach (var user in users)
            {
                try
                {
                    InlineKeyboardMarkup kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                           {

                            new []  {
                                    InlineKeyboardButton.WithCallbackData(("🔥"), @$"message"),
                            }

                           });

                    if (this.userState.Update.Message.Type != Telegram.Bot.Types.Enums.MessageType.Sticker)
                    {
                        kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                           {

                            new []  {
                                    InlineKeyboardButton.WithCallbackData(("🔥"), @$"messageReaction:{user.TelegramId}:{message.Id}:{(int)MessageReactionType.Fire}"),
                                    InlineKeyboardButton.WithCallbackData(("👍"), @$"messageReaction:{user.TelegramId}:{message.Id}:{(int)MessageReactionType.ThumbsUp}"),
                                    InlineKeyboardButton.WithCallbackData(("😃"), @$"messageReaction:{user.TelegramId}:{message.Id}:{(int)MessageReactionType.Smile}"),
                                    InlineKeyboardButton.WithCallbackData(("❤"), @$"messageReaction:{user.TelegramId}:{message.Id}:{(int)MessageReactionType.Heart}"),
                                    InlineKeyboardButton.WithCallbackData(("👏"), @$"messageReaction:{user.TelegramId}:{message.Id}:{(int)MessageReactionType.Applause}"),
                                    },

                           });
                    }

                    var messagePoll = this.userState.Update.Message.Poll;

                    if (messagePoll is not null)
                    {

                        await this.userState.BotClient.ForwardMessage(chatId: user.TelegramId,
                            fromChatId: this.userState.ChatId,
                            this.userState.Update.Message.MessageId);

                    }
                    else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Video)
                    {
                        var video = Telegram.Bot.Types.InputFile.FromFileId(this.userState.Update.Message.Video.FileId);

                        var telegramMessage = await this.userState.BotClient.SendVideo(chatId: user.TelegramId,
                            video: video,
                            caption: this.userState.Update.Message.Caption,
                            replyMarkup: kbrd);

                        var messageDelivery = new MessageDelivery
                        {
                            UserId = user.Id,

                            MessageId = message.Id,

                            TelegramMessageId = telegramMessage.MessageId,

                            ChatId = user.TelegramId,
                        };

                        await this.context.AddAsync(messageDelivery);

                        await this.context.SaveChangesAsync();
                    }
                    else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Sticker)
                    {
                        var sticker = Telegram.Bot.Types.InputFile.FromFileId(this.userState.Update.Message.Sticker.FileId);

                        await this.userState.BotClient.SendSticker(chatId: user.TelegramId,
                            sticker: sticker);
                    }
                    else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document
                        || this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Animation)
                    {
                        var document = InputFile.FromFileId(this.userState.Update.Message.Document.FileId);

                        var telegramMessage = await this.userState.BotClient.SendDocument(chatId: user.TelegramId,
                            document: document,
                            caption: this.userState.Update.Message.Caption,
                            replyMarkup: kbrd);

                        var messageDelivery = new MessageDelivery
                        {
                            UserId = user.Id,

                            MessageId = message.Id,

                            TelegramMessageId = telegramMessage.MessageId,

                            ChatId = user.TelegramId,
                        };

                        await this.context.AddAsync(messageDelivery);

                        await this.context.SaveChangesAsync();
                    }
                    else if (this.userState.Update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
                    {
                        var photo = InputFile.FromFileId(this.userState.Update.Message.Photo[this.userState.Update.Message.Photo.Count() - 1].FileId);

                        var telegramMessage = await this.userState.BotClient.SendPhoto(chatId: user.TelegramId,
                            photo: photo,
                            caption: this.userState.Update.Message.Caption,
                            replyMarkup: kbrd
                            );

                        var messageDelivery = new MessageDelivery
                        {
                            UserId = user.Id,

                            MessageId = message.Id,

                            TelegramMessageId = telegramMessage.MessageId,

                            ChatId = user.TelegramId,
                        };

                        await this.context.AddAsync(messageDelivery);

                        await this.context.SaveChangesAsync();
                    }
                    else
                    {
                        var telegramMessage = await this.userState.BotClient.SendMessage(
                           chatId: user.TelegramId,
                           text: text,
                           replyMarkup: kbrd,
                           cancellationToken: CancellationToken.None);

                        var messageDelivery = new MessageDelivery
                        {
                            UserId = user.Id,

                            MessageId = message.Id,

                            TelegramMessageId = telegramMessage.MessageId,

                            ChatId = user.TelegramId,
                        };

                        await this.context.AddAsync(messageDelivery);

                        await this.context.SaveChangesAsync();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
