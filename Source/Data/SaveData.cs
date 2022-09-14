using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;
using System.Xml.Serialization;
using System;
using ZA6.Models;
using System.Xml.Schema;
using System.Xml;
using TapsasEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ZA6
{
    [Serializable]
    public class SaveData : IXmlSerializable
    {
        public DataStore GameData { get; private set; }
        public float PlayTimeSeconds { get; private set; }
        public int Rupees { get; private set; }
        public string Scenario { get; private set; }

        private static SaveFileManager<SaveData> _saveFileManager = new SaveFileManager<SaveData>()
        {
            Directory = "ZA6_GameSave"
        };

        public static SaveData Empty { get; } = new SaveData()
        {
            GameData = new DataStore(),
            PlayTimeSeconds = 0f,
            Rupees = 0,
            Scenario = "None"
        };

        public void Apply()
        {
            Static.PlayTimeTimer.Seconds = PlayTimeSeconds;
            Static.GameData = GameData;
            Static.Player.Rupees = Rupees;
            Static.Scenarios.TransitionTo(Scenario);
        }

        public void Save()
        {
            _saveFileManager.Save(this, "SAVE_FILE.xml");
            Static.DevUtils.SetMessage("Progress saved");
        }

        public static SaveData Load()
        {
            SaveData loaded = _saveFileManager.Load("SAVE_FILE.xml");
            
            if (loaded == null)
                return Empty;

            return loaded;
        }

        public static SaveData Create()
        {
            SaveData saveData = new SaveData()
            {
                GameData = Static.GameData,
                PlayTimeSeconds = Static.PlayTimeTimer.Seconds,
                Rupees = Static.Player.Rupees,
                Scenario = Static.Scenarios.CurrentStateKey
            };

            return saveData;
        }

        public static void Clear()
        {
            _saveFileManager.Delete("SAVE_FILE.xml");
            Static.DevUtils.SetMessage("Progress deleted");
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("PlayTimeSeconds");
            writer.WriteValue(PlayTimeSeconds);
            writer.WriteEndElement();

            writer.WriteStartElement("Rupees");
            writer.WriteValue(Rupees);
            writer.WriteEndElement();

            writer.WriteStartElement("GameData");
            GameData.WriteXml(writer);
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
                    case "GameData":
                        GameData = new DataStore();
                        GameData.ReadXml(reader);
                        break;
                    case "PlayTimeSeconds":
                        reader.Read();
                        PlayTimeSeconds = float.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        reader.Read();
                        break;
                    case "Rupees":
                        reader.Read();
                        Rupees = Int32.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        reader.Read();
                        break;
                    default:
                        throw new Exception("Unexpected row '" + reader.ReadOuterXml() + "' in SaveData");
                }

                reader.Read();
            }
        }
    }
}