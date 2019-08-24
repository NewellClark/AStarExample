using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	public class ObservableSet<T> : Set<T>
	{
		public ObservableSet() : base() { }
		public ObservableSet(IEqualityComparer<T> comparer) : base(comparer) { }

		/// <summary>
		/// Raised when items are added or removed from the set.
		/// </summary>
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
