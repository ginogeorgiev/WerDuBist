using UnityEngine;

namespace Features.NPCs.Logic
{
    [CreateAssetMenu(fileName = "NewNPCData", menuName = "Feature/Dialog/NPCData")]
    public class NPCData_SO : ScriptableObject
    {
        [SerializeField] private string fullName;
        [SerializeField] private Sprite portrait;

        public string FullName => fullName;

        public Sprite Portrait => portrait;
    }
}
