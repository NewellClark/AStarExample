using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer
{
	/// <summary>
	/// A dictionary that disposes of its values when it is disposed itself.
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	/// <typeparam name="TDisposable"></typeparam>
	class DisposableDictionary<TKey, TDisposable> : IDictionary<TKey, TDisposable>, IDisposable
		where TDisposable : IDisposable
	{
		private readonly Dictionary<TKey, TDisposable> _inner;

		public DisposableDictionary()
		{
			_inner = new Dictionary<TKey, TDisposable>();
		}

		public TDisposable this[TKey key] { get => _inner[key]; set => _inner[key] = value; }

		public ICollection<TKey> Keys => ((IDictionary<TKey, TDisposable>)_inner).Keys;

		public ICollection<TDisposable> Values => ((IDictionary<TKey, TDisposable>)_inner).Values;

		public int Count => _inner.Count;

		public bool IsReadOnly => ((IDictionary<TKey, TDisposable>)_inner).IsReadOnly;

		public void Add(TKey key, TDisposable value)
		{
			_inner.Add(key, value);
		}

		public void Add(KeyValuePair<TKey, TDisposable> item)
		{
			((IDictionary<TKey, TDisposable>)_inner).Add(item);
		}

		public void Clear()
		{
			_inner.Clear();
		}

		public bool Contains(KeyValuePair<TKey, TDisposable> item)
		{
			return ((IDictionary<TKey, TDisposable>)_inner).Contains(item);
		}

		public bool ContainsKey(TKey key)
		{
			return _inner.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<TKey, TDisposable>[] array, int arrayIndex)
		{
			((IDictionary<TKey, TDisposable>)_inner).CopyTo(array, arrayIndex);
		}

		public IEnumerator<KeyValuePair<TKey, TDisposable>> GetEnumerator()
		{
			return ((IDictionary<TKey, TDisposable>)_inner).GetEnumerator();
		}

		public bool Remove(TKey key)
		{
			return _inner.Remove(key);
		}

		public bool Remove(KeyValuePair<TKey, TDisposable> item)
		{
			return ((IDictionary<TKey, TDisposable>)_inner).Remove(item);
		}

		public bool TryGetValue(TKey key, out TDisposable value)
		{
			return _inner.TryGetValue(key, out value);
		}

		public void Dispose()
		{
			foreach (var value in this.Values)
				value?.Dispose();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<TKey, TDisposable>)_inner).GetEnumerator();
		}
	}
}
