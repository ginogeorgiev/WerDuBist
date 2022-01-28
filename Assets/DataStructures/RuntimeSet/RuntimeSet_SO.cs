// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// Edited by: Gino Georgiev
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace DataStructures.RuntimeSet
{
    public abstract class RuntimeSet_SO<T> : ScriptableObject
    {
        [SerializeField] private List<T> items = new List<T>();

        public List<T> GetItems()
        {
            return items;
        }

        public void Add(T item)
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }

        public void Remove(T item)
        {
            if (!items.Contains(item))
            {
                items.Remove(item);
            }
        }

        public void Restore()
        {
            items.Clear();
        }
    }
}
