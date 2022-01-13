using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "newQuest", menuName = "Feature/Quests/Quest")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private List<Goal> goals;
        [SerializeField] private Vector2 position;
        [Tooltip("visible=true means the Quest will be displayed on the QuestUI and Map")]
        [SerializeField] private bool visible;
        
        public int QuestID => id;
        public string QuestTitle => title;
        public string QuestDescription => description;
        public List<Goal> GoalList => goals;
        public Vector2 QuestPosition => position;
        public bool Visible => visible;
        
        public bool IsUnlocked { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        
        public bool CheckGoals()
        {
            foreach (var goal in GoalList)
            {
                goal.Evaluate();
            }
            // quest completed if all goals are completed
            return GoalList.All(goal => goal.Completed);
        }

        public void Restore()
        {
            IsUnlocked = false;
            IsActive = false;
            IsCompleted = false;
            foreach (var goal in goals)
            {
                goal.Restore();
            }
        }
    }
}