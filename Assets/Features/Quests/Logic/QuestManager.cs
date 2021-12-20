using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestSet_SO questSet;
        [SerializeField] private QuestSetActive_SO activeQuests;
        [SerializeField] private QuestEvent onQuestAccepted;
        [SerializeField] private QuestEvent onDisplayQuest;
        [SerializeField] private QuestEvent onCompleteQuest;
        [SerializeField] private QuestEvent onRemoveQuest;
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
            onCompleteQuest.RegisterListener(CompleteQuest);
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

        public void CompleteQuest(Quest_SO quest)
        {
            if (!activeQuests.Items.Contains(quest))
            {
                Debug.Log("No Quest accepted yet");
                return;
            }
            
            quest.CheckGoals();
            // if completed
            if (quest.IsCompleted)
            {
                // remove all Quest Items from Inventory
                foreach (Goal goal in quest.GoalList)
                {
                    var item = goal.CurrentAmount;
                    item.Add(-goal.RequiredAmount);
                    // item.Set(item.Get() - goal.RequiredAmount);
                }
                
                Debug.Log("'" + quest.QuestTitle + "' Completed");
                onRemoveQuest.Raise(quest);
            }
            else
            {
                Debug.Log("not all Quest Goals have been completed yet");
            } 
        }

    }
}
