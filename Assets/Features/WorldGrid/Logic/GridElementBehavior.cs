using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.WorldGrid.Logic
{
    public class GridElementBehavior : MonoBehaviour
    {
        // The Content gameObject will be the Container for all the gameObjects which will represent the World
        // eg.: NPCs, Collectables and landscapes such as Foliage or GroundTextures
        
        [SerializeField] private GameObject content;
        public GameObject Content => content;

        [SerializeField] private List<int> gridNeighbourIndices;
        public IEnumerable<int> GridNeighbourIndices => gridNeighbourIndices;

        [SerializeField] private int gridIndex;
        public int GridIndex => gridIndex;

        [SerializeField] private bool isStartGrid;

        [Tooltip("The Grid is a square")]
        [SerializeField] private IntVariable gridLengthVariable;
        [SerializeField] private IntVariable gridSizeVariable;
        private int gridLength;
        private int gridSize;

        private void Start()
        {
            if (!isStartGrid)
            {
                content.SetActive(false);
            }

            gridLength = gridLengthVariable.Get();
            gridSize = gridSizeVariable.Get();

            gridNeighbourIndices = new List<int>();

            DetermineNeighbours();
        }

        private void DetermineNeighbours()
        {
            // Edge case handling for each specific Edge and Corner of the worldGrid:
            // Since the System is index based and not Vec2/Position based the '%' operator is used frequently
            // to determine if a specific gridElement is on the Edge and therefore does not assign the neighbour from a
            // different line
            
            // Bottom line edge case
            if (gridIndex - gridLength - 1 >= 0 && (gridIndex - gridLength - 1) % gridLength != gridLength - 1)
            {
                gridNeighbourIndices.Add(gridIndex - gridLength - 1);
            }
            
            if (gridIndex - gridLength >= 0)
            {
                gridNeighbourIndices.Add(gridIndex - gridLength);
            }
            
            if (gridIndex - gridLength + 1 >= 0 && (gridIndex - gridLength + 1) % gridLength != 0)
            {
                gridNeighbourIndices.Add(gridIndex - gridLength + 1);
            }
            
            // Middle line edge case
            if (gridIndex - 1 >= 0 && (gridIndex - 1) % gridLength != gridLength - 1)
            {
                gridNeighbourIndices.Add(gridIndex - 1);
            }
            
            if (gridIndex + 1 <= gridSize && (gridIndex + 1) % gridLength != 0)
            {
                gridNeighbourIndices.Add(gridIndex + 1);
            }
            
            // Top line edge case
            if (gridIndex + gridLength - 1 <= gridSize && (gridIndex + gridLength - 1) % gridLength != gridLength - 1)
            {
                gridNeighbourIndices.Add(gridIndex + gridLength - 1);
            }
            
            if (gridIndex + gridLength <= gridSize)
            {
                gridNeighbourIndices.Add(gridIndex + gridLength);
            }
            
            if (gridIndex + gridLength + 1 <= gridSize && (gridIndex + gridLength + 1) % gridLength != 0)
            {
                gridNeighbourIndices.Add(gridIndex + gridLength + 1);
            }
        }
    }
}
