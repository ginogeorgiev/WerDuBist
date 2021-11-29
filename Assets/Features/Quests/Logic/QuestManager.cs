using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Event;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {
        public QuestSet_SO questSet;
        public QuestSetActive_SO activeQuests;
        public GameEvent_SO removeQuest;

        private void Start()
        {
            // reset all quests
            foreach (var quest in questSet.Items)
            {
                quest.IsActive = false;
                quest.IsCompleted = false;
                activeQuests.Items.Clear();
            }
        }

        public void ItemCollected(IntVariable item)
        {
            // Items only get collected if there is a active Quest
            if (activeQuests.Items.Any())
            {
                item.Set(item.Get() + 1);
            }
        }

        public void CompleteQuest(Quest_SO quest)
        {
            quest.CheckGoals();
            if (quest.IsCompleted && activeQuests.Items.Contains(quest))
            {
                removeQuest.Raise();
                activeQuests.Items.Remove(quest);
                foreach (var goal in quest.Goals)
                {
                    var item = goal.CurrentAmount;
                    item.Set(item.Get() - goal.RequiredAmount);
                }
            }
            else
            {
                Debug.Log("NO!");
            }
        }

        public void SetQuestActive(Quest_SO quest)
        {
            if (!quest.IsActive) 
                // if not already active
            {
                activeQuests.Items.Add(quest);
                quest.IsActive = true;
                Debug.Log("'" + quest.QuestName + "' activated");
                
            }
        }
        
    }
}
