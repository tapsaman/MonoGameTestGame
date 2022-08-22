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
    public class SaveData : IXmlSerializable
    {
        public DataStore GameData;
        public string MapName;
        public Vector2 Location;
        public int Health;
        public float PlayTimeSeconds;

        private static SaveFileManager<SaveData> _saveFileManager = new SaveFileManager<SaveData>()
        {
            Directory = "ZA6_GameSave"
        };

        public static void CreateAndSave()
        {
            SaveData saving = new SaveData()
            {
                GameData = Static.GameData,
                MapName = Static.Scene.TileMap.Name,
                Location = Static.Player.Position,
                Health = Static.Player.Health,
                PlayTimeSeconds = Static.PlayTimeTimer.Seconds
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
                Static.Game.StateMachine.TransitionTo("StartOver");
            }
            else
            {
                Static.GameData = loaded.GameData;

                Static.Game.StateMachine.TransitionTo(
                    "StartOver",
                    new GameStateStartOver.Args()
                    {
                        SaveData = loaded
                    }
                );
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
            writer.WriteStartElement("MapName");
            writer.WriteValue(MapName);
            writer.WriteEndElement();

            writer.WriteStartElement("Location");
            writer.WriteAttributeString("X", Location.X.ToString());
            writer.WriteAttributeString("Y", Location.Y.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Health");
            writer.WriteValue(Health);
            writer.WriteEndElement();

            writer.WriteStartElement("PlayTimeSeconds");
            writer.WriteValue(PlayTimeSeconds);
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
                    case "MapName":
                        reader.Read();
                        MapName = reader.Value;
                        reader.Read();
                        break;
                    case "Location":
                        string x = reader.GetAttribute("X");
                        string y = reader.GetAttribute("Y");
                        Location = new Vector2(float.Parse(x), float.Parse(y));
                        break;
                    case "GameData":
                        GameData = new DataStore();
                        GameData.ReadXml(reader);
                        break;
                    case "Health":
                        reader.Read();
                        Health = Int16.Parse(reader.Value);
                        reader.Read();
                        break;
                    case "PlayTimeSeconds":
                        reader.Read();
                        PlayTimeSeconds = float.Parse(reader.Value);
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