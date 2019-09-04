using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	class SetPathEndpointsToggleTool : Toggleable
	{
		private readonly BitmapGraph _graph;
		private readonly Control _control;

		public SetPathEndpointsToggleTool(BitmapGraph graph, Control control)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (control is null) throw new ArgumentNullException(nameof(control));

			_graph = graph;
			_control = control;
		}

		protected override IDisposable Enable()
		{
			Debug.Assert(_graph != null);
			Debug.Assert(_control != null);

			var bothButtons = StartNodeButton | GoalNodeButton;
			var whenEitherMouseButton = _control.WhenMouseDown()
				.Merge(_control.WhenMouseMoved())
				.Where(e => (e.Button & bothButtons) != MouseButtons.None)
				.Select(e => (button: e.Button, point: ClientPointToBitmap(e.Location.ToIntVector2())))
				.Where(t => _graph.Bounds.Contains(t.point));

			void onClick((MouseButtons button, IntVector2 point) t)
			{
				if (t.button == StartNodeButton)
					_graph.Start = t.point;
				else if (t.button == GoalNodeButton)
					_graph.Goal = t.point;
			}

			return whenEitherMouseButton.Subscribe(onClick);
		}

		public override string DisplayName => "Set Start/Goal";

		public MouseButtons StartNodeButton => MouseButtons.Left;

		public MouseButtons GoalNodeButton => MouseButtons.Right;

		private Vector2 PixelSize => _control.ClientSize.ToVector2() / _graph.Image.Size.ToVector2();

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
