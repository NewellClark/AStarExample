using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Graphs.Tests
{
	[TestFixture]
	public class PathTests
	{
		private class Node
		{
			public Node(int position)
			{
				this.Position = position;
			}

			public int Position { get; }

			/// <summary>
			/// Computes cost between nodes as the absolute value of the difference in their positions.
			/// </summary>
			/// <param name="left"></param>
			/// <param name="right"></param>
			/// <returns></returns>
			public static int ComputeCost(Node left, Node right) => Math.Abs(left.Position - right.Position);
			public static int AddCosts(int left, int right) => left + right;
		}

		/// <summary>
		/// Creates a new empty path.
		/// </summary>
		/// <returns></returns>
		private Path<Node, int> Create() => Path.Create<Node, int>(Node.ComputeCost, Node.AddCosts, 0);

		private Path<Node, int> Create(params int[] positions)
		{
			var result = Create();
			foreach (int position in positions)
				result = result.Push(new Node(position));

			return result;
		}

		private IEnumerable<T> Iterate<T>(params T[] items) => items.AsEnumerable();

		private Path<Node, int> PushAll(Path<Node, int> path, params Node[] nodes)
		{
			var result = path;
			foreach (Node node in nodes)
				result = result.Push(node);

			return result;
		}

		[Test]
		public void AnEmptyPath_HasInitialCost()
		{
			const int initial = 4;
			var path = Path.Create<Node, int>(Node.ComputeCost, Node.AddCosts, initial);

			Assert.AreEqual(initial, path.Cost);
		}

		[Test]
		public void AnEmptyPath_ThrowsWhenPopped() 
		{
			var path = Create();

			void test() => path.Pop();

			Assert.Throws<InvalidOperationException>(test);
		}

		[Test]
		public void AnEmptyPath_ThrowsWhenPeeked()
		{
			var path = Create();

			void test() => path.Peek();

			Assert.Throws<InvalidOperationException>(test);
		}

		[Test]
		public void SingleNodePath_HasInitialCost()
		{
			const int initial = 5;
			var path = Path.Create<Node, int>(Node.ComputeCost, Node.AddCosts, initial);
			path = path.Push(new Node(2));

			Assert.AreEqual(initial, path.Cost);
		}

		[Test]
		public void NonEmptyPath_ComputesCostOfTraversingAllEdges()
		{
			var path = Create(1, 3, 2, -2);
			const int expected = 7;     //	2 + 1 + 4

			Assert.AreEqual(expected, path.Cost);
		}

		[Test]
		public void Nodes_EnumeratesHeadToTail()
		{
			Node first = new Node(2);
			Node second = new Node(5);
			Node third = new Node(3);
			var path = PushAll(Create(), first, second, third);

			var expected = Iterate(third, second, first);

			CollectionAssert.AreEqual(expected, path.Nodes);
		}

		[Test]
		public void Push_DoesNotModifyOriginal()
		{
			Node first = new Node(4);
			Node second = new Node(9);
			var original = Create().Push(first);
			var pushed = original.Push(second);

			var expected = Iterate(first);

			Assume.That(pushed.Nodes.SequenceEqual(Iterate(second, first)));

			CollectionAssert.AreEqual(expected, original.Nodes);
		}

		[Test]
		public void Pop_DoesNotModifyOriginal()
		{
			Node first = new Node(17);
			Node second = new Node(6);
			var original = Create().Push(first).Push(second);
			var popped = original.Pop();

			Assume.That(popped.Nodes.SequenceEqual(Iterate(first)));

			var expected = Iterate(second, first);

			CollectionAssert.AreEqual(expected, original.Nodes);
		}

		[Test]
		public void PoppingSingleNodePath_ProducesEmptyPath()
		{
			var original = Create(-6);
			var empty = original.Pop();

			CollectionAssert.IsEmpty(empty.Nodes);
		}


	}
}
