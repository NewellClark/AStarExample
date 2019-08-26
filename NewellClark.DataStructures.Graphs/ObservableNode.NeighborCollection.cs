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
		public class NeighborCollection : Set<ObservableNode<T>>
		{
			private readonly ObservableNode<T> _owner;

			internal NeighborCollection(ObservableNode<T> owner)
			{
				Debug.Assert(owner != null);

				_owner = owner;
			}

			protected override bool AddItem(ObservableNode<T> item)
			{
				if (item is null) throw new ArgumentNullException(nameof(item));

				if (item == _owner)
					return false;

				//	We must ensure that neighboring is symmetrical: If you're my neighbor, 
				//		then I'm also your neighbor. If they're in our collection, then 
				//		we must also be in their collection. 

				//	We also must ensure that events don't get raised until both nodes 
				//		are added to each others' neighbor collections. Otherwise, event 
				//		handlers would be able to observe the graph in an invalid state.

				bool weAddedThem = Items.Add(item);
				bool theyAddedUs = item.Neighbors.Items.Add(_owner);

				Debug.Assert(weAddedThem == theyAddedUs);

				if (weAddedThem)
				{
					var ownerArgs = CreateEventArgs(_owner, item);
					var neighborArgs = CreateEventArgs(item, _owner);

					_owner.OnNodesAdded(ownerArgs);
					item.OnNodesAdded(neighborArgs);
				}

				return weAddedThem;
			}

			protected override bool RemoveItem(ObservableNode<T> item)
			{
				if (item is null) throw new ArgumentNullException(nameof(item));

				bool weRemovedThem = Items.Remove(item);
				bool theyRemovedUs = item.Neighbors.Items.Remove(_owner);

				Debug.Assert(weRemovedThem == theyRemovedUs);

				if (weRemovedThem)
				{
					var ownerArgs = CreateEventArgs(_owner, item);
					var neighborArgs = CreateEventArgs(item, _owner);

					_owner.OnNodesRemoved(ownerArgs);
					item.OnNodesRemoved(neighborArgs);
				}

				return weRemovedThem;
			}

			protected override bool ContainsItem(ObservableNode<T> item)
			{
				if (item is null) throw new ArgumentNullException(nameof(item));

				return base.ContainsItem(item);
			}

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
