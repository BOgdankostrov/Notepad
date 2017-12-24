using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WeatherApi;

namespace BL
{
  public  class FileProcessing:IBL
    {
        IDAL reader;
        string pathFile;
        IWeather weather;
        public FileProcessing(Config config)
        {


           Logger.Log.Info($"FileProcessing ctor with {config}");

            if (!File.Exists(config.DataReaderAssembly))
                throw new ArgumentException("Can't find assembly!");
            if (config == null)
                throw new ArgumentNullException("Config is null");

            Assembly assembly = null;
            Type foundClass = null;
            try
            {
                assembly = Assembly.LoadFile(config.DataReaderAssembly);
                Logger.Log.Info($"assembly {assembly.FullName} loaded");
                foundClass = assembly.GetExportedTypes().FirstOrDefault(x => x.GetInterface("IDAL") != null);
                Logger.Log.Info($"class {foundClass.FullName} loaded");
            }
            catch (Exception ex)
            {

                Logger.Log.Error($"Can't create reader {ex}");
                throw new Exception("don't loaded assembly",ex);
            }
            if (foundClass == null)
                throw new InvalidOperationException("Can't find class with IDAL interface");


             reader = Activator.CreateInstance(assembly.FullName, foundClass.FullName).Unwrap() as IDAL;
            if (reader == null)
                throw new InvalidOperationException("Can't create reader instance");

            if (String.IsNullOrWhiteSpace(config.DataPath))
                throw new ArgumentNullException("Config.DataPath is null!");
            if (!File.Exists(config.DataPath))
                throw new FileNotFoundException($"Can't find file {config.DataPath}");

            pathFile = config.DataPath;

            weather = new Forecast();
        }

        public string GetData()
        {
            return reader.GetData(pathFile);
        }

        public string GetTemperature()
        {
            return weather.GetTemperature();
        }

        public void SaveDate(string data)
        {
            reader.saveDate(data);
        }
    }
}
