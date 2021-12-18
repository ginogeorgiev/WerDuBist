using UnityEngine;

namespace Features.Dialog.Logic
{
    public class NpcBehaviour : MonoBehaviour
    {
        [SerializeField] private NPCData_SO data;
        [SerializeField] private DialogConversation_SO conversation;

        public NPCData_SO Data => data;

        public DialogConversation_SO Conversation => conversation;
    }
}
