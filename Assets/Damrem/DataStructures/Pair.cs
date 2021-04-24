using Damrem.System;
using System;
using System.Collections.Generic;

namespace Damrem.DataStructures {

    [Serializable]
    public class Pair<T, U> : IEquatable<Pair<T, U>> {

        public T A;
        public U B;

        public Pair(T a, U b) {
            A = a;
            B = b;
        }

        public override string ToString() {
            return "[Pair ( a=" + A.ToString() + " ; b=" + B.ToString() + ")]";
        }

        public override bool Equals(object obj) {
            return Equals(obj as Pair<T, U>);
        }

        public bool Equals(Pair<T, U> other) {
            return (A.Equals(other.A) && B.Equals(other.B)) || (A.Equals(other.B) && B.Equals(other.A));
        }

        public override int GetHashCode() {
            return HashCodeHelper.CombineHashCodes(A, B);
        }

    }

    [Serializable]
    public class Pair<T> : Pair<T, T> {
        public Pair(T a, T b) : base(a, b) { }

        public bool Contains(T t) {
            return A.Equals(t) || B.Equals(t);
        }

        public T Other(T t) {
            if (A.Equals(t)) return B;
            if (B.Equals(t)) return A;
            return default;
        }

        public bool Any(Func<T, bool> predicate) {
            return predicate(A) || predicate(B);
        }

        public bool Both(Func<T, bool> predicate) {
            return predicate(A) && predicate(B);
        }

        public Pair<U> Map<U>(Func<T, U> mapper) {
            return new Pair<U>(mapper(A), mapper(B));
        }

        public override string ToString() {
            return $"Pair(A={A}, B={B})";
        }

        public T[] ToArray() {
            return new T[2] { A, B };
        }
    }

    public static class PairExt {
        public static T FirstCommonMember<T>(this List<Pair<T>> pairs) {
            var members = new HashSet<T>();
            foreach (var pair in pairs) {
                if (members.Contains(pair.A)) return pair.A;
                if (members.Contains(pair.B)) return pair.B;
                members.Add(pair.A);
                members.Add(pair.B);
            }
            return default;
        }
    }
}
