using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;
using System.Reactive.Disposables;
using System.Diagnostics;
using System.Reactive.Linq;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	class BlockUnblockNodeTool : Toggleable, IDisposable
	{
		private readonly BitmapGraph _graph;
		private readonly Control _control;

		public BlockUnblockNodeTool(BitmapGraph graph, Control control)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (control is null) throw new ArgumentNullException(nameof(control));

			_graph = graph;
			_control = control;
		}

		protected override IDisposable Enable()
		{
			Color? chooseColor(MouseButtons button)
			{
				if (button == MouseButtons.Left)
					return BlockedColor;
				else if (button == MouseButtons.Right)
					return UnblockedColor;
				else return null;
			}

			var mergedMouseEvents = _control.WhenMouseDown()
				.Merge(_control.WhenMouseMoved())
				.Select(e => (e: e, color: chooseColor(e.Button)))
				.Where(t => t.color.HasValue)
				.Select(t =>
				{
					return (
						point: ClientPointToBitmap(t.e.Location.ToIntVector2()),
						color: t.color.Value);
				})
				.Where(t => _graph.Bounds.Contains(t.point));

			void onNext((IntVector2 point, Color color) t) => 
				_graph.SetPixel(t.point, t.color);


			return mergedMouseEvents.Subscribe(onNext);
		}

		public override string DisplayName => "Add/Remove obstacles";

		public Color BlockedColor => _graph.BlockedColor;

		public Color UnblockedColor => _graph.UnblockedColor;

		/// <summary>
		/// Converts a point on the client area of the control to a point in the bitmap graph.
		/// </summary>
		/// <param name="clientPoint"></param>
		/// <returns></returns>
		private IntVector2 ClientPointToBitmap(IntVector2 clientPoint)
		{
			Vector2 imageSize = _graph.Image.Size.ToVector2();
			Vector2 controlSize = _control.ClientSize.ToVector2();
			Vector2 scaleFactor = imageSize / controlSize;

			return clientPoint.Multiply(scaleFactor).ToIntVector2();
		}
	}
}
