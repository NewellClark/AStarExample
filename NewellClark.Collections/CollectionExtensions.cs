using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Gets the value with the specified key, or calls the specified factory delegate to create it if it
		/// is not in the dictionary.
		/// </summary>
		/// <typeparam name="TKey">The type of key in the dictionary.</typeparam>
		/// <typeparam name="TValue">The type of value in the dictionary.</typeparam>
		/// <param name="this"></param>
		/// <param name="key">The key to search for.</param>
		/// <param name="valueFactory">A delegate to compute the value if the key is absent.</param>
		/// <returns>
		/// The value associated with the key, or the new value that was added if the key was not in the dictionary.
		/// </returns>
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey, TValue> valueFactory)
		{
			if (@this is null) throw new ArgumentNullException(nameof(@this)); 
			if (ReferenceEquals(key, null)) throw new ArgumentNullException(nameof(key));
			if (valueFactory is null) throw new ArgumentNullException(nameof(valueFactory));

			if (!@this.TryGetValue(key, out TValue result))
			{
				result = valueFactory(key);
				@this.Add(key, result);
			}

			return result;
		}

		/// <summary>
		/// Wraps the current <see cref="ICollection{T}"/> in a read-only wrapper.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <returns>A read-only wrapper that wraps the current <see cref="ICollection{T}"/>.</returns>
		public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> @this)
		{
			if (@this == null)
				throw new ArgumentNullException(nameof(@this));

			return new ReadOnlyCollectionWrapper<T>(@this);
		}

		private class ReadOnlyCollectionWrapper<T> : IReadOnlyCollection<T>
		{
			private readonly ICollection<T> _inner;

			public ReadOnlyCollectionWrapper(ICollection<T> inner)
			{
				Debug.Assert(inner != null);

				_inner = inner;
			}

			public int Count => _inner.Count;

			public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
		}
	}
}
