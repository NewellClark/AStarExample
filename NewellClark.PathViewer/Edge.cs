using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer
{
	struct Edge<TNode> : IEquatable<Edge<TNode>>
	{
		public Edge(TNode left, TNode right)
		{
			this.Left = left;
			this.Right = right;
		}

		public TNode Left { get; }

		public TNode Right { get; }

		public static bool Equals(Edge<TNode> first, Edge<TNode> second)
		{
			return NodeComparer.Equals(first.Left, second.Left) &&
				NodeComparer.Equals(first.Right, second.Right);
		}

		public bool Equals(Edge<TNode> other) => Equals(this, other);

		public override bool Equals(object obj)
		{
			if (obj is Edge<TNode> other)
				return Equals(this, other);

			return false;
		}

		public static bool operator ==(Edge<TNode> left, Edge<TNode> right) => Equals(left, right);

		public static bool operator !=(Edge<TNode> left, Edge<TNode> right) => !Equals(left, right);

		public override int GetHashCode()
		{
			int leftHash = NodeComparer.GetHashCode(Left);
			int rightHash = NodeComparer.GetHashCode(Right);

			return (leftHash, rightHash).GetHashCode();
		}

		public override string ToString() => $"{{ {Left}--{Right} }}";

		/// <summary>
		/// Determines whether the current edge and the other edge have the same two nodes, ignoring
		/// order.
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equivalent(Edge<TNode> other)
		{
			return Equals(this, other) || Equals(new Edge<TNode>(Right, Left), other);
		}

		private static IEqualityComparer<TNode> NodeComparer { get; } = EqualityComparer<TNode>.Default;

		/// <summary>
		/// Gets an <see cref="IEqualityComparer{Edge{TNode}}"/> that compares two edges as equal
		/// if they both have the same nodes, even if the nodes are in opposite positions (left vs right) in the two edges.
		/// </summary>
		public static IEqualityComparer<Edge<TNode>> UndirectedComparer { get; } = new UndirectedComparerImplementation();

		private class UndirectedComparerImplementation : IEqualityComparer<Edge<TNode>>
		{
			public bool Equals(Edge<TNode> x, Edge<TNode> y)
			{
				return x.Equivalent(y);
			}

			public int GetHashCode(Edge<TNode> obj)
			{
				int leftHash = obj.Left.GetHashCode();
				int rightHash = obj.Right.GetHashCode();

				//	To ensure that two edges with the same two nodes in opposite positions always get the same hash,
				//	we sort the hashes in ascending order.
				int smallHash = Math.Min(leftHash, rightHash);
				int largeHash = Math.Max(leftHash, rightHash);

				return (smallHash, largeHash).GetHashCode();
			}
		}
	}
}
