using System.Diagnostics.CodeAnalysis;
using Damrem.Collections;
namespace UnityEngine {
    public class BehaviourOrderer : MonoBehaviour {
        [SuppressMessage("lifecycle", "IDE0051")]
        void Start() {
            MonoBehaviour[] behaviours = GetComponents<MonoBehaviour>().FindAll(behaviour => behaviour != this);
            foreach (MonoBehaviour behaviour in behaviours)
                behaviour.enabled = true;
        }
    }
}