using BotWeather.Services.Weather.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace BotWeather.Services.Weather
{
    class CurrentWeather : IWeather
    {
        private string apiKey;
        public IWeatherModel weatherModel;

        public CurrentWeather(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public string GetCurrentTemp(IWeatherModel model)
        {
            string result;
            Model.CurrentWeather weather;
            if (model is Model.CurrentWeather)
            {
                weather = model as Model.CurrentWeather;
            }
            else
            {
                return null;
            }
            result = $"\"{weather.Name}\" - {weather.Main.Temp} \u2103, {weather.Weather[0].Description}";
            return result;
        }
        public string GetCurrentTemp()
        {
            string result;
            Model.CurrentWeather weather;
            if (weatherModel is Model.CurrentWeather)
            {
                weather = weatherModel as Model.CurrentWeather;
            }
            else
            {
                return null;
            }
            result = $"\"{weather.Name}\": {weather.Main.Temp} \u2103, {weather.Weather[0].Description}";
            return result;
        }

        public IWeatherModel GetWeather(string cityName, ModeType mode = ModeType.json, string units = null, string lang = null)
        {
            Model.CurrentWeather currentWeather;
            string str;
            string sUrl = $"https://api.openweathermap.org/data/2.5/weather?appid={apiKey}&q={cityName}";
            if (mode != ModeType.json)
            {
                sUrl = String.Join("&", sUrl, $"mode={mode}");
            }
            if (units != null)
            {
                sUrl = String.Join("&", sUrl, $"units={units}");
            }
            if (lang != null)
            {
                sUrl = String.Join("&", sUrl, $"lang={lang}");
            }
            
            if (mode == ModeType.json)
            {
                str = GetString(sUrl);
                currentWeather = JsonConvert.DeserializeObject<Model.CurrentWeather>(str);
                weatherModel = currentWeather;
                return currentWeather;
            }
            else if (mode == ModeType.xml)
            {
                throw new Exception("Нет реализации xml");
            }
            else if (mode == ModeType.html)
            {
                throw new Exception("Нет реализации html");
            }
            else
            {
                throw new Exception("Нет реализации");
            }
        }

        public IWeatherModel GetWeather(int id, ModeType mode = ModeType.json, string units = null, string lang = null)
        {
            Model.CurrentWeather currentWeather;
            string str;
            string sUrl = $"https://api.openweathermap.org/data/2.5/weather?appid={apiKey}&id={id}";
            if (mode != ModeType.json)
            {
                sUrl = String.Join("&", sUrl, $"mode={mode}");
            }
            if (units != null)
            {
                sUrl = String.Join("&", sUrl, $"units={units}");
            }
            if (lang != null)
            {
                sUrl = String.Join("&", sUrl, $"lang={lang}");
            }

            if (mode == ModeType.json)
            {
                str = GetString(sUrl);
                currentWeather = JsonConvert.DeserializeObject<Model.CurrentWeather>(str);
                weatherModel = currentWeather;
                return currentWeather;
            }
            else if (mode == ModeType.xml)
            {
                throw new Exception("Нет реализации xml");
            }
            else if (mode == ModeType.html)
            {
                throw new Exception("Нет реализации html");
            }
            else
            {
                throw new Exception("Нет реализации");
            }
        }

        public IWeatherModel GetWeather(double lat, double lon, ModeType mode = ModeType.json, string units = null, string lang = null)
        {
            Model.CurrentWeather currentWeather;
            string str;
            string sUrl = $"https://api.openweathermap.org/data/2.5/weather?appid={apiKey}&lat={lat}&lon={lon}";
            if (mode != ModeType.json)
            {
                sUrl = String.Join("&", sUrl, $"mode={mode}");
            }
            if (units != null)
            {
                sUrl = String.Join("&", sUrl, $"units={units}");
            }
            if (lang != null)
            {
                sUrl = String.Join("&", sUrl, $"lang={lang}");
            }

            if (mode == ModeType.json)
            {
                str = GetString(sUrl);
                currentWeather = JsonConvert.DeserializeObject<Model.CurrentWeather>(str);
                weatherModel = currentWeather;
                return currentWeather;
            }
            else if (mode == ModeType.xml)
            {
                throw new Exception("Нет реализации xml");
            }
            else if (mode == ModeType.html)
            {
                throw new Exception("Нет реализации html");
            }
            else
            {
                throw new Exception("Нет реализации");
            }
        }

        public IWeatherModel GetWeather(int zip, string countyCode, ModeType mode = ModeType.json, string units = null, string lang = null)
        {
            Model.CurrentWeather currentWeather;
            string str;
            string sUrl = $"https://api.openweathermap.org/data/2.5/weather?appid={apiKey}&zip={zip},{countyCode}";
            if (mode != ModeType.json)
            {
                sUrl = String.Join("&", sUrl, $"mode={mode}");
            }
            if (units != null)
            {
                sUrl = String.Join("&", sUrl, $"units={units}");
            }
            if (lang != null)
            {
                sUrl = String.Join("&", sUrl, $"lang={lang}");
            }

            if (mode == ModeType.json)
            {
                str = GetString(sUrl);
                currentWeather = JsonConvert.DeserializeObject<Model.CurrentWeather>(str);
                weatherModel = currentWeather;
                return currentWeather;
            }
            else if (mode == ModeType.xml)
            {
                throw new Exception("Нет реализации xml");
            }
            else if (mode == ModeType.html)
            {
                throw new Exception("Нет реализации html");
            }
            else
            {
                throw new Exception("Нет реализации");
            }
        }

        private string GetString(string sUrl)
        {
            string result = "";
            HttpWebRequest wrGETURL;
            wrGETURL = (HttpWebRequest)WebRequest.Create(sUrl);
            
            
            using (HttpWebResponse response = (HttpWebResponse)wrGETURL.GetResponse())
            {
                using (Stream objStream = response.GetResponseStream())
                {
                    using (StreamReader objReader = new StreamReader(objStream))
                    {
                        string line;
                        line = objReader.ReadLine();
                        if (line != null)
                        {
                            result += line;
                        }
                    }
                }
            }

            return result;
        }
    }
}
