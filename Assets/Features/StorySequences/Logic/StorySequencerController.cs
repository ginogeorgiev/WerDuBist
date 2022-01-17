using System.Collections.Generic;
using System.Linq;
using Features.NPCs.Logic;
using Features.Quests.Logic;
using UnityEngine;

namespace Features.StorySequences.Logic
{
    public class StorySequencerController : MonoBehaviour
    {
        [Header("Hier müssen alle existierenden Sequenzen rein!")]
        [SerializeField] private List<SingleSequenceData_SO> sequences;
        [SerializeField] private NpcBehaviourRuntimeSet npcBehaviourRuntimeSet;

        private void Start()
        {
            foreach (SingleSequenceData_SO sequence in sequences)
            {
                // Adds sequence to each of its quests accordingly
                foreach (Quest_SO quest in sequence.Quests)
                {
                    quest.SingleSequenceData = sequence;
                }
                
                // deactivates all npc from all sequences if they are part of the NpcsToActivateList
                if (sequence.NpcsToActivateList.Count == 0) continue;
                
                foreach (NpcBehaviour npcBehaviour in sequence.NpcsToActivateList.SelectMany
                    (npcData => npcBehaviourRuntimeSet.GetItems().Where(npcBehaviour => npcData.ID.Equals(npcBehaviour.Data.ID))))
                {
                    npcBehaviour.gameObject.SetActive(false);
                    Debug.Log(npcBehaviour.Data.name + " deactivated ");
                }
            }
        }
    }
}