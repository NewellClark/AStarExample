using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer
{
	class DisposableList<T> : IList<T>, IDisposable
		where T: IDisposable
	{
		private readonly List<T> _inner;

		public DisposableList()
		{
			_inner = new List<T>();
		}

		public T this[int index] { get => _inner[index]; set => _inner[index] = value; }

		public int Count => _inner.Count;

		public bool IsReadOnly => ((IList<T>)_inner).IsReadOnly;

		public void Add(T item)
		{
			_inner.Add(item);
		}

		public void Clear()
		{
			_inner.Clear();
		}

		public bool Contains(T item)
		{
			return _inner.Contains(item);
		}

		public void CopyTo(T[] array, int arrayIndex)
		{
			_inner.CopyTo(array, arrayIndex);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return ((IList<T>)_inner).GetEnumerator();
		}

		public int IndexOf(T item)
		{
			return _inner.IndexOf(item);
		}

		public void Insert(int index, T item)
		{
			_inner.Insert(index, item);
		}

		public bool Remove(T item)
		{
			return _inner.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_inner.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<T>)_inner).GetEnumerator();
		}

		public void Dispose()
		{
			foreach (var item in _inner)
				item?.Dispose();
		}
	}
}
