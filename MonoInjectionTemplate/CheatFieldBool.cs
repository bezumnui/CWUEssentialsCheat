namespace MonoInjectionTemplate
{
    public class CheatFieldBool
    {
        public bool Value;
        public string Name;


        public void SetActive(bool v)
        {
            Value = v;
        }
        public CheatFieldBool(string name, bool value = false)
        {
            Value = value;
            Name = name;
        }

        public static implicit operator bool(CheatFieldBool v)
        {
            return v.Value;
        }
    }
}