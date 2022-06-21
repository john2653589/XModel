using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rugal.Xamarin.BindingModel
{
    public class BaseObservableDictionary<TKey, TVal> : BaseOnChangeEvent, IDictionary<TKey, TVal>
    {
        internal Dictionary<TKey, TVal> Model;
        public BaseObservableDictionary()
        {
            Model = new Dictionary<TKey, TVal> { };
        }
        public ICollection<TVal> Values => Model.Values;
        public ICollection<TKey> Keys => Model.Keys;
        public int Count => Model.Count;
        public bool IsReadOnly => false;
        public void Clear() => Model.Clear();
        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator() => Model.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Model.GetEnumerator();
        public bool ContainsKey(TKey Key) => Model.ContainsKey(Key);
        public bool Remove(TKey Key) => Model.Remove(Key);
        public bool TryGetValue(TKey Key, out TVal Value) => Model.TryGetValue(Key, out Value);
        public TVal this[TKey Key]
        {
            get
            {
                if (!Model.ContainsKey(Key))
                {
                    Model.Add(Key, default);
                    OnChange(Key.ToString());
                }
                return Model[Key];
            }
            set
            {
                Model[Key] = value;
                OnChange(Key.ToString());
            }
        }
        public void Add(TKey Key, TVal Value)
        {
            Model[Key] = Value;
            OnChange(Key.ToString());
        }
        public void Add(KeyValuePair<TKey, TVal> Item) => Model.Add(Item.Key, Item.Value);
        public bool Contains(KeyValuePair<TKey, TVal> Item)
        {
            return Model.ContainsKey(Item.Key) && Model[Item.Key].Equals(Item.Value);
        }
        public void CopyTo(KeyValuePair<TKey, TVal>[] Array, int ArrayIndex)
        {
            return;
        }
        public bool Remove(KeyValuePair<TKey, TVal> item)
        {
            return false;
        }
    }
}