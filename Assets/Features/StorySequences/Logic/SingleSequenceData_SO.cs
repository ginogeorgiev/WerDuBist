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
        [Space (10, order = 3)]
        
        [Header("Hier kommen alle Quests rein, die alle abgeschlossen ", order = 4)]
        [Space (-10, order = 5)]
        [Header("sein müssen, damit diese Sequenz beendet ist", order = 6)]
        [SerializeField] private List<Quest_SO> quests;
        [Space (10, order = 7)]
        
        [Header("Hier kommen alle NPC rein, die aktiv werden ", order = 8)]
        [Space (-10, order = 9)]
        [Header("sollen, wenn diese Sequenz abgeschlossen ist.", order = 10)]
        [SerializeField] private List<NPCData_SO> npcsToActivateList;
        [Space (10, order = 11)]
        
        [Header("Hier die Kreise benutzen, sollte immer nur eins möglich sein", order = 12)]
        [SerializeField] private NpcBehaviourRuntimeSet behaviourRuntimeSet;
        [Header("Hoffentlich selbsterklärend (muss selbst erstellt werden)", order = 12)]
        [SerializeField] private GameEvent_SO sequenceCompletedEvent;

        private void OnEnable()
        {
            foreach (Quest_SO quest in quests)
            {
                quest.SingleSequenceData = this;
            }
        }

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
                Debug.Log(sequenceCompletedEvent.name);
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
                foreach (NpcBehaviour npcBehaviour in npcsToActivateList.SelectMany(
                    npcData => behaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID == npcBehaviour.Data.ID)))
                {
                    npcBehaviour.gameObject.SetActive(true);
                }
            }
        }
    }
}
