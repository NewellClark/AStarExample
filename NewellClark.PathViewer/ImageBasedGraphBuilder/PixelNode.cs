using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	struct PixelNode : IGraphNode<PixelNode>, IEquatable<PixelNode>
	{
		private readonly BitmapGraph _graph;

		/// <summary>
		/// Creates a new <see cref="PixelNode"/> at the specified position within the specified
		/// <see cref="Bitmap"/> image.
		/// </summary>
		/// <param name="bitmap">The <see cref="Bitmap"/> that the <see cref="PixelNode"/> resides in.</param>
		/// <param name="position">The position within the specified <see cref="Bitmap"/>.</param>
		public PixelNode(BitmapGraph graph, IntVector2 position)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (!graph.Bounds.Contains(position)) throw new ArgumentOutOfRangeException(
				nameof(position), $"'{nameof(position)}' ({position}) is outside the bounds of '{nameof(graph)}'.");

			_graph = graph;
			this.Position = position;
		}

		/// <summary>
		/// Gets the position of the <see cref="PixelNode"/> in the image.
		/// </summary>
		public IntVector2 Position { get; }

		/// <summary>
		/// Gets or sets the color of the <see cref="PixelNode"/>. Note: if the compiler won't
		/// let you set the <see cref="Color"/> of a "temporary copy", use the <see cref="SetColor(Color)"/> method.
		/// This struct wraps a bitmap, and changes to the <see cref="Color"/> property are applied directly to
		/// the underlying bitmap.
		/// </summary>
		public Color Color
		{
			get => _graph?.GetPixel(Position) ?? default(Color);
			set
			{
				if (_graph is null)
					throw new InvalidOperationException($"Can't set the color of a {nameof(PixelNode)} that was " +
						$"created with the default constructor because the underlying {nameof(Bitmap)} is null.");

				_graph.SetPixel(Position, value);
			}
		}

		/// <summary>
		/// Sets the <see cref="Color"/> of the current <see cref="PixelNode"/>. 
		/// </summary>
		/// <param name="color">The color to set.</param>
		/// <remarks>
		/// Yes, this is redundant with the  setable <see cref="Color"/> property. It's for those times when the compiler
		/// won't let you mutate a property of a struct that is a temporary copy. In this case it's fine to do that
		/// because this struct wraps a reference type.
		/// </remarks>
		public void SetColor(Color color) => this.Color = color;

		public bool IsPassable => !(Color.R == 0 && Color.G == 0 && Color.B == 0);

		public bool IsPath => _graph?.IsPath(this) ?? false;

		public bool IsStartOrGoal => IsStart || IsGoal;

		public bool IsStart => Position == _graph.Start;

		public bool IsGoal => Position == _graph.Goal;

		/// <summary>
		/// Gets the <see cref="PixelNode"/>s that are adjacent to the current <see cref="PixelNode"/>.
		/// </summary>
		public IEnumerable<PixelNode> Neighbors
		{
			get
			{
				var graph = _graph;

				if (graph is null)
					return Enumerable.Empty<PixelNode>();

				IEnumerable<IntVector2> unfilteredNeighbors = graph.DiagonalsAllowed ?
					GetNeighborPositionsDiagonalsIncluded() :
					GetNeighborPositionsDiagonalsExcluded();

				return unfilteredNeighbors
					.Where(p => graph.Bounds.Contains(p))
					.Select(p => new PixelNode(graph, p));
			}
		}

		private IEnumerable<IntVector2> GetNeighborPositionsDiagonalsIncluded()
		{
			yield return Position - IntVector2.UnitX;
			yield return Position - IntVector2.One;
			yield return Position - IntVector2.UnitY;
			yield return Position + IntVector2.UnitX - IntVector2.UnitY;
			yield return Position + IntVector2.UnitX;
			yield return Position + IntVector2.One;
			yield return Position + IntVector2.UnitY;
			yield return Position - IntVector2.UnitX + IntVector2.UnitY;
		}

		private IEnumerable<IntVector2> GetNeighborPositionsDiagonalsExcluded()
		{
			return Position.Neighbors();
		}

		public static bool Equals(PixelNode left, PixelNode right)
		{
			return left._graph == right._graph &&
				left.Position == right.Position;
		}

		public static bool operator ==(PixelNode left, PixelNode right) => Equals(left, right);

		public static bool operator !=(PixelNode left, PixelNode right) => !Equals(left, right);

		public bool Equals(PixelNode other) => Equals(this, other);

		public override bool Equals(object obj)
		{
			if (obj is PixelNode other)
				return Equals(this, other);

			return false;
		}

		public override int GetHashCode() => (_graph.GetHashCode(), Position.GetHashCode()).GetHashCode();

		public override string ToString()
		{
			return $"{Position}, {Color}";
		}
	}
}
