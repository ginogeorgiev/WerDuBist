using System.Collections.Generic;
using System.Linq;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Features.WorldGrid.Logic
{
    public class WorldGridController : MonoBehaviour
    {
        [SerializeField] private GridElementEnteredEvent onGridElementEntered;
        [FormerlySerializedAs("selectedGridIndex")] [SerializeField] private IntVariable activeGridIndex;
        
        // Those Values have to be set in the Inspector since there is no generation for the worldGrid
        [SerializeField] private IntVariable gridLengthVariable;
        [SerializeField] private IntVariable gridSizeVariable;
        
        private List<GridElementBehavior> gridElementBehaviors;

        private List<int> toDeactivateGridElements;
        private List<int> toActivateGridElements;

        private int lastIndex;

        private void Awake()
        {
            onGridElementEntered.RegisterListener(OnGridElementEntered);
            gridSizeVariable.Set(gridLengthVariable.Get() * gridLengthVariable.Get() - 1);
            gridElementBehaviors = new List<GridElementBehavior>();
        }

        private void Start()
        {
            foreach (Transform child in gameObject.transform)
            {
                gridElementBehaviors.Add(child.GetComponent<GridElementBehavior>());
            }
        }

        private void OnGridElementEntered(int index)
        {
            // lastIndex only correct if player starts in GridElement with Index 0, because default of lastIndex is 0

            // Create two Lists one with all the new GridElements, which need to be activated and one with all the last
            // GridElements which need to be deactivated. If a specific GridElement needs to stay active it will be
            // excluded from both lists and not touched by this code
            toDeactivateGridElements = gridElementBehaviors[lastIndex].GridNeighbourIndices
                .Except(gridElementBehaviors[index].GridNeighbourIndices).ToList();
            
            toActivateGridElements = gridElementBehaviors[index].GridNeighbourIndices
                .Except(gridElementBehaviors[lastIndex].GridNeighbourIndices).ToList();
            
            // Remove index from being toDeactivate List, because it was not handled by the lines above
            foreach (int toDeactivate in toDeactivateGridElements.Where(toDeactivate => toDeactivate != index))
            {
                gridElementBehaviors[toDeactivate].Content.SetActive(false);
            }
            
            foreach (int toActivate in toActivateGridElements)
            {
                gridElementBehaviors[toActivate].Content.SetActive(true);
            }

            activeGridIndex.Set(index);

            lastIndex = index;
        }
    }
}
