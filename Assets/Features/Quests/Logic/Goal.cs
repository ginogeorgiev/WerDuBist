using System;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    [Serializable] public class Goal
    {
        [SerializeField] private IntVariable currentAmount;
        [SerializeField] private int requiredAmount;
        
        public IntVariable CurrentAmount => currentAmount;
        public int RequiredAmount => requiredAmount;
        public bool Completed { get; set; }

        public void Evaluate()
        {
            Completed = CurrentAmount.Get() >= RequiredAmount;
        }
    }
}
