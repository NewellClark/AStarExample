using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Collections.Tests
{
	[TestFixture]
	public class PriorityQueueTests
	{
		private PriorityQueue<int, int> Create() => new PriorityQueue<int, int>();

		private void EnqueueAll<T>(PriorityQueue<T, T> queue, params T[] values)
		{
			foreach (var item in values)
				queue.Enqueue(item, item);
		}

		private void EnqueueAll<TPriority, TValue>(PriorityQueue<TPriority, TValue> queue, params (TPriority priority, TValue value)[] tuples)
		{
			foreach (var tuple in tuples)
				queue.Enqueue(tuple.priority, tuple.value);
		}

		private IEnumerable<TValue> DequeueAll<TPriority, TValue>(PriorityQueue<TPriority, TValue> queue)
		{
			IEnumerable<TValue> iterator()
			{
				while (queue.Count > 0)
					yield return queue.Dequeue();
			}

			return iterator().ToArray();
		}

		private IEnumerable<T> Series<T>(params T[] items) => items.AsEnumerable();

		[Test]
		public void UnorderedItems_ComeOutInAscendingOrder()
		{
			var queue = Create();

			EnqueueAll(queue, 3, 7, -4, 6, 5);

			var expected = Series(-4, 3, 5, 6, 7);
			var actual = DequeueAll(queue).ToArray();

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void MultipleUnequalItemsSamePriority_ComeOutInFifoOrder()
		{
			(int priority, object value) t(int priority) => (priority, new object());

			var queue = new PriorityQueue<int, object>();
			var series = Series(t(1), t(2), t(2), t(2), t(4), t(5), t(5), t(5), t(6));
			EnqueueAll(queue, series.ToArray());

			var expected = series.Select(i => i.value);
			var actual = DequeueAll(queue);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void MultipleEqualItemsSamePriority_AllComeOutInCorrectOrder()
		{
			var queue = Create();
			EnqueueAll(queue, 7, -4, 3, 6, 7, 3, -18, 3, 6);
			var expected = Series(-18, -4, 3, 3, 3, 6, 6, 7, 7);
			var actual = DequeueAll(queue);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void MultipleItemsSamePriority_CountedCorrectly()
		{
			var queue = Create();

			EnqueueAll(queue, 7, -4, 3, -4, 18, -6, 3, 3);

			const int expected = 8;
			int actual = queue.Count;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void DequeuedItems_AreRemoved()
		{
			var queue = Create();
			EnqueueAll(queue, 3, 7, -12, -16, 3);
			queue.Dequeue();

			var expected = Series(-12, 3, 3, 7);
			var actual = queue.ToArray();

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void Dequeue_ReturnsItemWithHighestPriority()
		{
			var queue = Create();
			EnqueueAll(queue, 7, 7, -16, 4, 12, 6, 6, 8);

			const int expected = -16;
			int actual = queue.Dequeue();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Peek_ReturnsItemWithHighestPriority()
		{
			var queue = Create();
			EnqueueAll(queue, -7, -19, 4, 6, -8, -6);

			const int expected = -19;
			int actual = queue.Peek();

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void EmptyQueue_ThrowsWhenDequeued()
		{
			var queue = Create();

			void test() => queue.Dequeue();

			Assert.Throws<InvalidOperationException>(test);
		}

		[Test]
		public void EmptyQueue_ThrowsWhenPeeked()
		{
			var queue = Create();

			void test() => queue.Peek();

			Assert.Throws<InvalidOperationException>(test);
		}

		[Test]
		public void EmptyQueue_HasCountOfZero()
		{
			var queue = Create();

			const int expected = 0;
			int actual = queue.Count;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Count_KeepsTrackOfMultipleEnqueuesAndDequeues()
		{
			var queue = Create();
			EnqueueAll(queue, 7, 6, 3, 6, 3);
			queue.Dequeue();
			queue.Dequeue();
			queue.Dequeue();

			const int expected = 2;
			int actual = queue.Count;

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetEnumerator_EnumeratesInSortedOrder()
		{
			var queue = Create();
			EnqueueAll(queue, 6, -4, 6, -3, -7, 18, -3);

			var expected = Series(-7, -4, -3, -3, 6, 6, 18);
			var actual = queue.ToArray();

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void EnumeratorMoveNext_ThrowsWhenEnqueued()
		{
			var queue = Create();
			EnqueueAll(queue, 3, 7, -6, 4);

			using (var iter = queue.GetEnumerator())
			{
				void test() => iter.MoveNext();

				iter.MoveNext();
				queue.Enqueue(14, 14);

				Assert.Throws<InvalidOperationException>(test);
			}
		}

		[Test]
		public void EnumeratorMoveNext_ThrowsWhenDequeued()
		{
			var queue = Create();
			EnqueueAll(queue, 3, 6, -4, 17);

			using (var iter = queue.GetEnumerator())
			{
				void test() => iter.MoveNext();

				iter.MoveNext();
				queue.Dequeue();

				Assert.Throws<InvalidOperationException>(test);
			}
		}

		[Test]
		public void EnumeratorMoveNext_ThrowsWhenCleared()
		{
			var queue = Create();
			EnqueueAll(queue, 7, 7, 4, 3, 1, 4);

			using (var iter = queue.GetEnumerator())
			{
				void test() => iter.MoveNext();

				iter.MoveNext();
				queue.Clear();

				Assert.Throws<InvalidOperationException>(test);
			}
		}

		[Test]
		public void Clear_RemovesAllItems()
		{
			var queue = Create();
			EnqueueAll(queue, 1, 3, 4, 7, 6, 3);

			queue.Clear();

			CollectionAssert.IsEmpty(queue);
		}

		[Test]
		public void ComparerCtor_ThrowsWhenNull()
		{
			void test() => new PriorityQueue<int, int>((IComparer<int>)null);

			Assert.Throws<ArgumentNullException>(test);
		}

		[Test]
		public void ComparisonCtor_ThrowsWhenNull()
		{
			void test() => new PriorityQueue<int, int>((Comparison<int>)null);

			Assert.Throws<ArgumentNullException>(test);
		}
	}
}
