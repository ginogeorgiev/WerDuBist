﻿using System;
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
        private Quest_SO otherQuest;
        
        [Header("For Talking Goal:")] [SerializeField]
        private NPCData_SO npc;

        [Header("For Collecting Goal:")] [SerializeField]
        private IntVariable current;

        [SerializeField] private int required;

        public GoalType Type => goalType;
        public Sprite GoalSprite => sprite;

        public Quest_SO OtherQuest => otherQuest;
        
        public NPCData_SO Npc => npc;

        public IntVariable CurrentAmount => current;
        public int RequiredAmount => required;

        public bool Completed { get; set; }

        public void Restore()
        {
            Completed = false;
            
            if (goalType == GoalType.talk)
            {
                sprite = npc.Icon;
            }
        }

        public void Evaluate()
        {
            Completed = goalType switch
            {
                GoalType.collect => CurrentAmount.Get() >= RequiredAmount,
                GoalType.quest => otherQuest.IsCompleted,
                _ => Completed
            };
        }

        public void Evaluate(NpcFocus_So npcFocus)
        {
            if (Npc.ID == npcFocus.Get().Data.ID)
            {
                Completed = true;
            }
        }
    }
}
