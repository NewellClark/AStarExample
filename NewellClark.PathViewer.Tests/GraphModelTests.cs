using NewellClark.DataStructures.Graphs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.Tests
{
	//[TestFixture]
	//public class GraphModelTests
	//{
	//	private GraphModel<object> model;
	//	private Listener<ObservableNode<object>> nodesAdded;
	//	private Listener<ObservableNode<object>> nodesRemoved;
	//	private Listener<Edge<ObservableNode<object>>> edgesAdded;
	//	private Listener<Edge<ObservableNode<object>>> edgesRemoved;
	//	private CompositeDisposable subscriptions;

	//	/// <summary>
	//	/// Gets an <see cref="IEqualityComparer{T}"/> that compares edges as equal if they have the same nodes,
	//	/// even if the nodes are in opposite positions.
	//	/// </summary>
	//	private IEqualityComparer<Edge<ObservableNode<object>>> EdgeComparer =>
	//		Edge<ObservableNode<object>>.UndirectedComparer;

	//	private void AssertSequenceEqual<T>(
	//		IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
	//	{
	//		Assert.That(expected.SequenceEqual(actual, comparer),
	//			$"Sequence was not equal to expected.");
	//	}

	//	private void AssertSequenceEquivalent<T>(
	//		IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
	//	{
	//		Assert.That(
	//			new HashSet<T>(expected, comparer).SetEquals(actual),
	//			$"Sequences were not equivalent.");
	//	}

	//	/// <summary>
	//	/// Creates and subscribes an observer that records
	//	/// elements from an observable and sets a boolean flag when the observable completes.
	//	/// </summary>
	//	/// <typeparam name="T"></typeparam>
	//	private class Listener<T> : IDisposable
	//	{
	//		private readonly IDisposable _subscription;

	//		public Listener(IObservable<T> observable)
	//			: this(observable, e => throw e) { }

	//		public Listener(IObservable<T> observable, Action<Exception> onError)
	//		{
	//			_Items = new List<T>();

	//			var observer = Observer.Create<T>(
	//				t => _Items.Add(t),
	//				onError,
	//				() => Completed = true);

	//			_subscription = observable.Subscribe(observer);
	//		}

	//		public IList<T> Items => _Items;
	//		private readonly List<T> _Items;

	//		public bool Completed { get; private set; }
			
	//		public void Dispose() => _subscription.Dispose();
			
	//	}
		
	//	[SetUp]
	//	public void Initialize()
	//	{
	//		model = new GraphModel<object>();
	//		nodesAdded = new Listener<ObservableNode<object>>(model.WhenNodeAdded);
	//		nodesRemoved = new Listener<ObservableNode<object>>(model.WhenNodeRemoved);
	//		edgesAdded = new Listener<Edge<ObservableNode<object>>>(model.WhenEdgeAdded);
	//		edgesRemoved = new Listener<Edge<ObservableNode<object>>>(model.WhenEdgeRemoved);

	//		subscriptions = new CompositeDisposable
	//		{
	//			nodesAdded,
	//			nodesRemoved,
	//			edgesAdded,
	//			edgesRemoved
	//		};
	//	}

	//	[TearDown]
	//	public void Cleanup()
	//	{
	//		subscriptions.Dispose();
	//	}

	//	private ObservableNode<object> Create() => Create(new object());
	//	private ObservableNode<object> Create(object value) => new ObservableNode<object>(value);
	//	private Edge<T> Create<T>(T left, T right) => new Edge<T>(left, right);

	//	private IEnumerable<T> Iterate<T>(params T[] items) => items.AsEnumerable();

	//	[Test]
	//	public void AddingNode_PushesEvent()
	//	{
	//		var node = Create();

	//		model.Nodes.Add(node);
	//		var expected = Iterate(node);

	//		CollectionAssert.AreEqual(expected, nodesAdded.Items);
	//	}

	//	[Test]
	//	public void RemovingNode_PushesEvent()
	//	{
	//		var node = Create();
	//		model.Nodes.Add(node);

	//		model.Nodes.Remove(node);
	//		var expected = Iterate(node);

	//		CollectionAssert.AreEqual(expected, nodesRemoved.Items);
	//	}

	//	[Test]
	//	public void SingleNeighborAdded_PushesEvent()
	//	{
	//		var left = Create($"left");
	//		model.Nodes.Add(left);
	//		var right = Create($"right");
	//		model.Nodes.Add(right);

	//		left.Neighbors.Add(right);
	//		var expected = Iterate(Create(left, right));

	//		AssertSequenceEquivalent(expected, edgesAdded.Items, EdgeComparer);
	//	}

	//	[Test]
	//	public void SingleNeighborRemoved_PushesEvent()
	//	{
	//		var left = Create("left");
	//		var right = Create("right");
	//		model.Nodes.Add(left);
	//		model.Nodes.Add(right);
	//		left.Neighbors.Add(right);

	//		left.Neighbors.Remove(right);
	//		var expected = Iterate(Create(left, right), Create(right, left));

	//		CollectionAssert.AreEquivalent(expected, edgesRemoved.Items);
	//	}

	//	[Test]
	//	public void NewNeighborsNotInSet_GetAddedToSet()
	//	{
	//		var inSet = Create("In Set");
	//		model.Nodes.Add(inSet);
	//		var notInSet = Create("Not In Set");

	//		inSet.Neighbors.Add(notInSet);

	//		CollectionAssert.Contains(model.Nodes, notInSet);
	//	}

	//	[Test]
	//	public void NodeRemovedFromSet_LosesAllNeighbors()
	//	{
	//		var banished = Create("Banished");
	//		model.Nodes.Add(banished);
	//		var friend1 = Create("friend1");
	//		var friend2 = Create("friend2");
	//		var friend3 = Create("friend3");
	//		banished.Neighbors.Add(friend1);
	//		banished.Neighbors.Add(friend2);
	//		banished.Neighbors.Add(friend3);

	//		Assume.That(banished.Neighbors.SetEquals(Iterate(friend1, friend2, friend3)));
	//		Assume.That(model.Nodes.SetEquals(Iterate(banished, friend1, friend2, friend3)));

	//		model.Nodes.Remove(banished);

	//		CollectionAssert.IsEmpty(banished.Neighbors);
	//	}
	//}
}
