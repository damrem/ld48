using System;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static class GameObjectExt {

        public static void DestroyChildren(this GameObject go) {
            for (var i = go.transform.childCount - 1; i >= 0; i--)
                go.transform.GetChild(i).gameObject.Destroy();
        }


        public static Mesh GetCombinedChildrenMeshes(this GameObject go) {
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                i++;
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine, true);
            return mesh;
        }

        public static GameObject AddChild(this GameObject target) {
            var child = new GameObject();
            child.transform.parent = target.transform;
            return child;
        }

        public static GameObject AddChild(this GameObject target, string name) {
            var child = new GameObject(name);
            child.transform.parent = target.transform;
            return child;
        }

        public static GameObject AddChild(this GameObject target, string name, params Type[] components) {
            var child = new GameObject(name, components);
            child.transform.parent = target.transform;
            return child;
        }

        public static GameObject AddChild(this GameObject target, GameObject child) {
            child.transform.parent = target.transform;
            return child;
        }

        public static GameObject AddChild(this GameObject target, Component child) {
            child.transform.parent = target.transform;
            return child.gameObject;
        }

        public static T AddChild<T>(this GameObject target, string name) where T : Component {
            return target.AddChild(name, typeof(T)).GetComponent<T>();
        }

        public static T GetOrAddComponent<T>(this GameObject entity) where T : Component {
            var component = entity.GetComponent<T>();
            if (component != null) return component;

            return entity.AddComponent<T>();
        }
    }
}

