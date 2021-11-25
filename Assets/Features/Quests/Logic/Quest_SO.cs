using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Feature/Quests/Quest")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string description;
        [SerializeField] private List<Goal> goals;
        
        public string QuestName => questName;
        public string Description => description;
        public List<Goal> Goals => goals;
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }

        private void Reset()
        {
            IsActive = false;
            IsCompleted = false;
        }

        public void CheckGoals()
        {
            foreach (var goal in Goals)
            {
                goal.Evaluate();
            }
            if (Goals.All(goal => goal.Completed))
            {
                IsActive = false;
                IsCompleted = true;
                Debug.Log("'" + QuestName + "' Completed");
            }
            
        }
    }
}