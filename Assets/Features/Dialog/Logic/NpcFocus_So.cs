using DataStructures.Focus;
using UnityEngine;

namespace Features.Dialog.Logic
{
    [CreateAssetMenu(fileName = "NewNpcFocus", menuName = "Feature/Dialog/NpcFocus")]
    public class NpcFocus_So : Focus_SO<NpcBehaviour>
    {
        private void OnEnable()
        {
            Restore();
        }
        
    }
}