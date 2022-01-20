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
        
        [Header("Hier kommt die weitere führende Conversation rein, wenn es gerade eine Aktive Quest zu erledigen gibt.", order = 2)]
        [Space (-10, order = 3)]
        [Header("Und die Quest muss mit übergeben werden, damit sie geprüft werden kann", order = 4)]
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
        [SerializeField] private NpcBehaviourRuntimeSet npcBehaviourRuntimeSet;
        [SerializeField] private GameEvent_SO onActiveConversationChanged;
        [SerializeField] private QuestEvent onCompleteQuest;
        [SerializeField] private QuestSet_SO questSet;

        [Header("Nichts ausfüllen, das ist zum debuggen")]
        [SerializeField] private int conversationIndex;
        [SerializeField] private DialogConversation_SO activeConversation;

        public NPCData_SO Data => data;

        public DialogConversation_SO ActiveConversation => activeConversation;

        public ConversationElement GetActiveConversationElement => conversationElements[conversationIndex];
        
        private void Awake()
        {
            npcBehaviourRuntimeSet.Add(this);
            
            playerControls = new PlayerControls();
        }
        
        private void Start()
        {
            if (conversationElements == null || conversationElements.Count == 0) return;
            
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;

            // set own position for all Quest this NPC starts/ends
            var pos = transform.position;
            foreach (var conversation in conversationElements.Where(element => element.Quest!=null))
            {
                conversation.Quest.StartPosition = pos;
                if (conversation.Quest.EndPosition == new Vector2(0,0))
                {
                    conversation.Quest.EndPosition = pos; 
                }
            }
            foreach (var conversation in conversationElements.Where(c => c.DialogConversationLeft != null))
            {
                if (conversation.DialogConversationLeft.DialogQuestion == null) continue;
                foreach (var con in conversation.DialogConversationLeft.DialogQuestion.Choices.Where(choice => choice.Quest!=null))
                {
                    con.Quest.StartPosition = pos;
                }
            }
            foreach (var quest in questSet.Items)
            {
                foreach (var goal in quest.GoalList.Where(goal => goal.Npc == data))
                {
                    quest.EndPosition = pos;
                }
            }
        }

        private void OnDestroy()
        {
            if (npcBehaviourRuntimeSet.GetItems().Contains(this))
            {
                npcBehaviourRuntimeSet.Remove(this);
            }
        }

        public void OnNpcFocusChanged()
        {
            if (conversationElements == null || conversationElements.Count == 0) return;
            
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
        }

        public void OnCheckForNextConversationPart()
        {
            // now it works i guess
            if (conversationElements[conversationIndex].Quest != null)
            {
                if (conversationElements[conversationIndex].Quest.CheckGoals(null))
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
                
                activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
                onActiveConversationChanged.Raise();
            }
        }

        public void AdvanceConvIndex()
        {
            if (conversationElements == null || conversationElements.Count == 0) return;

            if (conversationIndex + 1 >= conversationElements.Count) return;
            
            conversationIndex++;
            // Debug.Log(Data.name + "'s conversation advanced to " + conversationIndex);
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
            onActiveConversationChanged.Raise();
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