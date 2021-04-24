using Damrem.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.UnityEngine {
    public class InvalidVertexCountException : Exception {
        public InvalidVertexCountException(int vertexCount) : base($"An array of exactly {vertexCount} vertices.") { }
    }

    public static class MeshExt {
        public static void Extrude(this Mesh mesh, float length, Transform transform, int sectionCount = 1, bool invertFaces = false) {
            MeshExtrusion.Edge[] precomEdges = MeshExtrusion.BuildManifoldEdges(mesh);

            Matrix4x4[] sections = new Matrix4x4[sectionCount + 1];

            float subLength = length / sectionCount;

            sections[0] = transform.worldToLocalMatrix * Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
            for (int i = 1; i <= sectionCount; i++) {
                sections[i] = transform.worldToLocalMatrix * Matrix4x4.TRS(transform.position + subLength * Vector3.up, Quaternion.identity, Vector3.one);
            }

            MeshExtrusion.ExtrudeMesh(mesh, mesh, sections, precomEdges, invertFaces);
        }

        public static void BuildTriangle(this Mesh mesh, Vector3[] vertices, Vector3 normal) {
            if (vertices.Length != 3)
                throw new InvalidVertexCountException(3);

            mesh.vertices = vertices;
            mesh.triangles = new int[3] { 0, 1, 2 };
            mesh.normals = new Vector3[3] { normal, normal, normal };
            mesh.uv = vertices.Map(v => v.ToVector2());
        }

        public static void BuildTriangle(this Mesh mesh, Vector3[] vertices) {
            var a = vertices[0];
            var b = vertices[1];
            var c = vertices[2];
            var normal = Vector3.Cross(b - a, c - a);
            mesh.BuildTriangle(vertices, normal);
        }

        public static void BuildQuad(this Mesh mesh, Vector3[] vertices, Vector3 normal) {
            if (vertices.Length != 4)
                throw new InvalidVertexCountException(4);

            int[] triangles = new int[6] { 0, 1, 2, 0, 2, 3 };
            Vector3[] normals = new Vector3[4] { normal, normal, normal, normal };
            Vector2[] uv = new Vector2[4] {
                Vector2.zero,
                Vector2.right,
                Vector2.right+Vector2.up,
                Vector2.up,
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            mesh.uv = uv;
        }

        public static void BuildQuad(this Mesh mesh, Vector3[] vertices) {
            if (vertices.Length != 4)
                throw new InvalidVertexCountException(4);

            var a = vertices[0];
            var b = vertices[1];
            var d = vertices[3];

            var normal = Vector3.Cross(b - a, d - a);

            mesh.BuildQuad(vertices, normal);
        }

        public static void BuildConvexPolygon(this Mesh mesh, Vector3[] vertices, Vector3 normal) {
            var v3s = new List<Vector3>(vertices);
            v3s.Add(v3s.GetCentroid());

            List<int> triangles = new List<int>();
            vertices.ToList().ForEach((v, i) => {
                triangles.Add(i);
                triangles.Add((i + 1) % vertices.Length);
                triangles.Add(v3s.Count - 1);
            });

            var normals = new Vector3[v3s.Count];
            normals.Fill(() => normal);

            var uv = v3s.Map(v => v.ToVector2()).ToArray();

            mesh.vertices = v3s.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals;
            mesh.uv = uv;
        }

        public static void BuildConvexPolygon(this Mesh mesh, Vector3[] vertices) {
            var a = vertices[0];
            var b = vertices[1];
            var c = vertices.ToList().GetCentroid();
            var normal = Vector3.Cross(a - c, b - c);
            mesh.BuildConvexPolygon(vertices, normal);
        }

        public static void BuildDisk(this Mesh mesh, Vector3 center, float radius = 1, float sliceCount = 6) {
            var angle = 360 / sliceCount;
            var v2s = new List<Vector2>();
            var v = Vector2.right * radius;
            var center2 = center.ToVector2();
            for (var i = 0; i < sliceCount; i++) {
                var angleShift = v.Rotate(i * -angle);
                v2s.Add(center2 + angleShift);
            }
            var v3s = v2s.Map(v2 => v2.ToVector3(center.y)).ToArray();
            mesh.BuildConvexPolygon(v3s);
        }
    }
}