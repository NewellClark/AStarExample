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
	/// <typeparam name="TNode">Type of node.</typeparam>
	public interface IMutableNode<TNode> : IHasNeighbors<TNode>
		where TNode : IMutableNode<TNode>
	{
		/// <summary>
		/// Gets a set containing all neighbor nodes of the current <see cref="IMutableNode{TNode}"/>.
		/// </summary>
		new ISet<TNode> Neighbors { get; }
	}

	/// <summary>
	/// A node with a collection of neighbor nodes that supports adding and removing neighbors. Each node 
	/// carries a value.
	/// </summary>
	/// <typeparam name="TNode">The type of node.</typeparam>
	/// <typeparam name="TValue">The type of value that each node carries.</typeparam>
	public interface IMutableNode<TNode, TValue> : IMutableNode<TNode>
		where TNode: IMutableNode<TNode, TValue>
	{
		/// <summary>
		/// Gets the value stored in the current <see cref="IMutableNode{TNode, TValue}"/>.
		/// </summary>
		TValue Value { get; }
	}

	public static class NodeExtensions
	{
		/// <summary>
		/// Performs a depth-first traversal of the specified node.
		/// </summary>
		/// <typeparam name="TNode">Type of node.</typeparam>
		/// <param name="this">The root node.</param>
		/// <returns>
		/// A lazily-evaluated sequence of all nodes found in the search.
		/// </returns>
		/// <remarks>
		/// The sequence is lazily evaluated on demand.
		/// </remarks>
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

		/// <summary>
		/// Performs a breadth-first traversal of the specified node.
		/// </summary>
		/// <typeparam name="TNode">Type of node.</typeparam>
		/// <param name="this">The root node to start the search at.</param>
		/// <returns>A lazily-evaluated sequence representing the search.</returns>
		/// <remarks>
		/// The sequence is lazily-evaluated on demand.
		/// </remarks>
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
