using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace BotWeather.Model
{
    public class Message : INotifyPropertyChanged
    {
        private int messageId;
        private int userId;
        private long chatId;
        private string text;
        private DateTime date;

        public int Id { get; set; }
        public int MessageId
        {
            get { return messageId; }
            set
            {
                messageId = value;
                OnPropertyChanged();
            }
        }
        public int UserId
        {
            get { return userId; }
            set
            {
                userId = value;
                OnPropertyChanged();
            }
        }
        public long ChatId
        {
            get { return chatId; }
            set
            {
                chatId = value;
                OnPropertyChanged();
            }
        }
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
