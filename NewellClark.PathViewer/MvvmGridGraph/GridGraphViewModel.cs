using NewellClark.DataStructures.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	/// <summary>
	/// Responsible for GUI-agnostic members concerning how to display a <see cref="GridGraph"/>.
	/// </summary>
	public class GridGraphViewModel
	{
		public GridGraph Graph { get; }
		public GridGraphPathFinder PathFinder { get; }

		private static readonly Size smallestAllowedView = new Size(3, 3);

		/// <summary>
		/// Creates a new <see cref="GridGraphViewModel"/> for the specified <see cref="GridGraph"/>.
		/// </summary>
		/// <param name="graph">The graph to view.</param>
		public GridGraphViewModel(GridGraph graph, GridGraphPathFinder pathFinder)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (pathFinder is null) throw new ArgumentNullException(nameof(pathFinder));

			this.Graph = graph;
			this.PathFinder = pathFinder;
			_CellSizeChanged = new PropertySubject<IntVector2>(IntVector2.One);
			_ViewBoundsChanged = new PropertySubject<Rectangle>(new Rectangle(0, 0, 2, 2));
			_NodeChangedLocal = new Subject<GridNodeViewModel>();
			NodeChanged = Graph.NodeChanged
				.Merge(PathFinder.NodeChanged)
				.Select(node => new GridNodeViewModel(this, node))
				.Merge(_NodeChangedLocal);

			HighlightColor = new IndexedProperty<Color?>(
				this, new FlyweightCollection<GridNodeViewModel, Color?>(null));

			InitializeEventHandlers();
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			void onViewBoundsChanged(Rectangle bounds) => PathFinder.Bounds = ClientRectToCellRect(bounds);

			return new CompositeDisposable
			{
				_CellSizeChanged,
				_ViewBoundsChanged,
				_NodeChangedLocal,
				_ViewBoundsChanged.Subscribe(onViewBoundsChanged)
			};
		}

		public GridNodeViewModel this[IntVector2 cellIndex]
		{
			get => new GridNodeViewModel(this, Graph[cellIndex]);
		}

		/// <summary>
		/// Gets the size that a grid cell is rendered at.
		/// </summary>
		public IntVector2 CellSize
		{
			get => _CellSizeChanged.Value;
			set
			{
				if (CellSize.X < 0 || CellSize.Y < 0)
					throw new ArgumentOutOfRangeException(
						nameof(value),
						$"Both components of {nameof(CellSize)} must be positive and non-zero.");

				_CellSizeChanged.Value = value;
			}
		}
		public IObservable<IntVector2> CellSizeChanged => _CellSizeChanged.AsObservable();
		private readonly PropertySubject<IntVector2> _CellSizeChanged;

		/// <summary>
		/// Gets the bounds of the viewing area, in client coordinates.
		/// </summary>
		public Rectangle ViewBounds
		{
			get => _ViewBoundsChanged.Value;
			set => _ViewBoundsChanged.Value = value;
		}
		public IObservable<Rectangle> ViewBoundsChanged => _ViewBoundsChanged.AsObservable();
		private readonly PropertySubject<Rectangle> _ViewBoundsChanged;

		public IObservable<GridNodeViewModel> NodeChanged { get; }
		private readonly Subject<GridNodeViewModel> _NodeChangedLocal;

		/// <summary>
		/// Converts the specified point in client-coordinates to the corrasponding 
		/// cell index.
		/// </summary>
		/// <param name="clientPoint"></param>
		/// <returns></returns>
		public IntVector2 ClientPointToCell(IntVector2 clientPoint)
		{
			return clientPoint.DivideRoundDown(CellSize);
		}

		/// <summary>
		/// Gets the client coordinates of the upper-left corner of the grid cell
		/// with the specified index.
		/// </summary>
		/// <param name="cellIndex"></param>
		/// <returns></returns>
		public IntVector2 CellToClientPoint(IntVector2 cellIndex)
		{
			return cellIndex.Multiply(CellSize);
		}

		/// <summary>
		/// Converts a <see cref="Rectangle"/> from client coordinates to cell coordinates.
		/// </summary>
		/// <param name="clientBounds"></param>
		/// <returns></returns>
		public Rectangle ClientRectToCellRect(Rectangle clientBounds)
		{
			IntVector2 topLeft = ClientPointToCell(clientBounds.Location.ToIntVector2());
			IntVector2 bottomRight = ClientPointToCell(clientBounds.BottomRight());

			return Geometry.RectangleFromCorners(topLeft, bottomRight);
		}

		/// <summary>
		/// Gets all the <see cref="GridNodeViewModel"/>s that are visible within the <see cref="ViewBounds"/>.
		/// </summary>
		public IEnumerable<GridNodeViewModel> VisibleNodes => GetCellsInRect(ViewBounds);

		/// <summary>
		/// Gets all the cells that are visible within the specified bounds.
		/// </summary>
		/// <param name="clientBounds">Rectangle in client coordinates.</param>
		/// <returns></returns>
		public IEnumerable<GridNodeViewModel> GetCellsInRect(Rectangle clientBounds)
		{
			return ClientRectToCellRect(clientBounds).LatticePoints()
				.Select(p => new GridNodeViewModel(this, Graph[p]));
		}

		internal class IndexedProperty<T>
		{
			private readonly GridGraphViewModel _vm;
			private readonly FlyweightCollection<GridNodeViewModel, T> _lookup;

			public IndexedProperty(GridGraphViewModel vm, FlyweightCollection<GridNodeViewModel, T> lookup)
			{
				Debug.Assert(vm != null);
				Debug.Assert(lookup != null);

				_vm = vm;
				_lookup = lookup;
			}

			public T this[GridNodeViewModel nodeVM]
			{
				get => _lookup[nodeVM];
				set
				{
					if (EqualityComparer<T>.Default.Equals(_lookup[nodeVM], value))
						return;

					_lookup[nodeVM] = value;
					_vm._NodeChangedLocal.OnNext(nodeVM);
				}
			}
		}

		internal readonly IndexedProperty<Color?> HighlightColor;
	}
}
