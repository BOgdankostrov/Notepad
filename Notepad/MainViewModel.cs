using BL;
using Contracts;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace Notepad
{
    public class MainViewModel : INotifyPropertyChanged
    {


        FileProcessing fp;
        public MainViewModel()
        {
        
            Logger.Log.Info("Data saved");
            Logger.InitLogger();
            if (String.IsNullOrWhiteSpace(Notepad.Properties.Settings.Default.DataConnection))
                throw new ArgumentNullException("Data connection not found!");
            if (String.IsNullOrWhiteSpace(Notepad.Properties.Settings.Default.DALAssembly))
                throw new ArgumentNullException("DALAssembly setting not found!");
            if (String.IsNullOrWhiteSpace(Notepad.Properties.Settings.Default.ReaderType))
                throw new ArgumentNullException("ReaderType setting not found!");

            Environment.CurrentDirectory = Path.GetDirectoryName(
                Assembly.GetExecutingAssembly().Location);

            Config cfg = new Config();
            cfg.DataPath = Path.GetFullPath(Notepad.Properties.Settings.Default.DataConnection);
            cfg.DataReaderAssembly = Path.GetFullPath(Notepad.Properties.Settings.Default.DALAssembly);
            cfg.DataReader = Path.GetFullPath(Notepad.Properties.Settings.Default.ReaderType);

             fp = new FileProcessing(cfg);
            TextRichText = fp.GetData();
            Weather = fp.GetTemperature();
        }


        private string textRichText;
        public string TextRichText
        {
            get => textRichText; set
            {
                textRichText = value;
                DoProperyChanged("TextRichText");
            }
        }

        private string weather;
        public string Weather
        {
            get => weather; set
            {
                weather = value;
                DoProperyChanged("Weather");
            }
        }

        #region Command
        private Command saveData;
        public Command SaveData
        {
            get
            {
                return saveData ??
                    (saveData = new Command(obj =>
                    {
                        fp.SaveDate(textRichText);
                    }
                    ));
            }
        }

        private Command createDoc;
        public Command CreateDoc
        {
            get
            {
                return createDoc ??
                    (createDoc = new Command(obj =>
                    {
                        System.Threading.Tasks.Task.Run(() =>
                        {

                            if (!string.IsNullOrWhiteSpace(textRichText))
                                try
                                {
                                    Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                                    Document doc = app.Documents.Add(Visible: true);
                                    Range text = doc.Range();
                                    text.Text = textRichText;
                                    doc.Save();
                                    doc.Close();
                                    app.Quit();
                                    Logger.Log.Info("Date saved in Doc");
                                }
                                catch (InvalidCastException)
                                {
                                    MessageBox.Show("Word don't activate");
                                    Logger.Log.Error("Word don't activate");
                                }
                                catch (COMException)
                                {
                                    MessageBox.Show("failed to save");
                                    Logger.Log.Error("failed to save");
                                }
                        }
                        );
                      
                    }
                    ));
            }
        }

     
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void DoProperyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
