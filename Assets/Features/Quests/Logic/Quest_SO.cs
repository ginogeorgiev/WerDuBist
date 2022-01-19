using System;
using System.Collections.Generic;
using System.Linq;
using Features.NPCs.Logic;
using Features.StorySequences.Logic;
using UnityEngine;

namespace Features.Quests.Logic
{
    [CreateAssetMenu(fileName = "newQuest", menuName = "Feature/Quests/Quest")]
    public class Quest_SO : ScriptableObject
    {
        [SerializeField] private int id;
        [Header("Hier kommen alle NPC rein, bei denen die Conversation ", order = 0)]
        [Space (-10, order = 1)]
        [Header("jetzt weiter geht wenn diese Quest abgeschlossen ist.", order = 2)]
        [SerializeField] private List<NPCData_SO> npcsToAdvanceConversationsList;
        [Space (10, order = 3)]
        
        [SerializeField] private string title;
        [SerializeField] private string description;
        [Header("Der Harken muss hier gesetzt werden, wenn die Quests", order = 4)]
        [Space (-10, order = 5)]
        [Header("im QuestUI und auf der Map angezeigt werden soll", order = 6)]
        [SerializeField] private bool visible;
        [Header("Durch das + können hier Goals erstellt werden", order = 7)]
        [SerializeField] private List<Goal> goals;
        [SerializeField] private Vector2 startPosition;
        [SerializeField] private Vector2 endPosition;
        [Header("Hier muss einfach das _QuestSet mit rein", order = 8)]
        [Space (-10, order = 9)]
        [Header("(das erste, wenn man auf den Kreis klickt)", order = 10)]
        [SerializeField] private QuestSet_SO questSet;
        
        public int QuestID => id;
        
        public SingleSequenceData_SO SingleSequenceData { get; set; }

        public List<NPCData_SO> NpcsToAdvanceConversationsList => npcsToAdvanceConversationsList;
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

        private void OnEnable()
        {
            if (questSet.Items.Contains(this)) return;
                questSet.Items.Add(this);
        }

        public bool CheckGoals(NpcFocus_So  npcId)
        {
            foreach (var goal in GoalList)
            {
                switch (goal.Type)
                {
                    case Goal.GoalType.talk:
                        if (npcId != null)
                        {
                            goal.Evaluate(npcId);
                        }
                        break;
                    case Goal.GoalType.collect:
                        goal.Evaluate(null);
                        break;
                    case Goal.GoalType.quest:
                        goal.Evaluate(null);
                        break;
                }
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