// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// Edited by: Gino Georgiev
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace DataStructures.Event
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO @event;
        [SerializeField] private UnityEvent response;

        private void OnEnable()
        {
            @event.RegisterListener(this);
        }

        private void OnDisable()
        {
            @event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            response.Invoke();
        }
    }
}
