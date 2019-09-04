using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public class GridGraphPathFinder : IDisposable
	{
		private readonly GridGraph _graph;
		private readonly CompositeDisposable _disposables;

		public GridGraphPathFinder(GridGraph graph)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));

			_graph = graph;
			_NodeChanged = new Subject<(GridNode node, bool updatePath)>();
			_BoundsChanged = new PropertySubject<Rectangle>(new Rectangle(Point.Empty, new Size(20, 20)));
			_ShortestPath = Path.Create<GridNode, float>(
				(left, right) => (left.Position - right.Position).Length,
				(a, b) => a + b, 0);
			_disposables = InitializeEventHandlers();
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			Debug.Assert(_NodeChanged != null);

			var nodePassabilityChanged = _graph.NodeChanged.Select(_ => Unit.Default);
			var boundsChanged = BoundsChanged.Select(_ => Unit.Default);
			var endpointChanged = _NodeChanged
				.Where(t => t.updatePath)
				.Select(_ => Unit.Default);

			var pathNeedsUpdate = nodePassabilityChanged
				.Merge(boundsChanged)
				.Merge(endpointChanged);

			return new CompositeDisposable
			{
				_NodeChanged,
				_BoundsChanged,
				pathNeedsUpdate.Subscribe(_ => UpdatePath())
			};
		}

		public IObservable<GridNode> NodeChanged => _NodeChanged.Select(t => t.node);
		private readonly Subject<(GridNode node, bool updatePath)> _NodeChanged;

		public IntVector2? StartNode
		{
			get => _StartNode;
			set
			{
				if (_StartNode == value)
					return;

				var previous = _StartNode;
				_StartNode = value;

				UpdateEndpointIfNotNull(previous, false);
				UpdateEndpointIfNotNull(_StartNode, true);
			}
		}
		private IntVector2? _StartNode;

		public IntVector2? GoalNode
		{
			get => _GoalNode;
			set
			{
				if (_GoalNode == value)
					return;

				var previous = _GoalNode;
				_GoalNode = value;
				if (_GoalNode is null)
					return;

				UpdateEndpointIfNotNull(previous, false);
				UpdateEndpointIfNotNull(_GoalNode, true);
			}
		}
		private IntVector2? _GoalNode;

		private void UpdateEndpointIfNotNull(IntVector2? position, bool updatePath)
		{
			if (position == null)
				return;

			_NodeChanged.OnNext((_graph[position.Value], true));
		}

		public Path<GridNode, float> ShortestPath
		{
			get => _ShortestPath;
			set
			{
				if (_ShortestPath == value)
					return;

				var previous = _ShortestPath;
				_ShortestPath = value;

				var nodesToUpdate = previous.Nodes.Concat(_ShortestPath.Nodes).Distinct();

				foreach (var node in nodesToUpdate)
					_NodeChanged.OnNext((node, false));
			}
		}
		private Path<GridNode, float> _ShortestPath;

		public Rectangle Bounds
		{
			get => _BoundsChanged.Value;
			set => _BoundsChanged.Value = value;
		}
		public IObservable<Rectangle> BoundsChanged => _BoundsChanged.AsObservable();
		private readonly PropertySubject<Rectangle> _BoundsChanged;

		private Path<GridNode, float> FindPath(GridNode start, GridNode goal)
		{
			float costFunction(GridNode first, GridNode second) =>
				(first.Position - second.Position).Length;
			float costAdder(float left, float right) => left + right;
			bool nodeFilter(GridNode node)
			{
				return node.IsPassable &&
					Bounds.Contains(node.Position);
			}

			return AStar.FindPath(start, goal, costFunction, costFunction, costAdder, nodeFilter);
		}

		private void UpdatePath()
		{
			if (StartNode is null || GoalNode is null)
				return;

			ShortestPath = FindPath(_graph[StartNode.Value], _graph[GoalNode.Value]);
		}

		public void Dispose() => _disposables.Dispose();
	}
}
