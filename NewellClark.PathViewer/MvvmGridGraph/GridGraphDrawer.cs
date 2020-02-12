using NewellClark.DataStructures.Collections;
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

namespace NewellClark.PathViewer.MvvmGridGraph
{
	class GridGraphDrawer : IDisposable
	{
		private readonly Control _control;
		private readonly GridGraphViewModel _vm;
		private readonly DisposableDictionary<Color, Brush> _brushes;
		private readonly CompositeDisposable _disposables;

		public GridGraphDrawer(GridGraphViewModel vm, Control control)
		{
			if (vm is null) throw new ArgumentNullException(nameof(vm));
			if (control is null) throw new ArgumentNullException(nameof(control));

			_vm = vm;
			_control = control;
			_brushes = new DisposableDictionary<Color, Brush>();
			PathPen = new Pen(PathBrush, 8);
			_disposables = InitializeEventHandlers();
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			void onNodeChanged(GridNodeViewModel nodeVm) => _control.Invalidate(nodeVm.Bounds);
			void onViewBoundsChanged(Rectangle bounds) => _control.Invalidate(bounds);
			void onPathChanged(object sender, EventArgs e) => _control.Invalidate();
			_vm.PathFinder.ShortestPathChanged += onPathChanged;

			var viewBoundsChanged = _vm.CellSizeChanged
				.Select(_ => _vm.ViewBounds)
				.Merge(_vm.ViewBoundsChanged);
			
			return new CompositeDisposable
			{
				_vm.NodeChanged.Subscribe(onNodeChanged),
				viewBoundsChanged.Subscribe(onViewBoundsChanged),
				_control.WhenPaint().Subscribe(DrawAll),
				_brushes, 
				PathPen,
				Disposable.Create(() => _vm.PathFinder.ShortestPathChanged -= onPathChanged)

                
			};
		}

		public void Dispose() => _disposables.Dispose();

		protected virtual void DrawAll(PaintEventArgs e)
		{
			var nodesToDraw = _vm.VisibleNodes.Where(
				nodeVM => e.Graphics.Clip.IsVisible(nodeVM.Bounds));

			foreach (var nodeVM in nodesToDraw)
				DrawNode(e, nodeVM);

			var pathNodesToDraw = _vm.PathFinder.ShortestPath.Nodes
				.Select(node => _vm[node.Position])
				.Where(nodeVM => e.Graphics.Clip.IsVisible(nodeVM.Bounds));

			DrawPath(e, pathNodesToDraw);
		}

		protected virtual void DrawNode(PaintEventArgs e, GridNodeViewModel nodeVM)
		{
			var bounds = nodeVM.Bounds.Inset(NodeBoxInset);
			Brush brush = GetBrushForNode(nodeVM);

			e.Graphics.FillRectangle(brush, bounds);
		}

		protected virtual void DrawPath(PaintEventArgs e, IEnumerable<GridNodeViewModel> path)
		{
			void drawNode(GridNodeViewModel nodeVM)
			{
				IntVector2 scaledSize = (nodeVM.Bounds.Size.ToIntVector2() * PathOverlayScaleFactor).ToIntVector2();
				Rectangle scaledBounds = Geometry.CenteredRectangle(nodeVM.ClientCenterPosition, scaledSize);

				e.Graphics.FillEllipse(PathBrush, scaledBounds);
			}

			using (var iter = path.GetEnumerator())
			{
				if (!iter.MoveNext())
					return;

				drawNode(iter.Current);

				Point previous = iter.Current.ClientCenterPosition.ToPoint();
				while (iter.MoveNext())
				{
					drawNode(iter.Current);
					e.Graphics.DrawLine(PathPen, previous, iter.Current.ClientCenterPosition.ToPoint());
					previous = iter.Current.ClientCenterPosition.ToPoint();
				}
			}
		}

		private Brush GetBrushForNode(GridNodeViewModel nodeVM)
		{
			Brush brush;

			if (nodeVM.HighlightColor.HasValue)
				brush = _brushes.GetOrAdd(nodeVM.HighlightColor.Value, color => new SolidBrush(color));
			else if (nodeVM.Node.Position == _vm.PathFinder.StartNode)
				brush = StartBrush;
			else if (nodeVM.Node.Position == _vm.PathFinder.GoalNode)
				brush = GoalBrush;
			else
				brush = nodeVM.Node.IsPassable ? PassableBrush : BlockedBrush;
			return brush;
		}

		private Brush BlockedBrush => Brushes.Black;
		private Brush PassableBrush => Brushes.HotPink;
		private Brush StartBrush => Brushes.Green;
		private Brush GoalBrush => Brushes.Gold;
		private Brush PathBrush => Brushes.DarkViolet;
		private Pen PathPen { get; } 
		private int NodeBoxInset => 4;
		private float PathOverlayScaleFactor => 0.6f;
	}
}
