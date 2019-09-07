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
	[Flags]
	public enum GraphMovements
	{
		None = 0,
		Orthogonal = 0x1,
		Diagonal = 0x2,
		Knight = 0x4
	}
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
			_AllowedMovements = new PropertyChangedEvent<GraphMovements>(this, GraphMovements.Orthogonal);
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

		public GraphMovements AllowedMovements
		{
			get => _AllowedMovements.Value;
			set => _AllowedMovements.Value = value;
		}
		public event EventHandler AllowedMovementsChanged
		{
			add => _AllowedMovements.ValueChanged += value;
			remove => _AllowedMovements.ValueChanged -= value;
		}
		private PropertyChangedEvent<GraphMovements> _AllowedMovements;

		internal IndexedProperty<bool> IsPassable { get; }

		internal IEnumerable<GridNode> GetNeighbors(GridNode node)
		{
			Debug.Assert(node.Graph == this);

			var result = Enumerable.Empty<IntVector2>();

			if (AllowedMovements.HasFlag(GraphMovements.Orthogonal))
				result = result.Concat(OrthogonalNeighbors(node.Position));

			if (AllowedMovements.HasFlag(GraphMovements.Diagonal))
				result = result.Concat(DiagonalNeighbors(node.Position));

			if (AllowedMovements.HasFlag(GraphMovements.Knight))
				result = result.Concat(KnightsMoveNeighbors(node.Position));

			return result.Select(p => this[p]);
		}

		private IEnumerable<IntVector2> OrthogonalNeighbors(IntVector2 position) => position.Neighbors();

		private IEnumerable<IntVector2> DiagonalNeighbors(IntVector2 position)
		{
			IntVector2 v(int x, int y) => new IntVector2(x, y);

			yield return position + v(1, -1);
			yield return position + v(1, 1);
			yield return position + v(-1, 1);
			yield return position + v(-1, -1);
		}

		private IEnumerable<IntVector2> KnightsMoveNeighbors(IntVector2 position)
		{
			IntVector2 v(int x, int y) => new IntVector2(x, y);

			yield return position + v(2, -1);
			yield return position + v(2, 1);

			yield return position + v(1, 2);
			yield return position + v(-1, 2);

			yield return position + v(-2, 1);
			yield return position + v(-2, -1);

			yield return position + v(-1, -2);
			yield return position + v(1, -2);
		}

		private IEnumerable<GridNode> GetNeighborsDiagonalsAllowed(GridNode node)
		{
			IntVector2 v(int x, int y) => new IntVector2(x, y);

			IEnumerable<IntVector2> neighborPositions(IntVector2 p)
			{
				yield return p + v(1, -1);
				yield return p + v(1, 0);
				yield return p + v(1, 1);
				yield return p + v(0, 1);
				yield return p + v(-1, 1);
				yield return p + v(-1, 0);
				yield return p + v(-1, -1);
				yield return p + v(0, -1);
			}

			return neighborPositions(node.Position)
				.Select(p => this[p]);
		}

		private IEnumerable<GridNode> GetNeighborsDiagonalsProhibited(GridNode node)
		{
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
