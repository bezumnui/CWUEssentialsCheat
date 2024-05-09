using UnityEngine;

namespace CWUEssentialsCheat
{
    public class Loader
    {
        private static readonly GameObject _mGameObject = new GameObject();

        public static void Load()
        {
            _mGameObject.AddComponent<HackMain>();
             Object.DontDestroyOnLoad(_mGameObject);
        }
        public static void UnLoad()
        {
            Object.Destroy(_mGameObject);
        }
    }
}