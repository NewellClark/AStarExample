using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NewellClark.DataStructures.Graphs
{
	[TestFixture]
	partial class GraphNodeTests
	{
		[TestFixture]
		public class NeighborCollectionTests
		{
			[Test]
			public void Add_AddsNode_WhenNotPresent()
			{
				var primary = new GraphNode<string>(text1);
				var neighbor = new GraphNode<string>(text2);

				primary.Neighbors.Add(neighbor);

				CollectionAssert.Contains(primary.Neighbors, neighbor);
			}

			[Test]
			public void Add_AddsSelfToNeighbor_WhenNotPresent()
			{
				var primary = new GraphNode<string>(text1);
				var neighbor = new GraphNode<string>(text2);

				primary.Neighbors.Add(neighbor);
				
				CollectionAssert.Contains(neighbor.Neighbors, primary);
			}

			[Test]
			public void Add_ReturnsTrue_WhenNodeAdded()
			{
				var primary = GetNode();
				var neighbor = GetNode();

				bool result = primary.Neighbors.Add(neighbor);

				Assume.That(primary.Neighbors.Contains(neighbor));

				Assert.True(result);
			}

			[Test]
			public void Add_ReturnsFalse_WhenNotAdded()
			{
				var primary = GetNode();
				var neighbor = GetNode();
				primary.Neighbors.Add(neighbor);

				Assume.That(primary.Neighbors.Contains(neighbor));

				bool result = primary.Neighbors.Add(neighbor);

				Assert.False(result);
			}

			[Test]
			public void Add_WillNotAddSelf()
			{
				var node = GetNode();

				node.Neighbors.Add(node);

				CollectionAssert.DoesNotContain(node.Neighbors, node);
			}

			[Test]
			public void Add_ReturnsFalse_WhenAttemptingToAddSelf()
			{
				var node = GetNode();

				bool result = node.Neighbors.Add(node);

				Assume.That(!node.Neighbors.Contains(node));

				Assert.False(result);
			}

			[Test]
			public void Add_Throws_WhenArgumentNull()
			{
				var node = GetNode();

				Assert.Throws<ArgumentNullException>(() => node.Neighbors.Add(null));
			}

			[Test]
			public void Remove_RemovesContainedNeighbor()
			{
				var primary = GetNode();
				var neighbor = GetNode();
				primary.Neighbors.Add(neighbor);

				Assume.That(primary.Neighbors.Contains(neighbor));

				primary.Neighbors.Remove(neighbor);

				CollectionAssert.DoesNotContain(primary.Neighbors, neighbor);
			}

			[Test]
			public void Remove_RemovesSelfFromNeighbor_WhenPresent()
			{
				var primary = GetNode();
				var neighbor = GetNode();
				primary.Neighbors.Add(neighbor);

				Assume.That(neighbor.Neighbors.Contains(primary));

				primary.Neighbors.Remove(neighbor);

				CollectionAssert.DoesNotContain(neighbor.Neighbors, primary);
			}

			[Test]
			public void Remove_ReturnsTrue_WhenNodeWasPresent()
			{
				var primary = GetNode();
				var neighbor = GetNode();
				primary.Neighbors.Add(neighbor);

				Assume.That(primary.Neighbors.Contains(neighbor));

				bool result = primary.Neighbors.Remove(neighbor);

				Assert.True(result);
			}

			[Test]
			public void Remove_ReturnsFalse_WhenNodeWasNotPresent()
			{
				var primary = GetNode();
				var neighbor = GetNode();

				Assume.That(!primary.Neighbors.Contains(neighbor));

				bool result = primary.Neighbors.Remove(neighbor);

				Assert.False(result);
			}

			[Test]
			public void Remove_Throws_WhenArgumentNull()
			{
				var node = GetNode();

				Assert.Throws<ArgumentNullException>(() => node.Neighbors.Remove(null));
			}

			[Test]
			public void Clear_RemovesAllNeighbors()
			{
				var node = GetNode();
				const int count = 4;
				for (int i = 0; i < count; ++i)
					node.Neighbors.Add(GetNode());

				Assume.That(node.Neighbors.Count == count);

				node.Neighbors.Clear();

				CollectionAssert.IsEmpty(node.Neighbors);
			}

			[Test]
			public void Clear_RemovesSelfFromRemovedNeighbors()
			{
				var primary = GetNode();
				var neighbor = GetNode();
				primary.Neighbors.Add(neighbor);
				neighbor.Neighbors.Add(GetNode());
				neighbor.Neighbors.Add(GetNode());

				bool assumption = primary.Neighbors.Contains(neighbor) && neighbor.Neighbors.Contains(primary);
				Assume.That(assumption);

				primary.Neighbors.Clear();

				CollectionAssert.DoesNotContain(neighbor.Neighbors, primary);
			}

			[Test]
			public void Contains_ReturnsTrue_WhenNodePresent()
			{
				var primary = GetNode();
				var neighbor = GetNode();
				primary.Neighbors.Add(neighbor);

				bool result = primary.Neighbors.Contains(neighbor);

				Assert.True(result);
			}

			[Test]
			public void Contains_ReturnsFalse_WhenNodeNotPresent()
			{
				var primary = GetNode();
				var neighbor = GetNode();

				bool result = primary.Neighbors.Contains(neighbor);

				Assert.False(result);
			}

			[Test]
			public void Contains_Throws_WhenArgumentNull()
			{
				var primary = GetNode();

				Assert.Throws<ArgumentNullException>(
					() => primary.Neighbors.Contains(null));
			}
		}
	}
}
