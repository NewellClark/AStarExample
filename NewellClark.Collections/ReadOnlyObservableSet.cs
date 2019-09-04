using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// Provides a read-only wrapper around an <see cref="ObservableSet{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type of element.</typeparam>
	public class ReadOnlyObservableSet<T> : IReadOnlyCollection<T>
	{
		private readonly ObservableSet<T> _inner;

		/// <summary>
		/// Creates a new <see cref="ReadOnlyObservableSet{T}"/> that wraps the specified 
		/// <see cref="ObservableSet{T}"/>.
		/// </summary>
		/// <param name="inner">The <see cref="ObservableSet{T}"/> to wrap.</param>
		public ReadOnlyObservableSet(ObservableSet<T> inner)
		{
			if (inner is null) throw new ArgumentNullException(nameof(inner));

			_inner = inner;

			void handleSetChanged(object sender, SetChangedEventArgs<T> e) => 
				SetChanged?.Invoke(this, e);
			_inner.SetChanged += handleSetChanged;
		}

		/// <summary>
		/// Gets the number of elements in the set.
		/// </summary>
		public int Count => _inner.Count;

		/// <summary>
		/// Raised whenever the underlying <see cref="ObservableSet{T}"/> is modified.
		/// </summary>
		public event EventHandler<SetChangedEventArgs<T>> SetChanged;

		/// <summary>
		/// Gets an enumerator for the current <see cref="ReadOnlyObservableSet{T}"/>.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _inner.GetEnumerator();

		public bool SetEquals(IEnumerable<T> other) => _inner.SetEquals(other);

		public bool Overlaps(IEnumerable<T> other) => _inner.Overlaps(other);

		public bool IsSubsetOf(IEnumerable<T> other) => _inner.IsSubsetOf(other);

		public bool IsProperSubsetOf(IEnumerable<T> other) => _inner.IsProperSubsetOf(other);

		public bool IsSupersetOf(IEnumerable<T> other) => _inner.IsSupersetOf(other);

		public bool IsProperSupersetOf(IEnumerable<T> other) => _inner.IsProperSupersetOf(other);
	}
}
