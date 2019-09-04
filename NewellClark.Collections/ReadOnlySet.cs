using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// Provides a read-only wrapper around an <see cref="ISet{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type of element in the set.</typeparam>
	public class ReadOnlySet<T> : IReadOnlyCollection<T>
	{
		private readonly ISet<T> _inner;

		/// <summary>
		/// Creates a new <see cref="ReadOnlySet{T}"/> that wraps the specified <see cref="ISet{T}"/>.
		/// </summary>
		/// <param name="inner"></param>
		public ReadOnlySet(ISet<T> inner)
		{
			if (inner is null) throw new ArgumentNullException(nameof(inner));

			_inner = inner;
		}

		/// <summary>
		/// Indicates whether the specified element is in the set.
		/// </summary>
		/// <param name="item">The element to search for.</param>
		/// <returns>True if present, false otherwise.</returns>
		public bool Contains(T item) => _inner.Contains(item);

		/// <summary>
		/// Gets the number of elements in the set.
		/// </summary>
		public int Count => _inner.Count;

		/// <summary>
		/// Gets an enumerator for the current <see cref="ReadOnlySet{T}"/>.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		public bool SetEquals(IEnumerable<T> other) => _inner.SetEquals(other);

		public bool Overlaps(IEnumerable<T> other) => _inner.Overlaps(other);

		public bool IsSubsetOf(IEnumerable<T> other) => _inner.IsSubsetOf(other);

		public bool IsProperSubsetOf(IEnumerable<T> other) => _inner.IsProperSubsetOf(other);

		public bool IsSupersetOf(IEnumerable<T> other) => _inner.IsSupersetOf(other);

		public bool IsProperSupersetOf(IEnumerable<T> other) => _inner.IsProperSupersetOf(other);
	}
}
