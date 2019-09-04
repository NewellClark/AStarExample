using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Numerics;
using System.Diagnostics;
using NewellClark.DataStructures.Graphs;
using NewellClark.DataStructures.Collections;
using System.Reactive.Disposables;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public class GridGraphViewPanel : Panel
	{
		//private readonly EditNodePassabilityTool _editPassabilityTool;
		private readonly GridGraphDrawer _drawer;
		private readonly CompositeDisposable _disposables;

		public GridGraphViewModel ViewModel { get; }

		public GridGraphViewPanel()
		{
			var graph = new GridGraph();
			ViewModel = new GridGraphViewModel(graph, new GridGraphPathFinder(graph)) { CellSize = 50 * IntVector2.One };

			//_editPassabilityTool = new EditNodePassabilityTool(ViewModel, this.ToMouseProvider());
			_drawer = new GridGraphDrawer(ViewModel, this);
			_disposables = InitializeEventHandlers();

			DoubleBuffered = true;
			Anchor = AnchorStyles.Left | 
				AnchorStyles.Top | 
				AnchorStyles.Right | 
				AnchorStyles.Bottom;
		}

		private CompositeDisposable InitializeEventHandlers()
		{
			void onMyClientSizeChanged(object sender, EventArgs e) => ViewModel.ViewBounds = this.ClientRectangle;
			void onVMViewBoundsChanged(Rectangle bounds) => this.ClientSize = bounds.Size;

			this.ClientSizeChanged += onMyClientSizeChanged;

			return new CompositeDisposable
			{
				Disposable.Create(() => this.ClientSizeChanged -= onMyClientSizeChanged),
				ViewModel.ViewBoundsChanged.Subscribe(onVMViewBoundsChanged),
				_drawer,
				//_editPassabilityTool
			};
		}
	}
}
