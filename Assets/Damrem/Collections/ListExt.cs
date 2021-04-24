using System;
using System.Collections.Generic;

namespace Damrem.Collections {
    public static class ListExt {
        public static List<T> GetFlattened<T>(this List<List<T>> listList) {
            var flat = new List<T>();
            listList.ForEach(subList => {
                subList.ForEach(item => {
                    flat.Add(item);
                });
            });
            return flat;
        }

        public static void Deduplicate<T>(this List<T> list, Func<T, T, bool> compare) {
            var deduped = new List<T>();
            foreach (var item in list) {
                if (!deduped.Exists(other => {
                    var compared = compare(item, other);
                    return compared;
                }))
                    deduped.Add(item);
            }
            list.Replace(deduped);
        }

        public static void Replace<T>(this List<T> list, List<T> other) {
            list.Clear();
            list.AddRange(other);
        }

        public static void Replace<T>(this List<T> list, T replaced, T replacer) {
            var nextList = new List<T>();
            foreach (var item in list)
                nextList.Add(item.Equals(replaced) ? replacer : item);
            list.Replace(nextList);
        }

        public static T ReplaceAt<T>(this List<T> list, int index, T replacer) {
            T replaced = list[index];
            list.Insert(index, replacer);
            list.RemoveAt(index + 1);
            return replaced;
        }

        public static T[] ToArray<T>(this List<T> list) {
            T[] array = new T[list.Count];
            for (int i = 0; i < list.Count; i++) {
                array[i] = list[i];
            }
            return array;
        }

        public static List<T> Fill<T>(this List<T> list, Func<T> filler) {
            for (int i = 0; i < list.Capacity; i++)
                list.Insert(i, filler());
            return list;
        }

        public static List<T> Fill<T>(this List<T> list, Func<T, T> filler) {
            for (int i = 0; i < list.Capacity; i++)
                list.Insert(i, filler(list[i]));
            return list;
        }

        public static List<T> Fill<T>(this List<T> list, Func<T, int, T> filler) {
            for (int i = 0; i < list.Capacity; i++)
                list.Insert(i, filler(list[i], i));
            return list;
        }

        public static List<T> Fill<T>(this List<T> list, Func<T, int, List<T>, T> filler) {
            for (int i = 0; i < list.Capacity; i++)
                list.Insert(i, filler(list[i], i, list));
            return list;
        }

        public static List<U> Map<T, U>(this List<T> list, Func<T, U> mapper) {
            int capacity = list.Count;
            var output = new List<U>(capacity);
            for (int i = 0; i < capacity; i++)
                output.Insert(i, mapper(list[i]));
            return output;
        }

        public static List<U> Map<T, U>(this List<T> list, Func<T, int, U> mapper) {
            int capacity = list.Count;
            var output = new List<U>(capacity);
            for (int i = 0; i < capacity; i++)
                output.Insert(i, mapper(list[i], i));
            return output;
        }

        public static string Stringify<T>(this List<T> list) {
            //Dbg.Log("list", list.Count);
            T[] array = list.ToArray();
            //Dbg.Log("array", array.Length);
            string[] stringArray = array.Map((T item) => (item == null) ? "null" : item.ToString());
            //Dbg.Log("stringArray", stringArray.Length);
            return stringArray.AsString(";");
        }

        public static bool ContainsAny<T>(this List<T> container, List<T> containeds) {
            for (int i = 0; i < containeds.Count; i++)
                if (container.Contains(containeds[i]))
                    return true;
            return false;
        }

        public static bool ContainsAny<T>(this List<T> container, params T[] containeds) {
            foreach (T item in containeds)
                if (container.Contains(item))
                    return true;
            return false;
        }

        [Obsolete("Use Exists instead.")]
        public static bool Any<T>(this List<T> list, Predicate<T> match) {
            for (int i = 0; i < list.Count; i++)
                if (match(list[i]))
                    return true;
            return false;
        }
        public static bool None<T>(this List<T> list, Predicate<T> match) {
            return !list.Exists(match);
        }

