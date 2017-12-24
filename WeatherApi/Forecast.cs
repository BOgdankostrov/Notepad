using Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApi
{
    public class Forecast: IWeather
    {
         string APPID = "60114139acb3bddaa2c89d9c3c5e2d62";
        public string GetTemperature()
        {
            try
            {
                var request = new WebClient().DownloadString($"https://api.openweathermap.org/data/2.5/weather?id=2172797&APPID={APPID}");
                Rootobject eoot = JsonConvert.DeserializeObject<Rootobject>(request);
                string tr = ((int)eoot.main.temp - 273).ToString();
                return tr;
            }
            catch (WebException ex)
            {
                Logger.Log.Error($"Отсутствует подключение к интернету{ex}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Log.Error($"Неизвестная ошибка{ex}");
                throw new Exception();
            }



        }
    }
}
