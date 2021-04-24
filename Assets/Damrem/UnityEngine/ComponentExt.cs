using UnityEngine;

namespace Damrem.UnityEngine {
    public static class ComponentExt {
        public static T GetOrAddComponent<T>(this Component context) where T : Component {
            var component = context.GetComponent<T>();
            if (component != null) return component;

            return context.gameObject.AddComponent<T>();
        }

        public static GameObject AddChild(this Component component, string name) {
            return component.gameObject.AddChild(name);
        }
    }
}