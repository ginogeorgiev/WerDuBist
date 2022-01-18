using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewBoolVariable", menuName = "DataStructures/Variables/BoolVariable")]
    public class BoolVariable : AbstractVariable<bool>
    {
        public void SetTrue()
        {
            if (runtimeValue) return;
            Set(true);
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void SetFalse()
        {
            if (!runtimeValue) return;
            Set(false);
            if(onValueChanged != null) onValueChanged.Raise();
        }
    }
}