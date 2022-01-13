using System.Collections.Generic;
using System.Linq;
using DataStructures.Event;
using Features.Dialog.Logic;
using Features.Input;
using Features.Quests.Logic;
using UnityEngine;

namespace Features.NPCs.Logic
{
    [System.Serializable]
    public struct ConversationElement
    {
        [Header("Hier kommt die weitere führende Conversation rein", order = 0)]
        [SerializeField] private DialogConversation_SO dialogConversationLeft;
        
        [Header("Hier kommt die weitere führende Conversation rein, wenn es gerade eine Aktive Quest zu erledigen gibt.", order = 1)]
        [Space (-10, order = 2)]
        [Header("Und die Quest muss mit übergeben werden, damit sie geprüft werden kann", order = 3)]
        [SerializeField] private DialogConversation_SO dialogConversationRight;
        [SerializeField] private Quest_SO quest;

        public DialogConversation_SO DialogConversationLeft => dialogConversationLeft;

        public DialogConversation_SO DialogConversationRight => dialogConversationRight;

        public Quest_SO Quest => quest;
    }
    
    public class NpcBehaviour : MonoBehaviour
    {
        [Header("Hier kommt das SO zum NPC rein (den Kreis rechts dafür benutzen)")]
        [SerializeField] private NPCData_SO data;
        
        [Header("Hier kommen alle Dialoge in Reihenfolge rein, die der NPC führen kann.")]
        [SerializeField] private List<ConversationElement> conversationElements;
        
        [Header("Hier sollte bereits alles durch das Template ausgefüllt sein")]
        [SerializeField] private NpcFocus_So npcFocus;
        [SerializeField] private GameEvent_SO onActiveConversationChanged;
        [SerializeField] private QuestEvent onCompleteQuest;

        [Header("Nichts ausfüllen, das ist zum debuggen")]
        [SerializeField] private int conversationIndex;
        [SerializeField] private DialogConversation_SO activeConversation;

        public NPCData_SO Data => data;

        public DialogConversation_SO ActiveConversation => activeConversation;

        public ConversationElement GetActiveConversationElement => conversationElements[conversationIndex];
        
        private void Start()
        {
            if (conversationElements == null || conversationElements.Count == 0) return;
            
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;

            // set own position for all Quest this NPC gives
            foreach (var conversation in conversationElements.Where(element => element.Quest!=null))
            {
                var pos = transform.localPosition;
                Debug.Log(pos);
                conversation.Quest.QuestPosition = new Vector2(pos.x,pos.y);
            }
        }

        public void OnNpcFocusChanged()
        {
            if (conversationElements == null || conversationElements.Count == 0) return;
            
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
        }

        public void OnCheckForNextConversationPart()
        {
            // dunno if that works
            if (conversationElements[conversationIndex].Quest != null)
            {
                if (conversationElements[conversationIndex].Quest.CheckGoals())
                {
                    onCompleteQuest.Raise(conversationElements[conversationIndex].Quest);
                    activeConversation = conversationElements[conversationIndex].DialogConversationRight;
                    onActiveConversationChanged.Raise();
                }
                else
                {
                    activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
                    onActiveConversationChanged.Raise();
                }
            }
            else
            {
                if (conversationElements == null || conversationElements.Count == 0) return;
                
                if (conversationIndex + 1 < conversationElements.Count)
                {
                    conversationIndex++;
                }
                activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
                onActiveConversationChanged.Raise();
            }
        }

        public void SetNpcFocus()
        {
            npcFocus.Set(this);
        }

        public void RemoveNpcFocus()
        {
            npcFocus.Restore();
        }

        #region Input related
        
        private PlayerControls playerControls;
        private void Awake()
        {
            playerControls = new PlayerControls();
        }
        
        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        
        #endregion
    }
}