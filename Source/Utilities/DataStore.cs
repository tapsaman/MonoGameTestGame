using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TapsasEngine.Utilities
{
    [Serializable]
    public class DataStore : IXmlSerializable
    {
        public Dictionary<string, bool> BoolStore;
        public Dictionary<string, int> IntStore;
        public Dictionary<string, string> StringStore;

        public DataStore()
        {
            BoolStore = new Dictionary<string, bool>();
            IntStore = new Dictionary<string, int>();
            StringStore = new Dictionary<string, string>();
        }

        public void Save(string id, bool value)
        {
            BoolStore[id] = value;
        }

        public void Save(string id, int value)
        {
            IntStore[id] = value;
        }

        public void Save(string id, string value)
        {
            StringStore[id] = value;
        }

        public bool Get(string id)
        {
            return GetBool(id);
        }

        public T Get<T>(string id)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)GetInt(id);
            }
            if (typeof(T) == typeof(bool))
            {
                return (T)(object)GetBool(id);
            }
            if (typeof(T) == typeof(string))
            {
                return (T)(object)GetString(id);
            }
            
            throw new Exception("Type '" + typeof(T) + "' not supported by DataStore");
        }

        public int GetInt(string id)
        {
            if (IntStore.ContainsKey(id))
                return IntStore[id];
            
            Sys.LogError("Tried to fetch undefined int value '" + id + "'");

            return 0;
        }

        public bool GetBool(string id)
        {
            if (BoolStore.ContainsKey(id))
                return BoolStore[id];
            
            Sys.LogError("Tried to fetch undefined boolean value '" + id + "'");

            return false;
        }

        public string GetString(string id)
        {
            if (StringStore.ContainsKey(id))
                return StringStore[id];
            
            Sys.LogError("Tried to fetch undefined string value '" + id + "'");

            return null;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            WriteStoreXml<bool>(writer, "BoolStore", BoolStore);
            WriteStoreXml<int>(writer, "IntStore", IntStore);
            WriteStoreXml<string>(writer, "StringStore", StringStore);
        }

        public override string ToString()
        {
            string s = StoreToString<bool>("BoolStore", BoolStore)
                + StoreToString<int>("IntStore", IntStore)
                + StoreToString<string>("StringStore", StringStore);

            return s.Length == 0
                ? "No items in datastore"
                : s.TrimEnd('\n');
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;

            reader.Read();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                switch (reader.Name)
                {
                    case "BoolStore":
                        reader.Read();

                        while (reader.NodeType != XmlNodeType.EndElement && reader.Name == "Data")
                        {
                            string key = reader.GetAttribute("Key");
                            string value = reader.GetAttribute("Value");
                            BoolStore[key] = value == "True" || value == "true";
                            reader.Read();
                        }

                        break;
                    
                    case "IntStore":
                        reader.Read();

                        while (reader.NodeType != XmlNodeType.EndElement && reader.Name == "Data")
                        {
                            string key = reader.GetAttribute("Key");
                            string value = reader.GetAttribute("Value");
                            IntStore[key] = Int16.Parse(value);
                            reader.Read();
                        }

                        break;
                    
                    case "StringStore":
                        reader.Read();

                        while (reader.NodeType != XmlNodeType.EndElement && reader.Name == "Data")
                        {
                            string key = reader.GetAttribute("Key");
                            string value = reader.GetAttribute("Value");
                            StringStore[key] = value;
                            reader.Read();
                        }

                        break;
                    
                    default:
                        throw new Exception("Unexpected node " + reader.ReadOuterXml() + " in DataStore XML");
                }

                reader.Read();
            }
        }

        public string ToXmlString()
        {
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("DataStore");
                WriteXml(writer);
                writer.WriteEndElement();
            }

            var s = sb.ToString();
            return sb.ToString();
        }

        public static void WriteStoreXml<T>(XmlWriter writer, string name, Dictionary<string, T> store)
        {
            if (store.Count != 0)
            {
                writer.WriteStartElement(name);

                foreach (var item in store)
                {
                    writer.WriteStartElement("Data");
                    writer.WriteAttributeString("Key", item.Key);
                    writer.WriteAttributeString("Value", item.Value.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }
        }

        public static string StoreToString<T>(string name, Dictionary<string, T> store)
        {
            StringBuilder sb = new StringBuilder();

            if (store.Count != 0)
            {
                sb.Append(name);

                foreach (var item in store)
                {
                    sb.Append("\n    \"");
                    sb.Append(item.Key);
                    sb.Append("\" > ");
                    sb.Append(item.Value);
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}