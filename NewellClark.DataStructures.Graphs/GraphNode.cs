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
		/// <summary>
		/// Creates a new <see cref="GraphNode{T}"/> with the specified value.
		/// </summary>
		/// <param name="value">The value of the <see cref="GraphNode{T}"/>.</param>
		public GraphNode(T value)
		{
			this.Value = value;
			this.Neighbors = new NeighborCollection(this);
		}

		/// <summary>
		/// Creates a new <see cref="GraphNode{T}"/> with the specified value, and the specified
		/// collection of neighbors.
		/// </summary>
		/// <param name="value">The value of the <see cref="GraphNode{T}"/>.</param>
		/// <param name="neighbors">The neighbors of the <see cref="GraphNode{T}"/>.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="neighbors"/> is null.
		/// </exception>
		public GraphNode(T value, IEnumerable<GraphNode<T>> neighbors)
		{
			if (neighbors is null) throw new ArgumentNullException(nameof(neighbors));

			this.Value = value;
			this.Neighbors = new NeighborCollection(this);

			foreach (var neighbor in neighbors)
				this.Neighbors.Add(neighbor);
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

		/// <summary>
		/// Overridden to return <see cref="Value"/>'s <see cref="ToString"/> method, or the empty string
		/// if <see cref="Value"/> is null.
		/// </summary>
		/// <returns>
		/// <see cref="Value"/>'s <see cref="ToString"/> implementation, or <see cref="String.Empty"/> if
		/// <see cref="Value"/> is null.
		/// </returns>
		public override string ToString() => Value?.ToString() ?? string.Empty;
	}
}
