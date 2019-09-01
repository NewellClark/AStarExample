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
		public static Vector2 Scale(this Vector2 @this, IntVector2 scaleFactor)
		{
			return new Vector2(@this.X * scaleFactor.X, @this.Y * scaleFactor.Y);
		}

		public static Point ToPoint(this IntVector2 @this) => new Point(@this.X, @this.Y);
		public static Vector2 ToVector2(this IntVector2 @this) => new Vector2(@this.X, @this.Y);
		public static IntVector2 ToIntVector2(this Point @this) => new IntVector2(@this.X, @this.Y);
		public static IntVector2 ToIntVector2(this Size @this) => new IntVector2(@this.Width, @this.Height);

		public static IntVector2 Min(this Rectangle @this) => new IntVector2(@this.Left, @this.Top);
		public static IntVector2 Max(this Rectangle @this) => new IntVector2(@this.Right, @this.Bottom);

		public static Vector2 Min(this RectangleF @this) => new Vector2(@this.Left, @this.Top);
		public static Vector2 Max(this RectangleF @this) => new Vector2(@this.Right, @this.Bottom);

		public static RectangleF CenteredRectangle(Vector2 center, Vector2 size)
		{
			Vector2 upperLeft = center - size / 2;

			return new RectangleF(upperLeft.ToPointF(), size.ToSizeF());
		}
		public static bool Contains(this RectangleF @this, Vector2 point) => @this.Contains(point.ToPointF());

		public static Rectangle CenteredRectangle(IntVector2 center, IntVector2 size)
		{
			IntVector2 upperLeft = center - size / 2;

			return new Rectangle(upperLeft.ToPoint(), size.ToSize());
		}

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
	}
}
