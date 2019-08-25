using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Graphs
{
	partial class GraphNode<T>
	{
		public class NeighborCollection : INeighborCollection<GraphNode<T>>
		{
			/// <summary>
			/// The <see cref="GraphNode{T}"/> that owns the current <see cref="NeighborCollection"/>.
			/// </summary>
			private readonly GraphNode<T> _owner;
			private readonly HashSet<GraphNode<T>> _nodes;

			internal NeighborCollection(GraphNode<T> owner, IEnumerable<GraphNode<T>> neighbors)
			{
				Debug.Assert(owner != null, $"{nameof(owner)} was null.");
				Debug.Assert(neighbors != null, $"{nameof(neighbors)} was null.");
				Debug.Assert(!neighbors.Contains(_owner), 
					$"{nameof(neighbors)} should never contain {nameof(owner)}.");

				_owner = owner;
				_nodes = new HashSet<GraphNode<T>>(neighbors);
			}

			internal NeighborCollection(GraphNode<T> owner)
			{
				Debug.Assert(owner != null, $"{nameof(owner)} was null.");

				_owner = owner;
				_nodes = new HashSet<GraphNode<T>>();
			}
			

			/// <summary>
			/// Adds the specified node to the collection.
			/// </summary>
			/// <param name="node">The node to add.</param>
			/// <returns>True if added, false otherwise.</returns>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="node"/> was null.</exception>
			public bool Add(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				if (node == _owner)
					return false;

				bool neighborAdded = _nodes.Add(node);
				bool ownerAdded = node.Neighbors._nodes.Add(_owner);

				Debug.Assert(neighborAdded == ownerAdded);

				return neighborAdded;
			}

			/// <summary>
			/// Removes the specified node.
			/// </summary>
			/// <param name="node">The node to remove.</param>
			/// <returns>True if removed, false if not.</returns>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="node"/> was null.</exception>
			public bool Remove(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				bool neighborRemoved = _nodes.Remove(node);
				bool ownerRemoved = node.Neighbors._nodes.Remove(_owner);

				Debug.Assert(neighborRemoved == ownerRemoved);

				return neighborRemoved;
			}

			/// <summary>
			/// Removes all <see cref="GraphNode{T}"/>s from the collection.
			/// </summary>
			public void Clear()
			{
				var neighbors = new GraphNode<T>[Count];
				CopyTo(neighbors, 0);
				_nodes.Clear();

				foreach (var node in neighbors)
				{
					bool removed = node.Neighbors._nodes.Remove(_owner);

					Debug.Assert(removed);
				}
			}

			/// <summary>
			/// Gets the number of nodes in the <see cref="NeighborCollection"/>.
			/// </summary>
			public int Count => _nodes.Count;

			/// <summary>
			/// Returns a value indicating whether <paramref name="node"/> is in the collection.
			/// </summary>
			/// <param name="node">The node to look for.</param>
			/// <returns>True if <paramref name="node"/> is in the collection. False otherwise.</returns>
			public bool Contains(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				return _nodes.Contains(node);
			}

			public void CopyTo(GraphNode<T>[] array, int arrayIndex) => _nodes.CopyTo(array, arrayIndex);

			public IEnumerator<GraphNode<T>> GetEnumerator() => _nodes.GetEnumerator();

			#region Explicit interface implimentations
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

			void ICollection<GraphNode<T>>.Add(GraphNode<T> item) => Add(item);

			bool ICollection<GraphNode<T>>.IsReadOnly => false;
			#endregion
		}
	}
}
