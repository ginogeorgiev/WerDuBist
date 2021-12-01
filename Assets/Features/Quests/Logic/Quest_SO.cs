﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Feature/Quests/Quest")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private string questID;
        [SerializeField] private string questName;
        [SerializeField] private string description;
        [SerializeField] private List<Goal> goals;
        [SerializeField] private Vector2 position;
        
        public string QuestID => questID;
        public string QuestName => questName;
        public string Description => description;
        public List<Goal> Goals => goals;
        public Vector2 Position => position;
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        
        public void CheckGoals()
        {
            foreach (var goal in Goals)
            {
                goal.Evaluate();
            }
            // quest completed if all goals are completed
            if (Goals.All(goal => goal.Completed))
            {
                IsActive = false;
                IsCompleted = true;
                Debug.Log("'" + QuestName + "' Completed");
            }
        }

        public void reset()
        {
            IsActive = false;
            IsCompleted = false;
        }
    }
}