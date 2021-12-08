using System.Collections.Generic;
using UnityEngine;

namespace Features.WorldGrid.Logic
{
    public class WorldGridController : MonoBehaviour
    {
        private List<GridElementBehavior> gridElementBehaviors;

        [SerializeField] private GridElementEnteredEvent onGridElementEntered;

        private void Awake()
        {
            onGridElementEntered.RegisterListener(OnEnterGridElement);
        }

        private void Start()
        {
            gridElementBehaviors = new List<GridElementBehavior>();
            
            foreach (Transform child in gameObject.transform)
            {
                gridElementBehaviors.Add(child.GetComponent<GridElementBehavior>());
            }
        }

        private void OnEnterGridElement(int index)
        {
            
        }
        
        private void OnExitGridElement()
        {
            
        }
    }
}
