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
    public class AppSettings
    {
        private Point location;
        private Size size;
        private bool @checked;

        public Point LastWindowLocation { get; set; }

        public Size LastWindowSize { get; set; }

        public bool RememberUser { get; set; }

        public string LastAccessToken { get; set; }

        public AppSettings()
        {
            LastWindowSize = new Size(830, 610);
            LastWindowLocation = new Point(200, 50);
            RememberUser = false;
            LastAccessToken = string.Empty;
        }

        public AppSettings(Point i_Location, Size i_Size, bool i_Checked)
        {
            this.location = i_Location;
            this.size = i_Size;
            this.RememberUser = i_Checked;
        }

        public static AppSettings LoadSettingsFromFile()
        {
            AppSettings o_AppSettings = new AppSettings();

            if (File.Exists("AppSettings.xml"))
            {
                using (Stream stream = new FileStream("AppSettings.xml", FileMode.Open))
                {
                    stream.Position = 0;
                    XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
                    o_AppSettings = serializer.Deserialize(stream) as AppSettings;
                }
            }

            return o_AppSettings;
        }

        public void SaveSettingsToFile()
        {
            if (this.RememberUser)
            {
                using (Stream stream = new FileStream("AppSettings.xml", FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(this.GetType());
                    serializer.Serialize(stream, this);
                }
            }
            else
            {
                File.Delete("AppSettings.xml");
            }
        }
    }
}