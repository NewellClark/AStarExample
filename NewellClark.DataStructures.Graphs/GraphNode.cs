using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Graphs
{
	public partial class GraphNode<T> : IHasNeighbors<GraphNode<T>>
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

		public T Value { get; }

		public NeighborCollection Neighbors { get; }

		IEnumerable<GraphNode<T>> IHasNeighbors<GraphNode<T>>.Neighbors => this.Neighbors;

		public override string ToString() => Value?.ToString() ?? string.Empty;
	}
}
