using NewellClark.DataStructures.Collections;
using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	/// <summary>
	/// Represents a graph where every node is a cell in a 2D grid.
	/// </summary>
	public partial class GridGraph 
	{	
		public GridGraph()
		{
			IsPassable = new IndexedProperty<bool>(this, new FlyweightCollection<IntVector2, bool>(true));
			_NodeChanged = new Subject<GridNode>();
			NodeChanged = _NodeChanged.Synchronize();
		}

		/// <summary>
		/// Gets the <see cref="GridNode"/> at the specified position within the current <see cref="GridGraph"/>.
		/// </summary>
		/// <param name="position">The position of the node.</param>
		/// <returns>
		/// The <see cref="GridNode"/> at the specified position in the current <see cref="GridGraph"/>.
		/// </returns>
		public GridNode this[IntVector2 position]
		{
			get => new GridNode(this, position);
		}

		/// <summary>
		/// Observable stream that pushes an event whenever a property on a <see cref="GridNode"/> changes.
		/// </summary>
		public IObservable<GridNode> NodeChanged { get; }
		private readonly Subject<GridNode> _NodeChanged;

		internal IndexedProperty<bool> IsPassable { get; }

		internal IEnumerable<GridNode> GetNeighbors(GridNode node)
		{
			Debug.Assert(node.Graph == this);

			return node.Position.Neighbors()
				.Select(p => this[p]);
		}

		private bool SetField<TField, TEvent>(ref TField field, TField newValue, TEvent @event, Action<TEvent> onNext)
		{
			if (EqualityComparer<TField>.Default.Equals(field, newValue))
				return false;

			field = newValue;
			onNext(@event);

			return true;
		}

		private bool SetField<T>(ref T field, T newValue, Action<T> onNext)
		{
			if (EqualityComparer<T>.Default.Equals(field, newValue))
				return false;

			field = newValue;
			onNext(field);

			return true;
		}

		private bool SetField<T>(ref T field, T newValue, IObserver<T> observer)
		{
			return SetField(ref field, newValue, observer.OnNext);
		}
	}
}
