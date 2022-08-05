using System.Collections.Generic;

namespace MonoGameTestGame
{
    public class UpdatableList<T> : List<T>
    {
        private List<T> _removeOnUpdate = new List<T>();

        public void SetToRemove(T t)
        {
            _removeOnUpdate.Add(t);
        }

        public void Update()
        {
            foreach (var item in _removeOnUpdate)
            {
                Remove(item);
            }
            
            _removeOnUpdate.Clear();
        }
    }
}