using System;

namespace BotWeather.Services.TelegramService
{
    public interface ITelegramBot
    {
        void Start();
        void Stop();
        bool IsReceiving();
        void Send(long id, string text);
        public event EventHandler<object[]> ReceiveMessage;
    }
}
