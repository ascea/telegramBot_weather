# telegramBot_weather

Клиент telegram бота на основе WPF(C#).

Для работы необходимо добавить файл "./botweather/appsettings.json"

Пример содержания:
{
  "ConnectionStrings": {
      "DefaultConnection": "connectionString",
  },
  "TelegramToken": "telegramBotToken",
  "WeatherToken": "weatherToken"
}

connectionString - строка подключения к MsSql server;
telegramBotToken - токен telegram бота;
weatherToken - токен от сайта https://openweathermap.org/
