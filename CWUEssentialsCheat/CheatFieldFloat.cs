namespace CWUEssentialsCheat
{
    public class CheatFieldFloat
    {
        public float Value;
        public string Name;
        public readonly bool AsInt;


        public void SetFloat(float v)
        {
            Value = v;
            if (AsInt)
            {
                Value = (int)Value;
            }
        }
        public CheatFieldFloat(string name, float value = 0, bool asInt = false)
        {
            Value = value;
            Name = name;
            this.AsInt = asInt;
        }

        public static implicit operator float(CheatFieldFloat v)
        {
            if (v.AsInt) return (int)v.Value;
            return v.Value;
        }
    }
}