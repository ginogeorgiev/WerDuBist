using UnityEngine;

namespace Features.Player.Logic
{
    public class TeleportWayPointBehaviour : MonoBehaviour
    {
        [SerializeField] private PlayerTeleportFocus_SO playerTeleportFocus;
        void Start()
        {
            playerTeleportFocus.Set(transform);
        }
    }
}
