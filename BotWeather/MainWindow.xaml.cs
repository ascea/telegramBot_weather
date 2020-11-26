using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BotWeather.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using BotWeather.Services.TelegramService;
using System.Threading;
using BotWeather.Services.Weather;

namespace BotWeather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationContext db;
        private ITelegramBot telegramBot;
        private SynchronizationContext uiContext;
        private ApplicationViewModel applicationViewModel;
        private IWeather currentWeather;

        public MainWindow()
        {
            InitializeComponent();

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");
            string telegramToken = config.GetSection("TelegramToken").Value;
            string weatherToken = config.GetSection("WeatherToken").Value;

            DbContextOptionsBuilder<ApplicationContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            DbContextOptions<ApplicationContext> options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;

            uiContext = SynchronizationContext.Current;

            this.db = new ApplicationContext(options);
            this.telegramBot = new TelegramBot(db, telegramToken, uiContext);
            currentWeather = new CurrentWeather(weatherToken);

            applicationViewModel = new ApplicationViewModel(db, telegramBot, currentWeather);
            this.DataContext = applicationViewModel;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            applicationViewModel.Close();
        }
    }
}
