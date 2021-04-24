using System;
using System.Collections.Generic;

namespace Damrem.Collections {

    public static class IEnumerableExt {
        [Obsolete("Crashy.")]
        public static int GetCount<T>(this IEnumerable<T> enumerable) {
            int count = 0;
            while (enumerable.GetEnumerator().MoveNext()) count++;
            return count;
        }
    }
}