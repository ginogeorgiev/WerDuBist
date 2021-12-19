using DataStructures.Event;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestSet_SO questSet;
        [SerializeField] private QuestSetActive_SO activeQuests;
        [SerializeField] private GameEvent_SO removeQuest;
        [SerializeField] private QuestEvent onQuestAccepted;
        [SerializeField] private QuestEvent onDisplayQuest;
        [SerializeField] private QuestFocus_SO focus;

        private void Start()
        {
            // reset all quests
            foreach (Quest_SO quest in questSet.Items)
            {
                quest.Restore();
            }
            activeQuests.Items.Clear();
            focus.Restore();
            onQuestAccepted.RegisterListener(SetQuestActive);
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

        public void CompleteQuest()
        {
            if (focus.Get() == null)
            {
                Debug.Log("no Quest Focus");
                return;
            }
            
            focus.Get().CheckGoals();
            // if completed
            if (focus.Get().IsCompleted && activeQuests.Items.Contains(focus.Get()))
            {
                // remove all Quest Items from Inventory
                foreach (Goal goal in focus.Get().GoalList)
                {
                    IntVariable item = goal.CurrentAmount;
                    item.Set(item.Get() - goal.RequiredAmount);
                }
                
                Debug.Log("'" + focus.Get().QuestTitle + "' Completed");
                removeQuest.Raise();
            }
            else
            {
                Debug.Log("not all Quest Goals have been completed yet");
            } 
        }

    }
}
