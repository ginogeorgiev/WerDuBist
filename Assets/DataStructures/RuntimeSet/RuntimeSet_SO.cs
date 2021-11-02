using System.Collections.Generic;
using UnityEngine;

namespace DataStructures.RuntimeSet
{
    public abstract class RuntimeSet_SO<T> : ScriptableObject
    {
        private List<T> items = new List<T>();

        public List<T> GetItems()
        {
            return items;
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public void Remove(T item)
        {
            items.Remove(item);
        }
    }
}
