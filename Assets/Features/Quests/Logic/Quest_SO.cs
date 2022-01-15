using System.Collections.Generic;
using System.Linq;
using Features.NPCs.Logic;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "newQuest", menuName = "Feature/Quests/Quest")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string title;
        [SerializeField] private string description;
        [Tooltip("visible=true means the Quest will be displayed on the QuestUI and Map")]
        [SerializeField] private bool visible;
        [SerializeField] private List<Goal> goals;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 endPosition;
        
        
        public int QuestID => id;
        public string QuestTitle => title;
        public string QuestDescription => description;
        public List<Goal> GoalList => goals;
        public Vector2 StartPosition
        {
            get => startPosition;
            set => startPosition = value;
        }
        public Vector2 EndPosition
        {
            get => endPosition;
            set => endPosition = value;
        }

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
        
        public bool CheckGoals(NpcFocus_So  npcId)
        {
            foreach (var goal in GoalList)
            {
                goal.Evaluate(npcId);
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