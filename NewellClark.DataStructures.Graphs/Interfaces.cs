using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Graphs
{
	/// <summary>
	/// Represents a node that has neighbors.
	/// </summary>
	/// <typeparam name="TNode">The type of each neighbor.</typeparam>
	public interface IHasNeighbors<out TNode>
	{
		/// <summary>
		/// Gets a collection of all adjacent nodes.
		/// </summary>
		IEnumerable<TNode> Neighbors { get; }
	}

	/// <summary>
	/// A node with a collection of neighbor nodes that supports adding and removing neighbors.
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	public interface IMutableNode<TNode> : IHasNeighbors<TNode>
		where TNode : IMutableNode<TNode>
	{
		new INeighborCollection<TNode> Neighbors { get; }
	}

	/// <summary>
	/// A node with a collection of neighbor nodes that supports adding and removing neighbors. Each node 
	/// carries a value.
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	/// <typeparam name="TValue"></typeparam>
	public interface IMutableNode<TNode, TValue> : IMutableNode<TNode>
		where TNode: IMutableNode<TNode, TValue>
	{
		TValue Value { get; }
	}

	/// <summary>
	/// A collection of neighbor nodes.
	/// </summary>
	/// <typeparam name="TNode"></typeparam>
	public interface INeighborCollection<TNode> : ICollection<TNode>
	{
		/// <summary>
		/// Attempts to add the specified node to the collection. Returns a value indicating whether 
		/// the neighbor node was added.
		/// </summary>
		/// <param name="neighbor">The node to add.</param>
		/// <returns>True if the specified node was added, false otherwise.</returns>
		new bool Add(TNode neighbor);
	}

	public static class HasNeighborsExtensions
	{
		public static IEnumerable<TNode> TraverseDepthFirst<TNode>(this TNode @this)
			where TNode: IHasNeighbors<TNode>
		{
			if (@this == null) throw new ArgumentNullException();

			IEnumerable<TNode> iterator()
			{
				var seen = new HashSet<TNode>();
				var stack = new Stack<TNode>();
				seen.Add(@this);
				stack.Push(@this);

				void pushNeighbors(TNode node)
				{
					foreach (var neighbor in node.Neighbors)
						if (seen.Add(neighbor))
							stack.Push(neighbor);
				}

				while (stack.Count > 0)
				{
					var node = stack.Pop();
					pushNeighbors(node);

					yield return node;
				}
			}

			return iterator();
		}

		public static IEnumerable<TNode> TraverseBreadthFirst<TNode>(this TNode @this)
			where TNode: IHasNeighbors<TNode>
		{
			if (@this == null) throw new ArgumentNullException(nameof(@this));

			IEnumerable<TNode> iterator()
			{
				var seen = new HashSet<TNode>();
				var queue = new Queue<TNode>();
				seen.Add(@this);
				queue.Enqueue(@this);

				void enqueueNeighbors(TNode node)
				{
					foreach (var neighbor in node.Neighbors)
						if (seen.Add(neighbor))
							queue.Enqueue(neighbor);
				}

				while (queue.Count > 0)
				{
					var node = queue.Dequeue();
					enqueueNeighbors(node);

					yield return node;
				}
			}

			return iterator();
		}
	}
}
