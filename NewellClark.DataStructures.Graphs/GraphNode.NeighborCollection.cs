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
		/// <summary>
		/// A collection of neighboring <see cref="GraphNode{T}"/>s.
		/// </summary>
		public class NeighborCollection : Set<GraphNode<T>>
		{
			/// <summary>
			/// The <see cref="GraphNode{T}"/> that owns the current <see cref="NeighborCollection"/>.
			/// </summary>
			private readonly GraphNode<T> _owner;

			internal NeighborCollection(GraphNode<T> owner)
			{
				Debug.Assert(owner != null, $"{nameof(owner)} was null.");

				_owner = owner;
			}
			

			/// <summary>
			/// Adds the specified node to the collection. If the node was added, the node that owns
			/// the current <see cref="NeighborCollection"/> is added to other node's <see cref="NeighborCollection"/>.
			/// </summary>
			/// <param name="node">The node to add.</param>
			/// <returns>True if added, false otherwise.</returns>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="node"/> was null.
			/// </exception>
			protected override bool AddItem(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				if (node == _owner)
					return false;

				bool neighborAdded = Items.Add(node);
				bool ownerAdded = node.Neighbors.Items.Add(_owner);
				
				Debug.Assert(neighborAdded == ownerAdded,
					$"Our {nameof(NeighborCollection)} was not consistent with their {nameof(NeighborCollection)}.");

				return neighborAdded;
			}

			/// <summary>
			/// Removes the specified node. If the node was removed, the node that owns
			/// the current <see cref="NeighborCollection"/> is removed from the other node's 
			/// <see cref="NeighborCollection"/>.
			/// </summary>
			/// <param name="node">The node to remove.</param>
			/// <returns>True if removed, false if not.</returns>
			/// <exception cref="ArgumentNullException">
			/// <paramref name="node"/> was null.
			/// </exception>
			protected override bool RemoveItem(GraphNode<T> node)
			{
				if (node == null)
					throw new ArgumentNullException(nameof(node));

				bool neighborRemoved = Items.Remove(node);
				bool ownerRemoved = node.Neighbors.Items.Remove(_owner);
				
				Debug.Assert(neighborRemoved == ownerRemoved, InconsistentAssertMessage);

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

					Debug.Assert(wasRemoved, InconsistentAssertMessage);
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

				bool weContainThem = Items.Contains(node);
				bool theyContainUs = node.Neighbors.Items.Contains(_owner);

				Debug.Assert(weContainThem == theyContainUs, InconsistentAssertMessage);

				return weContainThem;
			}

			
			private static string InconsistentAssertMessage 
			{
				get
				{
					return $"Our {nameof(NeighborCollection)} was not consistent with their {nameof(NeighborCollection)}.";
				}

			}
		}
	}
}
