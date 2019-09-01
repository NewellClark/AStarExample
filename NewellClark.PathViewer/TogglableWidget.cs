using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer
{
	/// <summary>
	/// Base class for an object that can be enabled and disabled via a boolean property.
	/// </summary>
	abstract class TogglableWidget
	{
		protected TogglableWidget() { }

		/// <summary>
		/// Gets or sets whether the current <see cref="TogglableWidget"/> is enabled.
		/// </summary>
		public bool Enabled
		{
			get => _disposeToDisable != null;
			set
			{
				if (Enabled == value)
					return;

				if (value)
				{
					_disposeToDisable?.Dispose();
					_disposeToDisable = Enable();

					Debug.Assert(
						_disposeToDisable != null,
						$"A proper override of {nameof(TogglableWidget)}.{nameof(Enable)}() must never return null.");
				}
				else
				{
					Debug.Assert(_disposeToDisable != null);

					_disposeToDisable.Dispose();
					_disposeToDisable = null;
				}

				OnEnabledChanged(EventArgs.Empty);
			}
		}
		private IDisposable _disposeToDisable;

		/// <summary>
		/// Raised after the <see cref="Enabled"/> property has changed.
		/// </summary>
		public event EventHandler EnabledChanged;

		/// <summary>
		/// Raises the <see cref="EnabledChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			EnabledChanged?.Invoke(this, e);
		}

		/// <summary>
		/// When overridden in a derived class, enables the tool and returns an <see cref="IDisposable"/>
		/// that will disable the current <see cref="TogglableWidget"/> when <see cref="IDisposable.Dispose"/> is called.
		/// </summary>
		/// <returns>
		/// An <see cref="IDisposable"/> object that will disable the current <see cref="TogglableWidget"/> 
		/// when it is disposed.
		/// </returns>
		/// <remarks>
		/// This method will be called by the <see cref="Enabled"/> property setter. The setter will
		/// ensure that <see cref="Enabled"/> is only called once, and won't be called if the object is 
		/// already enabled.
		/// The <see cref="IDisposable.Dispose"/> method on the returned instance may be invoked more than once.
		/// This method must not return null.
		/// </remarks>
		protected abstract IDisposable Enable();
	}
}
