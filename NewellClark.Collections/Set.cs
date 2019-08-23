using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// Provides an implementation of the <see cref="ISet{T}"/> interface using the built-in
	/// <see cref="HashSet{T}"/> type. 
	/// </summary>
	/// <typeparam name="T">The type of item in the set.</typeparam>
	public class Set<T> : SetBase<T>
	{
		/// <summary>
		/// Creates a new <see cref="Set{T}"/> that uses the default <see cref="IEqualityComparer{T}"/>.
		/// </summary>
		public Set() : this(EqualityComparer<T>.Default) { }

		/// <summary>
		/// Creates a new <see cref="Set{T}"/> that uses the specified <see cref="IEqualityComparer{T}"/>.
		/// </summary>
		/// <param name="comparer">The <see cref="IEqualityComparer{T}"/> to use to determine if two items are equal.</param>
		public Set(IEqualityComparer<T> comparer) : base()
		{
			if (comparer is null) throw new ArgumentNullException(nameof(comparer));

			_Items = new HashSet<T>(comparer);
		}

		/// <summary>
		///	Gets the <see cref="ISet{T}"/> of items that the current instance wraps.
		/// </summary>
		protected ISet<T> Items => _Items;
		private readonly HashSet<T> _Items;

		protected override bool AddItem(T item) => _Items.Add(item);

		protected override bool RemoveItem(T item) => _Items.Remove(item);

		protected override void ClearItems() => _Items.Clear();

		protected override bool ContainsItem(T item) => _Items.Contains(item);

		public override IEqualityComparer<T> Comparer => _Items.Comparer;

		public override IEnumerator<T> GetEnumerator() => _Items.GetEnumerator();

		public override int Count => _Items.Count;

		public override bool IsReadOnly => false;
	}
}
