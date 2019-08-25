using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NewellClark.DataStructures.Collections;
//using MoreLinq.Extensions;

namespace NewellClark.DataStructures.Graphs
{
	[TestFixture]
	public abstract partial class MutableNodeTests<TNode>
		where TNode: class, IMutableNode<TNode, string>
	{
		private const string text1 = "A";
		private const string text2 = "B";
		private const string text3 = "C";

		protected abstract TNode GetNode(string text);

		protected TNode GetNode() => GetNode("dummy");

		protected IEnumerable<TNode> GetNodes(int count, string text)
		{
			IEnumerable<TNode> iter()
			{
				for (int i = 0; i < count; i++)
					yield return GetNode($"{text} [{i}]");
			}

			return iter().ToArray();
		}

		private IEnumerable<TNode> GetNodes(int count) => GetNodes(count, string.Empty);

		private static IEnumerable<TNode> GetNodes(params TNode[] nodes)
		{
			return nodes.AsEnumerable();
		}
	}

	[TestFixture]
	public class GraphNodeTests : MutableNodeTests<GraphNode<string>>
	{
		protected override GraphNode<string> GetNode(string text) => new GraphNode<string>(text);
	}

	[TestFixture]
	public class ObservableGraphNodeTests : MutableNodeTests<ObservableNode<string>>
	{
		protected override ObservableNode<string> GetNode(string text) => new ObservableNode<string>(text);

		private IEnumerable<T> Iterate<T>(params T[] items) => items.AsEnumerable();

		private Recorder recorder;

		[SetUp]
		public void Initialize()
		{
			recorder = new Recorder();
		}

		/// <summary>
		/// Records all events.
		/// </summary>
		private class Recorder
		{
			private readonly Dictionary<ObservableNode<string>, SenderData> _lookup;

			public Recorder()
			{
				_lookup = new Dictionary<ObservableNode<string>, SenderData>();
			}
			
			public void Watch(params ObservableNode<string>[] nodes)
			{
				foreach (var node in nodes)
				{
					node.NodesAdded += HandleAdded;
					node.NodesRemoved += HandleRemoved;
				}
			}

			public void AssertAdds(ObservableNode<string> sender)
			{
				var data = GetData(sender);
				CollectionAssert.AreEquivalent(data.ExpectedAdds, data.ActualAdds);
			}

			public void AssertAllAdds()
			{
				foreach (var data in _lookup.Values)
					CollectionAssert.AreEquivalent(data.ExpectedAdds, data.ActualAdds);
			}

			public void AssertRemoves(ObservableNode<string> sender)
			{
				var data = GetData(sender);
				CollectionAssert.AreEquivalent(data.ExpectedRemoves, data.ActualRemoves);
			}

			public void AssertAllRemoves()
			{
				foreach (var data in _lookup.Values)
					CollectionAssert.AreEquivalent(data.ExpectedRemoves, data.ActualRemoves);
			}

			public void AssertAll()
			{
				AssertAllAdds();
				AssertAllRemoves();
			}

			private SenderData GetData(ObservableNode<string> sender)
			{
				return _lookup.GetOrAdd(sender, node => new SenderData(node));
			}

			private void HandleAdded(object sender, NodeChangedEventArgs<ObservableNode<string>> e)
			{
				GetData(e.Sender).ActualAdds.AddRange(e.Neighbors);
			}

			private void HandleRemoved(object sender, NodeChangedEventArgs<ObservableNode<string>> e)
			{
				GetData(e.Sender).ActualRemoves.AddRange(e.Neighbors);
			}

			/// <summary>
			/// Adds an expected <see cref="ObservableNode{T}.NodesAdded"/> event. 
			/// </summary>
			/// <param name="expectedSender">The expected sender.</param>
			/// <param name="expectedNeighbors">The expected neighbors, in no particular order.</param>
			public void ExpectAdded(
				ObservableNode<string> expectedSender, 
				params ObservableNode<string>[] expectedNeighbors)
			{
				GetData(expectedSender).ExpectedAdds.AddRange(expectedNeighbors);
			}

			public void ExpectRemoved(
				ObservableNode<string> expectedSender,
				params ObservableNode<string>[] expectedNeighbors)
			{
				GetData(expectedSender).ExpectedRemoves.AddRange(expectedNeighbors);
			}

