using UnityEngine;

namespace Features.Map.Logic
{
    public class MapIconController  : MonoBehaviour
    {
        [SerializeField] private GameObject playerMarker;
        
        public void resizeIcon()
        {
            playerMarker.transform.localScale = new Vector3(8, 8, 1);
        }
    }
}