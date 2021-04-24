using System.Linq;
using UnityEngine;

namespace Damrem.UnityEngine {
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
        static T _instance = null;
        public static T Single {
            get {
                if (!_instance)
                    _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
                return _instance;
            }
        }
    }
}