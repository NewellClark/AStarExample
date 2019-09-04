using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	/// <summary>
	/// An implementation of <see cref="ISubject{T}"/> for exposing a property-changed event.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class PropertySubject<T> : ISubject<T>, IDisposable
	{
		private readonly Subject<T> _subject;

		public PropertySubject() : this(default(T)) { }
		public PropertySubject(T initial)
		{
			_subject = new Subject<T>();
			_Value = initial;
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="PropertySubject{T}"/>. When the value of this property
		/// is changed, an event is fired.
		/// </summary>
		public T Value
		{
			get => _Value;
			set
			{
				if (EqualityComparer<T>.Default.Equals(_Value, value))
					return;

				_Value = value;
				_subject.OnNext(_Value);
			}
		}
		private T _Value;

		/// <summary>
		/// Sets the value of the <see cref="Value"/> property without pushing an event.
		/// </summary>
		/// <param name="value"></param>
		public void SetWithoutRaisingEvent(T value) => _Value = value;

		public void OnNext(T value) => _subject.OnNext(value);

		public void OnError(Exception error) => _subject.OnError(error);

		public void OnCompleted() => _subject.OnCompleted();

		public IDisposable Subscribe(IObserver<T> observer) => _subject.Subscribe(observer);

		public void Dispose() => _subject.Dispose();
	}
}
