using NewellClark.PathViewer.MvvmGridGraph;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewellClark.ReflectionHelpers;

namespace NewellClark.PathViewer.Tests
{
	[TestFixture]
	public class GridGraphTests
	{
		private GridGraph graph;
		private IntVector2 testPosition;

		/// <summary>
		/// Gets an <see cref="IntVector2"/>.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private static IntVector2 Vec(int x, int y) => new IntVector2(x, y);
		
		/// <summary>
		/// Gets { (1, 0), (0, 1), (-1, 0), (0, -1) }
		/// </summary>
		private static IEnumerable<IntVector2> OrthogonalOffsets
		{
			get
			{
				yield return Vec(1, 0);
				yield return Vec(0, 1);
				yield return Vec(-1, 0);
				yield return Vec(0, -1);
			}
		}

		/// <summary>
		/// Gets { (1, -1), (1, 1), (-1, 1), (-1, -1) }
		/// </summary>
		private static IEnumerable<IntVector2> DiagonalOffsets
		{
			get
			{
				yield return Vec(1, -1);
				yield return Vec(1, 1);
				yield return Vec(-1, 1);
				yield return Vec(-1, -1);
			}
		}

		private static IEnumerable<T> Iterate<T>(params T[] values) => values.AsEnumerable();

		/// <summary>
		/// Asserts that changing the specified property on a <see cref="GridNode"/> will update
		/// the value of that property for any <see cref="GridNode"/> at that position in the
		/// <see cref="GridGraph"/>. 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="propertyName"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		private void AssertProperty_UpdatesValueInGraph<T>(string propertyName, T oldValue, T newValue)
		{
			GridNode node = graph[testPosition];
			var property = SafeProperty.Create<T, GridNode>(propertyName, node);
			property.Value = oldValue;

			Assume.That(EqualityComparer<T>.Default.Equals(property.Value, oldValue));
			if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
				Assert.Warn($"Bad test arguments. {nameof(oldValue)} and {nameof(newValue)} must be different.");

			property.Value = newValue;

			var refetchedNode = graph[testPosition];
			var refetchedNodeProperty = SafeProperty.Create<T, GridNode>(propertyName, refetchedNode);

			Assert.AreEqual(newValue, refetchedNodeProperty.Value);
		}

		private void AssertProperty_PushesEventWhenChanged<T>(string propertyName, T oldValue, T newValue)
		{
			var node = graph[testPosition];
			var property = SafeProperty.Create<T, GridNode>(propertyName, node);
			property.Value = oldValue;
			var events = new List<GridNode>();
			graph.NodeChanged.Subscribe(n => events.Add(n));

			property.Value = newValue;
			var expected = Iterate(node);

			CollectionAssert.AreEqual(expected, events);
		}

		private void AssertProperty_DoesNotPushEventWhenNotChanged<T>(
			string propertyName, T oldValue, T newValue)
		{
			var node = graph[testPosition];
			var property = SafeProperty.Create<T, GridNode>(propertyName, node);
			property.Value = oldValue;
			property.Value = newValue;
			var events = new List<GridNode>();
			graph.NodeChanged.Subscribe(n => events.Add(n));

			property.Value = newValue;

			CollectionAssert.IsEmpty(events);
		}

		[SetUp]
		public void Initialize()
		{
			graph = new GridGraph();
			testPosition = Vec(17, -41);
		}


		[Test]
		public void IsPassable_MutatesValueInGraph()
		{
			AssertProperty_UpdatesValueInGraph(nameof(GridNode.IsPassable), true, false);
			AssertProperty_UpdatesValueInGraph(nameof(GridNode.IsPassable), false, true);
		}

		[Test]
		public void IsPassable_PushesEventWhenChanged()
		{
			AssertProperty_PushesEventWhenChanged(
				nameof(GridNode.IsPassable), true, false);
			AssertProperty_PushesEventWhenChanged(
				nameof(GridNode.IsPassable), false, true);
		}

		[Test]
		public void IsPassable_DoesNotPushEventWhenNotChanged()
		{
			AssertProperty_DoesNotPushEventWhenNotChanged(
				nameof(GridNode.IsPassable), true, false);
			AssertProperty_DoesNotPushEventWhenNotChanged(
				nameof(GridNode.IsPassable), false, true);
		}
	}
}
