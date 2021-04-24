using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshFilter))]
public class NormalsVisualizer : Editor {

    bool shouldVisualizeNormals = false;
    private Mesh mesh;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        shouldVisualizeNormals = EditorGUILayout.Toggle("Visualize Normals", shouldVisualizeNormals);
    }

    void OnEnable() {
        MeshFilter mf = target as MeshFilter;
        if(mf != null) {
            mesh = mf.sharedMesh;
        }
    }

    void OnSceneGUI() {
        if(!shouldVisualizeNormals || mesh is null) {
            return;
        }

        for(int i = 0 ; i < mesh.vertexCount ; i++) {
            Handles.matrix = (target as MeshFilter).transform.localToWorldMatrix;
            Handles.color = Color.yellow;
            Handles.DrawLine(
                mesh.vertices[i],
                mesh.vertices[i] + mesh.normals[i]);
            var style = new GUIStyle();
            style.normal.textColor = Color.yellow;
            if(i >= mesh.uv.Length)
                return;

            Handles.Label(mesh.vertices[i] + mesh.normals[i], mesh.uv[i].ToString(), style);
        }
    }
}