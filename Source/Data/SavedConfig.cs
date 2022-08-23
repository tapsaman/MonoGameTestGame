using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;
using System.Xml.Serialization;
using System;
using ZA6.Models;
using System.Xml.Schema;
using System.Xml;
using TapsasEngine;

namespace ZA6
{
    [Serializable]
    public class SavedConfig : IXmlSerializable
    {
        public int ResolutionOption;
        public float MusicVolume;
        public float SFXVolume;

        private static SaveFileManager<SavedConfig> _saveFileManager = new SaveFileManager<SavedConfig>()
        {
            Directory = "ZA6_Config"
        };

        public static void CreateAndSave()
        {
            SavedConfig saving = new SavedConfig()
            {
                ResolutionOption = Array.IndexOf(Static.ResolutionOptions, Static.Renderer.Resolution),
                MusicVolume = Music.Volume,
                SFXVolume = SFX.Volume
            };

            _saveFileManager.Save(saving, "CONFIG.xml");
            
            Static.DevUtils.SetMessage("Config saved");
        }

        public static void LoadAndApply()
        {
            SavedConfig loaded = _saveFileManager.Load("CONFIG.xml");

            if (loaded != null)
            {
                Static.Renderer.Resolution = Static.ResolutionOptions[loaded.ResolutionOption];
                Music.Volume = loaded.MusicVolume;
                SFX.Volume = loaded.SFXVolume;
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("ResolutionOption");
            writer.WriteValue(ResolutionOption);
            writer.WriteEndElement();

            writer.WriteStartElement("MusicVolume");
            writer.WriteValue(MusicVolume);
            writer.WriteEndElement();

            writer.WriteStartElement("SFXVolume");
            writer.WriteValue(SFXVolume);
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                throw new Exception("Empty file");
            
            reader.Read();

            while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
            {
                switch (reader.Name)
                {
                    case "ResolutionOption":
                        reader.Read();
                        ResolutionOption = Int16.Parse(reader.Value);
                        reader.Read();
                        break;
                    case "MusicVolume":
                        reader.Read();
                        MusicVolume = float.Parse(reader.Value);
                        reader.Read();
                        break;
                    case "SFXVolume":
                        reader.Read();
                        SFXVolume = float.Parse(reader.Value);
                        reader.Read();
                        break;
                    default:
                        throw new Exception("Unexpected row '" + reader.ReadOuterXml() + "' in SavedConfig");
                }

                reader.Read();
            }
        }
    }
}