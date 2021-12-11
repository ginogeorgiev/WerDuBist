using System.Collections.Generic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.WorldGrid.Logic
{
    public class WorldGridController : MonoBehaviour
    {
        private List<GridElementBehavior> gridElementBehaviors;

        [SerializeField] private GridElementEnteredEvent onGridElementEntered;
        [SerializeField] private IntVariable selectedGridIndex;
        
        

        [Tooltip("The Grid is a square")]
        [SerializeField] private IntVariable gridLengthVariable;
        [SerializeField] private IntVariable gridSizeVariable;

        private int lastIndex;

        private void Awake()
        {
            onGridElementEntered.RegisterListener(OnEnterGridElement);
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

        private void OnEnterGridElement(int index)
        {
            gridElementBehaviors[lastIndex].Content.SetActive(false);
            
            selectedGridIndex.Set(index);
            gridElementBehaviors[index].Content.SetActive(true);

            lastIndex = index;
        }
        
        private void OnExitGridElement()
        {
            
        }
    }
}
