namespace System.Collections.Generic {
    public static class DictionaryExt {
        public static string Stringify<K, V>(this Dictionary<K, V> dictionary) {
            string result = "";
            foreach (KeyValuePair<K, V> pair in dictionary) {
                result += pair.Key.ToString() + ": " + pair.Value.ToString() + "; ";
            }
            return result;
        }

        public static Dictionary<K, V> Clone<K, V>(this Dictionary<K, V> dictionary) {
            return new Dictionary<K, V>(dictionary);
        }

        public static void RemoveValue<K, V>(this Dictionary<K, V> dictionary, V value, bool all = true) {
            Dictionary<K, V> clone = dictionary.Clone();
            foreach (KeyValuePair<K, V> pair in clone) {
                if (pair.Value.Equals(value)) {
                    dictionary.Remove(pair.Key);
                    if (!all)
                        return;
                }
            }
        }

        public static List<K> GetKeyList<K, V>(this Dictionary<K, V> dictionary) {
            return new List<K>(dictionary.Keys);
        }

        public static List<V> GetValueList<K, V>(this Dictionary<K, V> dictionary) {
            return new List<V>(dictionary.Values);
        }

        public static void AddRange<K, V>(this Dictionary<K, V> dictionary, Dictionary<K, V> range) {
            foreach (var kv in range)
                dictionary.Add(kv.Key, kv.Value);
        }

        public static void AddToKey<K, V>(this Dictionary<K, List<V>> d, K k, V v) {
            if (d.TryGetValue(k, out List<V> values)) values.Add(v);
            else d.Add(k, new List<V> { v });

        }

        public static void AddToKeys<K, V>(this Dictionary<K, List<V>> dictionary, List<K> keys, V value) {
            foreach (K key in keys)
                dictionary.AddToKey(key, value);
        }

        public static void AddRangeToKey<K, V>(this Dictionary<K, List<V>> dictionary, K key, List<V> values) {
            foreach (var value in values)
                dictionary.AddToKey(key, value);
        }

        public static void AddRangeToKeys<K, V>(this Dictionary<K, List<V>> dictionary, List<K> keys, List<V> values) {
            foreach (K key in keys)
                dictionary.AddRangeToKey(key, values);
        }

        public static void AddToKey<K, V>(this Dictionary<K, HashSet<V>> d, K k, V v) {
            if (d.TryGetValue(k, out HashSet<V> values)) values.Add(v);
            else d.Add(k, new HashSet<V> { v });
        }

        public static void AddToKeys<K, V>(this Dictionary<K, HashSet<V>> dictionary, List<K> keys, V value) {
            foreach (K key in keys)
                dictionary.AddToKey(key, value);
        }

        public static void AddRangeToKey<K, V>(this Dictionary<K, HashSet<V>> dictionary, K key, List<V> values) {
            foreach (var value in values)
                dictionary.AddToKey(key, value);
        }

        public static void AddRangeToKeys<K, V>(this Dictionary<K, HashSet<V>> dictionary, List<K> keys, List<V> values) {
            foreach (K key in keys)
                dictionary.AddRangeToKey(key, values);
        }

        public static V GetValue<K, V>(this Dictionary<K, V> dictionary, K key) {
            if (dictionary.TryGetValue(key, out var value)) return value;

            return default;
        }
    }
}

