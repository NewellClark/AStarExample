using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace NewellClark.DataStructures.Collections
{
	/// <summary>
	/// A queue that sorts items based on the supplied priority in ascending order. Items with equal priority will
	/// come out in the order they were added.
	/// </summary>
	/// <typeparam name="TPriority">The type of an item's priority.</typeparam>
	/// <typeparam name="TValue">The type of item in the queue.</typeparam>
	public class PriorityQueue<TPriority, TValue> : IEnumerable<TValue>
	{
		private static readonly int startingCapacity = 1;
		private readonly SortedDictionary<TPriority, Queue<TValue>> _lookup;
		private int _version = 0;

		/// <summary>
		/// Creates a new <see cref="PriorityQueue{TPriority, TValue}"/> that uses the specified 
		/// <see cref="IComparer{TPriority}"/> to sort items in ascending order based on priority.
		/// </summary>
		/// <param name="comparer">The <see cref="IComparer{TPriority}"/> to use to sort items.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="comparer"/> is null.
		/// </exception>
		public PriorityQueue(IComparer<TPriority> comparer)
		{
			if (comparer is null) throw new ArgumentNullException(nameof(comparer));

			_lookup = new SortedDictionary<TPriority, Queue<TValue>>(comparer);
		}

		/// <summary>
		/// Creates a new <see cref="PriorityQueue{TPriority, TValue}"/> that uses the specified <see cref="Comparison{TPriority}"/> 
		/// to sort items in ascending order.
		/// </summary>
		/// <param name="comparison">The <see cref="Comparer{TPriority}"/> to use to sort items.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="comparison"/> is null.
		/// </exception>
		public PriorityQueue(Comparison<TPriority> comparison)
		{
			if (comparison is null) throw new ArgumentNullException(nameof(comparison));

			_lookup = new SortedDictionary<TPriority, Queue<TValue>>(Comparer<TPriority>.Create(comparison));
		}

		/// <summary>
		/// Creates a new <see cref="PriorityQueue{TPriority, TValue}"/> that uses the default comparer.
		/// </summary>
		public PriorityQueue() : this(Comparer<TPriority>.Default) { }

		/// <summary>
		/// Gets the value at the head of the <see cref="PriorityQueue{TPriority, TValue}"/> without removing it.
		/// </summary>
		/// <returns>The value at the head of the <see cref="PriorityQueue{TPriority, TValue}"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// The current <see cref="PriorityQueue{TPriority, TValue}"/> is empty.
		/// </exception>
		public TValue Peek()
		{
			ThrowWhenEmpty();

			return GetFirst().Value.Peek();
		}

		private KeyValuePair<TPriority, Queue<TValue>> GetFirst()
		{
			Debug.Assert(_lookup.Count > 0);

			using (var iter = _lookup.GetEnumerator())
			{
				iter.MoveNext();
				return iter.Current;
			}
		}

		/// <summary>
		/// Adds the specified <paramref name="value"/> to the <see cref="PriorityQueue{TPriority, TValue}"/> with the specified
		/// <paramref name="priority"/>.
		/// </summary>
		/// <param name="priority">The priority of the item.</param>
		/// <param name="value">The value of the item.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="priority"/> is null.
		/// </exception>
		public void Enqueue(TPriority priority, TValue value)
		{
			Debug.Assert(_lookup != null);

			var queue = _lookup.GetOrAdd(priority, _ => new Queue<TValue>(startingCapacity));
			queue.Enqueue(value);
			UpdateVersion();
			Count += 1;
		}

		/// <summary>
		/// Removes the item at the head of the <see cref="PriorityQueue{TPriority, TValue}"/> and returns it.
		/// </summary>
		/// <returns>The item at the head of the <see cref="PriorityQueue{TPriority, TValue}"/>.</returns>
		/// <exception cref="InvalidOperationException">
		/// The current <see cref="PriorityQueue{TPriority, TValue}"/> is empty.
		/// </exception>
		public TValue Dequeue()
		{
			ThrowWhenEmpty();

			var first = GetFirst();

			Debug.Assert(first.Value.Count > 0);

			TValue result = first.Value.Dequeue();
			if (first.Value.Count == 0)
				_lookup.Remove(first.Key);
			Count -= 1;
			UpdateVersion();

			return result;
		}

		/// <summary>
		/// Removes all items from the <see cref="PriorityQueue{TPriority, TValue}"/>.
		/// </summary>
		public void Clear()
		{
			_lookup.Clear();
			Count = 0;
			UpdateVersion();
		}

		/// <summary>
		/// Gets the number of items in the current <see cref="PriorityQueue{TPriority, TValue}"/>.
		/// </summary>
		public int Count { get; private set; }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="PriorityQueue{TPriority, TValue}"/> is empty.
		/// </summary>
		public bool IsEmpty => Count == 0;

		/// <summary>
		/// Gets an <see cref="IEnumerator{TValue}"/> that iterates over the collection. The items will be iterated
		/// in the order that they will come out.
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator{TValue}"/> for the current <see cref="PriorityQueue{TPriority, TValue}"/>.
		/// </returns>
		public IEnumerator<TValue> GetEnumerator()
		{
			foreach (var queue in _lookup.Values)
			{
				foreach (var value in queue)
					yield return value;
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		private void UpdateVersion()
		{
			unchecked { ++_version; }
		}

		private void ThrowWhenEmpty()
		{
			if (IsEmpty) 
				throw new InvalidOperationException($"The {GetType().Name} is empty.");
		}
	}
}
