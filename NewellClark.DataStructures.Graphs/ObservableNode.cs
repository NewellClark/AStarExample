using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NewellClark.DataStructures.Graphs
{
	public partial class ObservableNode<T> : IMutableNode<ObservableNode<T>, T> 
	{
		public ObservableNode(T value)
		{
			this.Value = value;
			Neighbors = new NeighborCollection(this);
		}

		public T Value { get; }

		public NeighborCollection Neighbors { get; }

		/// <summary>
		/// Raised when one or more nodes get added to <see cref="Neighbors"/>.
		/// </summary>
		public event EventHandler<NodeChangedEventArgs<ObservableNode<T>>> NodesAdded;

		/// <summary>
		/// Raised when one or more nodes get removed from <see cref="Neighbors"/>.
		/// </summary>
		public event EventHandler<NodeChangedEventArgs<ObservableNode<T>>> NodesRemoved;

		/// <summary>
		/// Raises the <see cref="NodesAdded"/> event.
		/// </summary>
		/// <param name="e">The argument to pass to the event.</param>
		private void OnNodesAdded(NodeChangedEventArgs<ObservableNode<T>> e)
		{
			Debug.Assert(e != null);

			NodesAdded?.Invoke(this, e);
		}

		/// <summary>
		/// Raises the <see cref="NodesRemoved"/> event.
		/// </summary>
		/// <param name="e">The argument to pass to the event.</param>
		private void OnNodesRemoved(NodeChangedEventArgs<ObservableNode<T>> e)
		{
			Debug.Assert(e != null);

			NodesRemoved?.Invoke(this, e);
		}

		INeighborCollection<ObservableNode<T>> IMutableNode<ObservableNode<T>>.Neighbors => Neighbors;

		IEnumerable<ObservableNode<T>> IHasNeighbors<ObservableNode<T>>.Neighbors => Neighbors;
	}

	/// <summary>
	/// Arguments for the event that gets raised when a node gains or loses neighbors.
	/// </summary>
	/// <typeparam name="TNode">The type of node.</typeparam>
	public class NodeChangedEventArgs<TNode> : EventArgs
	{
		public NodeChangedEventArgs(TNode sender, params TNode[] neighbors)
			: this(sender, neighbors.AsEnumerable()) { }

		/// <summary>
		/// Creates a new <see cref="NodeChangedEventArgs{TNode}"/>.
		/// </summary>
		/// <param name="sender">The node that raised the event. Use null if raising a static event.</param>
		/// <param name="neighbors">The neighbors that were added or removed from <paramref name="sender"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="neighbors"/> was null.
		/// </exception>
		public NodeChangedEventArgs(TNode sender, IEnumerable<TNode> neighbors)
		{
			if (neighbors is null) throw new ArgumentNullException(nameof(neighbors));

			this.Sender = sender;
			this.Neighbors = neighbors;
		}

		/// <summary>
		/// Gets the node that raised the event.
		/// </summary>
		public TNode Sender { get; }

		/// <summary>
		/// Gets the neighbors that were added or removed. This will never be null.
		/// </summary>
		public IEnumerable<TNode> Neighbors { get; }
	}
}
