using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rugal.Xamarin.BindingModel
{
    public class BaseObservableDictionary<TVal> : BaseOnChangeEvent, IDictionary<string, TVal>
    {
        internal Dictionary<string, TVal> Model;
        public BaseObservableDictionary()
        {
            Model = new Dictionary<string, TVal> { };
        }
        public ICollection<TVal> Values => Model.Values;
        public ICollection<string> Keys => Model.Keys;
        public int Count => Model.Count;
        public bool IsReadOnly => false;
        public void Clear() => Model.Clear();
        public IEnumerator<KeyValuePair<string, TVal>> GetEnumerator() => Model.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Model.GetEnumerator();
        public bool ContainsKey(string Key) => Model.ContainsKey(Key);
        public bool Remove(string Key) => Model.Remove(Key);
        public bool TryGetValue(string Key, out TVal Value) => Model.TryGetValue(Key, out Value);
        public virtual TVal this[string Key]
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
        public void Add(string Key, TVal Value)
        {
            Model[Key] = Value;
            OnChange(Key.ToString());
        }
        public void Add(KeyValuePair<string, TVal> Item) => Model.Add(Item.Key, Item.Value);
        public bool Contains(KeyValuePair<string, TVal> Item)
        {
            return Model.ContainsKey(Item.Key) && Model[Item.Key].Equals(Item.Value);
        }
        public void CopyTo(KeyValuePair<string, TVal>[] Array, int ArrayIndex)
        {
            return;
        }
        public bool Remove(KeyValuePair<string, TVal> item)
        {
            return false;
        }
    }
}