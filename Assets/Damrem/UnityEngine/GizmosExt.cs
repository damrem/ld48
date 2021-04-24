using UnityEditor;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static partial class GizmosExt {
        public static void DrawDottedLine(Vector3 from, Vector3 to, float period = .1f) {
            Vector3 total = to - from;
            Vector3 step = total.normalized * period;
            int dotCount = Mathf.CeilToInt(total.magnitude / period);
            Vector3 current = from;
            for (int i = 0; i < dotCount; i++) {
                if (i % 2 == 0)
                    Gizmos.DrawLine(current, current + step);
                current += step;
            }
        }

        public static void DrawDottedLine(Vector3 from, Vector3 to, float full, float empty) {
            Vector3 total = to - from;
            var period = full + empty;
            Vector3 fullStep = total.normalized * full;
            Vector3 periodStep = total.normalized * period;
            int dotCount = Mathf.CeilToInt(total.magnitude / period);
            Vector3 current = from;
            for (int i = 0; i < dotCount; i++) {
                if (i % 2 == 0)
                    Gizmos.DrawLine(current, current + fullStep);
                current += periodStep;
            }
        }

        public static void DrawDottedGrid(Rect bound, float period, float y = 0) {
            for (float x = bound.xMin; x <= bound.xMax; x += period) {
                DrawDottedLine(new Vector3(x, y, bound.yMin), new Vector3(x, y, bound.yMax), .1f);
            }

            for (float z = bound.yMin; z <= bound.yMax; z += period) {
                DrawDottedLine(new Vector3(bound.xMin, y, z), new Vector3(bound.xMax, y, z), .1f);
            }
        }

        public static void DrawScaledLine(Vector3 from, Vector3 to, float scale) {
            Vector3Ext.Scale(from, to, out Vector3 scaledFrom, out Vector3 scaledTo, scale);
            Gizmos.DrawLine(scaledFrom, scaledTo);
        }

        public static void DrawScaledDottedLine(Vector3 from, Vector3 to, float scale, float period) {
            Vector3Ext.Scale(from, to, out Vector3 scaledFrom, out Vector3 scaledTo, scale);
            DrawDottedLine(scaledFrom, scaledTo, period);
        }

        public static void DrawScaledDottedLine(Vector3 from, Vector3 to, float scale, float full, float empty) {
            Vector3Ext.Scale(from, to, out Vector3 scaledFrom, out Vector3 scaledTo, scale);
            DrawDottedLine(scaledFrom, scaledTo, full, empty);
        }

        public static void DrawString(string text, Vector3 worldPos, Color? colour = null) {
            Handles.BeginGUI();
            if (colour.HasValue)
                GUI.color = colour.Value;
            var view = SceneView.currentDrawingSceneView;
            if (view is null) return;

            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height, size.x, size.y), text);
            Handles.EndGUI();
        }
    }
}
