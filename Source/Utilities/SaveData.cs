using Microsoft.Xna.Framework;
using TapsasEngine.Utilities;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System;
using System.IO;
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
        //public Config Config;

        private static SaveFileManager<SaveData> _saveFileManager = new SaveFileManager<SaveData>();

        public static void CreateAndSave()
        {
            SaveData saving = new SaveData()
            {
                GameData = Static.GameData,
                MapName = Static.Scene.TileMap.Name,
                Location = Static.Player.Position,
                Health = Static.Player.Health
            };

            _saveFileManager.Save(saving, "SAVE_FILE.xml");
        }

        public static void LoadAndApply()
        {
            SaveData loaded = _saveFileManager.Load("SAVE_FILE.xml");

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
                        MapName = loaded.MapName,
                        Location = loaded.Location
                    }
                );
            }
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

            writer.WriteStartElement("GameData");
            GameData.WriteXml(writer);
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                throw new Exception("Empty file");
            
            reader.Read();

            while (reader.NodeType != XmlNodeType.EndElement)
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
                        GameData = new DataStore();
                        GameData.ReadXml(reader);
                        break;
                    default:
                        throw new Exception("Unexpected row '" + reader.ReadOuterXml() + "' in SaveData");
                }

                reader.Read();
            }
        }
    }
}