using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewStringVariable", menuName = "DataStructures/Variables/StringVariable")]
    public class StringVariable : AbstractVariable<string>
    {
        public void Append(string value)
        {
            runtimeValue += value;
            if(onValueChanged != null) onValueChanged.Raise();
        }

        public void Append(StringVariable value)
        {
            runtimeValue += value.runtimeValue;
            if(onValueChanged != null) onValueChanged.Raise();
        }
    }
}
