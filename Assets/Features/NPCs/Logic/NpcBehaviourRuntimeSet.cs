using System.Linq;
using DataStructures.RuntimeSet;
using UnityEngine;

namespace Features.NPCs.Logic
{
    [CreateAssetMenu(fileName = "NewNpcBehaviourRuntimeSet", menuName = "Feature/NPCs/NpcBehaviourRuntimeSet")]
    public class NpcBehaviourRuntimeSet : RuntimeSet_SO<NpcBehaviour>
    {
        private void OnDisable()
        {
            Restore();
        }

        private void OnEnable()
        {
            foreach (NpcBehaviour item in GetItems().Where(item => item == null))
            {
                GetItems().Remove(item);
            }
        }
    }
}