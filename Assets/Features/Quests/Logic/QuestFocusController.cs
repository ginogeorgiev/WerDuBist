using DataStructures.Event;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestFocusController : MonoBehaviour
    {
        [SerializeField] private QuestFocus_SO focus;

        public Quest_SO Quest { get; set; }

        public void FocusOnQuest()
        {
            focus.Set(Quest);
        }
    }
}
