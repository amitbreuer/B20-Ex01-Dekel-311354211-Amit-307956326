using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace B20_Ex01_Dekel_311354211_Amit_307956326
{
    class AppSettings
    {
        public Point LastWindowLocation { get; set; }
        public Size LastWindowSize { get; set; }
        public bool RememberUser { get; set; }
        public string LastAccessToken { get; set; }

        public AppSettings()
        {
            LastWindowSize = new Size(1240,950);
            LastWindowLocation = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - this.LastWindowSize.Width) / 2 ,
                (Screen.PrimaryScreen.WorkingArea.Height - this.LastWindowSize.Height) / 2);
            RememberUser = false;
            LastAccessToken = string.Empty;
        }

        public static AppSettings LoadFromFile()
        {
            AppSettings o_AppSettings = new AppSettings();

            if (File.Exists("App Settings.xml"))
            {    
                using (Stream stream = new FileStream("App Settings.xml", FileMode.Open))
                {
                    stream.Position = 0;
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    o_AppSettings = serializer.Deserialize(stream) as AppSettings;
                }
            }

            return o_AppSettings; 
        }

        public void SaveToFile()
        {
            using (Stream stream = new FileStream("App Settings.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(stream, this);
            }
        }
    }
}
