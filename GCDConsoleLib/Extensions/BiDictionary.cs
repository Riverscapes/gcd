using System;
using System.Collections;
using System.Collections.Generic;

namespace GCDConsoleLib.Extensions
{
    public class BiDictionary<T1, T2, T>: IEnumerable
    {
        private Dictionary<T1, T> _key1 = new Dictionary<T1, T>();
        private Dictionary<T2, T> _key2 = new Dictionary<T2, T>();

        public BiDictionary()
        {
            ByKey1 = new Indexer<T1, T>(_key1);
            ByKey2 = new Indexer<T2, T>(_key2);
        }

        /// <summary>
        /// The indexer is an inner class that gets used to return the correct value
        /// </summary>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        public class Indexer<T3, T>
        {
            private Dictionary<T3, T> _dictionary;
            public Indexer(Dictionary<T3, T> dictionary)
            {
                _dictionary = dictionary;
            }
            public T this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }
        }

        public bool ContainsKey(T1 t1) { return _key1.ContainsKey(t1); }
        public bool ContainsKey(T2 t2) { return _key2.ContainsKey(t2); }
        public bool ContainsValue(T t) { return _key1.ContainsValue(t); }

        public void Add(T1 t1, T2 t2, T val)
        {
            _key1.Add(t1, val);
            _key2.Add(t2, val);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Indexer<T1, T> ByKey1 { get; private set; }
        public Indexer<T2, T> ByKey2 { get; private set; }
    }
}