        public static List<T> And<T>(this List<T> list1, List<T> list2) {
            var andList = new List<T>();
            foreach (T item1 in list1)
                if (list2.Contains(item1))
                    andList.Add(item1);
            return andList;
        }

        [Obsolete("Use AsString instead.")]
        public static string Join<T>(this List<T> list, string separator = ";") {
            return list.AsString(separator);
        }

        public static string AsString<T>(this List<T> list, string separator = ";") {
            return list.ToArray().AsString(separator);
        }

        public static T Pop<T>(this List<T> list) {
            int index = list.Count - 1;
            if (index < 0) return default;

            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static void ForEach<T>(this List<T> list, Action<T, int> action) {
            for (int i = 0; i < list.Count; i++)
                action.Invoke(list[i], i);
        }

        public static List<T> Exclude<T>(this List<T> list, T excluded) {
            return list.FindAll(item => !item.Equals(excluded));
        }

        public static List<T> ExcludeFirst<T>(this List<T> list, T excluded) {
            var result = new List<T>();
            bool isFirstExcluded = false;
            foreach (var item in list) {
                if (!isFirstExcluded && excluded.Equals(item)) {
                    isFirstExcluded = true;
                    continue;
                }
                result.Add(item);
            }
            return result;
        }

        public static List<T> Exclude<T>(this List<T> list, List<T> excludeds) {
            return list.FindAll(item => !excludeds.Contains(item));
        }

        public static T MaxBy<T>(this List<T> list, Func<T, float> extractor, out float maxValue) {
            T maxItem = list[0];
            maxValue = extractor.Invoke(maxItem);
            foreach (var item in list) {
                var value = extractor.Invoke(item);
                if (value > maxValue) {
                    maxValue = value;
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public static T MaxBy<T>(this List<T> list, Func<T, float> extractor) {
            return list.MaxBy(extractor, out float _);
        }

        public static T MaxByInt<T>(this List<T> list, Func<T, int> extractor, out int maxValue) {
            T maxItem = list[0];
            maxValue = extractor.Invoke(maxItem);
            foreach (var item in list) {
                var value = extractor.Invoke(item);
                if (value > maxValue) {
                    maxValue = value;
                    maxItem = item;
                }
            }
            return maxItem;
        }

        public static T MaxByInt<T>(this List<T> list, Func<T, int> extractor) {
            return list.MaxByInt(extractor, out int _);
        }

        public static T MinBy<T>(this List<T> list, Func<T, float> extractor, out float minValue) {
            T minItem = list[0];
            minValue = extractor.Invoke(minItem);
            foreach (var item in list) {
                var value = extractor.Invoke(item);
                if (value < minValue) {
                    minValue = value;
                    minItem = item;
                }
            }
            return minItem;
        }

        public static T MinBy<T>(this List<T> list, Func<T, float> extractor) {
            return list.MinBy(extractor, out float _);
        }

        public static T MinByInt<T>(this List<T> list, Func<T, int> extractor, out int minValue) {
            T minItem = list[0];
            minValue = extractor.Invoke(minItem);
            foreach (var item in list) {
                var value = extractor.Invoke(item);
                if (value < minValue) {
                    minValue = value;
                    minItem = item;
                }
            }
            return minItem;
        }

        public static T MinByInt<T>(this List<T> list, Func<T, int> extractor) {
            return list.MinByInt(extractor, out int _);
        }

        public static T Last<T>(this List<T> list) {
            return list[list.Count - 1];
        }

        public static T Shift<T>(this List<T> list) {
            var item = list[0];
            list.RemoveAt(0);
            return item;
        }

        public static List<int> FindAllIndexes<T>(this List<T> list, Predicate<T> match) {
            return list.FindAll(match).Map(item => list.IndexOf(item));
        }
    }


}
