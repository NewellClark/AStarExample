using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Graphs
{

	public static class AStar
	{
	}

	/// <summary>
	/// A function for computing the cost of traveling between two nodes.
	/// </summary>
	/// <typeparam name="TNode">The type of node.</typeparam>
	/// <typeparam name="TCost">The type of cost.</typeparam>
	/// <param name="first">The first node.</param>
	/// <param name="second">The second node.</param>
	/// <returns>The cost of traveling between the two nodes.</returns>
	public delegate TCost CostFunction<in TNode, out TCost>(TNode first, TNode second);

	/// <summary>
	/// Adds two costs together.
	/// </summary>
	/// <typeparam name="TCost">The type of cost.</typeparam>
	/// <param name="left">The left cost.</param>
	/// <param name="right">The right cost.</param>
	/// <returns>The result of adding the two costs together.</returns>
	public delegate TCost CostAdder<TCost>(TCost left, TCost right);
}
