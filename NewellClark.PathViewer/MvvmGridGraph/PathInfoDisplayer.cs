using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Disposables;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public partial class PathInfoDisplayer : UserControl
	{
		private readonly CompositeDisposable _disposables;

		public PathInfoDisplayer()
		{
			InitializeComponent();

			_EuclidianDistance = new PropertyChangedEvent<float?>(this, null);
			_ManhattanDistance = new PropertyChangedEvent<int?>(this, null);
			_NodeCount = new PropertyChangedEvent<int?>(this, null);
			_PathCost = new PropertyChangedEvent<float?>(this, null);
			_NullText = new PropertyChangedEvent<string>(this, defaultNullText);

			_disposables = InitializeEventHandlers();
			_NullText.OnValueChanged(EventArgs.Empty);
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			void onEuclidianDistanceChanged(object sender, EventArgs e)
			{
				euclidianDistanceLabel.Text = GetPropertyDisplayText(_EuclidianDistance.Value);
			}
			void onManhattanDistanceChanged(object sender, EventArgs e)
			{
				manhattanDistanceLabel.Text = GetPropertyDisplayText(_ManhattanDistance.Value);
			}
			void onNodeCountChanged(object sender, EventArgs e)
			{
				nodeCountLabel.Text = GetPropertyDisplayText(_NodeCount.Value);
			}
			void onPathCostChanged(object sender, EventArgs e)
			{
				pathCostLabel.Text = GetPropertyDisplayText(_PathCost.Value);
			}
			void onNullTextChanged(object sender, EventArgs _)
			{
				var e = EventArgs.Empty;
				_EuclidianDistance.OnValueChanged(e);
				_ManhattanDistance.OnValueChanged(e);
				_NodeCount.OnValueChanged(e);
				_PathCost.OnValueChanged(e);
			}

			_EuclidianDistance.ValueChanged += onEuclidianDistanceChanged;
			_ManhattanDistance.ValueChanged += onManhattanDistanceChanged;
			_NodeCount.ValueChanged += onNodeCountChanged;
			_PathCost.ValueChanged += onPathCostChanged;
			_NullText.ValueChanged += onNullTextChanged;

			void cleanup()
			{
				_EuclidianDistance.ValueChanged -= onEuclidianDistanceChanged;
				_ManhattanDistance.ValueChanged -= onManhattanDistanceChanged;
				_NodeCount.ValueChanged -= onNodeCountChanged;
				_PathCost.ValueChanged -= onPathCostChanged;
				_NullText.ValueChanged -= onNullTextChanged;
			}

			return new CompositeDisposable
			{
				Disposable.Create(cleanup)
			};
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get => groupBox.Text;
			set => groupBox.Text = value;
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler TextChanged
		{
			add => base.TextChanged += value;
			remove => base.TextChanged -= value;
		}

		public float? EuclidianDistance
		{
			get => _EuclidianDistance.Value;
			set => _EuclidianDistance.Value = value;
		}
		private PropertyChangedEvent<float?> _EuclidianDistance;

		public int? ManhattanDistance
		{
			get => _ManhattanDistance.Value;
			set => _ManhattanDistance.Value = value;
		}
		private PropertyChangedEvent<int?> _ManhattanDistance;

		public int? NodeCount
		{
			get => _NodeCount.Value;
			set => _NodeCount.Value = value;
		}
		private PropertyChangedEvent<int?> _NodeCount;

		public float? PathCost
		{
			get => _PathCost.Value;
			set => _PathCost.Value = value;
		}
		private PropertyChangedEvent<float?> _PathCost;

		public string NullText
		{
			get => _NullText.Value;
			set => _NullText.Value = value;
		}
		private PropertyChangedEvent<string> _NullText;

		private string GetPropertyDisplayText<T>(T value)
		{
			if (value == null)
				return NullText;

			return value.ToString();
		}

		private static readonly string defaultNullText = "---";
	}
}
