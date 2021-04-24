using Damrem.System;
using System;
using UnityEngine;

namespace Damrem.Procedural {
    public class PerlinGenerator {
        readonly PRNG PRNG;
        float OffsetX;
        float OffsetY;
        float Scale;

        void Construct(float offsetX, float offsetY, float ratio) {
            ValidateRatio(ratio);
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            Scale = 1 / ratio;
        }

        public PerlinGenerator(float offsetX, float offsetY, float ratio) {
            Construct(offsetX, offsetY, ratio);
        }

        public PerlinGenerator(float ratio, Rect bounds) {
            PRNG = new PRNG();
            ConstructOffsets(bounds, out float offsetX, out float offsetY);
            Construct(offsetX, offsetY, ratio);
        }

        public PerlinGenerator(float ratio, Rect bounds, int seed = 0) {
            PRNG = new PRNG(seed);
            ConstructOffsets(bounds, out float offsetX, out float offsetY);
            Construct(offsetX, offsetY, ratio);
        }

        void ConstructOffsets(Rect bounds, out float offsetX, out float offsetY) {
            offsetX = PRNG.Float(bounds.xMin, bounds.xMax);
            offsetY = PRNG.Float(bounds.yMin, bounds.yMax);
        }

        void ValidateRatio(float ratio) {
            if (ratio == 0 || ratio == 0f) throw new Exception("Ratio cannot be 0.");
        }

        public float GetValueAt(float x, float y) {
            return Mathf.PerlinNoise(x * Scale + OffsetX, y * Scale + OffsetY);
        }

        public float GetValueAt(Vector2 v) {
            return GetValueAt(v.x, v.y);
        }
    }

}