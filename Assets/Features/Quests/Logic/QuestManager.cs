using DataStructures.Variables;
using UnityEngine;

namespace Features.Quests.Logic
{
    public class QuestManager : MonoBehaviour
    {

        public void thisIsTheEvent()
        {
            Debug.Log("Item Collected");
        }

        public void ItemCollected(IntVariable item)
        {
            item.Set(item.Get()+1);
        }
        
        
        
    }
}
