using NewellClark.DataStructures.Collections;
using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Concurrency;
using System.Threading;

namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	class GraphPresenter : IDisposable
	{
		private readonly CompositeDisposable _disposables;

		public GraphPresenter(GraphEditor editor)
		{
			if (editor is null) throw new ArgumentNullException(nameof(editor));

			this.Editor = editor;
			_disposables = new CompositeDisposable();

			var regionsNeedRedrawing = CreateRegionRedrawStream();

			void onNext(Region region) => Editor.Control.Invalidate(region);
			AddSubscription(regionsNeedRedrawing.Subscribe(onNext));

			var paintStream = Editor.Control.WhenPaint().Publish();
			AddSubscription(paintStream.Connect());
			WhenRedrawNeeded = paintStream.AsObservable();
			AddSubscription(WhenRedrawNeeded.Subscribe(Draw));

			DefaultNodeBrush = Brushes.Blue;
			MouseOverNodeBrush = Brushes.Purple;
			DefaultEdgeBrush = Brushes.Blue;
			DefaultEdgePen = new Pen(DefaultEdgeBrush, 5);
			AddSubscription(DefaultEdgePen);
			StartNodeBrush = Brushes.Cyan;
			GoalNodeBrush = Brushes.Green;
			PathNodeBrush = Brushes.Navy;
			PathEdgeBrush = PathNodeBrush;
			PathEdgePen = new Pen(PathEdgeBrush, 9);
			AddSubscription(PathEdgePen);
		}

		private IObservable<Region> CreateRegionRedrawStream()
		{
			Region pathBoundsRegion(Path<ObservableNode<NodeData>, float> path)
			{
				var bounds = path.Edges().Select(edge => edge.Bounds())
					.Concat(path.Nodes.Select(node => node.Value.Bounds));

				var region = new Region();
				region.MakeEmpty();
				foreach (var rect in bounds)
					region.Union(rect);

				return region;
			}

			var startChanged = RebroadcastPrevious(Editor.WhenStartNodeChanged)
				.Where(node => node != null);
			var goalChanged = RebroadcastPrevious(Editor.WhenGoalNodeChanged)
				.Where(node => node != null);

			var nodeBounds = Editor.WhenNodeAdded
				.Merge(Editor.WhenNodeRemoved)
				.Merge(startChanged)
				.Merge(goalChanged)
				.Select(node => node.Value.Bounds);
			var edgeBounds = Editor.WhenEdgeAdded
				.Merge(Editor.WhenEdgeRemoved)
				.Select(edge => edge.Bounds());

			var pathRegions = RebroadcastPrevious(Editor.WhenShortestPathChanged)
				.Where(path => path != null)
				.Select(path => pathBoundsRegion(path));
			
			var regionNeedsRedrawing = nodeBounds
				.Merge(edgeBounds)
				.Select(rect => new Region(rect))
				.Merge(pathRegions);

			return regionNeedsRedrawing;
		}

		public GraphEditor Editor { get; }

		public IObservable<PaintEventArgs> WhenRedrawNeeded { get; }

		public Brush DefaultNodeBrush { get; }

		public Brush MouseOverNodeBrush { get; }

		public Brush StartNodeBrush { get; }

		public Brush GoalNodeBrush { get; }

		public Brush PathNodeBrush { get; }

		public Brush DefaultEdgeBrush { get; }

		public Pen DefaultEdgePen { get; }

		public Brush PathEdgeBrush { get; }

		public Pen PathEdgePen { get; }

		public float NodeScaleFactor { get; } = 0.9f;

		public void Dispose() => _disposables.Dispose();

		public Circle? HighlightedRegion { get; set; }

		protected void AddSubscription(IDisposable subscription) => _disposables.Add(subscription);

		protected void AddSubscription(Action cleanup) => AddSubscription(Disposable.Create(cleanup));

		protected virtual void DrawNode(PaintEventArgs e, ObservableNode<NodeData> node)
		{
			Debug.Assert(e != null);
			Debug.Assert(node != null);

			var rect = Geometry.CenteredRectangle(node.Value.Position, node.Value.Size * NodeScaleFactor);
			var brush = GetNodeBrush(node, DefaultNodeBrush);
			e.Graphics.FillEllipse(brush, rect);
		}
		
		protected virtual void DrawEdge(PaintEventArgs e, Edge<ObservableNode<NodeData>> edge)
		{
			Debug.Assert(e != null);
			Debug.Assert(edge.Left != null);
			Debug.Assert(edge.Right != null);

			PointF left = edge.Left.Value.Position.ToPointF();
			PointF right = edge.Right.Value.Position.ToPointF();
			e.Graphics.DrawLine(DefaultEdgePen, left, right);
		}

		protected virtual void DrawPath(PaintEventArgs e, Path<ObservableNode<NodeData>, float> path)
		{
			Debug.Assert(e != null);
			Debug.Assert(path != null);

			RectangleF clip = e.Graphics.ClipBounds;

			var edges = path.Edges()
				.Where(edge => e.Graphics.Clip.IsVisible(edge.Bounds()));
			foreach (var edge in edges)
				DrawPathEdge(e, edge);

			var nodes = path.Nodes
				.Where(node => e.Graphics.Clip.IsVisible(node.Value.Bounds));
			foreach (var node in nodes)
				DrawPathNode(e, node);
		}

		protected void DrawPathNode(PaintEventArgs e, ObservableNode<NodeData> node)
		{
			Debug.Assert(e != null);
			Debug.Assert(node != null);

			var rect = Geometry.CenteredRectangle(node.Value.Position, node.Value.Size * NodeScaleFactor);
			var brush = GetNodeBrush(node, PathNodeBrush);
			e.Graphics.FillEllipse(brush, rect);
		}

		protected virtual void DrawPathEdge(PaintEventArgs e, Edge<ObservableNode<NodeData>> edge)
		{
			Debug.Assert(e != null);
			Debug.Assert(edge.Left != null);
			Debug.Assert(edge.Right != null);

			PointF left = edge.Left.Value.Position.ToPointF();
			PointF right = edge.Right.Value.Position.ToPointF();
			e.Graphics.DrawLine(PathEdgePen, left, right);
		}

		protected virtual void Draw(PaintEventArgs e)
		{
			Debug.Assert(e != null);

			foreach (var edge in Editor.Edges)
			{
				if (e.Graphics.Clip.IsVisible(edge.Bounds()))
					DrawEdge(e, edge);
			}

			foreach (var node in Editor.Nodes)
			{
				if (e.Graphics.Clip.IsVisible(node.Value.Bounds))
					DrawNode(e, node);
			}

			if (Editor.ShortestPath != null)
				DrawPath(e, Editor.ShortestPath);
		}

		private Brush GetNodeBrush(ObservableNode<NodeData> node, Brush @default)
		{
			Debug.Assert(node != null);

			if (node == Editor.StartNode)
				return StartNodeBrush;

			if (node == Editor.GoalNode)
				return GoalNodeBrush;

			if (HighlightedRegion.HasValue && HighlightedRegion.Value.Overlaps(node.Value.Circle))
				return MouseOverNodeBrush;

			return @default;
		}

		private static IObservable<T> RebroadcastPrevious<T>(IObservable<T> observable) =>
			RebroadcastPrevious(observable, default(T));
		private static IObservable<T> RebroadcastPrevious<T>(IObservable<T> observable, T initial)
		{
			(T previous, T current) seed = (initial, initial);
			IEnumerable<T> deconstructTuple((T previous, T current) tuple)
			{
				yield return tuple.previous;
				yield return tuple.current;
			}

			var result = observable
				.Scan(seed, (tuple, item) => (tuple.current, item))
				.SelectMany(tuple => deconstructTuple(tuple));

			return result;
		}
	}
}
