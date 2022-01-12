using System;
using DataStructures.Variables;
using Features.NPCs.Logic;
using UnityEngine;

namespace Features.Quests.Logic
{
    [Serializable]
    public class Goal
    {
        public enum GoalType
        {
            collect,
            talk
        }

        [SerializeField] private GoalType goalType;
        [SerializeField] private Sprite sprite;

        [Header("For Talking Goal:")] [SerializeField]
        private string npcName;

        [Header("For Collecting Goal:")] [SerializeField]
        private IntVariable current;

        [SerializeField] private int required;

        public GoalType Type => goalType;
        public Sprite GoalSprite => sprite;

        public string NpcName => npcName;

        public IntVariable CurrentAmount => current;
        public int RequiredAmount => required;

        public bool Completed { get; set; }
        
        public void Restore()
        {
            Completed = false;
        }

        public void Evaluate()
        {
            if (goalType == GoalType.collect)
            {
                Completed = CurrentAmount.Get() >= RequiredAmount;
            }
        }

        public void Evaluate(NpcFocus_So npcFocus)
        {
            if (NpcName == npcFocus.Get().Data.FullName)
            {
                Completed = true;
            }
        }
    }
}
