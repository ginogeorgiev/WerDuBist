using UnityEngine;

namespace Features.NPCs.Logic
{
    [CreateAssetMenu(fileName = "NewNPCData", menuName = "Feature/NPCs/NPCData")]
    public class NPCData_SO : ScriptableObject
    {
        [SerializeField] private int id;
        [SerializeField] private string fullName;
        [SerializeField] private Sprite portrait;
        [SerializeField] private Sprite icon;

        public int ID => id;
        public string FullName => fullName;
        public Sprite Portrait => portrait;
        public Sprite Icon => icon;


    }
    
}
