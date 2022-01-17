using System.Collections.Generic;
using UnityEngine;

namespace Features.Quests.Logic
{
   [CreateAssetMenu(fileName = "NewQuestSet", menuName = "Feature/Quests/QuestSet")]
   public class QuestSet_SO : ScriptableObject
   {
       [SerializeField] private List<Quest_SO> quests;
      
       public List<Quest_SO> Items => quests;
   }       
}