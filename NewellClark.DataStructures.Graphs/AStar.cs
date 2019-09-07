using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewellClark.DataStructures.Collections;

namespace NewellClark.DataStructures.Graphs
{
	/// <summary>
	/// Contains methods for path-finding using the A* algorithm.
	/// </summary>
	public static class AStar
	{
		/// <summary>
		/// Attempts to find the path with the lowest cost between the specified start and goal nodes.
		/// </summary>
		/// <typeparam name="TNode">Type of node.</typeparam>
		/// <typeparam name="TCost">Type of cost.</typeparam>
		/// <param name="start">The starting node.</param>
		/// <param name="goal">The goal node.</param>
		/// <param name="knownCost">Function to calculate the known cost of traveling between 
		/// two adjacent nodes.</param>
		/// <param name="estimatedCost">
		/// Function to estimate the cost of traveling between two arbitrary nodes.
		/// The two nodes passed into this function are not guaranteed to be connected.</param>
		/// <param name="costAdder">Function to add two cost values together.</param>
		/// <param name="costComparer">Comparer used to choose the path with the lowest cost.</param>
		/// <param name="nodeFilter">Filter function to apply to nodes. Nodes will only be considered 
		/// if this function returns true for them.</param>
		/// <param name="initialCost">The initial cost value.</param>
		/// <exception cref="ArgumentNullException">
		/// Any argument other than <paramref name="initialCost"/> is null.
		/// </exception>
		/// <returns>
		/// The shortest path between the two specified nodes, as determined by the user-supplied cost functions.
		/// If no path exists, an empty path will be returned.
		/// </returns>
		/// <remarks>
		/// The <paramref name="estimatedCost"/> delegate will be used to estimate the cost of traveling between
		/// the node at the end of a potential path and the goal. This heuristic must be <b>admissible</b>: it must
		/// <i>never overestimate the cost</i>.
		/// </remarks>
		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder,
			IComparer<TCost> costComparer,
			Predicate<TNode> nodeFilter,
			TCost initialCost)
			where TNode : IGraphNode<TNode>
		{
			if (ReferenceEquals(start, null)) throw new ArgumentNullException(nameof(start));
			if (ReferenceEquals(goal, null)) throw new ArgumentNullException(nameof(goal));
			if (knownCost is null) throw new ArgumentNullException(nameof(knownCost));
			if (estimatedCost is null) throw new ArgumentNullException(nameof(estimatedCost));
			if (costAdder is null) throw new ArgumentNullException(nameof(costAdder));
			if (costComparer is null) throw new ArgumentNullException(nameof(costComparer));
			if (nodeFilter is null) throw new ArgumentNullException(nameof(nodeFilter));
			

			var seen = new HashSet<TNode>();
			var queue = new PriorityQueue<TCost, Path<TNode, TCost>>(costComparer);
			var empty = Path.Create(knownCost, costAdder, initialCost);
			enqueuePath(empty.Push(start));
			
			void enqueuePath(Path<TNode, TCost> path)
			{
				if (!nodeFilter(path.Peek()))
					return;

				TCost cost = totalCost(path);
				queue.Enqueue(cost, path);
			}
			TCost totalCost(Path<TNode, TCost> path)
			{
				TCost known = path.Cost;
				TCost estimate = estimatedCost(path.Peek(), goal);

				return costAdder(known, estimate);
			}
			void enqueueNeighbors(Path<TNode, TCost> path)
			{
				TNode head = path.Peek();
				foreach (var neighbor in head.Neighbors)
					enqueuePath(path.Push(neighbor));
			}


			while (!queue.IsEmpty)
			{
				var path = queue.Dequeue();
				TNode head = path.Peek();
				
				if (seen.Contains(head))
					continue;

				if (goal.Equals(head))
					return path;

				seen.Add(head);
				enqueueNeighbors(path);
			}

			return empty;
		}

		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder,
			Comparison<TCost> costComparison,
			Predicate<TNode> nodeFilter,
			TCost initialCost)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(start, goal, 
				knownCost, estimatedCost, costAdder,
				Comparer<TCost>.Create(costComparison),
				nodeFilter, initialCost);
		}

		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder,
			Comparison<TCost> costComparison,
			TCost initialCost)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(
				start, goal, knownCost, estimatedCost, costAdder, 
				Comparer<TCost>.Create(costComparison),
				_ => true, initialCost);
		}

		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder,
			Comparison<TCost> costComparison)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(
				start, goal, knownCost, estimatedCost, costAdder,
				Comparer<TCost>.Create(costComparison),
				_ => true, default(TCost));
		}

		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder,
			Predicate<TNode> nodeFilter,
			TCost initialCost)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(
				start, goal,
				knownCost, estimatedCost,
				costAdder,
				Comparer<TCost>.Default,
				nodeFilter,
				initialCost);
		}

		/// <summary>
		/// Attempts to find the shortest path between the specified start and goal nodes.
		/// </summary>
		/// <typeparam name="TNode">Type of node.</typeparam>
		/// <typeparam name="TCost">Type of cost.</typeparam>
		/// <param name="start">The starting node.</param>
		/// <param name="goal">The goal node.</param>
		/// <param name="knownCost">Function to calculate the known cost of traveling between 
		/// two adjacent nodes.</param>
		/// <param name="estimatedCost">
		/// Function to estimate the cost of traveling between two arbitrary nodes.
		/// The two nodes passed into this function are not guaranteed to be connected.</param>
		/// <param name="costAdder">Function to add two cost values together.</param>
		/// <param name="nodeFilter">Filter function to apply to nodes. Nodes will only be considered 
		/// if this function returns true for them.</param>
		/// <exception cref="ArgumentNullException">
		/// Any argument is null.
		/// </exception>
		/// <returns>
		/// The shortest path between the two specified nodes, as determined by the user-supplied cost functions.
		/// If no path exists, an empty path will be returned.
		/// </returns>
		/// <remarks>
		/// The <paramref name="estimatedCost"/> delegate will be used to estimate the cost of traveling between
		/// the node at the end of a potential path and the goal. This heuristic must be <b>admissible</b>: it must
		/// <i>never overestimate the cost</i>.
		/// </remarks>
		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost, 
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder, 
			Predicate<TNode> nodeFilter)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(
				start, goal, 
				knownCost, 
				estimatedCost, 
				costAdder, 
				nodeFilter, 
				default(TCost));
		}

		/// <summary>
		/// Attempts to find the shortest path between the specified start and goal nodes.
		/// </summary>
		/// <typeparam name="TNode">Type of node.</typeparam>
		/// <typeparam name="TCost">Type of cost.</typeparam>
		/// <param name="start">The starting node.</param>
		/// <param name="goal">The goal node.</param>
		/// <param name="knownCost">Function to calculate the known cost of traveling between 
		/// two adjacent nodes.</param>
		/// <param name="estimatedCost">
		/// Function to estimate the cost of traveling between two arbitrary nodes.
		/// The two nodes passed into this function are not guaranteed to be connected.</param>
		/// <param name="costAdder">Function to add two cost values together.</param>
		/// <param name="initialCost">The initial cost value.</param>
		/// <exception cref="ArgumentNullException">
		/// Any argument other than <paramref name="initialCost"/> is null.
		/// </exception>
		/// <returns>
		/// The shortest path between the two specified nodes, as determined by the user-supplied cost functions.
		/// If no path exists, an empty path will be returned.
		/// </returns>
		/// <remarks>
		/// The <paramref name="estimatedCost"/> delegate will be used to estimate the cost of traveling between
		/// the node at the end of a potential path and the goal. This heuristic must be <b>admissible</b>: it must
		/// <i>never overestimate the cost</i>.
		/// </remarks>
		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder,
			TCost initialCost)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(start, goal, knownCost, estimatedCost, costAdder, _ => true, initialCost);
		}

		/// <summary>
		/// Attempts to find the shortest path between the specified start and goal nodes.
		/// </summary>
		/// <typeparam name="TNode">Type of node.</typeparam>
		/// <typeparam name="TCost">Type of cost.</typeparam>
		/// <param name="start">The starting node.</param>
		/// <param name="goal">The goal node.</param>
		/// <param name="knownCost">Function to calculate the known cost of traveling between 
		/// two adjacent nodes.</param>
		/// <param name="estimatedCost">
		/// Function to estimate the cost of traveling between two arbitrary nodes.
		/// The two nodes passed into this function are not guaranteed to be connected.</param>
		/// <param name="costAdder">Function to add two cost values together.</param>
		/// <exception cref="ArgumentNullException">
		/// Any argument other than <paramref name="initialCost"/> is null.
		/// </exception>
		/// <returns>
		/// The shortest path between the two specified nodes, as determined by the user-supplied cost functions.
		/// If no path exists, an empty path will be returned.
		/// </returns>
		/// <remarks>
		/// The <paramref name="estimatedCost"/> delegate will be used to estimate the cost of traveling between
		/// the node at the end of a potential path and the goal. This heuristic must be <b>admissible</b>: it must
		/// <i>never overestimate the cost</i>.
		/// </remarks>
		public static Path<TNode, TCost> FindPath<TNode, TCost>(
			TNode start, TNode goal,
			CostFunction<TNode, TCost> knownCost,
			CostFunction<TNode, TCost> estimatedCost,
			CostAdder<TCost> costAdder)
			where TNode : IGraphNode<TNode>
		{
			return FindPath(start, goal, knownCost, estimatedCost, costAdder, default(TCost));
		}

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
