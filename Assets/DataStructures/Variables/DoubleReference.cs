using System;

namespace DataStructures.Variables
{
    [Serializable]
    public class DoubleReference : AbstractReference<double>
    {
        public DoubleReference(double value) : base(value)
        {
        }
    }
}
