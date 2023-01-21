using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace MinecraftMod
{
    public class Minecraft
    {
        // works but isn't the best
        public static bool MultiInstance 
        {
            get
            {
                return File.ReadAllText("MinecraftData\\AppxManifest.xml").Contains("SupportsMultipleInstances");
            }
            set
            {
                string data = File.ReadAllText("MinecraftData\\AppxManifest.xml");

                if (value)
                {
                    if (!MultiInstance)
                    {
                        data = data.Replace("<Package xmlns=", "<Package xmlns:desktop4=\"http://schemas.microsoft.com/appx/manifest/desktop/windows10/4\" xmlns=")
                                   .Replace("<Application Id=", "<Application desktop4:SupportsMultipleInstances=\"true\" Id=");
                    } // these errors should never be called unless someone manually adds ~MI themselves
                    //else throw new System.Exception("MultiInstance already enabled"); 
                }
                else
                {
                    if (MultiInstance)
                    {
                        data = data.Replace("<Package xmlns:desktop4=\"http://schemas.microsoft.com/appx/manifest/desktop/windows10/4\" xmlns=", "<Package xmlns=")
                                   .Replace("<Application desktop4:SupportsMultipleInstances=\"true\" Id=", "<Application Id=");
                    }
                   // else throw new System.Exception("MultiInstance not already enabled");
                }

                File.WriteAllText("MinecraftData\\AppxManifest.xml", data);
            }
        }

        // this is proper practice
        public static string Backcolor
        {
            get
            {
                string result = null;

                using (XmlReader reader = XmlReader.Create("MinecraftData\\AppxManifest.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && reader.Name == "uap:VisualElements")
                        {
                            string backgroundColor = reader["BackgroundColor"];
                            result = backgroundColor;
                            reader.Dispose();
                            break;
                        }
                    }
                }

                return result;
            }
            set
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(File.ReadAllText("MinecraftData\\AppxManifest.xml"));

                XmlNodeList nodes = doc.SelectNodes("//*[@BackgroundColor]");

                foreach (XmlNode node in nodes)
                {
                    node.Attributes["BackgroundColor"].Value = value;
                }

                doc.Save("MinecraftData\\AppxManifest.xml");
            }
        }


        public static string CaptionTitle
        {
            get
            {
                string result = null;

                using (XmlReader reader = XmlReader.Create("MinecraftData\\AppxManifest.xml"))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement() && reader.Name == "uap:VisualElements")
                        {
                            string backgroundColor = reader["DisplayName"];
                            result = backgroundColor;
                            reader.Dispose();
                            break;
                        }
                    }
                }

                return result;
            }
            set
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(File.ReadAllText("MinecraftData\\AppxManifest.xml"));

                XmlNodeList nodes = doc.SelectNodes("//*[@DisplayName]");

                foreach (XmlNode node in nodes)
                {
                    node.Attributes["DisplayName"].Value = value; // checking Attributes not Node types.
                }

                doc.Save("MinecraftData\\AppxManifest.xml");
            }
        }
    }
}
