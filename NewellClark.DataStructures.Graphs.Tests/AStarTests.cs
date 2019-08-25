using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Graphs.Tests
{
	[TestFixture]
	public class AStarTests
	{
		/// <summary>
		/// Returns the absolute value of the difference of the values of each node.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		private int CostFunction(GraphNode<int> first, GraphNode<int> second)
		{
			return Math.Abs(first.Value - second.Value);
		}

		private int CostAdder(int left, int right) => left + right;

		/// <summary>
		/// Creates a new node with the specified value and connects it to the specified node.
		/// </summary>
		/// <param name="node">The node to connect to the new node.</param>
		/// <param name="neighborValue">The value to give the new node.</param>
		/// <returns>The newly-created neighbor node.</returns>
		private GraphNode<int> Add(GraphNode<int> node, int neighborValue)
		{
			var neighbor = Create(neighborValue);
			node.Neighbors.Add(neighbor);

			return neighbor;
		}

		private GraphNode<T> Create<T>(T value) => new GraphNode<T>(value);

		private IEnumerable<T> Iterate<T>(params T[] elements) => elements.AsEnumerable();

		[Test]
		public void SingleNodePath_WhenGoalSameAsStart()
		{
			var startAndEnd = Create(-1);
			var expected = Iterate(startAndEnd);

			var path = AStar.FindPath(startAndEnd, startAndEnd, CostFunction, CostFunction, CostAdder);

			CollectionAssert.AreEqual(expected, path);
		}

		[Test]
		public void EmptyPath_WhenEndPointsDisconnected()
		{
			var start = Create(5);
			var end = Create(7);

			Assume.That(!start.TraverseBreadthFirst().Contains(end));

			var expected = Iterate<GraphNode<int>>();
			var actual = AStar.FindPath(start, end, CostFunction, CostFunction, CostAdder);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void CompletePath_WhenEndpointsAreConnected()
		{
			var start = Create(0);
			var middle = Add(start, 2);
			var end = Add(middle, 1);

			Assume.That(start.TraverseBreadthFirst().Contains(end));

			var expected = Iterate(end, middle, start);
			var actual = AStar.FindPath(start, end, CostFunction, CostFunction, CostAdder);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void ShortestPath_WhenMultiplePathsExist()
		{
			var start = Create(3);
			var cheapMiddle = Add(start, 4);
			var expensiveMiddle = Add(start, 5);
			var end = Create(2);
			end.Neighbors.Add(cheapMiddle);
			end.Neighbors.Add(expensiveMiddle);

			//	Cheap cost: 3, Expensive cost: 4

			var expected = Iterate(end, cheapMiddle, start);
			var actual = AStar.FindPath(start, end, CostFunction, CostFunction, CostAdder);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void Terminates_WhenGraphHasCycles()
		{
			var start = Create(4);
			var cycle1 = Add(start, 4);
			var cycle2 = Add(cycle1, 4);
			cycle2.Neighbors.Add(start);

			var expensive1 = Add(cycle1, 17);
			var expensive2 = Add(expensive1, -26);
			var end = Add(expensive2, 100);

			var expected = Iterate(end, expensive2, expensive1, cycle1, start);
			var actual = AStar.FindPath(start, end, CostFunction, CostFunction, CostAdder);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void IgnoresNodes_WhenExcludedByFilter()
		{
			bool filter(GraphNode<int> node) => node.Value % 3 != 0;

			var start = Create(2);
			var illegalCheap = Add(start, 3);
			var legalExpensive = Add(start, -25);
			var end = Create(5);
			illegalCheap.Neighbors.Add(end);
			legalExpensive.Neighbors.Add(end);

			var expected = Iterate(end, legalExpensive, start);
			var actual = AStar.FindPath(start, end, CostFunction, CostFunction, CostAdder, filter, 0);

			CollectionAssert.AreEqual(expected, actual);
		}

		[Test]
		public void FindsNoPath_WhenFilterBlocksGoal()
		{
			bool filter(GraphNode<int> node) => node.Value >= 0;

			var start = Create(5);
			var middle = Add(start, 7);
			var blockedGoal = Add(middle, -14);

			var path = AStar.FindPath(start, blockedGoal, CostFunction, CostFunction, CostAdder, filter);

			CollectionAssert.IsEmpty(path);
		}

		[Test]
		public void FindsNoPath_WhenFilterBlocksStart()
		{
			bool filter(GraphNode<int> node) => node.Value >= 0;

			var start = Create(-1);
			var middle = Add(start, 0);
			var end = Add(middle, 1);

			var path = AStar.FindPath(start, end, CostFunction, CostFunction, CostAdder, filter);

			CollectionAssert.IsEmpty(path);
		}
	}
}
