using System.Collections.Generic;
using DataStructures.Event;
using Features.Dialog.Logic;
using Features.Input;
using Features.Quests.Logic;
using Features.Survey.Logic;
using UnityEngine;

namespace Features.NPCs.Logic
{
    [System.Serializable]
    public struct ConversationElement
    {
        [SerializeField] private DialogConversation_SO dialogConversationLeft;
        
        [SerializeField] private bool checkQuestCompletion;
        [SerializeField] private DialogConversation_SO dialogConversationRight;
        [SerializeField] private Quest_SO quest;

        public DialogConversation_SO DialogConversationLeft => dialogConversationLeft;

        public bool CheckQuestCompletion => checkQuestCompletion;

        public DialogConversation_SO DialogConversationRight => dialogConversationRight;

        public Quest_SO Quest => quest;
    }
    
    public class NpcBehaviour : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private NpcFocus_So npcFocus;

        [SerializeField] private NPCData_SO data;
        [SerializeField] private DialogConversation_SO activeConversation;
        [SerializeField] private GameEvent_SO onActiveConversationChanged;
        [SerializeField] private List<ConversationElement> conversationElements;
        [SerializeField] private int conversationIndex;

        [SerializeField] private QuestEvent onCompleteQuest;

        public NPCData_SO Data => data;

        public DialogConversation_SO ActiveConversation => activeConversation;

        public ConversationElement GetActiveConversationElement => conversationElements[conversationIndex];
        
        private void Start()
        {
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
        }

        public void OnNpcFocusChanged()
        {
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
        }

        public void OnCheckForNextConversationPart()
        {
            // dunno if that works
            if (conversationElements[conversationIndex].CheckQuestCompletion)
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