using System;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    [Serializable] public class Goal
    {
        
        [SerializeField] private string description;
        [SerializeField] private int itemID;
        [SerializeField] private IntVariable currentAmount;
        [SerializeField] private int requiredAmount;
        
        public string Description => description;
        public int ItemID => itemID;
        public IntVariable CurrentAmount => currentAmount;
        public int RequiredAmount => requiredAmount;
        public bool Completed { get; set; }
       

        public virtual void Init()
        {
            // add Collect Event
        }

        public void Evaluate()
        {
            if (CurrentAmount.Get() >= RequiredAmount)
            {
                Complete();
            }
        }

        public void Complete()
        {
            Completed = true;
        }

        void ItemCollected( /*Item*/)
        {
            // check if Item.ID is correct
            // but need ItemScript for that 
            // CurrentAmount++;
            //
        }
    }
}
