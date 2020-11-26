using BotWeather.Services.Weather.Model;

namespace BotWeather.Services.Weather
{
    public interface IWeather
    {
        IWeatherModel GetWeather(string cityName,
            ModeType mode = ModeType.json, string units = null, string lang = null);
        IWeatherModel GetWeather(int id,
            ModeType mode = ModeType.json, string units = null, string lang = null);
        IWeatherModel GetWeather(double lat, double lon,
            ModeType mode = ModeType.json, string units = null,
            string lang = null);
        IWeatherModel GetWeather(int zip, string countyCode,
            ModeType mode = ModeType.json, string units = null,
            string lang = null);
    }

    public enum ModeType
    {
        json,
        xml,
        html
    }
}
