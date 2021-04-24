using System;

namespace Damrem.Geom2D {
    public class InvalidVertexCountException : Exception {
        public InvalidVertexCountException() : base("Invalid corner count.") { }
        public InvalidVertexCountException(int count) : base($"Invalid corner count: {count}.") { }
    }
}
