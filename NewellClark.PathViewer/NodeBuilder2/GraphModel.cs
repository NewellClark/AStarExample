using NewellClark.DataStructures.Collections;
using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using System.Numerics;

namespace NewellClark.PathViewer.NodeBuilder2
{
	/// <summary>
	/// Contains a set of nodes and provides event notifications whenever a node is added or removed 
	/// from the set, and whenever any node in the set gains or loses a neighbor.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	class GraphModel<T> : IDisposable
		where T: INodeData
	{
		private readonly CompositeDisposable _disposables;

		public GraphModel()
		{
			_Nodes = new ObservableSet<ObservableNode<T>>();

			WhenNodeAdded = _Nodes.WhenItemAdded();
			WhenNodeRemoved = _Nodes.WhenItemRemoved();

			WhenEdgeAdded = _Nodes.WhenEdgeAdded();
			WhenEdgeRemoved = _Nodes.WhenEdgeRemoved();

			var neighborAdded = WhenNodeAdded
				.SelectMany(node => node.WhenNeighborAdded());

			_disposables = new CompositeDisposable
			{
				neighborAdded.Subscribe(node => _Nodes.Add(node)),
				WhenNodeRemoved.Subscribe(node => node.Neighbors.Clear())
			};
		}

		public ISet<ObservableNode<T>> Nodes => _Nodes;
		private readonly ObservableSet<ObservableNode<T>> _Nodes;

		public IObservable<ObservableNode<T>> WhenNodeAdded { get; }

		public IObservable<ObservableNode<T>> WhenNodeRemoved { get; }

		public IObservable<Edge<ObservableNode<T>>> WhenEdgeAdded { get; }

		public IObservable<Edge<ObservableNode<T>>> WhenEdgeRemoved { get; }

		public ObservableNode<T> GetNodeAtPosition(Vector2 position)
		{
			return Nodes.Where(node => node.Value.Bounds.Contains(position.ToPointF()))
				.OrderBy((node => (node.Value.Position - position).Length()))
				.FirstOrDefault();
		}

		public void Dispose() => _disposables.Dispose();
	}
}
