using UnityEngine;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "Feature/Dialog/Character")]
    public class Character : ScriptableObject
    {
        public string fullName;
        public Sprite portrait;
    }
}
