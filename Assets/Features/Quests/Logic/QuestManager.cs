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
        public List<Quest_SO> activeQuests;

        private void Start()
        {
            foreach (var quest in questSet.Items)
            {
                quest.IsActive = false;
                quest.IsCompleted = false;
            }
        }

        public void ItemCollected(IntVariable item)
        {
            if (activeQuests.Count > 0)
            {
                item.Set(item.Get() + 1);
                
                foreach (var quest in activeQuests)
                {
                    quest.CheckGoals();
                }
                activeQuests.RemoveAll(quest => quest.IsCompleted);
            }
        }

        public void SetQuestActive(Quest_SO quest)
        {
            if (!quest.IsActive)
            {
                activeQuests.Add(quest);
                quest.IsActive = true;
                Debug.Log("'" + quest.QuestName + "' activated");
            }
                
        }
        
    }
}
