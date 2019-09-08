using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Graphs
{
	static class GraphNodeExtensions
	{
		/// <summary>
		/// Adds all the specified nodes to the current <see cref="MutableNode{T}.NeighborCollection"/>.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="this"></param>
		/// <param name="newNeighbors">Nodes to add.</param>
		/// <returns>The number of nodes that were added.</returns>
		public static int Add<T>(this MutableNode<T>.NeighborCollection @this, IEnumerable<MutableNode<T>> newNeighbors)
		{
			int added = 0;
			foreach (var node in newNeighbors)
				if (@this.Add(node))
					++added;

			return added;
		}
	}
}
