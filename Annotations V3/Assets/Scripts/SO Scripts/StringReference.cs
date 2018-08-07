namespace Annotation.SO
{
    [System.Serializable]
    public class StringReference
    {
        /// <summary>Whether to use an editor-assigned constant or a ScriptableObject variable</summary>
        public bool UseVariable = false;
        /// <summary>Constant value. Primarily used for testing and debuging</summary>
        public string ConstantValue = string.Empty;
        /// <summary>Linked ScriptableObject variable</summary>
        public StringVariable Variable;

        /// <summary>Both Get and Set affect ConstantValue or Variable.Value, depending on UseVariable</summary>
        public string Value
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

        public static implicit operator string(StringReference reference)
        {
            return reference.Value;
        }
    }
}