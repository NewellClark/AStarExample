using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// Base class for implementing the <see cref="ISet{T}"/> interface. Provides default implementations
	/// of all the set operations, which can optionally be overridden. 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SetBase<T> : ISet<T>, IReadOnlyCollection<T>
	{
		protected SetBase() { }

		/// <summary>
		/// Adds the specified item to the set if it doesn't exist.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added, false otherwise.</returns>
		public bool Add(T item) => AddItem(item);

		/// <summary>
		/// Removes the specified item from the set.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed. False otherwise.</returns>
		public bool Remove(T item) => RemoveItem(item);

		/// <summary>
		/// Indicates whether the specified item is in the set.
		/// </summary>
		/// <param name="item">The item to look for.</param>
		/// <returns>True if the item is in the set. False otherwise.</returns>
		public bool Contains(T item) => ContainsItem(item);

		/// <summary>
		/// Removes all items from the set.
		/// </summary>
		public void Clear() => ClearItems();

		/// <summary>
		/// When overridden in a derived class, adds the specified item to the set. This is called by the
		/// <see cref="SetBase{T}.Add(T)"/> method.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added. False otherwise.</returns>
		protected abstract bool AddItem(T item);

		/// <summary>
		/// When overridden in a derived class, removes the specified item from the set. This is called by the
		/// <see cref="SetBase{T}.Remove(T)"/> method.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed, false otherwise.</returns>
		protected abstract bool RemoveItem(T item);

		/// <summary>
		/// When overridden in a derived class, indicates whether the specified item is in the set. This is called by the
		/// <see cref="SetBase{T}.Contains(T)"/> method.
		/// </summary>
		/// <param name="item">The item to search for.</param>
		/// <returns>True if the item is in the set. False otherwise</returns>
		protected abstract bool ContainsItem(T item);

		/// <summary>
		/// When overridden in a derived class, clears all items from the set. This is called by the
		/// <see cref="SetBase{T}.Clear"/> method.
		/// </summary>
		protected abstract void ClearItems();

		/// <summary>
		/// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine item equality.
		/// </summary>
		public abstract IEqualityComparer<T> Comparer { get; }

		/// <summary>
		/// When overridden in a derived class, gets an enumerator that enumerates over the collection.
		/// </summary>
		/// <returns>An enumerator that can enumerate the collection.</returns>
		public abstract IEnumerator<T> GetEnumerator();

		/// <summary>
		/// When overridden in a derived class, gets the number of items in the set.
		/// </summary>
		public abstract int Count { get; }

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current <see cref="SetBase{T}"/> is
		/// read-only.
		/// </summary>
		public abstract bool IsReadOnly { get; }

		/// <summary>
		/// Copies the current <see cref="SetBase{T}"/> to the specified array, starting at the
		/// specified index in the array.
		/// </summary>
		/// <param name="array">The array to copy to.</param>
		/// <param name="arrayIndex">The index to start copying at.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is negative, or the array doesn't have the capacity
		/// to hold entire collection starting at the specified index.
		/// </exception>
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (array is null) throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), $"Can't be negative.");
			if (arrayIndex + Count > array.Length)
				throw new ArgumentOutOfRangeException(nameof(arrayIndex), $"Not enough room in array to copy collection.");

			using (var iter = GetEnumerator())
			{
				for (int index = arrayIndex; iter.MoveNext(); ++index)
				{
					array[index] = iter.Current;
				}
			}
		}

		/// <summary>
		/// Removes all items that match the specified <see cref="Predicate{T}"/>. 
		/// </summary>
		/// <param name="predicate">Predicate to determine whether an item should be removed.</param>
		/// <returns>The number of items that were removed.</returns>
		public virtual int RemoveWhere(Predicate<T> predicate)
		{
			if (predicate is null) throw new ArgumentNullException(nameof(predicate));

			var remove = this.Where(i => predicate(i)).ToArray();
			foreach (T item in remove)
				Remove(item);

			return remove.Length;
		}

		public virtual void ExceptWith(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			if (other == this)
			{
				Clear();
				return;
			}

			foreach (var item in other)
				Remove(item);
		}

		public virtual void SymmetricExceptWith(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			if (other == this)
			{
				Clear();
				return;
			}

			foreach (T item in other.Distinct(Comparer))
			{
				if (!Remove(item))
					Add(item);
			}
		}

		public virtual void IntersectWith(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			if (other == this)
				return;

			var theirs = new HashSet<T>(other, Comparer);

			RemoveWhere(i => !theirs.Contains(i));
		}

		public virtual void UnionWith(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			if (other == this)
				return;

			foreach (T item in other)
				Add(item);
		}

		public virtual bool SetEquals(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			foreach (T item in other)
				if (!Contains(item))
					return false;

			return true;
		}

		public virtual bool Overlaps(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			foreach (T item in other)
				if (Contains(item))
					return true;

			return false;
		}

		public virtual bool IsSubsetOf(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			var theirs = new HashSet<T>(other, Comparer);

			if (Count > theirs.Count)
				return false;

			foreach (T item in this)
				if (!theirs.Contains(item))
					return false;

			return true;
		}

		public virtual bool IsProperSubsetOf(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			var theirs = new HashSet<T>(other, Comparer);

			if (Count >= theirs.Count)
				return false;

			foreach (T item in this)
				if (!theirs.Contains(item))
					return false;

			return true;
		}

		public virtual bool IsSupersetOf(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			var theirs = new HashSet<T>(other, Comparer);

			if (Count < theirs.Count)
				return false;

			foreach (T item in theirs)
				if (!Contains(item))
					return false;

			return true;
		}

		public virtual bool IsProperSupersetOf(IEnumerable<T> other)
		{
			if (other is null) throw new ArgumentNullException(nameof(other));

			var theirs = new HashSet<T>(other, Comparer);

			if (Count <= theirs.Count)
				return false;

			foreach (T item in theirs)
				if (!Contains(item))
					return false;

			return true;
		}


		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		void ICollection<T>.Add(T item) => AddItem(item);
	}
}
