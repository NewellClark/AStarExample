using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.DataStructures.Collections
{
	public class ReadOnlyObservableSet<T> : IReadOnlyCollection<T>
	{
		private readonly ObservableSet<T> _inner;

		public ReadOnlyObservableSet(ObservableSet<T> inner)
		{
			if (inner is null) throw new ArgumentNullException(nameof(inner));

			_inner = inner;

			void handleSetChanged(object sender, SetChangedEventArgs<T> e) => 
				SetChanged?.Invoke(this, e);
			_inner.SetChanged += handleSetChanged;
		}

		public int Count => _inner.Count;

		public event EventHandler<SetChangedEventArgs<T>> SetChanged;

		public IEnumerator<T> GetEnumerator() => _inner.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _inner.GetEnumerator();
	}
}
