using UnityEngine;

namespace DataStructures.Focus
{
    public abstract class Focus_SO<T> : ScriptableObject
    {
        [SerializeField] private T focus;

        public T Get()
        {
            return focus;
        }

        public void Set(T value)
        {
            focus = value;
        }
    }
}
