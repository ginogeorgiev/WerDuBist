using DataStructures.Event;
using UnityEngine;

namespace Features.WorldGrid.Logic
{
    [CreateAssetMenu(fileName = "newGridElementEnteredEvent", menuName = "Feature/WorldGrid/GridElementEnteredEvent")]
    public class GridElementEnteredEvent : ActionEventWithParameter<int>
    {
    }
}
