using System;
using System.Collections.Generic;
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
            talk,
            quest
        }

        [SerializeField] private GoalType goalType;
        [SerializeField] private Sprite sprite;

        [Header("For other Quest as Goal:")] [SerializeField]
        private List<Quest_SO> otherQuests;
        
        [Header("For Talking Goal:")] [SerializeField]
        private NPCData_SO npc;

        [Header("For Collecting Goal:")] [SerializeField]
        private IntVariable current;

        [SerializeField] private int required;

        public GoalType Type => goalType;
        public Sprite GoalSprite => sprite;

        public List<Quest_SO> OtherQuests => otherQuests;
        
        public NPCData_SO Npc => npc;

        public IntVariable CurrentAmount => current;
        public int RequiredAmount => required;

        public bool Completed { get; set; }

        public void Restore()
        {
            Completed = false;
            
            switch (goalType)
            {
                case GoalType.talk when npc.Icon!=null:
                    sprite = npc.Icon;
                    break;
                case GoalType.quest:
                    required = otherQuests.Count;
                    break;
            }
        }

        public void Evaluate(NpcFocus_So npcFocus)
        {
            switch (goalType)
            {
                case GoalType.talk:
                    if (npcFocus != null)
                    {
                        if (Npc.ID == npcFocus.Get().Data.ID)
                        {
                            Completed = true;
                        }
                    } 
                    break;
                case GoalType.collect:
                    Completed = CurrentAmount.Get() >= RequiredAmount;
                    break;
                case GoalType.quest:
                    Completed = otherQuests.TrueForAll(quest => quest.IsCompleted);
                    break;
            }

        }

    }
}
