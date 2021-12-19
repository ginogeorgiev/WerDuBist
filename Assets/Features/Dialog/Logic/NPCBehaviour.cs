using System.Collections.Generic;
using DataStructures.Event;
using Features.Input;
using Features.Quests.Logic;
using UnityEngine;

namespace Features.Dialog.Logic
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
        
        [SerializeField] private GameEvent_SO onAdvanceConversationLine;

        [SerializeField] private NPCData_SO data;
        [SerializeField] private DialogConversation_SO activeConversation;
        [SerializeField] private GameEvent_SO onActiveConversationChanged;
        [SerializeField] private List<ConversationElement> conversationElements;
        [SerializeField] private int conversationIndex;

        public NPCData_SO Data => data;

        public DialogConversation_SO ActiveConversation => activeConversation;

        public void OnNpcFocusChanged()
        {
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
        }

        public void OnCheckForNextConversationPart()
        {
            // dunno if that works
            if (conversationElements[conversationIndex].CheckQuestCompletion)
            {
                conversationElements[conversationIndex].Quest.CheckGoals();
                if (conversationElements[conversationIndex].Quest.IsCompleted)
                {
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
                activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
                conversationIndex++;
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

        private void Start()
        {
            playerControls.Player.SkipDialog.started += _ => onAdvanceConversationLine.Raise();
            activeConversation = conversationElements[conversationIndex].DialogConversationLeft;
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