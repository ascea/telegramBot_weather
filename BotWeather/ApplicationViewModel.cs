using System.ComponentModel;
using System.Runtime.CompilerServices;
using BotWeather.Model;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using BotWeather.Services.TelegramService;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using BotWeather.Services.Weather;
using System.Text.RegularExpressions;
using System;

namespace BotWeather
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        ITelegramBot telegramBot;
        IWeather currentWeather;

        public System.Timers.Timer timerUpdateUserMessages;

        private object _lockObj = new object();

        private User selectedUser;
        private string textMessage;
        private ObservableCollection<Message> userMessages;

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

        public ApplicationViewModel(ApplicationContext db, ITelegramBot telegramBot, IWeather currentWeather)
        {
            this.db = db;
            this.currentWeather = currentWeather;
            this.telegramBot = telegramBot;
            this.telegramBot.ReceiveMessage += TelegramBot_ReceiveMessage;
            
            this.db.Users.Load();
            Users = this.db.Users.Local.ToObservableCollection();
            this.db.Messages.Load();
            Messages = this.db.Messages.Local.ToObservableCollection();

            timerUpdateUserMessages = new System.Timers.Timer(1000);
            timerUpdateUserMessages.Elapsed += Timer_Elapsed;
            timerUpdateUserMessages.Start();
        }

        public void Close()
        {
            if (timerUpdateUserMessages.Enabled)
            {
                timerUpdateUserMessages.Stop();
            }
            db.Dispose();
            if (telegramBot.IsReceiving())
            {
                telegramBot.Stop();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SelectedUser != null)
            {
                UpdateUserMessages();
            }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged();
                UpdateUserMessages();
            }
        }
        public string TextMessage
        {
            get { return textMessage; }
            set
            {
                textMessage = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Message> UserMessages
        {
            get { return userMessages; }
            set
            {
                userMessages = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand startTelegramReceived;
        public RelayCommand StartTelegramReceived
        {
            get
            {
                return startTelegramReceived ??
                    (startTelegramReceived = new RelayCommand((obj) =>
                    {
                        telegramBot.Start();
                    },
                    (obj) => !telegramBot.IsReceiving()));
            }
        }
        private RelayCommand stopTelegramReceived;
        public RelayCommand StopTelegramReceived
        {
            get
            {
                return stopTelegramReceived ??
                    (stopTelegramReceived = new RelayCommand((obj) =>
                    {
                        if (telegramBot.IsReceiving())
                        {
                            telegramBot.Stop();
                        }
                    },
                    (obj) => telegramBot.IsReceiving()));
            }
        }
        private RelayCommand sendMessageCommand;
        public RelayCommand SendMessageCommand
        {
            get
            {
                return sendMessageCommand ??
                    (sendMessageCommand = new RelayCommand((obj) =>
                    {
                        telegramBot.Send(SelectedUser.TelegramId, TextMessage);
                        TextMessage = "";
                    }));
            }
        }
        private RelayCommand sendWeather;
        public RelayCommand SendWeather
        {
            get
            {
                return sendWeather ??
                    (sendWeather = new RelayCommand((obj) =>
                    {
                        if (SelectedUser != null)
                        {
                            CurrentWeather weather = currentWeather as CurrentWeather;
                            weather.GetWeather("Moscow", units: "metric", lang: "ru");
                            string str = weather.GetCurrentTemp();
                            telegramBot.Send(SelectedUser.TelegramId, str);
                        }
                    }));
            }
        }

        private void UpdateUserMessages()
        {
            lock (_lockObj)
            {
                IEnumerable<Message> col = db.Messages.Local.Where((m) => m.UserId == SelectedUser.Id || (m.UserId == 0 && m.ChatId == SelectedUser.TelegramId));
                col = col.Reverse();
                UserMessages = new ObservableCollection<Message>(col);
            }
        }

        private void TelegramBot_ReceiveMessage(object sender, object[] obj)
        {
            long chatId = (long)obj[0];
            string message = obj[1].ToString();
            Regex regex = new Regex(@"^/([tт])\s(\w+)$", RegexOptions.IgnoreCase);
            Match match = regex.Match(message);
            
            if (match.Success)
            {
                try
                {
                    GroupCollection groups = match.Groups;

                    CurrentWeather weather = currentWeather as CurrentWeather;
                    weather.GetWeather(groups[2].Value, units: "metric", lang: "ru");
                    string str = weather.GetCurrentTemp();
                    telegramBot.Send(chatId, str);
                }
                catch (Exception)
                {
                    telegramBot.Send(chatId, "Неправильное название города");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
