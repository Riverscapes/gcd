using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GCDConsoleLib.Extensions
{
    public class BiDictionary<T1, T2, T> : IEnumerable
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
        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;
            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            public T4 this[T3 index]
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
            _key1[t1] = val;
            _key2[t2] = val;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public List<T1> Keys1 { get { return _key1.Keys.ToList(); } }
        public List<T2> Keys2 { get { return _key2.Keys.ToList(); } }
        public List<T> Values { get { return _key1.Values.ToList(); } }

        public Indexer<T1, T> ByKey1 { get; private set; }
        public Indexer<T2, T> ByKey2 { get; private set; }

        public int Count { get { return _key1.Count;  } }

        /// <summary>
        /// Return the two indeces from a value
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public Tuple<T1, T2> GetIndecesByValue(T val)
        {
            return new Tuple<T1, T2>(
                _key1.FirstOrDefault(x => x.Value.Equals(val)).Key,
                _key2.FirstOrDefault(x => x.Value.Equals(val)).Key);
        }
    }
}
