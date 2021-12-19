using System.Collections.Generic;
using UnityEngine;

namespace Features.Quests.Logic
{
   [CreateAssetMenu(fileName = "NewQuestSet", menuName = "Feature/Quests/QuestSet")]
   public class QuestSet_SO : ScriptableObject
   {
       // class holds all existing quests
       [SerializeField] private List<Quest_SO> existingQuests;
      
       public IEnumerable<Quest_SO> Items => existingQuests;
   }       
}