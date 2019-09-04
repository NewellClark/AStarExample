using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	class BitmapGraph : IDisposable
	{
		private readonly CompositeDisposable _disposables;

		public BitmapGraph(IntVector2 imageSize)
		{
			_bitmap = new Bitmap(imageSize.X, imageSize.Y);
			foreach (var point in _bitmap.Bounds().LatticePoints())
				_bitmap.SetPixel(point, UnblockedColor);

			_subjectWhenNodeChanged = new Subject<PixelNode>();
			_WhenNodeChanged = _subjectWhenNodeChanged.AsObservable().Synchronize();
			_pathNodes = new HashSet<IntVector2>();

			_disposables = new CompositeDisposable
			{
				this._bitmap,
				_subjectWhenNodeChanged
			};
		}

		public Image Image => _bitmap;
		private readonly Bitmap _bitmap;

		public Color BlockedColor { get; } = Color.FromArgb(255, 0, 0, 0);

		public Color UnblockedColor => Color.HotPink;

		public Color StartColor => Color.Yellow;

		public Color GoalColor => Color.LimeGreen;

		public Color PathColor => Color.DarkOrange;

		public Color GetPixel(IntVector2 position)
		{
			ThrowIfOutOfBounds(position);
			ThrowIfDisposed();
			Debug.Assert(_bitmap != null);

			return _bitmap.GetPixel(position);
		}

		public void SetPixel(IntVector2 position, Color color)
		{
			ThrowIfOutOfBounds(position);
			ThrowIfDisposed();
			Debug.Assert(_bitmap != null);

			Color previous = GetPixel(position);
			if (previous == color)
				return;

			_bitmap.SetPixel(position, color);
			_subjectWhenNodeChanged.OnNext(new PixelNode(this, position));
		}

		/// <summary>
		/// Gets the <see cref="PixelNode"/> at the specified position.
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public PixelNode this[IntVector2 position]
		{
			get
			{
				ThrowIfOutOfBounds(position);
				ThrowIfDisposed();

				return new PixelNode(this, position);
			}
		}

		public IntVector2 Start
		{
			get => _Start;
			set
			{
				ThrowIfDisposed();
				ThrowIfOutOfBounds(value);

				if (_Start == value)
					return;

				var previous = _Start;
				_Start = value;
				//SetPixel(previous, UnblockedColor);
				//SetPixel(_Start, StartColor);
				_subjectWhenNodeChanged.OnNext(this[previous]);
				_subjectWhenNodeChanged.OnNext(this[_Start]);
			}
		}
		private IntVector2 _Start;

		public IntVector2 Goal
		{
			get => _Goal;
			set
			{
				ThrowIfDisposed();
				ThrowIfOutOfBounds(value);

				if (_Goal == value)
					return;

				var previous = _Goal;
				_Goal = value;
				_subjectWhenNodeChanged.OnNext(this[previous]);
				_subjectWhenNodeChanged.OnNext(this[_Goal]);
				//SetPixel(previous, UnblockedColor);
				//SetPixel(_Goal, GoalColor);
			}
		}
		private IntVector2 _Goal;

		public Path<PixelNode, float> Path
		{
			get => _Path;
			set
			{
				if (_Path == value)
					return;

				//	Update the nodes that were previously in the path.
				if (_Path != null)
					foreach (var node in _Path)
						_subjectWhenNodeChanged.OnNext(node);
			
				_Path = value;
				_pathNodes.Clear();

				if (_Path is null)
					return;

				//	Update the nodes that are in the new path.
				foreach (var node in _Path)
					_pathNodes.Add(node.Position);

				foreach (var point in _pathNodes)
					_subjectWhenNodeChanged.OnNext(this[point]);
			}
		}
		private Path<PixelNode, float> _Path;
		private readonly HashSet<IntVector2> _pathNodes;

		public bool IsPath(IntVector2 position) => _pathNodes.Contains(position);

		public bool IsPath(PixelNode node) => _pathNodes.Contains(node.Position);

		public bool DiagonalsAllowed
		{
			get => _DiagonalsAllowed;
			set
			{
				if (_DiagonalsAllowed == value)
					return;

				_DiagonalsAllowed = value;

				// Force path to update.
				_subjectWhenNodeChanged.OnNext(this[Start]);
				DiagonalsAllowedChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		private bool _DiagonalsAllowed;

		public event EventHandler DiagonalsAllowedChanged;

		public bool UseManhattanDistance
		{
			get => _UseManhattanDistance;
			set
			{
				if (_UseManhattanDistance == value)
					return;

				_UseManhattanDistance = value;
				_subjectWhenNodeChanged.OnNext(this[Start]);
				UseManhattanDistanceChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		private bool _UseManhattanDistance;

		public event EventHandler UseManhattanDistanceChanged;

		public bool UseInvertedComparison
		{
			get => _UseInvertedComparison;
			set
			{
				if (_UseInvertedComparison == value)
					return;

				_UseInvertedComparison = value;
				_subjectWhenNodeChanged.OnNext(this[Start]);
				UseInvertedComparisonChanged?.Invoke(this, EventArgs.Empty);
			}
		}
		private bool _UseInvertedComparison;

		public event EventHandler UseInvertedComparisonChanged;

		public Rectangle Bounds
		{
			get
			{
				ThrowIfDisposed();
				Debug.Assert(_bitmap != null);
				
				return _bitmap.Bounds();
			}
		}

		public void Dispose()
		{
			Debug.Assert(_disposables != null);
			_disposables.Dispose();
		}

		public IObservable<PixelNode> WhenNodeChanged
		{
			get
			{
				ThrowIfDisposed();
				Debug.Assert(_WhenNodeChanged != null);

				return _WhenNodeChanged;
			}
		}
		private readonly IObservable<PixelNode> _WhenNodeChanged;
		private readonly Subject<PixelNode> _subjectWhenNodeChanged;

		private void ThrowIfOutOfBounds(IntVector2 position)
		{
			if (!_bitmap.Bounds().Contains(position))
				throw new ArgumentOutOfRangeException(
					nameof(position), $"{nameof(position)} is outside of the underlying {nameof(System.Drawing.Bitmap)}.");
		}

		private void ThrowIfDisposed()
		{
			if (_disposables.IsDisposed)
				throw new ObjectDisposedException(
					nameof(BitmapGraph),
					$"The {nameof(BitmapGraph)} has been disposed.");
		}
	}
}
