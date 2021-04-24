using UnityEngine;

namespace Damrem.UnityEngine {

    public class CombineChildMeshes : MonoBehaviour {

        public void Init() {
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length) {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);

                i++;
            }


            var meshFilter = transform.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = new Mesh();
            meshFilter.mesh.CombineMeshes(combine, true);
        }
    }
}