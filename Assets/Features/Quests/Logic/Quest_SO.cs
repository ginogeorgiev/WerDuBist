using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Feature/Quests/Quest")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private List<Goal> goals;
        [SerializeField] private Vector2 position;
        
        public int questID => id;
        public string questTitle => title;
        public string questDescription => description;
        public List<Goal> goalList => goals;
        public Vector2 questPosition => position;
        public bool isActive { get; set; }
        public bool isCompleted { get; private set; }
        
        public void CheckGoals()
        {
            foreach (var goal in goalList)
            {
                goal.Evaluate();
            }
            // quest completed if all goals are completed
            if (!goalList.All(goal => goal.completed)) return;
            isActive = false;
            isCompleted = true;
            Debug.Log("'" + questTitle + "' Completed");
        }

        public void Restore()
        {
            isActive = false;
            isCompleted = false;
        }
    }
}