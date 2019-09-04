using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer
{
	/// <summary>
	/// Represents a tool that can be enabled and disabled by setting its boolean <see cref="Enabled"/> property.
	/// </summary>
	public abstract class Toggleable : IDisposable
	{
		protected Toggleable() { }

		/// <summary>
		/// Gets or sets whether the current <see cref="Toggleable"/> is enabled.
		/// </summary>
		/// <remarks>
		/// Setting this property to the same value multiple times in a row is idempotent.
		/// </remarks>
		public bool Enabled
		{
			get => _disposeToDisable != null;
			set
			{
				ThrowIfDisposed();

				if (Enabled == value)
					return;

				if (value)
				{
					//	If we were already enabled, we should have returned already.
					Debug.Assert(_disposeToDisable is null);

					_disposeToDisable = Enable();

					Debug.Assert(_disposeToDisable != null,
						$"Bad implementation of {nameof(Toggleable)}. {nameof(Toggleable.Enable)}() should " +
						$"never return null.");
				}
				else
				{
					//	If we were already disabled, we should have returned already.
					Debug.Assert(_disposeToDisable != null);

					_disposeToDisable.Dispose();
					_disposeToDisable = null;
				}

				OnEnabledChanged(EventArgs.Empty);
			}
		}
		private IDisposable _disposeToDisable;

		/// <summary>
		/// Raised when the <see cref="Enabled"/> property has changed.
		/// </summary>
		/// <exception cref="ObjectDisposedException">
		/// An event handler is added after the current <see cref="Toggleable"/> has been disposed.
		/// Removing a handler will <i>not</i> raise this exception.
		/// </exception>
		public event EventHandler EnabledChanged
		{
			add
			{
				ThrowIfDisposed();
				_EnabledChanged += value;
			}
			remove => _EnabledChanged -= value;
		}
		private EventHandler _EnabledChanged;

		/// <summary>
		/// Raises the <see cref="EnabledChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			_EnabledChanged?.Invoke(this, e);
		}

		/// <summary>
		/// When overridden in a derived class, enables the current <see cref="Toggleable"/> and
		/// returns an <see cref="IDisposable"/> that will disable the current <see cref="Toggleable"/>
		/// when it is disposed.
		/// </summary>
		/// <returns>
		/// An <see cref="IDisposable"/> that will disable the current <see cref="Toggleable"/> when
		/// <see cref="IDisposable.Dispose"/> is called.
		/// </returns>
		/// <remarks>
		/// This will be called be the <see cref="ENabled"/> property setter when <see cref="ENabled"/> is set to 
		/// true. This method must return a non-null <see cref="IDisposable"/> instance that will disable
		/// the current <see cref="Toggleable"/> when it is disposed.
		/// The <see cref="Enabled"/> property setter will ensure that this method is not called unnecessarily; for example,
		/// if the current <see cref="Toggleable"/> is already enabled, setting <see cref="Enabled"/> to true will
		/// be a no-op, and vice-versa. The <see cref="Enabled"/> property will also be responsible for raising
		/// the <see cref="EnabledChanged"/> event.
		/// When the current <see cref="Toggleable"/> is disposed, <see cref="Enabled"/> will be set to false. 
		/// Also, the <see cref="Enabled"/> property setter takes care of throwing an <see cref="ObjectDisposedException"/> if
		/// the current <see cref="Toggleable"/> has been disposed.
		/// </remarks>
		protected abstract IDisposable Enable();

		/// <summary>
		/// Disposes the current <see cref="Toggleable"/>, setting <see cref="Enabled"/> to false.
		/// </summary>
		public void Dispose()
		{
			if (IsDisposed)
				return;

			Enabled = false;
			_EnabledChanged = null;
			IsDisposed = true;
			OnDisposed(EventArgs.Empty);
		}

		/// <summary>
		/// Raised after the current <see cref="Toggleable"/> has been disposed.
		/// </summary>
		public event EventHandler Disposed;

		/// <summary>
		/// Raises the <see cref="Disposed"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDisposed(EventArgs e)
		{
			Disposed?.Invoke(this, e);
		}

		/// <summary>
		/// Gets a value indicating whether the current <see cref="Toggleable"/> has been disposed.
		/// </summary>
		public bool IsDisposed { get; private set; } = false;

		public virtual string DisplayName => "Toggle Tool";

		/// <summary>
		/// Throws an <see cref="ObjectDisposedException"/> if the current <see cref="Toggleable"/> has been disposed.
		/// </summary>
		protected void ThrowIfDisposed()
		{
			if (IsDisposed)
				throw new ObjectDisposedException(
					GetType().Name, $"{GetType().Name} has been disposed.");
		}

		private class CustomToggleable : Toggleable
		{
			private readonly string _displayName;
			private readonly Func<IDisposable> _enable;

			public CustomToggleable(Func<IDisposable> enable) : this(enable, "Toggle Tool") { }
			public CustomToggleable(Func<IDisposable> enable, string displayName)
			{
				Debug.Assert(enable != null);

				_enable = enable;
				_displayName = displayName;
			}

			protected override IDisposable Enable() => _enable();

			public override string DisplayName => _displayName;
		}

		public static Toggleable Create(Func<IDisposable> enable) => new CustomToggleable(enable);

		public static Toggleable Create(Func<IDisposable> enable, string displayName) => new CustomToggleable(enable, displayName);
	}
}
