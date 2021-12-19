using DataStructures.Event;
using UnityEngine;

namespace DataStructures.Focus
{
    public abstract class Focus_SO<T> : ScriptableObject
    {
        [SerializeField] private T focus;
        [SerializeField] private GameEvent_SO onFocusChanged;

        public T Get()
        {
            return focus;
        }

        public void Set(T value)
        {
            focus = value;
            onFocusChanged.Raise();
        }

        public void Restore()
        {
            focus = default;
            onFocusChanged.Raise();
        }
    }
}
