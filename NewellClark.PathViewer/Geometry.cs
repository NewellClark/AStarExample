using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;
using NewellClark.DataStructures.Graphs;

namespace NewellClark.PathViewer
{
	static class Geometry
	{
		public static Vector2 ToVector2(this PointF @this) => new Vector2(@this.X, @this.Y);
		public static Vector2 ToVector2(this SizeF @this) => new Vector2(@this.Width, @this.Height);
		public static Vector2 ToVector2(this Point @this) => new Vector2(@this.X, @this.Y);
		public static Vector2 ToVector2(this Size @this) => new Vector2(@this.Width, @this.Height);

		public static PointF ToPointF(this Vector2 @this) => new PointF(@this.X, @this.Y);
		public static SizeF ToSizeF(this Vector2 @this) => new SizeF(@this.X, @this.Y);
		public static Size ToSize(this Vector2 @this) => new Size((int)@this.X, (int)@this.Y);
		public static Point ToPoint(this PointF @this) => Point.Round(@this);
		public static Size ToSize(this SizeF @this) => new Size((int)@this.Width, (int)@this.Height);
		public static Vector2 Scale(this Vector2 @this, IntVector2 scaleFactor)
		{
			return new Vector2(@this.X * scaleFactor.X, @this.Y * scaleFactor.Y);
		}

		public static Point ToPoint(this IntVector2 @this) => new Point(@this.X, @this.Y);
		public static Point ToPoint(this Vector2 @this) => new Point((int)@this.X, (int)@this.Y);
		public static Vector2 ToVector2(this IntVector2 @this) => new Vector2(@this.X, @this.Y);
		public static IntVector2 ToIntVector2(this Point @this) => new IntVector2(@this.X, @this.Y);
		public static IntVector2 ToIntVector2(this Size @this) => new IntVector2(@this.Width, @this.Height);
		public static IntVector2 ToIntVector2(this Vector2 @this) => new IntVector2((int)@this.X, (int)@this.Y);

		public static IntVector2 Min(this Rectangle @this) => new IntVector2(@this.Left, @this.Top);
		public static IntVector2 Max(this Rectangle @this) => new IntVector2(@this.Right, @this.Bottom);
		public static IEnumerable<IntVector2> Neighbors(this IntVector2 @this)
		{
			yield return @this - IntVector2.UnitX;
			yield return @this - IntVector2.UnitY;
			yield return @this + IntVector2.UnitX;
			yield return @this + IntVector2.UnitY;
		}

		/// <summary>
		/// Enumerates every lattice point in the current rectangle, including the points
		/// along the left and top edges, and excluding the points along the right and bottom edges.
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static IEnumerable<IntVector2> LatticePoints(this Rectangle @this)
		{
			//for (int x = @this.Left; x < @this.Right; ++x)
			//	for (int y = @this.Top; y < @this.Bottom; ++y)
			//		yield return new IntVector2(x, y);

			var min = IntVector2.Min(@this.Location.ToIntVector2(), @this.BottomRight());
			var max = IntVector2.Max(@this.Location.ToIntVector2(), @this.BottomRight());

			for (int x = min.X; x <= max.X; ++x)
				for (int y = min.Y; y <= max.Y; ++y)
					yield return new IntVector2(x, y);
		}

		public static Vector2 Min(this RectangleF @this) => new Vector2(@this.Left, @this.Top);
		public static Vector2 Max(this RectangleF @this) => new Vector2(@this.Right, @this.Bottom);

		public static RectangleF CenteredRectangle(Vector2 center, Vector2 size)
		{
			Vector2 upperLeft = center - size / 2;

			return new RectangleF(upperLeft.ToPointF(), size.ToSizeF());
		}
		public static bool Contains(this RectangleF @this, Vector2 point) => @this.Contains(point.ToPointF());
		public static bool Contains(this Rectangle @this, IntVector2 point) => @this.Contains(point.ToPoint());
		public static IntVector2 Center(this Rectangle @this)
		{
			return @this.Location.ToIntVector2() + @this.Size.ToIntVector2() / 2;
		}
		public static Vector2 Center(this RectangleF @this)
		{
			return @this.Location.ToVector2() + @this.Size.ToVector2() / 2;
		}

		public static Rectangle CenteredRectangle(IntVector2 center, IntVector2 size)
		{
			IntVector2 upperLeft = center - size / 2;

			return new Rectangle(upperLeft.ToPoint(), size.ToSize());
		}
		public static Rectangle Inset(this Rectangle @this, int inset)
		{
			return new Rectangle(
				@this.Left + inset,
				@this.Top + inset,
				@this.Width - 2 * inset,
				@this.Height - 2 * inset);
		}

		/// <summary>
		/// Creates a <see cref="Rectangle"/> from the two specified corners.
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Rectangle RectangleFromCorners(IntVector2 first, IntVector2 second)
		{
			var min = IntVector2.Min(first, second);
			var max = IntVector2.Max(first, second);
			
			return Rectangle.FromLTRB(min.X, min.Y, max.X, max.Y);
		}
		public static IntVector2 BottomRight(this Rectangle @this) => new IntVector2(@this.Right, @this.Bottom);

		public static RectangleF Bounds(this Edge<ObservableNode<NodeData>> @this)
		{
			Vector2 topLeft = Vector2.Min(
				@this.Left.Value.Bounds.Min(),
				@this.Right.Value.Bounds.Min());

			Vector2 bottomRight = Vector2.Max(
				@this.Left.Value.Bounds.Max(),
				@this.Right.Value.Bounds.Max());

			Vector2 size = bottomRight - topLeft;

			return new RectangleF(topLeft.ToPointF(), size.ToSizeF());
		}

		public static int DivRoundDown(this int @this, int divisor)
		{
			if (divisor == 0) throw new DivideByZeroException();

			int roundedTowardsZeroQuotient = @this / divisor;

			if (@this % divisor == 0)
				return roundedTowardsZeroQuotient;

			if (Math.Sign(@this) == Math.Sign(divisor))
				return roundedTowardsZeroQuotient;

			return roundedTowardsZeroQuotient - 1;
		}
	}
}
