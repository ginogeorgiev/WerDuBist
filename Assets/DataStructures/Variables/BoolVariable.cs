using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewBoolVariable", menuName = "DataStructures/Variables/BoolVariable")]
    public class BoolVariable : AbstractVariable<bool>
    {
        public void SetTrue()
        {
            Set(true);
            onValueChanged.Raise();
        }

        public void SetFalse()
        {
            Set(false);
            onValueChanged.Raise();
        }
    }
}