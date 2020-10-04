using System;

namespace Settings.Core
{
    public abstract class SettingBase<TValue> : ISetting
    {
        public string Id => GetType().Name;

        private TValue _value;
        public virtual TValue Value
        {
            get => _value;
            set
            {
                if (IsValueEqual(value, Value))
                    return;

                _value = value;
                OnChanged();
            }
        }

        public event Action Changed;

        public virtual bool IsValueEqual(TValue a, TValue b)
        {
            if (a is IComparable<TValue>)
                return (a as IComparable<TValue>).CompareTo(b) == 0;
            throw new ArgumentException($"'{typeof(TValue).FullName}' does not implement {typeof(IComparable<TValue>).Name}, so you need to override {nameof(IsValueEqual)}.");
        }

        public virtual void SetDefault()
        {
        }

        public abstract void Load(string saveData);
        
        public abstract string Save();

        protected virtual void OnChanged()
        {
            Changed?.Invoke();
        }
    }
}