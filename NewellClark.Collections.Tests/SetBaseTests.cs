using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Collections.Tests
{
	[TestFixture]
	public abstract class SetBaseTests
	{
		protected abstract ISet<T> Create<T>();

		protected virtual ISet<T> Create<T>(IEnumerable<T> items)
		{
			var result = Create<T>();
			foreach (var item in items)
				result.Add(item);

			return result;
		}

		private IEnumerable<T> Sequence<T>(params T[] items) => items.AsEnumerable();

		private void AddAll<T>(ISet<T> set, params T[] items)
		{
			foreach (var item in items)
				set.Add(item);
		}

		private ISet<int> set;

		[SetUp]
		public void Initialize() => set = Create<int>();

		[Test]
		public void Add_WillNotAddDuplicates()
		{
			AddAll(set, 3, 3);

			var expected = Sequence(3);
			var actual = set;

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_TrueWhenAbsent()
		{
			Assert.True(set.Add(1));
		}

		[Test]
		public void Add_FalseWhenPresent()
		{
			set.Add(1);
			Assert.False(set.Add(1));
		}

		[Test]
		public void Remove_RemovesSpecifiedElement()
		{
			AddAll(set, 3, 7, 14);
			set.Remove(7);

			var expected = Sequence(3, 14);
			var actual = set;

			CollectionAssert.AreEquivalent(expected, actual);
		}

		[Test]
		public void Remove_TrueWhenPresent()
		{
			AddAll(set, 5, 3, -2);
			Assert.True(set.Remove(-2));
		}

		[Test]
		public void Remove_FalseWhenAbsent()
		{
			Assert.False(set.Remove(15));
		}

		[Test]
		public void Clear_RemovesAllItems()
		{
			AddAll(set, 0, 1, 2, 3, 4, 5);

			Assume.That(set.Count == 6);

			set.Clear();

			CollectionAssert.IsEmpty(set);
		}

		[Test]
		public void Contains_TrueWhenPresent()
		{
			set.Add(5);
			set.Add(3);
			Assert.True(set.Contains(5));
		}

		[Test]
		public void Contains_FalseWhenAbsent()
		{
			AddAll(set, 4, 16, 3);
			Assert.False(set.Contains(17));
		}

		[Test]
		public void Count_EmptySet_IsZero()
		{
			Assert.AreEqual(0, set.Count);
		}

		[Test]
		public void Count_KeepsTrackOfAddsAndRemoves()
		{
			AddAll(set, 4, 3, 7, 7, 5);     //	4
			set.Remove(7);                  //	3
			set.Remove(5);                  //	2
			set.Remove(4);                  //	1

			Assert.AreEqual(1, set.Count);
		}

		[Test]
		public void Enumerator_IteratesAllItems()
		{
			AddAll(set, 3, 7, 6, 6, 4, 3, -7);
			var expected = Sequence(3, 7, 6, 4, -7);
			var actual = set.AsEnumerable();

			CollectionAssert.AreEquivalent(expected, actual);
		}

		[Test]
		public void CopyTo_JustEnoughRoom_Succeeds()
		{
			AddAll(set, 4, 5, 6, 7);
			var array = new int[7];
			array[0] = 1;
			array[1] = 2;
			array[2] = 3;

			set.CopyTo(array, 3);

			var expected = Sequence(1, 2, 3, 4, 5, 6, 7);

			CollectionAssert.AreEquivalent(expected, array);
		}

		[Test]
		public void CopyTo_NegativeIndex_Throws()
		{
			void test() => set.CopyTo(new int[1], -1);

			Assert.Throws<ArgumentOutOfRangeException>(test);
		}

		[Test]
		public void CopyTo_NotEnoughRoom_Throws()
		{
			AddAll(set, 1, 2, 3);
			var array = new int[5];

			void test() => set.CopyTo(array, 3);

			Assert.Throws<ArgumentOutOfRangeException>(test);
		}

		[Test]
		public void CopyTo_NullArray_Throws()
		{
			void test() => set.CopyTo(null, 0);

			Assert.Throws<ArgumentNullException>(test);
		}


		[Test]
		public void ExceptWith_EmptySet_DoesNothing()
		{
			AddAll(set, 5, 3, 4, 2);
			var expected = set.ToArray();

			set.ExceptWith(Sequence<int>());

			CollectionAssert.AreEquivalent(expected, set);
		}

		[Test]
		public void ExceptWith_RemovesOtherElements()
		{
			AddAll(set, 5, 4, 3, 2, 1);
			var other = Sequence(4, 2, 1, 6);
			var expected = Sequence(5, 3);

			set.ExceptWith(other);

			CollectionAssert.AreEquivalent(expected, set);
		}

		[Test]
		public void ExceptWith_Self_RemovesAllItems()
		{
			AddAll(set, 1, 2, 3, 4);

			Assume.That(set.Count == 4);

			set.ExceptWith(set);

			CollectionAssert.IsEmpty(set);
		}

		[Test]
		public void SymmetricExceptWith_ActsAsExclusiveOr()
		{
			AddAll(set, 1, 2, 3, 4, 5, 6);
			var other = Sequence(4, 5, 6, 7, 8, 9);
			var expected = Sequence(1, 2, 3, 7, 8, 9);

			set.SymmetricExceptWith(other);

			CollectionAssert.AreEquivalent(expected, set);
		}

		[Test]
		public void SymmetricExceptWith_Self_RemovesAllItems()
		{
			AddAll(set, 1, 2, 3, 4);

			Assume.That(set.Count == 4);

			set.SymmetricExceptWith(set);

			CollectionAssert.IsEmpty(set);
		}

		[Test]
		public void IntersectWith_RemovesElementsNotInOther()
		{
			AddAll(set, 1, 2, 3, 4, 5, 6);
			var other = Sequence(3, 4, 5, 17, -2);
			var expected = Sequence(3, 4, 5);

			set.IntersectWith(other);

			CollectionAssert.AreEquivalent(expected, set);
		}

		[Test]
		public void IntersectWith_Self_DoesNothing()
		{
			AddAll(set, 1, 2, 3, 4);
			var before = set.ToArray();

			set.IntersectWith(set);

			CollectionAssert.AreEquivalent(before, set);
		}

		[Test]
		public void UnionWith_AddsMissingElements()
		{
			AddAll(set, 1, 2, 3, 4, 5, 6);
			var other = Sequence(-1, 0, 1, 2, 0, 17, 17, 18, 19);
			var expected = Sequence(-1, 0, 1, 2, 3, 4, 5, 6, 17, 18, 19);

			set.UnionWith(other);

			CollectionAssert.AreEquivalent(expected, set);
		}

		[Test]
		public void UnionWith_Self_DoesNothing()
		{
			AddAll(set, 1, 2, 3, 4);
			var before = set.ToArray();

			set.UnionWith(set);

			CollectionAssert.AreEquivalent(before, set);
		}

		[Test]
		public void SetEquals_IgnoresDuplicates()
		{
			AddAll(set, 1, 2, 3, 4, 5);
			var other = Sequence(1, 2, 2, 3, 4, 4, 4, 5, 5);

			Assert.True(set.SetEquals(other));
		}

		[Test]
		public void SetEquals_IgnoresOrder()
		{
			AddAll(set, 1, 2, 3, 4, 5);
			var other = Sequence(5, 4, 1, 3, 2);

			Assert.True(set.SetEquals(other));
		}

		[Test]
		public void EmptySet_IsSubsetOfEmptySet()
		{
			var other = Sequence<int>();

			Assert.True(set.IsSubsetOf(other));
		}

		[Test]
		public void EmptySet_IsNotProperSubsetOfEmptySet()
		{
			var other = Sequence<int>();
			Assert.False(set.IsProperSubsetOf(other));
		}

		[Test]
		public void IsSubsetOf_EqualSets_True()
		{
			AddAll(set, 1, 2, 3, 4, 5);
			var other = Sequence(1, 2, 3, 4, 5);

			Assert.True(set.IsSubsetOf(other));
		}

		[Test]
		public void IsSubsetOf_SingleMissingElement_False()
		{
			AddAll(set, 1, 2, 4, 5);
			var other = Sequence(1, 2, 3);

			Assert.False(set.IsSubsetOf(other));
		}

		[Test]
		public void IsProperSubsetOf_EqualSets_False()
		{
			AddAll(set, 1, 2, 3, 4, 5);
			var other = Sequence(1, 2, 3, 4, 5);

			Assert.False(set.IsProperSubsetOf(other));
		}

		[Test]
		public void IsProperSubsetOf_OneExtraElement_True()
		{
			AddAll(set, 1, 2, 3);
			var other = Sequence(0, 1, 2, 3);

			Assert.True(set.IsProperSubsetOf(other));
		}

		[Test]
		public void EmptySet_IsSupersetOfEmptySet()
		{
			Assert.True(set.IsSupersetOf(Sequence<int>()));
		}

		[Test]
		public void IsSupersetOf_EqualSets_True()
		{
			AddAll(set, 1, 2, 3);
			var other = Sequence(1, 3, 2);

			Assert.True(set.IsSupersetOf(other));
		}

		[Test]
		public void IsProperSupersetOf_EqualSets_False()
		{
			AddAll(set, 1, 2, 3);
			var other = Sequence(3, 2, 1);

			Assert.False(set.IsProperSupersetOf(other));
		}

		[Test]
		public void IsProperSupersetOf_OneExtra_True()
		{
			AddAll(set, 0, 1, 2, 3);
			var other = Sequence(1, 3, 2);

			Assert.True(set.IsProperSupersetOf(other));
		}

		[Test]
		public void Overlaps_OneInCommon_True() 
		{
			AddAll(set, 0, 1, 2, 3);
			var other = Sequence(3, 4, 5, 6);

			Assert.True(set.Overlaps(other));
		}

		[Test]
		public void Overlaps_NoneInCommon_False()
		{
			AddAll(set, 0, 1, 2, 3);
			var other = Sequence(4, 5, 6, 7);

			Assert.False(set.Overlaps(other));
		}
	}

	[TestFixture]
	public class SetTests : SetBaseTests
	{
		protected override ISet<T> Create<T>() => new Set<T>();
	}
}
