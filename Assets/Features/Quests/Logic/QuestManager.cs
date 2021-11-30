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
        public QuestFocus_SO focus;

        private void Start()
        {
            // reset all quests
            foreach (var quest in questSet.Items)
            {
                quest.reset();
            }
            activeQuests.Items.Clear();
            focus.reset();
        }
        
        public void SetQuestActive(Quest_SO quest)
        {
           // check if not already active
            if (quest.IsActive)
            { 
                return;
            }
            
            activeQuests.Items.Add(quest);
            quest.IsActive = true;
        }

        public void ItemCollected(IntVariable item)
        {
            // Items only get collected if there is a active Quest
            if (activeQuests.Items.Any())
            {
                item.Set(item.Get() + 1);
            }
        }

        public void CompleteQuest()
        {
            if (focus.focus==null)
            {
                Debug.Log("no Quest Focus");
                return;
            }
            
            focus.focus.CheckGoals();
            if (focus.focus.IsCompleted && activeQuests.Items.Contains(focus.focus))
            {
                removeQuest.Raise();
                activeQuests.Items.Remove(focus.focus);
                foreach (var goal in focus.focus.Goals)
                {
                    var item = goal.CurrentAmount;
                    item.Set(item.Get() - goal.RequiredAmount);
                }
                focus.reset();
            }
            else
            {
                Debug.Log("NO!");
            } 
        }

    }
}
