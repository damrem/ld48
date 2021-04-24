using Damrem.Collections;
using Random = System.Random;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Damrem.Procedural {

    [Serializable]
    public class PRNG : Random {
        public PRNG() : base() { }

        public PRNG(int seed) : base(seed) { }

        public int Int(int min, int max) {
            return Next(min, max);
        }

        public int Int(int max) {
            return Next(0, max);
        }

        public double Double(double min = 0, double max = 1) {
            return min + NextDouble() * (max - min);
        }

        public float Float(float min = 0, float max = 1) {
            return (float)Double(min, max);
        }

        public float SignedFloat() {
            return Float(-1, 1);
        }

        public bool Bool(float probability = 0.5f) {
            return NextDouble() < Mathf.Clamp01(probability);
        }

        public T InArray<T>(T[] array) {
            return InArray(array, out var _);
        }

        public T InArray<T>(T[] array, out int index) {
            index = Int(0, array.Length);
            return array[index];
        }

        public T InArray<T>(T[,] array) {
            return array[Int(0, array.GetLength(0)), Int(0, array.GetLength(1))];
        }

        public T InList<T>(List<T> list) {
            return InArray(list.ToArray());
        }

        public T InList<T>(List<T> list, out int index) {
            return InArray(list.ToArray(), out index);
        }

        public T InEnumerable<T>(IEnumerable<T> enumerable) {
            List<T> list = new List<T>(enumerable);
            return InList(list);
        }

        public T InEnumerable<T>(IEnumerable<T> enumerable, out int index) {
            List<T> list = new List<T>(enumerable);
            return InList(list, out index);
        }

        public Vector2 Vector2(float magnitude = 1) {
            return new Vector2(SignedFloat(), SignedFloat()).normalized * magnitude;
        }

        public T[] Shuffle<T>(T[] array, int count = -1) {
            if (count <= 0 || count > array.Length) count = array.Length;

            var result = new T[count];
            array.GetRange(0, count).CopyTo(result, 0);
            for (int i = 0; i < result.Length; i++) {
                T tmp = result[i];
                int r = Int(i, result.Length);
                result[i] = result[r];
                result[r] = tmp;
                if (count > 0 && i >= count - 1) break;
            }
            return result;
        }

        public List<T> Shuffle<T>(List<T> list, int count = -1) {
            return Shuffle(list.ToArray(), count).ToList();
        }
    }
}
