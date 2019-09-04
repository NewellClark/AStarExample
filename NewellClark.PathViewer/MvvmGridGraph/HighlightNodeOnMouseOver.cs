using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	class HighlightNodeOnMouseOver : IDisposable
	{
		private readonly GridGraphViewModel _vm;
		private readonly IMouseProvider _mouseProvider;
		private readonly CompositeDisposable _disposables;

		public HighlightNodeOnMouseOver(GridGraphViewModel vm, IMouseProvider mouseProvider, Color highlightColor)
			: this(vm, mouseProvider, highlightColor, IntVector2.One) { }
		public HighlightNodeOnMouseOver(GridGraphViewModel vm, IMouseProvider mouseProvider,
			Color highlightColor, IntVector2 highlightArea)
		{
			if (vm is null) throw new ArgumentNullException(nameof(vm));
			if (mouseProvider is null) throw new ArgumentNullException(nameof(mouseProvider));

			_vm = vm;
			_mouseProvider = mouseProvider;
			this.HighlightColor = highlightColor;
			this.HighlightArea = highlightArea;
			_disposables = InitializeEventHandlers();
		}

		public Color HighlightColor { get; set; }

		public IntVector2 HighlightArea { get; set; }

		private CompositeDisposable InitializeEventHandlers()
		{
			IEnumerable<GridNodeViewModel> selectNodeVMs(MouseEventArgs e)
			{
				return _vm.GetCellsInRect(Geometry.CenteredRectangle(e.Location.ToIntVector2(), HighlightArea));
			}

			var mouseEvents = _mouseProvider.WhenMouseMove
				.Select(selectNodeVMs);

			var previous = Enumerable.Empty<GridNodeViewModel>();
			void onNext(IEnumerable<GridNodeViewModel> nodeVMs)
			{
				foreach (var node in previous)
				{
					var temp = node;
					temp.HighlightColor = null;
				}

				previous = nodeVMs;
				foreach (var node in nodeVMs)
				{
					var temp = node;
					temp.HighlightColor = HighlightColor;
				}
			}

			return new CompositeDisposable { mouseEvents.Subscribe(onNext) };
		}

		public void Dispose() => _disposables.Dispose();
	}
}
