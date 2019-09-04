using NewellClark.DataStructures.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	public class DrawingPanel : Panel
	{
		internal BitmapGraph Graph { get; }

		internal IReadOnlyList<Toggleable> Tools { get; }
		private readonly DisposableList<Toggleable> _Tools;

		private readonly DisposableDictionary<Color, Brush> _brushes;
		private readonly CompositeDisposable _disposables;

		private static readonly float invalidatedRegionInflationFactor = 1.25f;
		private static readonly int pixelBorderInset = 2;


		public DrawingPanel()
		{
			this.Graph = new BitmapGraph(new IntVector2(30, 24));
			_Tools = new DisposableList<Toggleable>()
			{
				new BlockUnblockNodeTool(Graph, this) { Enabled = true },
				new SetPathEndpointsToggleTool(Graph, this)
			};
			Tools = new ReadOnlyCollection<Toggleable>(_Tools);
			_brushes = new DisposableDictionary<Color, Brush>();
			_disposables = InitializeEventHandlers();

			this.DoubleBuffered = true;
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			Debug.Assert(Graph != null);
			Debug.Assert(_Tools != null);
			Debug.Assert(_brushes != null);

			IObservable<Rectangle> whenRedrawNeeded = Graph.WhenNodeChanged
				.Select(node => PixelPointToClientBounds(node.Position));

			void onRectNeedsRedraw(Rectangle bounds)
			{
				var inflation = Vector2.One * invalidatedRegionInflationFactor;
				bounds.Inflate(inflation.ToSize());
				this.Invalidate(bounds);
			}

			void onPaint(PaintEventArgs e)
			{
				e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				DrawImagePixelsManually(e);
			}


			return new CompositeDisposable
			{
				Graph, _Tools, _brushes,
				whenRedrawNeeded.Subscribe(onRectNeedsRedraw),
				this.WhenPaint().Subscribe(onPaint)
			};
		}

		private void DrawImagePixelsManually(PaintEventArgs e)
		{
			Color selectColor(IntVector2 pixelPoint)
			{
				var node = Graph[pixelPoint];

				if (node.IsStart)
					return Graph.StartColor;

				if (node.IsGoal)
					return Graph.GoalColor;

				if (node.IsPath)
					return Graph.PathColor;

				return node.Color;
			}

			foreach (IntVector2 pixelPoint in Graph.Bounds.LatticePoints())
			{
				Color color = selectColor(pixelPoint);
				Brush brush = _brushes.GetOrAdd(color, c => new SolidBrush(c));
				Rectangle fillBounds = PixelPointToClientBounds(pixelPoint).Inset(pixelBorderInset);
				e.Graphics.FillRectangle(brush, fillBounds);
			}
		}

		private Vector2 PixelSize
		{
			get
			{
				Vector2 client = this.ClientSize.ToVector2();
				Vector2 image = Graph.Image.Size.ToVector2();

				return client / image;
			}
		}

		private IntVector2 PixelPointToClient(IntVector2 pixelPoint)
		{
			return (pixelPoint.Multiply(PixelSize)).ToIntVector2();
		}

		private Rectangle PixelPointToClientBounds(IntVector2 pixelPoint)
		{
			Point topLeft = PixelPointToClient(pixelPoint).ToPoint();
			Size size = PixelSize.ToSize();

			return new Rectangle(topLeft, size);
		}
	}
}
