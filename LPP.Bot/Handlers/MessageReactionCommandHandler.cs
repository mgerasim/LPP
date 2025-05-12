using LPP.Bot.Core;
using LPP.DAL.Context;
using LPP.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace LPP.Bot.Handlers
{
    public class MessageReactionCommand : IRequest
    {

    }

    public class MessageReactionCommandHandler : IRequestHandler<MessageReactionCommand>
    {
        private readonly LPPContext context;
        private readonly KeyboardHandler keyboardHandler;
        private readonly CurrentUserState userState;

        public MessageReactionCommandHandler(LPPContext context, KeyboardHandler keyboardHandler, CurrentUserState userState)
        {
            this.context = context;
            this.keyboardHandler = keyboardHandler;
            this.userState = userState;
        }

        public async Task Handle(MessageReactionCommand request, CancellationToken cancellationToken)
        {
            var userChatId = this.userState.Update.CallbackQuery.Data.Split(new char[] { ':' })[1];

            var messageId = Convert.ToInt32(this.userState.Update.CallbackQuery.Data.Split(new char[] { ':' })[2]);

            var message = await this.context.Messages.SingleAsync(x => x.Id == messageId);

            var messageReactionType = (MessageReactionType)Convert.ToInt32(this.userState.Update.CallbackQuery.Data.Split(new char[] { ':' })[3]);

            var list = await this.context.MessageReactions.AsNoTracking().Where(x => x.UserId == this.userState.User.Id && x.MessageId == messageId).ToListAsync();

            var messageReaction = list.SingleOrDefault();

            if (messageReaction is null)
            {
                messageReaction = new MessageReaction
                {
                    MessageId = messageId,

                    UserId = this.userState.User.Id,

                    MessageReactionType = messageReactionType,
                };

                await this.context.MessageReactions.AddAsync(messageReaction);

                await this.context.SaveChangesAsync();
            }
            else
            {
                messageReaction.MessageReactionType = messageReactionType;

                this.context.MessageReactions.Update(messageReaction);

                await this.context.SaveChangesAsync();
            }


            var messageDeliveries = await this.context.MessageDeliveries.Where(x => x.MessageId == messageId).ToListAsync();

            var messageReactions = await this.context.MessageReactions.Where(x => x.MessageId == messageId).ToListAsync();

            foreach (var messageDelivery in messageDeliveries)
            {
                try
                {
                    var kbrd = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new []  {
                        InlineKeyboardButton.WithCallbackData($@"🔥 {messageReactions.Count(x => x.MessageReactionType == MessageReactionType.Fire)}", @$"messageReaction:{messageDelivery.ChatId}:{messageId}:{(int)MessageReactionType.Fire}"),
                        InlineKeyboardButton.WithCallbackData($@"👍 {messageReactions.Count(x => x.MessageReactionType == MessageReactionType.ThumbsUp)}", @$"messageReaction:{messageDelivery.ChatId}:{messageId}:{(int)MessageReactionType.ThumbsUp}"),
                        InlineKeyboardButton.WithCallbackData($@"😃 {messageReactions.Count(x => x.MessageReactionType == MessageReactionType.Smile)}", @$"messageReaction:{messageDelivery.ChatId}:{messageId}:{(int)MessageReactionType.Smile}"),
                        InlineKeyboardButton.WithCallbackData($@"❤ {messageReactions.Count(x => x.MessageReactionType == MessageReactionType.Heart)}", @$"messageReaction:{messageDelivery.ChatId}:{messageId}:{(int)MessageReactionType.Heart}"),
                        InlineKeyboardButton.WithCallbackData($@"👏 {messageReactions.Count(x => x.MessageReactionType == MessageReactionType.Applause)}", @$"messageReaction:{messageDelivery.ChatId}:{messageId}:{(int)MessageReactionType.Applause}")
//                        InlineKeyboardButton.WithCallbackData(L("👏"), @$"messageReaction:{user.TelegramId}:{message.Id}:{(int)MessageReactionType.Applause}"),
                    },
                });

                    switch (message.MessageType)
                    {
                        case MessageType.Text:
                            await this.userState.BotClient.EditMessageText(chatId: messageDelivery.ChatId, messageId: messageDelivery.TelegramMessageId,
                                text: message.Text, replyMarkup: kbrd);
                            break;
                        case MessageType.Photo:
                        case MessageType.Video:
                        case MessageType.Document:
                        case MessageType.Sticker:
                            await this.userState.BotClient.EditMessageCaption(chatId: messageDelivery.ChatId,
                                messageId: messageDelivery.TelegramMessageId,
                                caption: message.Text,
                                replyMarkup: kbrd);
                            break;

                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                }
            }
        }
    }
}
