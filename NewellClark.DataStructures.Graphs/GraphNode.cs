using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Graphs
{
	/// <summary>
	/// A simple graph node that can connect to other nodes.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public partial class GraphNode<T> : IMutableNode<GraphNode<T>, T>
	{
		public GraphNode(T value)
		{
			this.Value = value;
			this.Neighbors = new NeighborCollection(this);
		}

		public GraphNode(T value, IEnumerable<GraphNode<T>> neighbors)
		{
			if (neighbors is null)
				throw new ArgumentNullException(nameof(neighbors));

			this.Value = value;
			this.Neighbors = new NeighborCollection(this, neighbors);
		}

		/// <summary>
		/// Gets the value of the current <see cref="GraphNode{T}"/>.
		/// </summary>
		public T Value { get; }

		/// <summary>
		/// Gets a collection of all the nodes that are directly connected to the current 
		/// <see cref="GraphNode{T}"/>.
		/// </summary>
		public NeighborCollection Neighbors { get; }

		ISet<GraphNode<T>> IMutableNode<GraphNode<T>>.Neighbors => Neighbors;

		IEnumerable<GraphNode<T>> IHasNeighbors<GraphNode<T>>.Neighbors => this.Neighbors;

		public override string ToString() => Value?.ToString() ?? string.Empty;
	}
}
