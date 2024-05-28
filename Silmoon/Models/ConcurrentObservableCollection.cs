using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Silmoon.Models
{
    public class ConcurrentObservableCollection<T> : ObservableCollection<T>
    {
        private readonly object _syncLock = new object();

        protected override void InsertItem(int index, T item)
        {
            lock (_syncLock)
            {
                base.InsertItem(index, item);
            }
        }

        protected override void SetItem(int index, T item)
        {
            lock (_syncLock)
            {
                base.SetItem(index, item);
            }
        }

        protected override void RemoveItem(int index)
        {
            lock (_syncLock)
            {
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            lock (_syncLock)
            {
                base.ClearItems();
            }
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            lock (_syncLock)
            {
                base.MoveItem(oldIndex, newIndex);
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handlers = null;
            lock (_syncLock)
            {
                var eventField = typeof(ObservableCollection<T>).GetField(nameof(CollectionChanged), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                handlers = (NotifyCollectionChangedEventHandler)eventField?.GetValue(this);
            }
            if (handlers != null)
            {
                foreach (NotifyCollectionChangedEventHandler handler in handlers.GetInvocationList())
                {
                    handler(this, e);
                }
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handlers = null;
            lock (_syncLock)
            {
                var eventField = typeof(ObservableCollection<T>).GetField(nameof(PropertyChanged), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                handlers = (PropertyChangedEventHandler)eventField?.GetValue(this);
            }
            if (handlers != null)
            {
                foreach (PropertyChangedEventHandler handler in handlers.GetInvocationList())
                {
                    handler(this, e);
                }
            }
        }
    }
}
