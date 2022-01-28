using System.Collections.Generic;
using System.Linq;
using DataStructures.Event;
using Features.NPCs.Logic;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestSet_SO questSet;
        [SerializeField] private QuestSet_SO activeQuests;
        
        [SerializeField] private QuestEvent onQuestUnlocked;
        [SerializeField] private QuestEvent onDisplayUnlockedQuest;
        [SerializeField] private QuestEvent onQuestAccepted;
        [SerializeField] private QuestEvent onDisplayQuest;
        [SerializeField] private QuestEvent onCompleteQuest;
        [SerializeField] private QuestEvent onRemoveQuest;
        
        [SerializeField] private QuestFocus_SO questFocus;
        [SerializeField] private NpcFocus_So npcFocus;
        [SerializeField] private NpcBehaviourRuntimeSet behaviourRuntimeSet;

        [SerializeField] private GameEvent_SO eventForQuestAcceptedSound;
        [SerializeField] private GameEvent_SO eventForQuestCompletedSound;
        
        [Header(" des Games direkt unlocked werden sollen")]
        [Space(-10)]
        [Header("In diese Liste kommen alle Quests, die zu Beginn")]
        [SerializeField] private List<Quest_SO> firstQuests;

        private void Start()
        {
            // reset all quests
            foreach (var quest in questSet.Items)
            {
                quest.Restore();
            }
            foreach (var quest in questSet.Items.Where(q => q.EndPosition == new Vector2(0,0)))  
            {
                if (quest.StartPosition != new Vector2(0,0))
                {
                    quest.EndPosition = quest.StartPosition;
                }
            }
            foreach (var quest in questSet.Items.Where(q => q.StartPosition == new Vector2(0,0)))  
            {
                if (quest.EndPosition != new Vector2(0,0))
                {
                    quest.StartPosition = quest.EndPosition;
                }
            }
           
            activeQuests.Items.Clear();
            questFocus.Restore();
            onQuestUnlocked.RegisterListener(SetQuestUnlocked);
            onQuestAccepted.RegisterListener(SetQuestActive);
            onCompleteQuest.RegisterListener(CompleteQuest);

            foreach (var quest in firstQuests)
            {
                onQuestUnlocked.Raise(quest);
            }
        }

        private void SetQuestUnlocked(Quest_SO quest)
        {
            // check if not already unlocked
            if (quest.IsUnlocked || quest.IsActive || quest.IsCompleted)
            {
                return;
            }
            
            quest.IsUnlocked = true;
            onDisplayUnlockedQuest.Raise(quest);
        }
        
        private void SetQuestActive(Quest_SO quest)
        {
           // check if not already active
           if (quest.IsActive || quest.IsCompleted)
           {
               return;
           }
           
           activeQuests.Items.Add(quest);
           quest.IsActive = true;
           
           if (eventForQuestAcceptedSound != null)
           {
               eventForQuestAcceptedSound.Raise();
           }
           
           // advance certain conversations after quest is accepted if necessary
           if (quest.NpcsToAdvanceConversationsListForTalkQuest.Count != 0)
           {
               foreach (NpcBehaviour npcBehaviour in quest.NpcsToAdvanceConversationsListForTalkQuest.SelectMany(
                   npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
               {
                   npcBehaviour.AdvanceConvIndex();
               }
           }
           
           onDisplayQuest.Raise(quest);
        }

        private void CompleteQuest(Quest_SO quest)
        {
            if (!activeQuests.Items.Contains(quest)) return;

            if (!quest.CheckGoals(null)) return;
            
            quest.IsActive = false;
            quest.IsCompleted = true;

            if (eventForQuestCompletedSound != null)
            {
                eventForQuestCompletedSound.Raise(); 
            }
            
            if (quest.NpcsToAdvanceConversationsList.Count != 0)
            {
                foreach (NpcBehaviour npcBehaviour in quest.NpcsToAdvanceConversationsList.SelectMany(
                    npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
                {
                    npcBehaviour.AdvanceConvIndex();
                }
            }

            // check after each quest completion if sequence is completed
            if (quest.SingleSequenceData != null)
            {
                quest.SingleSequenceData.CheckForNextSequence();
            }
            
            // remove all Collect Quest Items from Inventory
            foreach (var goal in quest.GoalList.Where(goal => goal.Type==Goal.GoalType.collect))
            {
                goal.CurrentAmount.Add(-goal.RequiredAmount);
            }
            
            // reevaluate each Quest with goalType Quest
            foreach (var q in questSet.Items.Where(q => q.GoalList.Any(goal => goal.Type == Goal.GoalType.quest)))
            {
                q.CheckGoals(null);
            }
                
            Debug.Log("'" + quest.QuestTitle + "' Completed");
            onRemoveQuest.Raise(quest);
        }
        
        public void UpdateTalkQuest()
        {
            // for each Quest with goalType Talk
            foreach (var quest in questSet.Items.Where(q => q.GoalList.Any(goal => goal.Type == Goal.GoalType.talk)))
            {
                if (!quest.IsActive) continue;
                
                if (npcFocus.Get()!=null)
                {
                    quest.CheckGoals(npcFocus);
                    CompleteQuest(quest);
                }
            }
        }
    }
} 