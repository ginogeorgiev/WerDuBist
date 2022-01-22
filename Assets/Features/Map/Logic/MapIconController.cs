using UnityEngine;

namespace Features.Map.Logic
{
    public class MapIconController  : MonoBehaviour
    {
        [SerializeField] private GameObject playerMarker;
        private bool isIslandSwitched=false;
        
        public void resizeIcon()
        {
            if (isIslandSwitched) return;
            playerMarker.transform.localScale = new Vector3(8, 8, 1);
            isIslandSwitched = true;
        }
    }
}