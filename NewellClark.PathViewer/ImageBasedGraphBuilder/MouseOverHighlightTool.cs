using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	class MouseOverHighlighter : IDisposable
	{
		private readonly BitmapGraph _graph;
		private readonly Control _control;
		private readonly CompositeDisposable _disposables;

		private const float penSize = 5;
		private readonly Pen pen1 = new Pen(Brushes.Navy, penSize);
		private readonly Pen pen2 = new Pen(Brushes.HotPink, penSize);
		private Rectangle _freeRect;
		private Rectangle _gridAlignedIntRect;

		public MouseOverHighlighter(BitmapGraph graph, Control control)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (control is null) throw new ArgumentNullException(nameof(control));

			_graph = graph;
			_control = control;
			_disposables = InitializeEventHandlers();
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			var mouseMove = _control.WhenMouseMoved();
			var paint = _control.WhenPaint();

			void onMouseMove(MouseEventArgs e)
			{
				IntVector2 cursor = e.Location.ToIntVector2();
				_freeRect = Geometry.CenteredRectangle(cursor, 30 * IntVector2.One);
				_gridAlignedIntRect = GetGridAlignedIntRect(e.Location.ToIntVector2());

				_control.Invalidate();
			}

			void onPaint(PaintEventArgs e)
			{
				e.Graphics.FillRectangle(Brushes.Navy, _gridAlignedIntRect);
				e.Graphics.FillRectangle(Brushes.HotPink, _freeRect);
			}

			return new CompositeDisposable
			{
				mouseMove.Subscribe(onMouseMove),
				paint.Subscribe(onPaint),
				pen1, pen2
			};
		}

		public void Dispose() => _disposables.Dispose();

		private Rectangle GetGridAlignedIntRect(IntVector2 clientPoint)
		{
			Vector2 scaleFactor = BitmapToClientSize;
			Vector2 bitmapPoint = ClientPointToBitmap(clientPoint);
			IntVector2 lowerLeft = (bitmapPoint * scaleFactor).ToIntVector2();

			return new Rectangle(lowerLeft.ToPoint(), scaleFactor.ToIntVector2().ToSize());
		}

		private IntVector2 ClientPointToBitmap(IntVector2 clientPoint)
		{
			Vector2 imageSize = _graph.Image.Size.ToVector2();
			Vector2 controlSize = _control.ClientSize.ToVector2();
			Vector2 scaleFactor = imageSize / controlSize;

			return clientPoint.Multiply(scaleFactor).ToIntVector2();
		}

		private Vector2 BitmapToClientSize
		{
			get => _control.ClientSize.ToVector2() / _graph.Image.Size.ToVector2();
		}
	}
}
