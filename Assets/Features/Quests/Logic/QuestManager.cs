using System.Collections.Generic;
using System.Linq;
using Features.NPCs.Logic;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestSet_SO questSet;
        [SerializeField] private QuestSetActive_SO activeQuests;
        
        [SerializeField] private QuestEvent onQuestUnlocked;
        [SerializeField] private QuestEvent onDisplayUnlockedQuest;
        [SerializeField] private QuestEvent onQuestAccepted;
        [SerializeField] private QuestEvent onDisplayQuest;
        [SerializeField] private QuestEvent onCompleteQuest;
        [SerializeField] private QuestEvent onRemoveQuest;
        
        [SerializeField] private QuestFocus_SO questFocus;
        [SerializeField] private NpcFocus_So npcFocus;
        [SerializeField] private NpcBehaviourRuntimeSet behaviourRuntimeSet;
        
        [SerializeField] private List<Quest_SO> firstQuests;

        private void Start()
        {
            // reset all quests
            foreach (var quest in questSet.Items)
            {
                quest.Restore();
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
           
           // advance certain conversations after quest is accepted if necessary
           if (quest.NpcsToAdvanceConversationsList.Count != 0)
           {
               foreach (NpcBehaviour npcBehaviour in quest.NpcsToAdvanceConversationsList.SelectMany(
                   npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
               {
                   npcBehaviour.AdvanceConvIndex();
               }
           }
           
           onDisplayQuest.Raise(quest);
        }

        private void CompleteQuest(Quest_SO quest)
        {
            //TODO ich habe das hier auskommentiert weils so geht, bitte prüfen !!
            // if (quest.GoalList.All(goal => goal.Type==Goal.GoalType.collect)) return;
            
            if (!activeQuests.Items.Contains(quest)) return;
            
            quest.CheckGoals();
            
            if (!quest.CheckGoals()) return;
            
            quest.IsActive = false;
            quest.IsCompleted = true;

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
            
            // reevaluate each Quest.Goal with goalType Quest
            foreach (var goal in questSet.Items.SelectMany(q => q.GoalList.Where(goal => goal.Type==Goal.GoalType.quest)))
            {
                goal.Evaluate();
            }
                
            Debug.Log("'" + quest.QuestTitle + "' Completed");
            onRemoveQuest.Raise(quest);
        }
        
        public void UpdateTalkQuest()
        {
            // for each activeQuest.Goal with goalType Talk
            foreach (var quest in questSet.Items.Where(q => q.GoalList.Any(goal => goal.Type == Goal.GoalType.talk)))
            {
                quest.CheckGoals(npcFocus);
                CompleteQuest(quest);
            }
        }
    }
} 