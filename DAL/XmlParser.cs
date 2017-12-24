using Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;

namespace DAL
{
    public class XmlParser : IDAL
    {
       
        string path;
        public string GetData(string path)
        {
            this.path = path;
            string st;
            try
            {
                BinaryFormatter Serializer = new BinaryFormatter();
                using (FileStream stream = new FileStream(this.path, FileMode.Open))
                {
                    st = (String)Serializer.Deserialize(stream);
                }
            }
            catch(Exception ex)
            {
                Logger.Log.Error("XmlParser getData " + ex);
                throw;
            }
            Logger.Log.Info("Data got");
            return st;
        }

        public void saveDate(string data)
        {
            try
            {
                BinaryFormatter Serializer = new BinaryFormatter();
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    Serializer.Serialize(stream, data);
                }
                Logger.Log.Info("Data saved");
            }
            catch(Exception ex)
            {
                Logger.Log.Error("XmlParser saveDate" + ex);
                throw 
            }
        }
    }
}