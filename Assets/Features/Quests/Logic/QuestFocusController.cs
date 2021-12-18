using DataStructures.Event;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestFocusController : MonoBehaviour
    {
        [SerializeField] private QuestFocus_SO focus;
        [SerializeField] private GameEvent_SO onQuestFocusUpdated;

        public Quest_SO quest { get; set; }

        public void FocusOnQuest()
        {
            focus.Set(quest);
            Debug.Log("Focus on: " + quest.questID);
            onQuestFocusUpdated.Raise();
        }
    }
}
