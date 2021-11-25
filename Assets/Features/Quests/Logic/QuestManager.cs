using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {
        public QuestSet_SO questSet;
        public QuestSetActive_SO activeQuests;

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
                
                foreach (var quest in activeQuests.Items)
                {
                    quest.CheckGoals();
                }
                activeQuests.Items.RemoveAll(quest => quest.IsCompleted);
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
