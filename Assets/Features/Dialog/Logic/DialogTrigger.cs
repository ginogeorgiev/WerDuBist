using DataStructures.Event;
using Features.NPCs.Logic;
using Features.Player.Logic;
using UnityEngine;

namespace Features.Dialog.Logic
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO onStartConversation;
        [SerializeField] private bool teleportPlayerForTrigger = false;
        [SerializeField] private PlayerTeleportFocus_SO teleportFocus;

        public bool TeleportPlayerForTrigger => teleportPlayerForTrigger;

        public PlayerTeleportFocus_SO TeleportFocus => teleportFocus;

        public GameEvent_SO OnStartConversation => onStartConversation;

        private bool convStarted;

        public void SetTeleportFocus()
        {
            teleportFocus.Set(transform);
        }

        public void StartConversation()
        {
            if (onStartConversation == null) return;
            
            onStartConversation.Raise();
            
            convStarted = true;
        }

        public void OnConversationOver()
        {
            if(convStarted) gameObject.SetActive(false);
        }
    }
}
