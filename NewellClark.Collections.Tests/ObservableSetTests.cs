using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Collections.Tests
{
	[TestFixture]
	public class ObservableSetTests : SetBaseTests
	{
		protected override ISet<T> Create<T>() => new ObservableSet<T>();

		private Listener<int> listener;

		protected new ObservableSet<int> set;

		public override void Initialize()
		{
			base.Initialize();
			set = base.set as ObservableSet<int>;
			listener = new Listener<int>(set);
		}

		private class Listener<T>
		{
			private readonly ObservableSet<T> _set;

			public Listener(ObservableSet<T> set)
			{
				_set = set;
				_Added = new List<T>();
				Added = _Added.AsReadOnly();
				_Removed = new List<T>();
				Removed = _Removed.AsReadOnly();

				_set.SetChanged += HandleSetChanged;
			}

			private readonly List<T> _Added;
			public IReadOnlyCollection<T> Added { get; }

			private readonly List<T> _Removed;
			public IReadOnlyCollection<T> Removed { get; }

			private void HandleSetChanged(object sender, SetChangedEventArgs<T> e)
			{
				_Added.AddRange(e.Added);
				_Removed.AddRange(e.Removed);
			}

			public void AssertAdded(params T[] expected)
			{
				CollectionAssert.AreEquivalent(expected, Added);
			}

			public void AssertRemoved(params T[] expected)
			{
				CollectionAssert.AreEquivalent(expected, Removed);
			}
		}

		[Test]
		public void Add_RaisesEvent()
		{
			AddAll(set, 1);
			listener.AssertAdded(1);
		}

		[Test]
		public void Remove_RaisesEvent()
		{
			AddAll(set, 3, 7, 7, 4);
			set.Remove(7);
			set.Remove(7);
			set.Remove(4);

			listener.AssertRemoved(7, 4);
		}

		[Test]
		public void Add_NoEventRegistered_DoesNotThrow()
		{
			var set = new ObservableSet<int>();

			Assert.DoesNotThrow(() => set.Add(1));
		}

		[Test]
		public void Clear_RaisesEvent()
		{
			AddAll(set, 3, 4, 5, 6, 2);

			set.Clear();

			listener.AssertRemoved(3, 4, 5, 6, 2);
		}
	}
}
