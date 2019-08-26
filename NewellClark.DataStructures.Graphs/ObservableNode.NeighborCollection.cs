using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewellClark.DataStructures.Collections;

namespace NewellClark.DataStructures.Graphs
{
	partial class ObservableNode<T>
	{
		/// <summary>
		/// A collection of neighboring <see cref="ObservableNode{T}"/>s.
		/// </summary>
		public class NeighborCollection : Set<ObservableNode<T>>
		{
			private readonly ObservableNode<T> _owner;

			internal NeighborCollection(ObservableNode<T> owner)
			{
				Debug.Assert(owner != null);

				_owner = owner;
			}

			/// <summary>
			/// Adds the specified node to the collection. If the node was added, the node that
			/// owns the current <see cref="NeighborCollection"/> is added to the other node's 
			/// <see cref="NeighborCollection"/>. The <see cref="NodesAdded"/> events are raised for both nodes
			/// only after both <see cref="NeighborCollection"/>s have been updated.
			/// </summary>
			/// <param name="node">The node to add.</param>
			/// <returns>
			/// True if the node was added.
			/// </returns>
			protected override bool AddItem(ObservableNode<T> node)
			{
				if (node is null) throw new ArgumentNullException(nameof(node));

				if (node == _owner)
					return false;

				//	We must ensure that neighboring is symmetrical: If you're my neighbor, 
				//		then I'm also your neighbor. If they're in our collection, then 
				//		we must also be in their collection. 

				//	We also must ensure that events don't get raised until both nodes 
				//		are added to each others' neighbor collections. Otherwise, event 
				//		handlers would be able to observe the graph in an invalid state.

				bool weAddedThem = Items.Add(node);
				bool theyAddedUs = node.Neighbors.Items.Add(_owner);

				Debug.Assert(weAddedThem == theyAddedUs);

				if (weAddedThem)
				{
					var ownerArgs = CreateEventArgs(_owner, node);
					var neighborArgs = CreateEventArgs(node, _owner);

					_owner.OnNodesAdded(ownerArgs);
					node.OnNodesAdded(neighborArgs);
				}

				return weAddedThem;
			}

			/// <summary>
			/// Removes the specified node from the collection. If the node was removed, the owner of the
			/// current <see cref="NeighborCollection"/> is removed from the other node's <see cref="NeighborCollection"/>.
			/// The <see cref="NodesRemoved"/> events for both nodes are raised only after both
			/// <see cref="NeighborCollection"/>s have been updated.
			/// </summary>
			/// <param name="node">The node to remove.</param>
			/// <returns>
			/// True if the node was removed.
			/// </returns>
			protected override bool RemoveItem(ObservableNode<T> node)
			{
				if (node is null) throw new ArgumentNullException(nameof(node));

				bool weRemovedThem = Items.Remove(node);
				bool theyRemovedUs = node.Neighbors.Items.Remove(_owner);

				Debug.Assert(weRemovedThem == theyRemovedUs);

				if (weRemovedThem)
				{
					var ownerArgs = CreateEventArgs(_owner, node);
					var neighborArgs = CreateEventArgs(node, _owner);

					_owner.OnNodesRemoved(ownerArgs);
					node.OnNodesRemoved(neighborArgs);
				}

				return weRemovedThem;
			}

			/// <summary>
			/// Determines whether the current <see cref="NeighborCollection"/> contains the specified node.
			/// </summary>
			/// <param name="node">The node to search for.</param>
			/// <returns>
			/// True if the current <see cref="NeighborCollection"/> contains the specified node.
			/// </returns>
			protected override bool ContainsItem(ObservableNode<T> node)
			{
				if (node is null) throw new ArgumentNullException(nameof(node));

				return base.ContainsItem(node);
			}

			/// <summary>
			/// Removes all nodes from the collection. Also removes the owner of the current
			/// <see cref="NeighborCollection"/> from the <see cref="NeighborCollection"/> of each
			/// node that was removed.
			/// </summary>
			protected override void ClearItems()
			{
				var removed = Items.ToArray();
				Items.Clear();

				foreach (var neighbor in removed)
				{
					bool theyRemovedUs = neighbor.Neighbors.Items.Remove(_owner);

					Debug.Assert(theyRemovedUs);
				}

				var ownerArgs = CreateEventArgs(_owner, removed);
				_owner.OnNodesRemoved(ownerArgs);

				foreach (var neighbor in removed)
				{
					var neighborArgs = CreateEventArgs(neighbor, _owner);
					neighbor.OnNodesRemoved(neighborArgs);
				}
			}

			private NodeChangedEventArgs<ObservableNode<T>> CreateEventArgs(
				ObservableNode<T> sender, IEnumerable<ObservableNode<T>> neighbors)
			{
				return new NodeChangedEventArgs<ObservableNode<T>>(sender, neighbors);
			}

			private NodeChangedEventArgs<ObservableNode<T>> CreateEventArgs(
				ObservableNode<T> sender, params ObservableNode<T>[] neighbors)
			{
				return CreateEventArgs(sender, neighbors.AsEnumerable());
			}
		}
	}
}
