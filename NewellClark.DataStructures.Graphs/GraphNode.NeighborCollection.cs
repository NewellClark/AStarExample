using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewellClark.DataStructures.Collections;

namespace NewellClark.DataStructures.Graphs
{
	partial class GraphNode<T>
	{
		public class NeighborCollection : Set<GraphNode<T>>
		{
			/// <summary>
			/// The <see cref="GraphNode{T}"/> that owns the current <see cref="NeighborCollection"/>.
			/// </summary>
			private readonly GraphNode<T> _owner;

			internal NeighborCollection(GraphNode<T> owner, IEnumerable<GraphNode<T>> neighbors)
			{
				Debug.Assert(owner != null, $"{nameof(owner)} was null.");
				Debug.Assert(neighbors != null, $"{nameof(neighbors)} was null.");
				Debug.Assert(!neighbors.Contains(_owner), 
					$"{nameof(neighbors)} should never contain {nameof(owner)}.");

				_owner = owner;
			}

			internal NeighborCollection(GraphNode<T> owner)
			{
				Debug.Assert(owner != null, $"{nameof(owner)} was null.");

				_owner = owner;
			}
			

			/// <summary>
			/// Adds the specified node to the collection.
			/// </summary>
			/// <param name="node">The node to add.</param>
			/// <returns>True if added, false otherwise.</returns>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="node"/> was null.</exception>
			protected override bool AddItem(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				if (node == _owner)
					return false;

				bool neighborAdded = Items.Add(node);
				bool ownerAdded = node.Neighbors.Items.Add(_owner);

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
			protected override bool RemoveItem(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				bool neighborRemoved = Items.Remove(node);
				bool ownerRemoved = node.Neighbors.Items.Remove(_owner);

				Debug.Assert(neighborRemoved == ownerRemoved);

				return neighborRemoved;
			}

			/// <summary>
			/// Removes all <see cref="GraphNode{T}"/>s from the collection.
			/// </summary>
			protected override void ClearItems()
			{
				var removed = Items.ToArray();
				Items.Clear();

				foreach (var node in removed)
				{
					bool wasRemoved = node.Neighbors.Items.Remove(_owner);

					Debug.Assert(wasRemoved);
				}
			}

			/// <summary>
			/// Returns a value indicating whether <paramref name="node"/> is in the collection.
			/// </summary>
			/// <param name="node">The node to look for.</param>
			/// <returns>True if <paramref name="node"/> is in the collection. False otherwise.</returns>
			protected override bool ContainsItem(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				return base.ContainsItem(node);
			}
		}
	}
}
