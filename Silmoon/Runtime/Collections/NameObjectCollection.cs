﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace Silmoon.Runtime.Collections
{
    [Serializable]
    public class NameObjectCollection : NameObjectCollectionBase
    {
        DataCacheGetEnumerator dataCacheGetEnumerator = null;
        public NameObjectCollection()
        {
            dataCacheGetEnumerator = new DataCacheGetEnumerator(this);
        }
        public void Add(string name, object value)
        {
            base.BaseAdd(name, value);
        }
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }
        public void Clear()
        {
            base.BaseClear();
        }
        public object Get(int index)
        {
            return base.BaseGet(index);
        }
        public object Get(string name)
        {
            return base.BaseGet(name);
        }
        public string[] GetAllKeys()
        {
            return base.BaseGetAllKeys();
        }
        public object[] GetAllValues()
        {
            return base.BaseGetAllValues();
        }
        public object[] GetAllValues(Type type)
        {
            return base.BaseGetAllValues(type);
        }
        public string GetKey(int index)
        {
            return base.BaseGetKey(index);
        }
        public bool HasKeys()
        {
            return base.BaseHasKeys();
        }
        public void Set(int index, object value)
        {
            base.BaseSet(index, value);
        }
        public void Set(string name, object value)
        {
            base.BaseSet(name, value);
        }

        public object this[int index]
        {
            get
            {
                return base.BaseGet(index);
            }
            set
            {
                base.BaseSet(index, value);
            }
        }
        public object this[string name]
        {
            get
            {
                return base.BaseGet(name);
            }
            set
            {
                base.BaseSet(name, value);
            }
        }
        public override IEnumerator GetEnumerator()
        {
            return dataCacheGetEnumerator;
        }
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
                return (_current < domainArray.Count);
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
        DataCacheGetEnumerator ie = null;
        public NameObjectCollection()
        {
            ie = new DataCacheGetEnumerator(this);
        }
        public void Add(string name, T value)
        {
            base.BaseAdd(name, value);
        }
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }
        public void RemoveAt(int index)
        {
            base.BaseRemoveAt(index);
        }
        public void Clear()
        {
            base.BaseClear();
        }
        public T Get(int index)
        {
            return (T)base.BaseGet(index);
        }
        public T Get(string name)
        {
            return (T)base.BaseGet(name);
        }
        public string[] GetAllKeys()
        {
            return base.BaseGetAllKeys();
        }
        public object[] GetAllValues()
        {
            return base.BaseGetAllValues();
        }
        public object[] GetAllValues(Type type)
        {
            return base.BaseGetAllValues(type);
        }
        public string GetKey(int index)
        {
            return base.BaseGetKey(index);
        }
        public bool HasKeys()
        {
            return base.BaseHasKeys();
        }
        public void Set(int index, object value)
        {
            base.BaseSet(index, value);
        }
        public void Set(string name, object value)
        {
            base.BaseSet(name, value);
        }

        public T this[int index]
        {
            get
            {
                return (T)base.BaseGet(index);
            }
            set
            {
                base.BaseSet(index, value);
            }
        }
        public T this[string name]
        {
            get
            {
                return (T)base.BaseGet(name);
            }
            set
            {
                base.BaseSet(name, value);
            }
        }
        public override IEnumerator GetEnumerator()
        {
            return ie;
        }
        class DataCacheGetEnumerator : IEnumerator
        {
            #region IEnumerator 成员
            NameObjectCollection<T> domainArray;
            int _current = -1;
            public DataCacheGetEnumerator(NameObjectCollection<T> array)
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
                return (_current < domainArray.Count);
            }

            public void Reset()
            {
                _current = -1;
            }

            #endregion
        }
    }
}
