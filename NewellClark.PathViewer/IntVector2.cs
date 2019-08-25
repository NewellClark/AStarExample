﻿using System;
using System.Numerics;
using System.Drawing;

namespace NewellClark.PathViewer
{
	struct IntVector2 : IEquatable<IntVector2>
	{
		public IntVector2(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
		
		public int X { get; }

		public int Y { get; }
		
		public IntVector2 Scale(IntVector2 other)
		{
			return new IntVector2(this.X * other.X, this.Y * other.Y);
		}

		public Vector2 Scale(Vector2 other)
		{
			return new Vector2(this.X * other.X, this.Y * other.Y);
		}

		public override bool Equals(object obj)
		{
			var casted = obj as IntVector2?;

			if (!casted.HasValue)
				return false;

			return Equals(this, casted.GetValueOrDefault());
		}

		public bool Equals(IntVector2 other) => Equals(this, other);

		public override int GetHashCode()
		{
			return (X, Y).GetHashCode();
		}

		public float Length => (float)Math.Sqrt(X * X + Y * Y);

		public int ManhattanLength => Math.Abs(X) + Math.Abs(Y);

		public float Distance(IntVector2 other) => (this - other).Length;

		public int ManhattanDistance(IntVector2 other) => (this - other).ManhattanLength;

		public static bool operator ==(IntVector2 left, IntVector2 right) => Equals(left, right);

		public static bool operator !=(IntVector2 left, IntVector2 right) => !Equals(left, right);

		public static bool Equals(IntVector2 left, IntVector2 right)
		{
			return left.X == right.X && left.Y == right.Y;
		}

		public static IntVector2 Min(IntVector2 left, IntVector2 right)
		{
			return new IntVector2(
				Math.Min(left.X, right.X),
				Math.Min(left.Y, right.Y));
		}

		public static IntVector2 Max(IntVector2 left, IntVector2 right)
		{
			return new IntVector2(
				Math.Max(left.X, right.X),
				Math.Max(left.Y, right.Y));
		}

		public static IntVector2 Add(IntVector2 left, IntVector2 right)
		{
			return new IntVector2(left.X + right.X, left.Y + right.Y);
		}

		public static IntVector2 Subtract(IntVector2 left, IntVector2 right)
		{
			return new IntVector2(left.X - right.X, left.Y - right.Y);
		}

		public static IntVector2 Negate(IntVector2 vector) => new IntVector2(-vector.X, -vector.Y);

		public static IntVector2 Multiply(IntVector2 vector, int scalar)
		{
			return new IntVector2(vector.X * scalar, vector.Y * scalar);
		}

		public static Vector2 Multiply(IntVector2 vector, float scalar)
		{
			return new Vector2(vector.X * scalar, vector.Y * scalar);
		}

		public static IntVector2 Divide(IntVector2 vector, int scalar)
		{
			return new IntVector2(vector.X / scalar, vector.Y / scalar);
		}

		public static IntVector2 operator +(IntVector2 left, IntVector2 right) => Add(left, right);

		public static IntVector2 operator -(IntVector2 left, IntVector2 right) => Subtract(left, right);

		public static IntVector2 operator -(IntVector2 vector) => Negate(vector);

		public static IntVector2 operator *(IntVector2 vector, int scalar) => Multiply(vector, scalar);

		public static IntVector2 operator *(int scalar, IntVector2 vector) => Multiply(vector, scalar);

		public static Vector2 operator *(IntVector2 vector, float scalar) => Multiply(vector, scalar);

		public static Vector2 operator *(float scalar, IntVector2 vector) => Multiply(vector, scalar);

		public static IntVector2 operator /(IntVector2 vector, int scalar) => Divide(vector, scalar);

		public static IntVector2 operator /(int scalar, IntVector2 vector) => Divide(vector, scalar);

		public static implicit operator Vector2(IntVector2 intVector) => new Vector2(intVector.X, intVector.Y);

		public static explicit operator IntVector2(Vector2 vector) => new IntVector2((int)vector.X, (int)vector.Y);

		public Point ToPoint() => new Point(X, Y);

		public Size ToSize() => new Size(X, Y);

		public PointF ToPointF() => new PointF(X, Y);

		public SizeF ToSizeF() => new SizeF(X, Y);

		/// <summary>
		/// (0, 0)
		/// </summary>
		public static IntVector2 Zero { get; } = new IntVector2(0, 0);

		/// <summary>
		/// (1, 1)
		/// </summary>
		public static IntVector2 One { get; } = new IntVector2(1, 1);

		/// <summary>
		/// (1, 0)
		/// </summary>
		public static IntVector2 UnitX { get; } = new IntVector2(1, 0);

		/// <summary>
		/// (0, 1)
		/// </summary>
		public static IntVector2 UnitY { get; } = new IntVector2(0, 1);
	}
}