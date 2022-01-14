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
           onDisplayQuest.Raise(quest);
        }

        private void CompleteQuest(Quest_SO quest)
        {
            if (quest.GoalList.All(goal => goal.Type==Goal.GoalType.collect) && !activeQuests.Items.Contains(quest)) return;

            quest.CheckGoals();
            
            if (!quest.CheckGoals()) return;
            
            quest.IsActive = false;
            quest.IsCompleted = true;

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
            foreach (var goal in questSet.Items.SelectMany(q => q.GoalList.Where(goal => goal.Type==Goal.GoalType.talk)))
            {
                goal.Evaluate(npcFocus);
            }
        }
    }
} 