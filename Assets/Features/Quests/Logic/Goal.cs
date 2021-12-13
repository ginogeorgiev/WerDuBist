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
        
        public IntVariable currentAmount => current;
        public int requiredAmount => required;
        
        public Sprite goalSprite => sprite;
        public bool completed { get; set; }

        public void Evaluate()
        {
            completed = currentAmount.Get() >= requiredAmount;
        }
    }
}
