using Damrem.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Procedural {
    public class PointFactory {
        readonly PRNG PRNG;
        public PointFactory(int seed) {
            PRNG = new PRNG(seed);
        }

        Vector2 CreateRandomPoint(Rect plotBounds) {
            float x = PRNG.Float(plotBounds.xMin, plotBounds.xMax);
            float y = PRNG.Float(plotBounds.yMin, plotBounds.yMax);
            return new Vector2(x, y);
        }

        // public List<Vector2> CreatePoints(int count, Rect plotBounds) {
        //     List<Vector2> points = new List<Vector2>(count);
        //     points.Fill(() => CreateRandomPoint(plotBounds));
        //     return points;
        // }

        public Vector2[] CreatePoints(int count, Rect plotBounds) {
            Vector2[] points = new Vector2[count];
            points.Fill(() => CreateRandomPoint(plotBounds));
            return points;
        }

        [Obsolete("should not be part of Damrem.Procedural.")]
        public List<Vector2> CreateSquareGrid(Vector2Int gridSize, Rect plotBounds) {
            float columnWidth = plotBounds.width / gridSize.x;
            float halfColumnWidth = columnWidth / 2;

            float rowHeight = plotBounds.height / gridSize.y;
            float halfRowHeight = rowHeight / 2;

            List<Vector2> points = new List<Vector2>(gridSize.x * gridSize.y);
            for (float x = plotBounds.xMin + halfColumnWidth; x < plotBounds.xMax; x += columnWidth) {
                for (float y = plotBounds.yMin + halfRowHeight; y < plotBounds.yMax; y += rowHeight) {
                    points.Add(new Vector2(x, y));
                }
            }
            return points;
        }

        [Obsolete("should not be part of Damrem.Procedural.")]
        public List<Vector2> CreateHexGrid(float radius, Rect plotBounds) {
            float w = radius * 2;
            float h = radius * Mathf.Sqrt(3);
            float xDistance = w * 3 / 4;
            float yDistance = h;
            List<Vector2> points = new List<Vector2>();
            for (float y = plotBounds.yMin; y < plotBounds.yMax; y += yDistance) {
                for (float x = plotBounds.xMin; x < plotBounds.xMax; x += xDistance) {
                    bool isEven = (x - plotBounds.xMin) % (2 * xDistance) == 0;
                    float actualY = isEven ? y : y + h / 2;
                    points.Add(new Vector2(x, actualY));
                }
            }
            return points;

        }

    }
}