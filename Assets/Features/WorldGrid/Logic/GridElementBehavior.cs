using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.WorldGrid.Logic
{
    public class GridElementBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        public GameObject Content => content;

        [SerializeField] private List<int> gridNeighbourIndices;
        public List<int> GridNeighbourIndices => gridNeighbourIndices;

        [SerializeField] private int gridIndex;
        public int GridIndex => gridIndex;

        [Tooltip("The Grid is a square")]
        [SerializeField] private IntVariable gridLengthVariable;
        [SerializeField] private IntVariable gridSizeVariable;
        private int gridLength;
        private int gridSize;

        private void Start()
        {
            //content.SetActive(false);

            gridLength = gridLengthVariable.Get();
            gridSize = gridSizeVariable.Get();

            gridNeighbourIndices = new List<int>();

            DetermineNeighbours();
        }

        private void DetermineNeighbours()
        {
            // Bottom line edge case
            if (gridIndex - gridLength - 1 >= 0 && (gridIndex - gridLength - 1) % gridLength != gridLength - 1)
            {
                gridNeighbourIndices.Add(gridIndex - gridLength - 1);
            }
            
            if (gridIndex - gridLength >= 0)
            {
                gridNeighbourIndices.Add(gridIndex - gridLength);
            }
            
            if (gridIndex - gridLength - 1 >= 0 && (gridIndex - gridLength + 1) % gridLength != 0)
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
