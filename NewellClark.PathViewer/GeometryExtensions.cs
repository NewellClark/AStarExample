using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Numerics;

namespace NewellClark.PathViewer
{
	static class GeometryExtensions
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

		public static IntVector2 Min(this Rectangle @this) => new IntVector2(@this.Left, @this.Top);
		public static IntVector2 Max(this Rectangle @this) => new IntVector2(@this.Right, @this.Bottom);
	}
}
