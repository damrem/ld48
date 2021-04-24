using System;
using System.Collections.Concurrent;

namespace Damrem.Pooling {

    public class Pool<T> {

        readonly ConcurrentBag<T> Objects;
        readonly Func<T> CreateObject;
        readonly Action<T> CleanObject;

        public Pool(Func<T> createObject, Action<T> cleanObject) {
            Objects = new ConcurrentBag<T>();
            CreateObject = createObject ?? throw new ArgumentNullException("createObject");
            CleanObject = cleanObject ?? throw new ArgumentNullException("cleanObject");
        }

        public T TakeObject() {
            if (Objects.TryTake(out T obj))
                return obj;

            return CreateObject();
        }

        public void PutObject(T obj) {
            CleanObject(obj);
            Objects.Add(obj);
        }
    }

    public class Pool<T, U> {

        readonly ConcurrentBag<T> Objects;
        readonly Func<U, T> CreateObject;
        readonly Action<T, U> SetupObject;
        readonly Action<T> CleanObject;

        public Pool(Func<U, T> createObject, Action<T, U> setupObject, Action<T> cleanObject = null
        ) {
            Objects = new ConcurrentBag<T>();
            CreateObject = createObject ?? throw new ArgumentNullException("createObject");
            SetupObject = setupObject ?? throw new ArgumentNullException("setupObject");
            CleanObject = cleanObject;
        }

        public T TakeObject(U arg) {
            if (Objects.TryTake(out T obj)) {
                SetupObject(obj, arg);
                return obj;
            }

            return CreateObject(arg);
        }

        public void PutObject(T obj) {
            CleanObject?.Invoke(obj);
            Objects.Add(obj);
        }
    }

    public class Pool<T, U, V> {

        readonly ConcurrentBag<T> Objects;
        readonly Func<U, V, T> CreateObject;
        readonly Action<T, U, V> SetupObject;
        readonly Action<T> CleanObject;

        public Pool(Func<U, V, T> createObject, Action<T, U, V> setupObject, Action<T> cleanObject) {
            Objects = new ConcurrentBag<T>();
            CreateObject = createObject ?? throw new ArgumentNullException("createObject");
            SetupObject = setupObject ?? throw new ArgumentNullException("setupObject");
            CleanObject = cleanObject ?? throw new ArgumentNullException("cleanObject");
        }

        public T TakeObject(U arg0, V arg1) {
            if (Objects.TryTake(out T obj)) {
                SetupObject(obj, arg0, arg1);
                return obj;
            }

            return CreateObject(arg0, arg1);
        }

        public void PutObject(T obj) {
            CleanObject(obj);
            Objects.Add(obj);
        }
    }
}