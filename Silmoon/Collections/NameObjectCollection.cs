using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace Silmoon.Collections
{
    [Serializable]
    public class NameObjectCollection : NameObjectCollectionBase
    {
        DataCacheGetEnumerator dataCacheGetEnumerator = null;
        public NameObjectCollection()
        {
            dataCacheGetEnumerator = new DataCacheGetEnumerator(this);
        }
        public void Add(string name, object value) => BaseAdd(name, value);
        public void Remove(string name) => BaseRemove(name);
        public void RemoveAt(int index) => BaseRemoveAt(index);
        public void Clear() => BaseClear();
        public object Get(int index) => BaseGet(index);
        public object Get(string name) => BaseGet(name);
        public string[] GetAllKeys() => BaseGetAllKeys();
        public object[] GetAllValues() => BaseGetAllValues();
        public object[] GetAllValues(Type type) => BaseGetAllValues(type);
        public string GetKey(int index) => BaseGetKey(index);
        public bool HasKeys() => BaseHasKeys();
        public void Set(int index, object value) => BaseSet(index, value);
        public void Set(string name, object value) => BaseSet(name, value);

        public object this[int index]
        {
            get => BaseGet(index);
            set => BaseSet(index, value);
        }
        public object this[string name]
        {
            get => BaseGet(name);
            set => BaseSet(name, value);
        }
        public override IEnumerator GetEnumerator() => dataCacheGetEnumerator;
        class DataCacheGetEnumerator : IEnumerator
        {
            #region IEnumerator 成员
            NameObjectCollection domainArray;
            int _current = -1;
            public DataCacheGetEnumerator(NameObjectCollection array)
            {
                domainArray = array;
            }
            public object Current
            {
                get
                {
                    try
                    {
                        return domainArray[_current];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public bool MoveNext()
            {
                _current++;
                return _current < domainArray.Count;
            }

            public void Reset()
            {
                _current = -1;
            }

            #endregion
        }
    }
    [Serializable]
    public class NameObjectCollection<T> : NameObjectCollectionBase
    {
        DataCacheGetEnumerator dataCacheGetEnumerator = null;
        public NameObjectCollection() => dataCacheGetEnumerator = new DataCacheGetEnumerator(this);
        public void Add(string name, T value) => BaseAdd(name, value);
        public void Remove(string name) => BaseRemove(name);
        public void RemoveAt(int index) => BaseRemoveAt(index);
        public void Clear() => BaseClear();
        public T Get(int index) => (T)BaseGet(index);
        public T Get(string name) => (T)BaseGet(name);
        public string[] GetAllKeys() => BaseGetAllKeys();
        public object[] GetAllValues() => BaseGetAllValues();
        public object[] GetAllValues(Type type) => BaseGetAllValues(type);
        public string GetKey(int index)
        => BaseGetKey(index);
        public bool HasKeys() => BaseHasKeys();
        public void Set(int index, object value) => BaseSet(index, value);
        public void Set(string name, object value) => BaseSet(name, value);

        public T this[int index]
        {
            get => (T)BaseGet(index);
            set => BaseSet(index, value);
        }
        public T this[string name]
        {
            get => (T)BaseGet(name);
            set => BaseSet(name, value);
        }
        public override IEnumerator GetEnumerator() => dataCacheGetEnumerator;
        class DataCacheGetEnumerator : IEnumerator
        {
            #region IEnumerator 成员
            NameObjectCollection<T> domainArray;
            int _current = -1;
            public DataCacheGetEnumerator(NameObjectCollection<T> array) => domainArray = array;
            public object Current
            {
                get
                {
                    try
                    {
                        return domainArray[_current];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public bool MoveNext()
            {
                _current++;
                return _current < domainArray.Count;
            }

            public void Reset() => _current = -1;

            #endregion
        }
    }
}
