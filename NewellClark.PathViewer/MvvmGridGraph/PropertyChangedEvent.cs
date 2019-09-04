using System;
using System.Collections.Generic;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	internal struct PropertyChangedEvent<T>
	{
		private readonly object _sender;

		public PropertyChangedEvent(object sender, T initial)
		{
			_sender = sender;
			_Value = initial;
			ValueChanged = null;
		}

		public T Value
		{
			get => _Value;
			set
			{
				if (EqualityComparer<T>.Default.Equals(_Value, value))
					return;

				_Value = value;
				OnValueChanged(EventArgs.Empty);
			}
		}
		private T _Value;

		public event EventHandler ValueChanged;

		public void OnValueChanged(EventArgs e)
		{
			ValueChanged?.Invoke(_sender, e);
		}
	}
}
