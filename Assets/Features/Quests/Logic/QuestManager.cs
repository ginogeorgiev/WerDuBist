using System.Linq;
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
            foreach (var quest in questSet.items)
            {
                quest.Restore();
            }
            activeQuests.items.Clear();
            focus.Restore();
            onQuestAccepted.RegisterListener(SetQuestActive);
        }

        public void SetQuestActive(Quest_SO quest)
        {
           // check if not already active
           if (quest.isActive || quest.isCompleted)
           {
               return;
           }
           
           activeQuests.items.Add(quest);
           quest.isActive = true;
           onDisplayQuest.Raise(quest);
        }

        public void ItemCollected(IntVariable item)
        {
            // Items only get collected if there is a active Quest
            if (activeQuests.items.Any())
            {
                item.Add(1);
            }
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
            if (focus.Get().isCompleted && activeQuests.items.Contains(focus.Get()))
            {
                // remove all Quest Items from Inventory
                foreach (var goal in focus.Get().goalList)
                {
                    var item = goal.currentAmount;
                    item.Set(item.Get() - goal.requiredAmount);
                }
                removeQuest.Raise();
            }
            else
            {
                Debug.Log("not all Quest Goals have been completed yet");
            } 
        }

    }
}
