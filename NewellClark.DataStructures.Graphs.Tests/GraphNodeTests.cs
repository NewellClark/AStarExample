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
	public abstract partial class MutableNodeTests<TNode>
		where TNode: class, IMutableNode<TNode, string>
	{
		private const string text1 = "A";
		private const string text2 = "B";
		private const string text3 = "C";

		protected abstract TNode GetNode(string text);

		protected TNode GetNode() => GetNode("dummy");

		protected IEnumerable<TNode> GetNodes(int count, string text)
		{
			IEnumerable<TNode> iter()
			{
				for (int i = 0; i < count; i++)
					yield return GetNode($"{text} [{i}]");
			}

			return iter().ToArray();
		}

		private IEnumerable<TNode> GetNodes(int count) => GetNodes(count, string.Empty);

		private static IEnumerable<TNode> GetNodes(params TNode[] nodes)
		{
			return nodes.AsEnumerable();
		}
	}

	[TestFixture]
	public class GraphNodeTests : MutableNodeTests<GraphNode<string>>
	{
		protected override GraphNode<string> GetNode(string text) => new GraphNode<string>(text);
	}
}
