using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	/// <summary>
	/// Represents a single node in a <see cref="GridGraph"/>.
	/// </summary>
	public struct GridNode : IEquatable<GridNode>, IGraphNode<GridNode>
	{
		/// <summary>
		/// Creates a new <see cref="GridNode"/> that resides in the specified
		/// <see cref="GridGraph"/> at the specified <see cref="Position"/>.
		/// </summary>
		/// <param name="graph">The <see cref="GridGraph"/> that the <see cref="GridNode"/> resides in.</param>
		/// <param name="position">The position of the <see cref="GridNode"/> in its <see cref="GridGraph"/>.</param>
		internal GridNode(GridGraph graph, IntVector2 position)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));

			this.Graph = graph;
			this.Position = position;
		}

		/// <summary>
		/// Gets the position of the current <see cref="GridNode"/>.
		/// </summary>
		public IntVector2 Position { get; }

		/// <summary>
		/// Gets the <see cref="GridGraph"/> that the current <see cref="GridNode"/> resides in.
		/// </summary>
		public GridGraph Graph { get; }

		/// <summary>
		/// Gets or sets whether the current <see cref="GridNode"/> is passable.
		/// </summary>
		public bool IsPassable
		{
			get => Graph?.IsPassable[this] ?? true;
			set
			{
				ThrowIfInvalid();
				Graph.IsPassable[this] = value;
			}
		}

		/// <summary>
		/// Gets all the nodes that are adjacent to the current <see cref="GridNode"/>.
		/// </summary>
		public IEnumerable<GridNode> Neighbors => Graph?.GetNeighbors(this) ?? Enumerable.Empty<GridNode>();

		/// <summary>
		/// Gets a value indicating whether the current <see cref="GridNode"/> is valid.
		/// <see cref="GridNode"/>s are valid unless they were created with the default
		/// constructor.
		/// </summary>
		public bool IsValid => Graph != null;

		/// <summary>
		/// Checks for value equality.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool Equals(GridNode left, GridNode right)
		{
			return left.Graph == right.Graph &&
				left.Position == right.Position;
		}

		/// <summary>
		/// Checks for value equality.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator ==(GridNode left, GridNode right) => Equals(left, right);

		/// <summary>
		/// Checks for value equality.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		/// <returns></returns>
		public static bool operator !=(GridNode left, GridNode right) => !Equals(left, right);

		/// <summary>
		/// Checks for value equality.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(GridNode other) => Equals(this, other);

		/// <summary>
		/// Overridden to check for value equality.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is GridNode other)
				return Equals(this, other);

			return false;
		}

		/// <summary>
		/// Overridden for value equality.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return (Graph, Position).GetHashCode();
		}

		private void ThrowIfInvalid([CallerMemberName] string caller = "")
		{
			if (!IsValid)
				throw new InvalidOperationException(
					$"The current {nameof(GridNode)} was created with the default constructor, and is therefor " +
					$"invalid. Member {caller} can only be called on valid {nameof(GridNode)} instances.");
		}
	}
}
