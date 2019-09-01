using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Linq;

namespace NewellClark.PathViewer
{
	/// <summary>
	/// An object that can provide mouse event observables.
	/// </summary>
	interface IMouseProvider
	{
		IObservable<MouseEventArgs> WhenMouseClicked { get; }

		IObservable<MouseEventArgs> WhenMouseDoubleClicked { get; }

		IObservable<MouseEventArgs> WhenMouseDown { get; }

		IObservable<MouseEventArgs> WhenMouseUp { get; }

		IObservable<MouseEventArgs> WhenMouseMove { get; }

		IObservable<MouseEventArgs> WhenMouseWheel { get; }
	}
}
