using UnityEngine;

namespace DataStructures.Variables
{
    [CreateAssetMenu(fileName = "NewStringVariable", menuName = "Utils/Variables/StringVariable")]
    public class StringVariable : AbstractVariable<string>
    {
        public void Append(string value)
        {
            runtimeValue += value;
            onValueChanged.Raise();
        }

        public void Append(StringVariable value)
        {
            runtimeValue += value.runtimeValue;
            onValueChanged.Raise();
        }
    }
}
