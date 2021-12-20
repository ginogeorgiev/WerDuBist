using System;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    [Serializable] public class Goal
    {
        [SerializeField] private IntVariable current;
        [SerializeField] private int required;
        [SerializeField] private Sprite sprite;
        
        public IntVariable CurrentAmount => current;
        public int RequiredAmount => required;
        
        public Sprite GoalSprite => sprite;
        public bool Completed { get; set; }

        public void Evaluate()
        {
            Completed = CurrentAmount.Get() >= RequiredAmount;
        }
    }
}
