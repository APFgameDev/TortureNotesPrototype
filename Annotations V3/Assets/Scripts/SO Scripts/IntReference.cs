namespace Brinx.SO
{
    [System.Serializable]
    public class IntReference
    {
        /// <summary>Whether to use an editor-assigned constant or a ScriptableObject variable</summary>
        public bool UseVariable = false;
        /// <summary>Constant value. Primarily used for testing and debuging</summary>
        public int ConstantValue;
        /// <summary>Linked ScriptableObject variable</summary>
        public IntVariable Variable;

        /// <summary>Both Get and Set affect ConstantValue or Variable.Value, depending on UseVariable</summary>
        public int Value
        {
            get
            {
                if (!UseVariable || (!UnityEngine.Application.isPlaying && Variable == null))
                    return ConstantValue;
                else
                    return Variable.Value;
            }
            set
            {
                if (!UseVariable || (!UnityEngine.Application.isPlaying && Variable == null))
                    ConstantValue = value;
                else
                    Variable.Value = value;
            }
        }

        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }
}