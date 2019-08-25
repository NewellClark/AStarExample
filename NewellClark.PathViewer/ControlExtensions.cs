using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace NewellClark.PathViewer
{
	static class ControlExtensions
	{
		public static IObservable<MouseEventArgs> GetMouseClickStream(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseClick += h,
				h => @this.MouseClick -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> GetMouseMoveStream(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseMove += h,
				h => @this.MouseMove -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> GetMouseDownStream(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseDown += h,
				h => @this.MouseDown -= h)
				.Select(evp => evp.EventArgs);
		}

		public static IObservable<MouseEventArgs> GetMouseUpStream(this Control @this)
		{
			return Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(
				h => @this.MouseUp += h,
				h => @this.MouseUp -= h)
				.Select(evp => evp.EventArgs);
		}
	}
}
