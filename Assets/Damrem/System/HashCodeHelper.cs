using System.Collections.Generic;

namespace Damrem.System {
    public class HashCodeHelper {
        public static int CombineHashCodes(int h1, int h2) {
            return (((h1 << 5) + h1) ^ h2);
        }

        public static int CombineHashCodes(object o1, object o2) {
            return CombineHashCodes(o1.GetHashCode(), o2.GetHashCode());
        }

        public static int CombineHashCodes<T>(List<T> objects) {
            int h = 0;
            foreach (object o in objects)
                h = CombineHashCodes(h, o.GetHashCode());
            return h;
        }
    }
}