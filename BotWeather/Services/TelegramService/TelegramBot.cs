using Telegram.Bot;
using BotWeather.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading;

namespace BotWeather.Services.TelegramService
{
    class TelegramBot : ITelegramBot
    {
        private ApplicationContext _db;
        private TelegramBotClient _bot;
        private SynchronizationContext _uiContext;

        public event EventHandler<object[]> ReceiveMessage;
        //private object _lockObj = new object();
        public TelegramBot(ApplicationContext db, string token, SynchronizationContext synchronizationContext = null)
        {
            this._db = db;
            this._uiContext = synchronizationContext;

            _bot = new TelegramBotClient(token);

            _bot.OnMessage += Bot_OnMessage;
        }

        public void Start()
        {
            _bot.StartReceiving(allowedUpdates: new UpdateType[] { UpdateType.Message });
        }

        public void Stop()
        {
            _bot.StopReceiving();
        }

        public bool IsReceiving()
        {
            return _bot.IsReceiving;
        }

        public void Send(long id, string text)
        {
            string mes = text;
            Telegram.Bot.Types.Message message = _bot.SendTextMessageAsync(id, mes).Result;
            Model.Message messageDb = new Model.Message()
            {
                UserId = 0,
                MessageId = message.MessageId,
                ChatId = message.Chat.Id,
                Text = message.Text,
                Date = message.Date
            };
            _uiContext.Send((obj) =>
            {
                _db.Messages.Add(messageDb);
                _db.SaveChanges();
            }, null);
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Model.User user;
            Telegram.Bot.Types.Message message = e.Message;
            if (message != null && message.Type == MessageType.Text)
            {
                IQueryable<Model.User> users = _db.Users.Where((u) => u.TelegramId == message.From.Id);
                if (users.Count() > 0)
                {
                    user = users.FirstOrDefault();
                    user.UserName = message.From.Username;
                    user.FirstName = message.From.FirstName;
                    user.LastName = message.From.LastName;

                    //lock (_lockObj)
                    //{
                    //    _uiContext.Send(async (obj) =>
                    //    {
                    //        _db.Users.Update(user);
                    //        await _db.SaveChangesAsync();
                    //    }, null);
                    //}

                    _uiContext.Send((obj) =>
                    {
                        _db.Users.Update(user);
                        _db.SaveChanges();
                    }, null);
                }
                else
                {
                    user = new Model.User()
                    {
                        TelegramId = message.From.Id,
                        UserName = message.From.Username,
                        FirstName = message.From.FirstName,
                        LastName = message.From.LastName,
                        Date = DateTime.Now
                    };

                    //lock (_lockObj)
                    //{
                    //    _uiContext.Send(async (obj) =>
                    //    {
                    //        _db.Users.Add(user);
                    //        await _db.SaveChangesAsync();
                    //    }, null);
                    //}

                    _uiContext.Send((obj) =>
                    {
                        _db.Users.Add(user);
                        _db.SaveChanges();
                    }, null);
                }

                //if (user == null)
                //{
                //    user = new Model.User()
                //    {
                //        TelegramId = message.From.Id,
                //        UserName = message.From.Username,
                //        FirstName = message.From.FirstName,
                //        LastName = message.From.LastName,
                //        Date = DateTime.Now
                //    };
                //    await _db.Users.AddAsync(user);
                //}
                //else
                //{
                //    _db.Users.Update(user);
                //}

                Model.Message messageDb = new Model.Message()
                {
                    UserId = user.Id,
                    MessageId = message.MessageId,
                    ChatId = message.Chat.Id,
                    Text = message.Text,
                    Date = message.Date
                };

                //lock (_lockObj)
                //{
                //    _uiContext.Send(async (obj) =>
                //    {
                //        _db.Messages.Add(messageDb);
                //        await _db.SaveChangesAsync();
                //    }, null);
                //}

                _uiContext.Send((obj) =>
                {
                    _db.Messages.Add(messageDb);
                    _db.SaveChanges();
                }, null);

                ReceiveMessage?.Invoke(this, new object[]
                    {
                        message.Chat.Id,
                        message.Text
                    });
            }
        }
    }
}
