using System.Collections.Generic;
using UnityEngine;

namespace Features.Quests.Logic
{
   [CreateAssetMenu(fileName = "NewQuestSet", menuName = "Feature/Quests/QuestSet")]
           public class QuestSet_SO : ScriptableObject
           {
               [SerializeField] private List<Quest_SO> items;
   
               public IEnumerable<Quest_SO> Items => items;
           }
}