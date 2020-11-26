# telegramBot_weather

Клиент telegram бота на основе WPF(C#).

Для работы необходимо добавить файл "./botweather/appsettings.json"

Пример содержания:
{
  "ConnectionStrings": {
      "DefaultConnection": "<connection string>",
  },
  "TelegramToken": "<telegram bot token>",
  "WeatherToken": "<weather token>"
}

<connection string> - строка подключения к MsSql server;
<telegram bot token> - токен telegram бота;
<weather token> - токен от сайта https://openweathermap.org/
