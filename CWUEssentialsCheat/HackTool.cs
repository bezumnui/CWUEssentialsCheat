using System;
using System.Reflection;

namespace CWUEssentialsCheat
{
    public class HackTool
    {
        public static T GetPrivateField<T>(object obj, string name)
        {
            var field = obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T) field.GetValue(obj);
        }
        public static bool SetPrivateField<T>(object obj, string name, T value)
        {
            var field = obj.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) return false;
            field.SetValue(obj, value);
            return true;

        }
    }
}