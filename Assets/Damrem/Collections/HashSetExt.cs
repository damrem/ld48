using System;
using System.Collections.Generic;

namespace Damrem.Collections {
    public static class HashSetExt {

        public static HashSet<U> Map<T, U>(this HashSet<T> hashSet, Func<T, U> mapper) {
            var output = new HashSet<U>();
            foreach (var item in hashSet) output.Add(mapper(item));
            return output;
        }

        public static HashSet<T> Filter<T>(this HashSet<T> hashSet, Func<T, bool> callback) {
            var filtered = new HashSet<T>();
            foreach (var item in hashSet)
                if (callback(item))
                    filtered.Add(item);
            return filtered;
        }

        public static void Replace<T>(this HashSet<T> set, HashSet<T> other) {
            set.Clear();
            set.UnionWith(other);
        }

        public static T GetByHashCode<T>(this HashSet<T> set, int hashCode) {
            foreach (var item in set)
                if (item.GetHashCode() == hashCode)
                    return item;

            return default;
        }
    }
}