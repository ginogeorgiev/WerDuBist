using DataStructures.Event;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestFocusController : MonoBehaviour
    {
        [SerializeField] public Quest_SO quest;
        [SerializeField] private QuestFocus_SO focus;
        public GameEvent_SO onQuestFocusUpdated;
        
        public void FocusOnQuest()
        {
            focus.focus = quest;
            Debug.Log("Focus on: " + quest.QuestID);
            onQuestFocusUpdated.Raise();
        }
    }
}
