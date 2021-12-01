using System;
using UnityEngine;

namespace DataStructures.Event
{
    [CreateAssetMenu(fileName = "newActionEvent", menuName = "DataStructures/Event/ActionEvent")]
    public class ActionEvent : ScriptableObject
    {
        private Action listeners;
    
        public void Raise()
        {
            this.listeners?.Invoke();
        }

        public void RegisterListener(Action listener)
        {
            this.listeners += listener;
        }

        public void UnregisterListener(Action listener)
        {
            this.listeners -= listener;
        }
    }
}