using Damrem.Collections;
using Damrem.UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Damrem.Grids.Irregular {

    public class GridEntity : MonoBehaviour {

        [SerializeField] public GridSO GridSO = null;
        internal IrregularQuadGrid Grid { get; set; }

        [SerializeField] string Path = null;
        [SerializeField] string FileName = "Grid";

        void OnValidate() {
            if (GridSO is null) return;

            Load();
        }

        internal void Load() {
            Load(GridSO);
        }

        internal void Load(GridSO gridSO) {
            Grid = gridSO.ToGrid();
        }

        //string GenerateUniqueAssetPath

        internal void Save() {
            if (GridSO is null) {
                GridSO = ScriptableObject.CreateInstance<GridSO>();
                string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"Assets/{Path}/{FileName}.asset");
                AssetDatabase.CreateAsset(GridSO, assetPathAndName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = GridSO;
            }

            GridSO.SetGrid(Grid);
            EditorUtility.SetDirty(GridSO);
            AssetDatabase.SaveAssets();
        }

        internal void Clear() {
            Grid.Clear();
            Grid = null;
        }
    }

    [CustomEditor(typeof(GridEntity))]
    public class GridEntityEditor : Editor {

        GridEntity GridEntity { get { return (GridEntity)target; } }
        IrregularQuadGrid Grid { get { return GridEntity.Grid; } }
        GridSO GridSO { get { return GridEntity.GridSO; } }

        public override void OnInspectorGUI() {
            DrawCustomInspector();

            Grid.DrawGUIInfo();
        }

        void DrawCustomInspector() {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("GridSO"));

            if (GridSO is null) {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Path"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FileName"));
            }

            serializedObject.ApplyModifiedProperties();

            GUI.enabled = GridSO != null;
            if (GUILayout.Button("Load")) GridEntity.Load();

            GUI.enabled = Grid != null;
            if (GUILayout.Button("Save")) GridEntity.Save();
            if (GUILayout.Button("Clear")) GridEntity.Clear();

            GUI.enabled = true;
        }

        void OnSceneGUI() {
            //HandleRemoveQuadButton();
        }

        void HandleRemoveQuadButton() {
            //Event evt = Event.current;
            //Vector2 mousePos = HandleUtility.GUIPointToWorldRay(evt.mousePosition).origin;
            //mousePos = GridEntity.transform.InverseTransformPoint(mousePos);
            //if(evt.type == EventType.MouseDown && evt.button == MouseButton.LEFT) {

            //}

            GridEntity?.Grid?.Quadrangles?.ToList().ForEach((q, i) => {
                if (Handles.Button(q.GetCentroid().ToVector3(), Quaternion.identity, .5f, .5f, Handles.CubeHandleCap)) {
                    Grid.RemoveQuad(q);
                }
                //Handles.DotHandleCap(i, q.GetCentroid().ToVector3(), Quaternion.identity, .25f, EventType.Repaint);
            });
        }
    }

    public static class GridExt {
        public static void DrawGUIInfo(this IrregularQuadGrid grid) {
            if (grid is null) {
                EditorGUILayout.LabelField("No Grid");
                return;
            }

            var vertices = grid.Vertices;
            EditorGUILayout.LabelField($"{(vertices is null ? "No" : vertices.Count.ToString())} Vertices");

            var quadrangles = grid.Quadrangles;
            EditorGUILayout.LabelField($"{(quadrangles is null ? "No" : quadrangles.Count.ToString())} Quadrangles");

            var sides = grid.Edges;
            EditorGUILayout.LabelField($"{(sides is null ? "No" : sides.Count.ToString())} Sides");

            var neighborings = grid.Neighborings;
            EditorGUILayout.LabelField($"{(neighborings is null ? "No" : neighborings.Count.ToString())} Neighborings");

            EditorGUILayout.LabelField($"Average Edge Size: {grid.AverageEdgeSize}");
        }
    }
}