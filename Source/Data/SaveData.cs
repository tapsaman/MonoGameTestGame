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
        public DataStore GameData;
        public float PlayTimeSeconds;
        public int Rupees;

        private static SaveFileManager<SaveData> _saveFileManager = new SaveFileManager<SaveData>()
        {
            Directory = "ZA6_GameSave"
        };

        public static void CreateAndSave()
        {
            SaveData saving = new SaveData()
            {
                GameData = Static.GameData,
                PlayTimeSeconds = Static.PlayTimeTimer.Seconds,
                Rupees = Static.Player.Rupees
            };

            _saveFileManager.Save(saving, "SAVE_FILE.xml");

            Static.LoadedGame = saving;

            Static.DevUtils.SetMessage("Progress saved");
        }

        public static SaveData Load()
        {
            SaveData loaded = _saveFileManager.Load("SAVE_FILE.xml");
            Static.LoadedGame = loaded;
            
            return loaded;
        }

        public static void LoadAndApply()
        {
            SaveData loaded = Load();

            if (loaded == null)
            {
                Static.Game.StateMachine.TransitionTo("Intro");
            }
            else
            {
                Static.PlayTimeTimer.Seconds = loaded.PlayTimeSeconds;
                Static.GameData = loaded.GameData;
                Static.Player.Rupees = loaded.Rupees;

                Static.Game.StateMachine.TransitionTo("StartOver");
            }
        }

        public static void Clear()
        {
            _saveFileManager.Delete("SAVE_FILE.xml");
            Static.LoadedGame = null;

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
                        PlayTimeSeconds = float.Parse(reader.Value);
                        reader.Read();
                        break;
                    case "Rupees":
                        reader.Read();
                        Rupees = Int32.Parse(reader.Value);
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