using Damrem.UnityEngine;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Geom2D {
    public class Hexagon : Polygon {
        public Hexagon(Vector2 center, float outerRadius, float firstAngle = 0) {
            Vector2 currentPos = Vector2.right * outerRadius;
            currentPos.Rotate(firstAngle);
            for (int i = 0; i < 6; i++) {
                Vertices.Add(Vertex.Pool.TakeObject(center + currentPos));
                currentPos = currentPos.Rotate(60);
            }
        }

        public List<Triangle> Triangles {
            get {
                var list = new List<Triangle>();
                var centroid = GetCentroid();
                for (int i = 0; i < 6; i++) {
                    var j = (i + 1) % 6;
                    var a = Vertex.Pool.TakeObject(centroid);
                    var b = Vertices[i];
                    var c = Vertices[j];
                    list.Add(new Triangle(new List<Vertex>() { a, b, c }));
                }
                return list;
            }
        }
    }
}
