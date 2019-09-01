using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer
{
	struct Circle : IEquatable<Circle>
	{
		/// <summary>
		/// Creates a new <see cref="Circle"/> with the specified <see cref="Center"/> and <see cref="Radius"/>.
		/// </summary>
		/// <param name="center">The center of the circle.</param>
		/// <param name="radius">The radius of the circle.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="radius"/> is negative.
		/// </exception>
		public Circle(Vector2 center, float radius)
		{
			if (radius < 0)
				throw new ArgumentOutOfRangeException(
					nameof(radius),
					$"{nameof(radius)} cannot be negative.");

			this.Center = center;
			this.Radius = radius;
		}

		public Vector2 Center { get; }

		public float Radius { get; }

		public float Diameter => 2 * Radius;

		public bool Overlaps(Circle other)
		{
			return (other.Center - Center).Length() <= other.Radius + Radius;
		}

		public bool Contains(Vector2 point)
		{
			return (Center - point).Length() <= Radius;
		}

		public RectangleF Bounds => Geometry.CenteredRectangle(Center, Diameter * Vector2.One);

		/// <summary>
		/// Produces a new circle by translating the current circle by the specified offset.
		/// </summary>
		/// <param name="offset"></param>
		/// <returns>
		/// A new <see cref="Circle"/> created by translating the current <see cref="Circle"/> by the
		/// specified displacement.
		/// </returns>
		public Circle Translate(Vector2 offset) => new Circle(Center + offset, Radius);

		/// <summary>
		/// Produces a new circle by multiplying the current circle's radius by the specified amount.
		/// </summary>
		/// <param name="scaleFactor"></param>
		/// <returns></returns>
		public Circle Scale(float scaleFactor) => new Circle(Center, Radius * scaleFactor);

		public static bool Equals(Circle left, Circle right)
		{
			return left.Center == right.Center &&
				left.Radius == right.Radius;
		}

		public bool Equals(Circle other) => Equals(this, other);

		/// <summary>
		/// Overridden for value equality.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is Circle other)
				return Equals(this, other);

			return false;
		}

		public override int GetHashCode()
		{
			return (Center.X, Center.Y, Radius).GetHashCode();
		}

		public static bool operator ==(Circle left, Circle right) => Equals(left, right);

		public static bool operator !=(Circle left, Circle right) => !Equals(left, right);
	}
}
