using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Grids {
    public class HexagonalGrid {
        public static List<Vector2> CreatePoints(float size, Rect plotBounds) {
            float w = size * 2;
            float h = size * Mathf.Sqrt(3);
            float xDistance = w * 3 / 4;
            float yDistance = h;
            var points = new List<Vector2>();
            for (float y = plotBounds.yMin; y < plotBounds.yMax; y += yDistance) {
                for (float x = plotBounds.xMin; x < plotBounds.xMax; x += xDistance) {
                    bool isEven = (x - plotBounds.xMin) % (2 * xDistance) == 0;
                    float actualY = isEven ? y : y + h / 2;
                    points.Add(new Vector2(x, actualY));
                }
            }
            return points;

        }

        public static List<Vector2> CreatePoints(float size, int radius) {
            float w = size * 2;
            float h = size * Mathf.Sqrt(3);
            float xDistance = w * 3 / 4;
            float yDistance = h;
            var points = new List<Vector2>();

            for (int i = 0; i < radius; i++) {

            }

            //for(float y = plotBounds.yMin ; y < plotBounds.yMax ; y += yDistance) {
            //    for(float x = plotBounds.xMin ; x < plotBounds.xMax ; x += xDistance) {
            //        bool isEven = (x - plotBounds.xMin) % (2 * xDistance) == 0;
            //        float actualY = isEven ? y : y + h / 2;
            //        points.Add(new Vector2(x, actualY));
            //    }
            //}
            return points;

        }
    }
}
