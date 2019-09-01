using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	public class ReadOnlySet<T> : IReadOnlyCollection<T>
	{
		private readonly ISet<T> _inner;

		public ReadOnlySet(ISet<T> inner)
		{
			if (inner is null) throw new ArgumentNullException(nameof(inner));

			_inner = inner;
		}

		public bool Contains(T item) => _inner.Contains(item);

		public int Count => _inner.Count;

		public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
