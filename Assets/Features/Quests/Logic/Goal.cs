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

        [Header("Hier GoalType über das Dropdown auswählen", order = 1)]
        [SerializeField] private GoalType goalType;
        [Header("Icon, das neben dem Goal angezeigt werden soll", order = 2)]
        [Space(-10, order = 3)]
        [Header("(wird bei Talking Goals automatisch gesetzt)", order = 4)]
        [SerializeField] private Sprite sprite;
        [Space (20, order = 5)]
        
        [Header("Nur ausfüllen für Quest Goal", order = 6)]
        [Header("in diese Liste kommen alle Quests, die für", order = 7)]
        [Space (-10, order = 8)]
        [Header(" dieses Goal erfüllt sein müssen", order = 9)]
        [SerializeField] private List<Quest_SO> otherQuests;
        [Space (20, order = 10)]
        
        [Header("Nur ausfüllen für  Talking Goal", order = 11)] 
        [Header("Hier kommt rein, welcher NPC angesprochen werden soll", order = 12)]
        [SerializeField] private NPCData_SO npc;
        [Space (20, order = 13)]

        [Header("Nur ausfüllen für  Collecting Goal", order = 14)] 
        [Header("Hier kommt die zum Collectable", order = 15)] 
        [Space (-10, order = 16)]
        [Header(" dazugehörige IntVariable rein", order = 17)]
        [SerializeField] private IntVariable current;
        [Header("Wie viele davon gesammelt werden solln", order = 18)]
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

            if (required == 0)
            {
                required = 1;
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
