using System;
using UnityEngine;
using UnityEditor;

namespace Damrem.Grids.Irregular {

    [RequireComponent(typeof(GridEntity))]
    public class GridGenerator : MonoBehaviour {

        internal double GeneratedInSecs { get; private set; }
        internal IrregularQuadGrid Grid {
            get { return GetComponent<GridEntity>().Grid; }
            set { GetComponent<GridEntity>().Grid = value; }
        }
        internal event Action OnGenerated;

        [SerializeField] public GenerationDef GenerationDef;
        [SerializeField] public BaseTriangleGridHexagonalDef HexagonalDef;
        [SerializeField] public BaseTriangleGridHiveDef HiveDef;
        [SerializeField] public BaseTriangleGridRectangularDef RectangularDef;
        [SerializeField] public RelaxationDef RelaxationDef;

        Relaxation Relaxation;
        DateTime StartDateTime;
        ITriangleGrid TriangleGrid = null;

        internal void Generate() {
            StartDateTime = DateTime.Now;
            InitGrid();
        }

        Relaxation GetRelaxation() {
            if (Relaxation != null) Relaxation.Clear();
            Relaxation = new Relaxation(Grid, RelaxationDef.Type/*, GetComponent<RelaxationAnimator>()*/);
            Relaxation.OnRelaxed += OnRelaxed;
            return Relaxation;
        }

        void InitGrid() {
            switch (GenerationDef.BaseTriangleGridType) {
                default:
                case BaseTriangleGridType.Hexagonal:
                    TriangleGrid = new HexagonalTriangleGrid(Vector2.zero, HexagonalDef.Radius, HexagonalDef.SubdivisionCount);
                    break;

                case BaseTriangleGridType.Hive:
                    TriangleGrid = new MultiHexagonalTriangleGrid(HiveDef.CellRadius, HiveDef.CellSubdivisionCount, HiveDef.Bound);
                    break;

                case BaseTriangleGridType.Rectangular:
                    TriangleGrid = new TriangulatedQuadGrid(RectangularDef.Bound, RectangularDef.Subdivision, GenerationDef.Seed);
                    break;
            }
            Grid = new IrregularQuadGrid(TriangleGrid, GenerationDef.Seed, this);
        }

        internal void Clear(bool totally = false) {
            Grid?.Clear(totally);
        }

        void OnDestroy() {
            Clear(true);
        }

        internal void OnRelaxed() {
            Grid.ComputeGeometryAndNeighboring();
            Grid.ComputeBorders();

            OnGenerated?.Invoke();
            GeneratedInSecs = (DateTime.Now - StartDateTime).TotalSeconds;

            GetComponent<GridEntity>().Grid = Grid;
        }

        internal void Subquadrangulate() {
            Grid.SubQuadrangulate();
        }

        internal void Relax() {
            GetRelaxation().Relax(RelaxationDef.Strength, RelaxationDef.IterationCount, RelaxationDef.FreezeBorders);
        }
    }

    [CustomEditor(typeof(GridGenerator))]
    public class GridGeneratorEditor : Editor {

        GridGenerator GridGenerator;
        void OnEnable() {
            GridGenerator = (GridGenerator)target;
        }

        public override void OnInspectorGUI() {
            DrawGenerationInspector();

            GUI.enabled = GridGenerator.Grid != null;

            if (GUILayout.Button("Subquadrangulate"))
                GridGenerator.Subquadrangulate();

            DrawRelaxationInspector();

            GUI.enabled = true;

            if (GUILayout.Button("Clear"))
                GridGenerator.Clear(true);


            EditorGUILayout.LabelField($"Generated in {GridGenerator.GeneratedInSecs} secs.");

            serializedObject.ApplyModifiedProperties();
        }

        void DrawGenerationInspector() {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenerationDef"));

            string propertyRadix;
            switch (GridGenerator.GenerationDef.BaseTriangleGridType) {
                default:
                case BaseTriangleGridType.Hive:
                    propertyRadix = "Hive";
                    break;

                case BaseTriangleGridType.Hexagonal:
                    propertyRadix = "Hexagonal";
                    break;

                case BaseTriangleGridType.Rectangular:
                    propertyRadix = "Rectangular";
                    break;
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty($"{propertyRadix}Def"));

            if (GUILayout.Button("Generate"))
                GridGenerator.Generate();
        }

        void DrawRelaxationInspector() {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("RelaxationDef"));

            var relaxDef = GridGenerator.RelaxationDef;
            if (GUILayout.Button($"Relax {relaxDef.IterationCount}x {relaxDef.Strength}"))
                GridGenerator.Relax();
        }

    }


}
