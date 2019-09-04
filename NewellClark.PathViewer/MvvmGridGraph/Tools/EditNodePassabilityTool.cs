using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	/// <summary>
	/// A tool for setting whether nodes are passable or blocked by using the mouse.
	/// </summary>
	class NodePassabilityTool : IDisposable
	{
		private readonly GridGraphViewModel _vm;
		private readonly IMouseProvider _mouseProvider;
		private readonly CompositeDisposable _disposables;

		public NodePassabilityTool(GridGraphViewModel vm, IMouseProvider mouseProvider)
		{
			if (vm is null) throw new ArgumentNullException(nameof(vm));
			if (mouseProvider is null) throw new ArgumentNullException(nameof(mouseProvider));

			_vm = vm;
			_mouseProvider = mouseProvider;
			_disposables = InitializeEventHandlers();
		}

		public MouseButtons BlockButton { get; set; } = MouseButtons.Left;

		public MouseButtons UnblockButton { get; set; } = MouseButtons.Right;

		private CompositeDisposable InitializeEventHandlers()
		{
			Debug.Assert(_vm != null);
			Debug.Assert(_mouseProvider != null);

			var bothButtons = BlockButton | UnblockButton;

			GridNode selectNode(MouseEventArgs e)
			{
				var cell = _vm.ClientPointToCell(e.Location.ToIntVector2());
				return _vm[cell].Node;
			}

			//	Don't take action if both BlockButton and UnblockButton are pressed.
			bool? selectPassableState(MouseButtons buttons)
			{
				var filtered = buttons & bothButtons;
				if (filtered == BlockButton)
					return false;
				if (filtered == UnblockButton)
					return true;

				return null;
			}

			var nodesUnderPressedMouse = _mouseProvider.WhenMouseMove
				.Merge(_mouseProvider.WhenMouseDown)
				.Where(e => (e.Button & bothButtons) != MouseButtons.None)
				.Select(e => (node: selectNode(e), passable: selectPassableState(e.Button)))
				.Where(t => t.passable != null)
				.Select(t => (node: t.node, passable: t.passable.Value));

			void onNext((GridNode node, bool passable) t)
			{
				t.node.IsPassable = t.passable;
			}

			return new CompositeDisposable
			{
				nodesUnderPressedMouse.Subscribe(onNext)
			};
		}

		public void Dispose() => _disposables.Dispose();
	}
}
