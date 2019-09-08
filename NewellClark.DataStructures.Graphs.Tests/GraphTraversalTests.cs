using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Graphs.Tests
{
	[TestFixture]
	public abstract class TraversalTests
	{
		public abstract IEnumerable<MutableNode<string>> Traverse(MutableNode<string> root);

		private static MutableNode<string> GetNode(string text) => new MutableNode<string>(text);

		private static MutableNode<string> GetNode() => GetNode("dummy");

		private static IEnumerable<MutableNode<string>> GetNodes(int count, string text)
		{
			IEnumerable<MutableNode<string>> iterator()
			{
				for (int i = 0; i < count; i++)
					yield return GetNode($"{text} [{i}]");
			}

			return iterator().ToArray();
		}

		private static IEnumerable<MutableNode<string>> GetNodes(int count) => GetNodes(count, string.Empty);

		private static IEnumerable<MutableNode<string>> GetNodes(params MutableNode<string>[] nodes) => nodes.AsEnumerable();

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

			IEnumerable<MutableNode<string>> expected()
			{
				var result = new HashSet<MutableNode<string>>();
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

			IEnumerable<MutableNode<string>> expected()
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
			void makeCycle(MutableNode<string> a, MutableNode<string> b, MutableNode<string> c)
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

			IEnumerable<MutableNode<string>> expected()
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


		[TestFixture]
		public class DepthFirst : TraversalTests
		{
			public override IEnumerable<MutableNode<string>> Traverse(MutableNode<string> root)
			{
				return root.TraverseDepthFirst();
			}
		}

		[TestFixture]
		public class BreadthFirst : TraversalTests
		{
			public override IEnumerable<MutableNode<string>> Traverse(MutableNode<string> root)
			{
				return root.TraverseBreadthFirst();
			}
		}
	}

}
