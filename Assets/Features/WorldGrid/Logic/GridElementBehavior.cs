using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.WorldGrid.Logic
{
    public class GridElementBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject content;
        [SerializeField] private List<Vector2> gridNeighbourPosition;
        [Tooltip("The Grid is a square")]
        [SerializeField] private IntVariable gridSizeVariable;
        private int gridSize;
        [SerializeField] private Vector2 gridPosition;

        private void Awake()
        {
            content.SetActive(false);

            gridSize = gridSizeVariable.Get();

            gridPosition.x = (int)(transform.position.x / 10);
            gridPosition.y = (int)(transform.position.y / 10);
            
            gridNeighbourPosition = new List<Vector2>();

            if (gridPosition.x - 1 >= 1 && gridPosition.y + 1 <= gridSize)
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x - 1, gridPosition.y + 1));
            }

            if (gridPosition.y + 1 <= gridSize)
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x, gridPosition.y + 1));
            }

            if (gridPosition.x + 1 <= gridSize && gridPosition.y + 1 <= gridSize)
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x + 1, gridPosition.y + 1));
            }

            if (gridPosition.x - 1 >= 1)
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x - 1, gridPosition.y));
            }

            if (gridPosition.x + 1 <= gridSize)
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x + 1, gridPosition.y));
            }

            if (gridPosition.x - 1 >= 1 && gridPosition.y - 1 >= 1 )
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x - 1, gridPosition.y - 1));  
            }

            if (gridPosition.y - 1 >= 1)
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x, gridPosition.y - 1));
            }

            if (gridPosition.x + 1 <= gridSize && gridPosition.y - 1 >= 1 )
            {
                gridNeighbourPosition.Add(new Vector2(gridPosition.x + 1, gridPosition.y - 1)); 
            }
        }
    }
}
