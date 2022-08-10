using System.Collections.Generic;

namespace ZA6
{
    public class UpdatableList<T> : List<T>
    {
        private List<T> _addOnUpdate = new List<T>();
        private List<T> _removeOnUpdate = new List<T>();

        public void SetToAdd(T t)
        {
            _addOnUpdate.Add(t);
        }

        public void SetToRemove(T t)
        {
            _removeOnUpdate.Add(t);
        }

        public void Update()
        {
            foreach (var item in _addOnUpdate)
            {
                Add(item);
            }
            foreach (var item in _removeOnUpdate)
            {
                Remove(item);
            }
            
            _addOnUpdate.Clear();
            _removeOnUpdate.Clear();
        }
    }
}