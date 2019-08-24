using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
//using MoreLinq.Extensions;

namespace NewellClark.DataStructures.Graphs
{
	[TestFixture]
	public partial class GraphNodeTests
	{
		private const string text1 = "A";
		private const string text2 = "B";
		private const string text3 = "C";

		private static GraphNode<string> GetNode(string text)
		{
			return new GraphNode<string>(text);
		}

		private static GraphNode<string> GetNode() => new GraphNode<string>("dummy");

		private static IEnumerable<GraphNode<string>> GetNodes(int count, string text)
		{
			IEnumerable<GraphNode<string>> iter()
			{
				for (int i = 0; i < count; i++)
					yield return GetNode($"{text} [{i}]");
			}

			return iter().ToArray();
		}

		private static IEnumerable<GraphNode<string>> GetNodes(int count) => GetNodes(count, string.Empty);

		private static IEnumerable<GraphNode<T>> GetNodes<T>(params GraphNode<T>[] nodes)
		{
			return nodes.AsEnumerable();
		}

		//[Test]
		//public void IsolatedGraphs_ReturnsSingleNode_WhenAllOneGraph()
		//{
		//	var primary = GetNode();
		//	var neighbors = GetNodes(5);
		//	primary.Neighbors.Add(neighbors);
		//	var input = neighbors.Append(primary);

		//	var actual = GraphNode.IsolatedGraphs(input);

		//	Assert.That(actual.Count() == 1);
		//	Assert.That(input.Contains(actual.Single()));
		//}

		//[Test]
		//public void IsolatedGraphs_ReturnsOneNodeFromEachGroup()
		//{
		//	var graph1 = GetNode();
		//	graph1.Neighbors.Add(GetNodes(4));

		//	var graph2 = GetNode();
		//	graph2.Neighbors.Add(GetNodes(4));

		//	var graph3 = GetNode();
		//	graph3.Neighbors.Add(GetNodes(4));

		//	var input = graph1.TraverseBreadthFirst()
		//		.Concat(graph2.TraverseBreadthFirst())
		//		.Concat(graph3.TraverseBreadthFirst());

		//	var actual = GraphNode.IsolatedGraphs(input);
			
		//}

		[TestFixture]
		public abstract class TraversalTests
		{
			public abstract IEnumerable<GraphNode<string>> Traverse(GraphNode<string> root);

			[Test]
			public void SingleNode()
			{
				var node = GetNode();
				var expected = Enumerable.Repeat(node, 1);
				var actual = Traverse(node);

				CollectionAssert.AreEquivalent(expected, actual);
			}

			[Test]
			public void HierarchyWithoutCycles()
			{
				var grandparent = GetNode("Grandparent");
				var parent = GetNode("Parent");

				grandparent.Neighbors.Add(parent);
				const int parentSiblings = 4;
				for (int i = 0; i < parentSiblings; i++) grandparent.Neighbors.Add(GetNode($"parent sibling {i}"));

				var child = GetNode("Child");
				parent.Neighbors.Add(child);
				const int childSiblings = 3;
				for (int i = 0; i < childSiblings; i++) parent.Neighbors.Add(GetNode($"child sibling {i}"));

				IEnumerable<GraphNode<string>> expected()
				{
					var result = new HashSet<GraphNode<string>>();
					result.Add(grandparent);
					result.Add(parent);
					result.Add(child);
					result.UnionWith(grandparent.Neighbors);
					result.UnionWith(parent.Neighbors);
					result.UnionWith(child.Neighbors);

					return result.AsEnumerable();
				}

				var actual = Traverse(parent);

				CollectionAssert.AreEquivalent(expected(), actual);
			}

			[Test]
			public void SingleCycle()
			{
				var a = GetNode("a");
				var b = GetNode("b");
				var c = GetNode("c");
				a.Neighbors.Add(b);
				b.Neighbors.Add(c);
				c.Neighbors.Add(a);

				var expected = GetNodes(a, b, c);
				var actual = Traverse(a);

				CollectionAssert.AreEquivalent(expected, actual);
			}

			[Test]
			public void CycleAndHierarchy()
			{
				var a = GetNode("A");
				var b = GetNode("B");
				var c = GetNode("C");
				a.Neighbors.Add(b);
				b.Neighbors.Add(c);
				c.Neighbors.Add(a);

				var extraA = GetNode("A extra");
				a.Neighbors.Add(extraA);

				var extraAChildren = GetNodes(3, "A extra child");
				extraA.Neighbors.Add(extraAChildren);

				IEnumerable<GraphNode<string>> expected()
				{
					yield return a;
					yield return b;
					yield return c;
					yield return extraA;

					foreach (var node in extraAChildren)
						yield return node;
				}
				var actual = Traverse(c);

				CollectionAssert.AreEquivalent(expected(), actual);
			}

			[Test]
			public void MultipleCycles()
			{
				void makeCycle(GraphNode<string> a, GraphNode<string> b, GraphNode<string> c)
				{
					a.Neighbors.Add(b);
					b.Neighbors.Add(c);
					c.Neighbors.Add(a);
				}

				var fire = GetNode("fire");
				var grass = GetNode("grass");
				var water = GetNode("water");
				makeCycle(fire, grass, water);

				var fighting = GetNode("fighting");
				var dark = GetNode("dark");
				var psychic = GetNode("psychic");
				makeCycle(fighting, dark, psychic);

				var polywrath = GetNode("polywrath");
				polywrath.Neighbors.Add(water);
				polywrath.Neighbors.Add(fighting);

				IEnumerable<GraphNode<string>> expected()
				{
					yield return fire;
					yield return grass;
					yield return water;

					yield return fighting;
					yield return dark;
					yield return psychic;

					yield return polywrath;
				}
				var actual = Traverse(grass);

				CollectionAssert.AreEquivalent(expected(), actual);
			}
		}

		[TestFixture]
		public class DepthFirst : TraversalTests
		{
			public override IEnumerable<GraphNode<string>> Traverse(GraphNode<string> root)
			{
				return root.TraverseDepthFirst();
			}
		}

		[TestFixture]
		public class DepthFirstExtensionMethod : TraversalTests
		{
			public override IEnumerable<GraphNode<string>> Traverse(GraphNode<string> root)
			{
				var casted = root as IHasNeighbors<GraphNode<string>>;

				return casted.TraverseDepthFirst()
					.Select(node => node as GraphNode<string>);
			}
		}

		[TestFixture]
		public class BreadthFirst : TraversalTests
		{
			public override IEnumerable<GraphNode<string>> Traverse(GraphNode<string> root)
			{
				return root.TraverseBreadthFirst();
			}
		}

		[TestFixture]
		public class BreadthFirstExtensionMethod : TraversalTests
		{
			public override IEnumerable<GraphNode<string>> Traverse(GraphNode<string> root)
			{
				var casted = root as IHasNeighbors<GraphNode<string>>;

				return casted.TraverseBreadthFirst()
					.Select(node => node as GraphNode<string>);
			}
		}
	}
}
