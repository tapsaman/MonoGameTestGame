using System.Collections.Generic;

namespace TapsasEngine.Utilities
{
    public class DataStore
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
    }
}