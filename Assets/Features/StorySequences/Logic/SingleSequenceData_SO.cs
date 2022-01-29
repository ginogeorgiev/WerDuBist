using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.Event;
using Features.NPCs.Logic;
using Features.Quests.Logic;
using UnityEngine;

namespace Features.StorySequences.Logic
{
    [CreateAssetMenu(fileName = "NewSingleSequenceData", menuName = "Feature/StorySequence/SingleSequenceData")]
    public class SingleSequenceData_SO : ScriptableObject
    {
        [Header("Hier kommen alle NPC rein, bei denen die Conversation ", order = 0)]
        [Space (-10, order = 1)]
        [Header("jetzt weiter geht wenn diese Sequenz abgeschlossen ist.", order = 2)]
        [SerializeField] private List<NPCData_SO> npcsToAdvanceConversationsList;
        
        [Header("Hier kommen alle Quests rein, die alle abgeschlossen ", order = 4)]
        [Space (-10, order = 5)]
        [Header("sein müssen, damit diese Sequenz beendet ist", order = 6)]
        [SerializeField] private List<Quest_SO> quests;
        
        [Header("Hier kommen alle NPC rein, die aktiv werden ", order = 8)]
        [Space (-10, order = 9)]
        [Header("sollen, wenn diese Sequenz abgeschlossen ist.", order = 10)]
        [SerializeField] private List<NPCData_SO> npcsToActivateList;
        
        [Header("Hier kommen alle NPC rein, die inaktiv werden ", order = 12)]
        [Space (-10, order = 13)]
        [Header("sollen, wenn diese Sequenz abgeschlossen ist.", order = 14)]
        [SerializeField] private List<NPCData_SO> npcsToDeactivateList;

        [Header("Hier kommen alle Quests rein, die unlocked werden ", order = 16)]
        [Space (-10, order = 17)]
        [Header("sollen, wenn diese Sequenz abgeschlossen ist.", order = 18)]
        [SerializeField] private List<Quest_SO> questsToUnlockList;
        [SerializeField] private QuestEvent onQuestUnlocked;
        
        [Header("Hier die Kreise benutzen, sollte immer nur eins möglich sein", order = 12)]
        [SerializeField] private NpcBehaviourRuntimeSet behaviourRuntimeSet;
        [Header("Hoffentlich selbsterklärend (muss selbst erstellt werden)", order = 21)]
        [SerializeField] private GameEvent_SO sequenceCompletedEvent;

        [SerializeField] private QuestSet_SO activeQuests;
        [SerializeField] private Quest_SO luisArmaband;
        [SerializeField] private NPCData_SO luis;
        [SerializeField] private NPCData_SO luisEnd;

        public NPCData_SO LuisEnd => luisEnd;

        public List<NPCData_SO> NpcsToAdvanceConversationsList => npcsToAdvanceConversationsList;

        public List<Quest_SO> Quests => quests;

        public List<NPCData_SO> NpcsToActivateList => npcsToActivateList;

        public List<Quest_SO> QuestsToUnlockList => questsToUnlockList;

        public QuestEvent ONQuestUnlocked => onQuestUnlocked;

        public GameEvent_SO SequenceCompletedEvent => sequenceCompletedEvent;

        public void CheckForNextSequence()
        {
            bool allComplete = true;

            if (quests.Count == 0 || quests == null)
            {
                Debug.LogWarning("Es wurden keine Quests für diese Sequenz gesetzt");
                return;
            }
            
            foreach (Quest_SO quest in quests.Where(quest => !quest.IsCompleted))
            {
                allComplete = false;
            }

            if (allComplete)
            {
                // Debug.Log(sequenceCompletedEvent.name);
                sequenceCompletedEvent.Raise();
                OnSequenceCompleted();
            }
        }

        private void OnSequenceCompleted()
        {
            if (npcsToAdvanceConversationsList.Count != 0)
            {
                foreach (NpcBehaviour npcBehaviour in npcsToAdvanceConversationsList.SelectMany(
                    npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
                {
                    npcBehaviour.AdvanceConvIndex();
                }
            }

            if (npcsToActivateList.Count != 0)
            {
                foreach (NpcBehaviour npcBehaviour in npcsToActivateList.SelectMany
                    (npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
                {
                    npcBehaviour.gameObject.SetActive(true);
                }
            }
            
            if (npcsToDeactivateList.Count != 0)
            {
                foreach (NpcBehaviour npcBehaviour in npcsToDeactivateList.SelectMany
                    (npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
                {
                    npcBehaviour.gameObject.SetActive(false);
                }
            }
            
            if (questsToUnlockList.Count != 0)
            {
                foreach (Quest_SO quest in questsToUnlockList)
                {
                    onQuestUnlocked.Raise(quest);
                }
            }

            if (luis == null || luisEnd == null || activeQuests == null || luisArmaband == null) return;
            {
                // Debug.Log(!activeQuests.Items.Contains(luisArmaband));
                if (!activeQuests.Items.Contains(luisArmaband)) return;
                foreach (NpcBehaviour npcBehaviour in behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcBehaviour.Data.ID == luis.ID))
                {
                    npcBehaviour.gameObject.SetActive(false);
                }

                foreach (NpcBehaviour npcBehaviour in behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcBehaviour.Data.ID == luisEnd.ID))
                {
                    npcBehaviour.gameObject.SetActive(true);
                }
            }
        }
    }
}
