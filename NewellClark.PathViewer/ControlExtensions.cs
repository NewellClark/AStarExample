using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using NewellClark.DataStructures.Collections;
using System.Drawing;

namespace NewellClark.PathViewer
{
	static class ControlExtensions
	{
		public static IObservable<MouseEventArgs> WhenMouseClicked(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseClick += h,
				h => @this.MouseClick -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> WhenMouseDoubleClicked(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseDoubleClick += h,
				h => @this.MouseDoubleClick -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> WhenMouseMoved(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseMove += h,
				h => @this.MouseMove -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> WhenMouseDown(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseDown += h,
				h => @this.MouseDown -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> WhenMouseUp(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseUp += h,
				h => @this.MouseUp -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> WhenMouseWheel(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseWheel += h,
				h => @this.MouseWheel -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<PaintEventArgs> WhenPaint(this Control @this)
		{
			return Observable.FromEventPattern<PaintEventHandler, PaintEventArgs>(
				h => @this.Paint += h,
				h => @this.Paint -= h)
				.Select(evp => evp.EventArgs);
		}

		/// <summary>
		/// Wraps the current <see cref="Control"/> in an <see cref="IMouseProvider"/> instance, which
		/// forwards all mouse events produced by the current <see cref="Control"/>.
		/// </summary>
		/// <param name="this"></param>
		/// <returns></returns>
		public static IMouseProvider ToMouseProvider(this Control @this)
		{
			if (@this is null) throw new ArgumentNullException(nameof(@this));

			return new MouseEventProvider(@this);
		}

		private class MouseEventProvider : IMouseProvider
		{
			public MouseEventProvider(Control inner)
			{
				Debug.Assert(inner != null);
				
				WhenMouseClicked = inner.WhenMouseClicked();
				WhenMouseDoubleClicked = inner.WhenMouseDoubleClicked();
				WhenMouseDown = inner.WhenMouseDown();
				WhenMouseUp = inner.WhenMouseUp();
				WhenMouseMove = inner.WhenMouseMoved();
				WhenMouseWheel = inner.WhenMouseWheel();
			}

			public IObservable<MouseEventArgs> WhenMouseClicked { get; }

			public IObservable<MouseEventArgs> WhenMouseDoubleClicked { get; }

			public IObservable<MouseEventArgs> WhenMouseDown { get; }

			public IObservable<MouseEventArgs> WhenMouseUp { get; }

			public IObservable<MouseEventArgs> WhenMouseMove { get; }

			public IObservable<MouseEventArgs> WhenMouseWheel { get; }
		}

		public static Color GetPixel(this Bitmap @this, Point point) => @this.GetPixel(point.X, point.Y);
		public static Color GetPixel(this Bitmap @this, IntVector2 point) => @this.GetPixel(point.X, point.Y);
		public static void SetPixel(this Bitmap @this, Point point, Color color) => @this.SetPixel(point.X, point.Y, color);
		public static void SetPixel(this Bitmap @this, IntVector2 point, Color color) => @this.SetPixel(point.X, point.Y, color);
		public static Rectangle Bounds(this Image @this) => new Rectangle(Point.Empty, @this.Size);
	}
}
