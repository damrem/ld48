using System;
using System.Collections.Generic;

namespace Damrem.Collections {
    public static class ArrayExt {
        public static U[] Map<T, U>(this T[] array, Func<T, U> callback) {
            int length = array.Length;

            var output = new U[length];

            for (int i = 0; i < length; i++)
                output[i] = callback(array[i]);

            return output;
        }

        public static U[] Map<T, U>(this T[] array, Func<T, int, U> callback) {
            int length = array.Length;

            var output = new U[length];

            for (int i = 0; i < length; i++)
                output[i] = callback(array[i], i);

            return output;
        }

        [Obsolete("Use AsString(string separator = \",\") instead.")]
        public static string Join<T>(this T[] array, string separator = ",") {
            return array.AsString(separator);
        }

        public static string AsString<T>(this T[] array, string separator = ",") {
            if (array.Length == 0) return "EMPTY";

            string r = "";
            foreach (T item in array) {
                r += item?.ToString() + separator;
            }
            r = r.Remove(r.Length - separator.Length);
            return r;
        }

        public static T[] FindAll<T>(this T[] array, Predicate<T> match) {
            return array.ToList().FindAll(match).ToArray();
        }

        public static int Max(this int[] array) {
            int max = array[0];
            foreach (int val in array) {
                if (val > max)
                    max = val;
            }
            return max;
        }

        public static int Min(this int[] array) {
            int min = array[0];
            foreach (int val in array) {
                if (val < min)
                    min = val;
            }
            return min;
        }

        public static List<T> ToList<T>(this T[] array) {
            return new List<T>(array);
        }

        public static T[] Unshifted<T>(this T[] array, params T[] items) {
            T[] unshifted = new T[array.Length + items.Length];
            for (int i = 0; i < items.Length; i++) {
                unshifted[i] = items[i];
            }
            for (int i = 0; i < array.Length; i++) {
                unshifted[items.Length + i] = array[i];
            }
            return unshifted;
        }

        public static T[] GetRange<T>(this T[] array, int index, int count) {
            return array.ToList().GetRange(index, count).ToArray();
        }

        public static void Fill<T>(this T[] array, Func<T> filler) {
            for (int i = 0; i < array.Length; i++)
                array[i] = filler();
        }

        public static void Fill<T>(this T[] array, Func<T, T> filler) {
            for (int i = 0; i < array.Length; i++)
                array[i] = filler(array[i]);
        }

        public static void Fill<T>(this T[] array, Func<T, int, T> filler) {
            for (int i = 0; i < array.Length; i++)
                array[i] = filler(array[i], i);
        }

        public static void Fill<T>(this T[] array, Func<T, int, T[], T> filler) {
            for (int i = 0; i < array.Length; i++)
                array[i] = filler(array[i], i, array);
        }

        public static bool Any<T>(this T[] array, Func<T, bool> callback) {
            for (int i = 0; i < array.Length; i++)
                if (callback(array[i]))
                    return true;
            return false;
        }
        public static bool None<T>(this T[] array, Func<T, bool> callback) {
            return !array.Any(callback);
        }

    }
}
