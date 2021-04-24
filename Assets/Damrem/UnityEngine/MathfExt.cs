using Damrem.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.UnityEngine {
    public static class MathfExt {
        public static float Sum(this IEnumerable<float> numbers) {
            float sum = 0;
            foreach (var n in numbers) sum += n;
            return sum;
        }

        public static int Sum(this IEnumerable<int> numbers) {
            int sum = 0;
            foreach (var n in numbers) sum += n;
            return sum;
        }

        public static float Round(float number, float precision = .001f) {
            return Mathf.Round(number / precision) * precision;
        }
    }
}
