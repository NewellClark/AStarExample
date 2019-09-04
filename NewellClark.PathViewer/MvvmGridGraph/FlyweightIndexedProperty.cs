using NewellClark.DataStructures.Collections;
using System;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	internal class FlyweightIndexedProperty<TKey, TValue>
	{
		public FlyweightIndexedProperty(FlyweightCollection<TKey, TValue> lookup)
		{
			if (lookup is null) throw new ArgumentNullException(nameof(lookup));

			this.Lookup = lookup;
		}

		protected FlyweightCollection<TKey, TValue> Lookup { get; }

		protected virtual TValue GetValue(TKey key) => Lookup[key];
		protected virtual void SetValue(TKey key, TValue value) => Lookup[key] = value;

		public TValue this[TKey key]
		{
			get => GetValue(key);
			set => SetValue(key, value);
		}
	}
}
