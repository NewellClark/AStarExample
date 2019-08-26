using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// A set that raises an event whenever items are added or removed. 
	/// </summary>
	/// <typeparam name="T">Type of element in the set.</typeparam>
	public class ObservableSet<T> : Set<T>
	{
		/// <summary>
		/// Creates a new <see cref="ObservableSet{T}"/>.
		/// </summary>
		public ObservableSet() : base() { }

		/// <summary>
		/// Creates a new <see cref="ObservableSet{T}"/> with the specified <see cref="IEqualityComparer{T}"/>.
		/// </summary>
		/// <param name="comparer">The comparer to use.</param>
		public ObservableSet(IEqualityComparer<T> comparer) : base(comparer) { }

		/// <summary>
		/// Raised when items are added or removed from the set. The <see cref="SetChangedEventArgs{T}"/> instance 
		/// contains collections of all the items that were added and/or removed.
		/// </summary>
		/// <remarks>
		/// When multiple items are added or removed in a single operation (ie by calling <see cref="SetBase{T}.Clear<"/> or
		/// one of the set operations that modifies the set such as <see cref="SetBase{T}.ExceptWith(IEnumerable{T})"/>), the 
		/// implementation is free to combine all added or removed items into a single event, but is not required to do so. 
		/// </remarks>
		public event EventHandler<SetChangedEventArgs<T>> SetChanged;

		/// <summary>
		/// Raises the <see cref="SetChanged"/> event.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected void OnSetChanged(SetChangedEventArgs<T> e)
		{
			if (e == null)
				throw new ArgumentNullException(nameof(e));

			SetChanged?.Invoke(this, e);
		}

		/// <summary>
		/// Adds the specified item to the set and raises the <see cref="SetChanged"/> event if the item was added.
		/// </summary>
		/// <param name="item">The item to add.</param>
		/// <returns>True if the item was added.</returns>
		protected override bool AddItem(T item)
		{
			bool added = base.AddItem(item);
			if (added)
			{
				var e = new SetChangedEventArgs<T>(
					new T[] { item }.AsReadOnly(), new T[0]);
				OnSetChanged(e);
			}

			return added;
		}

		/// <summary>
		/// Removes the specified item from the set and raises the <see cref="SetChanged"/> event if the item
		/// was removed.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>True if the item was removed.</returns>
		protected override bool RemoveItem(T item)
		{
			bool removed = base.RemoveItem(item);
			if (removed)
			{
				var e = new SetChangedEventArgs<T>(
					new T[0], new T[] { item }.AsReadOnly());
				OnSetChanged(e);
			}

			return removed;
		}

		/// <summary>
		/// Removes all items from the set and raises the <see cref="SetChanged"/> event if any items were removed.
		/// </summary>
		protected override void ClearItems()
		{
			if (Count == 0)
				return;

			var e = new SetChangedEventArgs<T>(new T[0], this.ToArray().AsReadOnly());
			base.ClearItems();
			OnSetChanged(e);
		}
	}
}
