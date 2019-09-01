using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	class NodeHighlighter : IDisposable
	{
		private readonly GraphEditor _editor;
		private readonly CompositeDisposable _disposables;
		private IEnumerable<ObservableNode<NodeData>> _previous;

		public NodeHighlighter(GraphEditor editor, float radius)
		{
			if (editor is null) throw new ArgumentNullException(nameof(editor));

			if (radius < 0)
				throw new ArgumentOutOfRangeException(nameof(radius), $"{nameof(radius)} cannot be negative.");

			_editor = editor;
			this.Radius = radius;
			_previous = Enumerable.Empty<ObservableNode<NodeData>>();

			var highlightedNodes = _editor.Control.WhenMouseMoved()
				.Select(e => new Circle(e.Location.ToVector2(), Radius))
				.Select(circle => (circle: circle, nodes: _editor.GetNodesTouchingRegion(circle)));

			Region getRedrawRegion(IEnumerable<ObservableNode<NodeData>> nodes)
			{
				var seed = new Region();
				seed.MakeEmpty();

				return nodes.Aggregate(seed, (region, node) =>
				{
					region.Union(node.Value.Bounds);
					return region;
				});
			}

			void onNext((Circle circle, IEnumerable<ObservableNode<NodeData>> nodes) t)
			{
				_editor.Presenter.HighlightedRegion = t.circle;
				var nodesNeedingUpdate = new HashSet<ObservableNode<NodeData>>(_previous);
				nodesNeedingUpdate.SymmetricExceptWith(t.nodes);
				_previous = t.nodes;

				var region = getRedrawRegion(nodesNeedingUpdate);
				_editor.Control.Invalidate(region);
			}

			_disposables = new CompositeDisposable { highlightedNodes.Subscribe(onNext) };
		}

		public void Dispose()
		{
			_disposables.Dispose();
			_editor.Presenter.HighlightedRegion = null;
		}

		public float Radius { get; }
	}
}