			private class SenderData
			{
				public SenderData(ObservableNode<string> sender)
				{
					this.Sender = sender;
					ExpectedAdds = GetList();
					ActualAdds = GetList();
					ExpectedRemoves = GetList();
					ActualRemoves = GetList();
				}

				public ObservableNode<string> Sender { get; }
				public List<ObservableNode<string>> ExpectedAdds { get; }
				public List<ObservableNode<string>> ActualAdds { get; }
				public List<ObservableNode<string>> ExpectedRemoves { get; }
				public List<ObservableNode<string>> ActualRemoves { get; }

				private List<ObservableNode<string>> GetList() => new List<ObservableNode<string>>();
			}
		}

		[Test]
		public void Add_RaisesEventInBothNodes()
		{
			var self = GetNode();
			var other = GetNode();
			recorder.Watch(self, other);
			recorder.ExpectAdded(self, other);
			recorder.ExpectAdded(other, self);

			self.Neighbors.Add(other);

			recorder.AssertAllAdds();
		}

		[Test]
		public void Remove_RaisesEventInBothNodes()
		{
			var self = GetNode();
			var other = GetNode();
			self.Neighbors.Add(other);

			recorder.Watch(self, other);
			recorder.ExpectRemoved(self, other);
			recorder.ExpectRemoved(other, self);

			self.Neighbors.Remove(other);

			recorder.AssertAllRemoves();
		}

		[Test]
		public void Add_NotRaisedWhenAddingSelf()
		{
			var self = GetNode();
			recorder.Watch(self);

			self.Neighbors.Add(self);

			recorder.AssertAllAdds();
		}

		[Test]
		public void Clear_RaisesEventsForAllRemovedNeighbors()
		{
			var center = GetNode();
			var a = GetNode();
			var b = GetNode();
			var c = GetNode();
			center.Neighbors.Add(a);
			center.Neighbors.Add(b);
			center.Neighbors.Add(c);

			recorder.Watch(center, a, b, c);
			recorder.ExpectRemoved(center, a, b, c);
			recorder.ExpectRemoved(a, center);
			recorder.ExpectRemoved(b, center);
			recorder.ExpectRemoved(c, center);

			center.Neighbors.Clear();

			recorder.AssertAllRemoves();
		}

		[Test]
		public void ExceptWith_RaisesAllEvents()
		{
			var center = GetNode();
			var a = GetNode();
			var b = GetNode();
			var c = GetNode();
			var d = GetNode();
			var e = GetNode();
			center.Neighbors.AddRange(a, b, c, d, e);
			recorder.Watch(center, a, b, c, d, e);

			var excepted = Iterate(a, d, e);
			recorder.ExpectRemoved(center, excepted.ToArray());
			foreach (var node in excepted)
				recorder.ExpectRemoved(node, center);

			center.Neighbors.ExceptWith(excepted);

			recorder.AssertAll();
		}

		[Test]
		public void SymmetricExceptWith_RaisesAllEvents()
		{
			var center = GetNode();
			var a = GetNode();
			var b = GetNode();
			var c = GetNode();
			var d = GetNode();
			var e = GetNode();
			var f = GetNode();

			center.Neighbors.AddRange(a, b, c, d);
			var other = Iterate(c, d, e, f);

			recorder.Watch(center, a, b, c, d, e, f);

			recorder.ExpectAdded(center, e, f);
			recorder.ExpectAdded(e, center);
			recorder.ExpectAdded(f, center);

			recorder.ExpectRemoved(center, c, d);
			recorder.ExpectRemoved(c, center);
			recorder.ExpectRemoved(d, center);

			center.Neighbors.SymmetricExceptWith(other);

			recorder.AssertAll();
		}

		[Test]
		public void IntersectWith_RaisesAllEvents()
		{
			var center = GetNode();
			var a = GetNode();
			var b = GetNode();
			var c = GetNode();
			var d = GetNode();
			var e = GetNode();
			center.Neighbors.AddRange(a, b, c, d, e);

			recorder.Watch(center, a, b, c, d, e);

			var other = Iterate(b, c, d);
			recorder.ExpectRemoved(center, a, e);
			recorder.ExpectRemoved(a, center);
			recorder.ExpectRemoved(e, center);

			center.Neighbors.IntersectWith(other);

			recorder.AssertAllRemoves();
		}
	}
}
