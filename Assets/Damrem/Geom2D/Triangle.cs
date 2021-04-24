using Damrem.Pooling;
using Damrem.Collections;
using System.Collections.Generic;
using System;

namespace Damrem.Geom2D {
    [Serializable]
    public class Triangle : Polygon {
        public Vertex A { get { return Vertices[0]; } }
        public Vertex B { get { return Vertices[1]; } }
        public Vertex C { get { return Vertices[2]; } }

        public Triangle(List<Vertex> corners) : base(corners, 3) { }

        override public string ToString() {
            return ToString("Triangle");
        }

        public static Pool<Triangle, List<Vertex>> pool = new Pool<Triangle, List<Vertex>>(
            corners => new Triangle(corners),
            (t, corners) => { t.Vertices.Replace(corners); }
        );
    }
}
