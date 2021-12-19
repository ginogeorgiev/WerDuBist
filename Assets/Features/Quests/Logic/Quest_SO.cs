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
        
        public int QuestID => id;
        public string QuestTitle => title;
        public string QuestDescription => description;
        public List<Goal> GoalList => goals;
        public Vector2 QuestPosition => position;
        public bool IsActive { get; set; }
        public bool IsCompleted { get; private set; }
        
        public void CheckGoals()
        {
            foreach (Goal goal in GoalList)
            {
                goal.Evaluate();
            }
            // quest completed if all goals are completed
            if (!GoalList.All(goal => goal.Completed)) return;
            IsActive = false;
            IsCompleted = true;
        }

        public void Restore()
        {
            IsActive = false;
            IsCompleted = false;
        }
    }
}