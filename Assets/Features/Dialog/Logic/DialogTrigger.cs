using DataStructures.Event;
using Features.NPCs.Logic;
using UnityEngine;

namespace Features.Dialog.Logic
{
    public class DialogTrigger : MonoBehaviour
    {
        [SerializeField] private NpcBehaviour npcForDialog;
        [SerializeField] private GameEvent_SO onStartConversation;

        public void StartConversation()
        {
            if (npcForDialog == null || onStartConversation == null) return;
            
            npcForDialog.OnCheckForNextConversationPart();
            npcForDialog.SetNpcFocus();
            onStartConversation.Raise();
            
            this.gameObject.SetActive(false);
        }
    }
}
