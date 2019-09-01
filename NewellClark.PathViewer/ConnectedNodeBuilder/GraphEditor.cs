using NewellClark.DataStructures.Collections;
using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	class GraphEditor : IDisposable
	{
		private readonly CompositeDisposable _disposables;

		public GraphEditor(Control control)
		{
			if (control is null) throw new ArgumentNullException(nameof(control));

			this.Control = control;
			_Nodes = new ObservableSet<ObservableNode<NodeData>>();

			{
				var nodeAdded = CreateNodeAddedStream();
				var nodeRemoved = CreateNodeRemovedStream();

				var edgeStream = CreateUnfilteredEdgeStream(ref nodeAdded, ref nodeRemoved);
				var edgeAdded = edgeStream
					.Where(t => t.added)
					.Select(t => t.edge);
				var edgeRemoved = edgeStream
					.Where(t => !t.added)
					.Select(t => t.edge);

				WhenNodeAdded = nodeAdded;
				WhenNodeRemoved = nodeRemoved;
				WhenEdgeAdded = FilterEquivalentEdges(edgeAdded);
				WhenEdgeRemoved = FilterEquivalentEdges(edgeRemoved);
			}

			_WhenStartNodeChanged = new Subject<ObservableNode<NodeData>>();
			WhenStartNodeChanged = _WhenStartNodeChanged;

			_WhenGoalNodeChanged = new Subject<ObservableNode<NodeData>>();
			WhenGoalNodeChanged = _WhenGoalNodeChanged;

			WhenStartNodeChanged.Merge(WhenGoalNodeChanged)
				.Subscribe(async _ => await UpdatePath());

			_WhenShortestPathChanged = new Subject<Path<ObservableNode<NodeData>, float>>();
			WhenShortestPathChanged = _WhenShortestPathChanged.AsObservable();

			this.Presenter = new GraphPresenter(this);

			_Tools = new ObservableSet<NodeTool>();
			Tools = new ReadOnlyObservableSet<NodeTool>(_Tools);

			_Tools.Add(new AddRemoveNodeTool(this) { Enabled = true });
			_Tools.Add(new SimpleConnectDisconnectNodeTool(this));
			_Tools.Add(new ConnectDisconnectManyNodeTool(this));
			_Tools.Add(new SetStartGoalNode(this));

			_disposables = new CompositeDisposable();
			this.Control.Disposed += (s, e) => Dispose();
		}

		/// <summary>
		/// Gets the <see cref="System.Windows.Forms.Control"/> that is used for displaying
		/// and modifying nodes.
		/// </summary>
		public Control Control { get; }

		public GraphPresenter Presenter { get; }

		public ISet<ObservableNode<NodeData>> Nodes => _Nodes;
		private readonly ObservableSet<ObservableNode<NodeData>> _Nodes;

		public IEnumerable<Edge<ObservableNode<NodeData>>> Edges
		{
			get
			{
				var seen = new HashSet<Edge<ObservableNode<NodeData>>>(
					Edge<ObservableNode<NodeData>>.UndirectedComparer);

				foreach (var node in Nodes)
					foreach (var edge in node.Edges())
					{
						if (seen.Add(edge))
							yield return edge;
					}
			}
		}

		public IObservable<ObservableNode<NodeData>> WhenNodeAdded { get; }

		public IObservable<ObservableNode<NodeData>> WhenNodeRemoved { get; }

		public IObservable<Edge<ObservableNode<NodeData>>> WhenEdgeAdded { get; }

		public IObservable<Edge<ObservableNode<NodeData>>> WhenEdgeRemoved { get; }

		/// <summary>
		/// Gets the node that is touching the specified point. If multiple nodes are 
		/// touching the specified point, the one who's position is closest to the specified
		/// position is returned. If no node is touching the specified position, null is returned.
		/// </summary>
		/// <param name="position">The position to search at.</param>
		/// <returns>The closest node who's bounding box is touching the specified position. If no
		/// node is touching the specified position, null is returned.</returns>
		public ObservableNode<NodeData> GetNodeAtPosition(Vector2 position)
		{
			var result = Nodes
				.Where(node => node.Value.Bounds.Contains(position))
				.OrderBy(node => (node.Value.Position - position).Length())
				.FirstOrDefault();

			return result;
		}

		public IEnumerable<ObservableNode<NodeData>> GetNodesTouchingRegion(Circle circle)
		{
			return Nodes
				.Where(node => node.Value.Circle.Overlaps(circle));
		}

		public IEnumerable<ObservableNode<NodeData>> GetNodesTouchingRegion(RectangleF bounds)
		{
			return Nodes
				.Where(node => node.Value.Bounds.IntersectsWith(bounds));
		}

		public ObservableNode<NodeData> StartNode
		{
			get => _StartNode;
			set
			{
				if (value == _StartNode)
					return;

				_StartNode = value;
				_WhenStartNodeChanged.OnNext(_StartNode);
			}
		}
		private ObservableNode<NodeData> _StartNode;

		public IObservable<ObservableNode<NodeData>> WhenStartNodeChanged { get; }
		private readonly Subject<ObservableNode<NodeData>> _WhenStartNodeChanged;

		public ObservableNode<NodeData> GoalNode
		{
			get => _GoalNode;
			set
			{
				if (_GoalNode == value)
					return;

				_GoalNode = value;
				_WhenGoalNodeChanged.OnNext(_GoalNode);
			}
		}
		private ObservableNode<NodeData> _GoalNode;

		public IObservable<ObservableNode<NodeData>> WhenGoalNodeChanged { get; }
		private readonly Subject<ObservableNode<NodeData>> _WhenGoalNodeChanged;

		public ReadOnlyObservableSet<NodeTool> Tools { get; }
		private readonly ObservableSet<NodeTool> _Tools;

		public Path<ObservableNode<NodeData>, float> ShortestPath
		{
			get => _ShortestPath;
			private set
			{
				if (_ShortestPath == value)
					return;

				_ShortestPath = value;
				_WhenShortestPathChanged.OnNext(_ShortestPath);
			}
		}
		private Path<ObservableNode<NodeData>, float> _ShortestPath;

		public IObservable<Path<ObservableNode<NodeData>, float>> WhenShortestPathChanged { get; }
		private readonly Subject<Path<ObservableNode<NodeData>, float>> _WhenShortestPathChanged;

		public void Dispose()
		{
			_disposables.Dispose();
		}

		private IObservable<ObservableNode<NodeData>> CreateNodeAddedStream()
		{
			var setChanged = Observable.FromEventPattern<SetChangedEventArgs<ObservableNode<NodeData>>>(
				h => _Nodes.SetChanged += h,
				h => _Nodes.SetChanged -= h);

			return setChanged.SelectMany(evp => evp.EventArgs.Added);
		}

		private IObservable<ObservableNode<NodeData>> CreateNodeRemovedStream()
		{
			var setChanged = Observable.FromEventPattern<SetChangedEventArgs<ObservableNode<NodeData>>>(
				h => _Nodes.SetChanged += h,
				h => _Nodes.SetChanged -= h);

			return setChanged.SelectMany(evp => evp.EventArgs.Removed);
		}

		private IObservable<(Edge<ObservableNode<NodeData>> edge, bool added)> CreateUnfilteredEdgeStream(
			ref IObservable<ObservableNode<NodeData>> addedStream,
			ref IObservable<ObservableNode<NodeData>> removedStream)
		{
			var subject = new Subject<(Edge<ObservableNode<NodeData>> edge, bool added)>();

			void onAdded(ObservableNode<NodeData> node)
			{
				node.NodesAdded += handleNeighborAdded;
				node.NodesRemoved += handleNeighborRemoved;
			}
			void onRemoved(ObservableNode<NodeData> node)
			{
				node.NodesAdded -= handleNeighborAdded;
				node.NodesRemoved -= handleNeighborRemoved;
				node.Neighbors.Clear();
			}
			void handleNeighborAdded(object sender, NodeChangedEventArgs<ObservableNode<NodeData>> e)
			{
				foreach (var neighbor in e.Neighbors)
				{
					_Nodes.Add(neighbor);
					var edge = new Edge<ObservableNode<NodeData>>(e.Sender, neighbor);
					subject.OnNext((edge, true));
				}
			}
			void handleNeighborRemoved(object sender, NodeChangedEventArgs<ObservableNode<NodeData>> e)
			{
				foreach (var neighbor in e.Neighbors)
				{
					var edge = new Edge<ObservableNode<NodeData>>(e.Sender, neighbor);
					subject.OnNext((edge, false));
				}
			}

			addedStream = addedStream.Do(onAdded);
			removedStream = removedStream.Do(onRemoved);

			return subject;
		}

		private IObservable<Edge<ObservableNode<NodeData>>> FilterEquivalentEdges(IObservable<Edge<ObservableNode<NodeData>>> unfiltered)
		{
			var emptyEdge = new Edge<ObservableNode<NodeData>>();
			var seed = (previous: emptyEdge, current: emptyEdge);

			var pairs = unfiltered
				.Scan(seed, (accumulate, edge) =>
				{
					accumulate.previous = accumulate.current;
					accumulate.current = edge;

					return accumulate;
				});

			var equivalentEdgesFiltered = pairs
				.Where(t => !t.current.Equivalent(t.previous))
				.Select(t => t.current);

			return equivalentEdgesFiltered;
		}

		private async Task UpdatePath()
		{
			if (StartNode is null || GoalNode is null)
				return;

			float costAdder(float left, float right) => left + right;
			float costFunc(ObservableNode<NodeData> left, ObservableNode<NodeData> right)
			{
				return (left.Value.Position - right.Value.Position).Length();
			}

			ShortestPath = await Task.Run(() => AStar.FindPath(StartNode, GoalNode, costFunc, costFunc, costAdder));
		}

	}
}
