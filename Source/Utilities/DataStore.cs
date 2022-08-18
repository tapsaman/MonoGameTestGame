using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TapsasEngine.Utilities
{
    [Serializable]
    public class DataStore : IXmlSerializable
    {
        public Dictionary<string, bool> BoolStore;

        public DataStore()
        {
            BoolStore = new Dictionary<string, bool>();
        }

        public void Save(string id, bool value)
        {
            BoolStore[id] = value;
        }

        public bool Get(string id)
        {
            if (BoolStore.ContainsKey(id))
                return BoolStore[id];
            
            Sys.LogError("Tried to fetch undefined boolean value '" + id + "'");

            return false;
        }

        public XmlSchema GetSchema()
        {
            return null;
            //throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("BoolStore");

            foreach (var item in BoolStore)
            {
                writer.WriteStartElement("Data");
                writer.WriteAttributeString("Key", item.Key);
                writer.WriteAttributeString("Value", item.Value.ToString());
                writer.WriteEndElement();
            }
            
            writer.WriteEndElement();
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;

            reader.Read();

            if (reader.Name == "BoolStore")
            {
                reader.Read();

                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.Name == "Data")
                    {
                        string key = reader.GetAttribute("Key");
                        string value = reader.GetAttribute("Value");
                        BoolStore[key] = value == "true";
                        reader.Read();
                    }
                    else
                    {
                        throw new Exception("Unexpected row '" + reader.ReadOuterXml() + "' in BoolStore");
                    }
                }
            }
        }
    }
}