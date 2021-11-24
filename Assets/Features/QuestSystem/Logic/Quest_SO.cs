using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.QuestSystem.Logic
{
    [CreateAssetMenu(fileName = "NewQuest", menuName = "Feature/QuestSystem/Quests")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string description;
        [SerializeField] private List<Goal> goals;
        public string QuestName => questName;
        public string Description => description;
        public IEnumerable<Goal> Goals => goals;
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        
        
        public void CheckGoals()
        {
            if (Goals.All(goal => goal.Completed))
            {
                IsActive = false;
                IsCompleted = true;
            }
            
        }
    }
}