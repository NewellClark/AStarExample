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
	/// <summary>
	/// Component responsible for computing the shortest path when given a start and goal position.
	/// </summary>
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
			_HeuristicChanged = new PropertyChangedEvent<PathFinderHeuristic>(this, PathFinderHeuristic.Euclidian);
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
			var heuristicChanged = Observable.FromEventPattern(
				h => HeuristicChanged += h,
				h => HeuristicChanged -= h)
				.Select(_ => Unit.Default);
			var allowedMovements = Observable.FromEventPattern(
				h => _graph.AllowedMovementsChanged += h,
				h => _graph.AllowedMovementsChanged -= h)
				.Select(_ => Unit.Default);

			var pathNeedsUpdate = nodePassabilityChanged
				.Merge(boundsChanged)
				.Merge(endpointChanged)
				.Merge(heuristicChanged)
				.Merge(allowedMovements);

			return new CompositeDisposable
			{
				_NodeChanged,
				_BoundsChanged,
				pathNeedsUpdate.Subscribe(_ => UpdatePath())
			};
		}

		/// <summary>
		/// Pushes an event whenever any information about a node within the path finder gets updated,
		/// for example, if a node becomes the start/goal node, or stops being the start/goal node, or if a node
		/// becomes or stops being part of the shortest path. Basically subscribe to this to know when 
		/// a node needs to be redrawn when drawing pathfinder-related information.
		/// </summary>
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
				OnShortestPathChanged(EventArgs.Empty);

				var nodesToUpdate = previous.Nodes.Concat(_ShortestPath.Nodes).Distinct();
				foreach (var node in nodesToUpdate)
					_NodeChanged.OnNext((node, false));
			}
		}
		private Path<GridNode, float> _ShortestPath;

		public event EventHandler ShortestPathChanged;
		private void OnShortestPathChanged(EventArgs e) => ShortestPathChanged?.Invoke(this, e);

		public Rectangle Bounds
		{
			get => _BoundsChanged.Value;
			set => _BoundsChanged.Value = value;
		}
		public IObservable<Rectangle> BoundsChanged => _BoundsChanged.AsObservable();
		private readonly PropertySubject<Rectangle> _BoundsChanged;

		private Path<GridNode, float> FindPath(GridNode start, GridNode goal)
		{
			var knownCost = GetHeuristic(PathFinderHeuristic.Euclidian);
			var estimatedCost = GetHeuristic(Heuristic);
			float costAdder(float left, float right) => left + right;
			bool nodeFilter(GridNode node)
			{
				return node.IsPassable &&
					Bounds.Contains(node.Position);
			}

			return AStar.FindPath(start, goal, knownCost, estimatedCost, costAdder, nodeFilter);
		}

		private void UpdatePath()
		{
			if (StartNode is null || GoalNode is null)
				return;

			ShortestPath = FindPath(_graph[StartNode.Value], _graph[GoalNode.Value]);
		}

		public void Dispose() => _disposables.Dispose();

		public PathFinderHeuristic Heuristic
		{
			get => _HeuristicChanged.Value;
			set => _HeuristicChanged.Value = value;
		}
		public event EventHandler HeuristicChanged
		{
			add => _HeuristicChanged.ValueChanged += value;
			remove => _HeuristicChanged.ValueChanged -= value;
		}
		private PropertyChangedEvent<PathFinderHeuristic> _HeuristicChanged;

		private CostFunction<GridNode, float> GetHeuristic(PathFinderHeuristic heuristic)
		{
			float euclidian(GridNode left, GridNode right) => left.Position.Distance(right.Position);
			float manhattan(GridNode left, GridNode right) => left.Position.ManhattanDistance(right.Position);
			float djikstra(GridNode left, GridNode right) => 0;
			
			switch (heuristic)
			{
				case PathFinderHeuristic.Euclidian:
					return euclidian;
				case PathFinderHeuristic.Manhattan:
					return manhattan;
				case PathFinderHeuristic.Djikstra:
					return djikstra;
				default:
					throw new ArgumentOutOfRangeException(nameof(heuristic),
						$"Unexpected enum value {heuristic} in {nameof(PathFinderHeuristic)} enum.");
			}
		}
	}

	public enum PathFinderHeuristic
	{
		Euclidian,
		Manhattan,
		Djikstra
	}
}
