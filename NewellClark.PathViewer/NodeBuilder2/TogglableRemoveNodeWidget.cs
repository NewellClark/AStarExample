using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using NewellClark.DataStructures.Graphs;
using System.Numerics;
using System.Windows.Forms;

namespace NewellClark.PathViewer.NodeBuilder2
{
	class TogglableRemoveNodeWidget<T> : TogglableMouseTool
		where T: INodeData
	{
		private readonly GraphModel<T> _graph;
		private readonly Action<Exception> _onError;

		public TogglableRemoveNodeWidget(
			GraphModel<T> graph, IMouseProvider mouseProvider, 
			Action<Exception> onError, MouseButtons removeButon)
			: base(mouseProvider, removeButon)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (onError is null) throw new ArgumentNullException(nameof(onError));

			_graph = graph;
			_onError = onError;
		}

		protected override IDisposable Enable()
		{
			var removeNode = MouseProvider.WhenMouseClicked
				.Where(e => e.Button.HasFlag(MouseButton))
				.Select(e => _graph.GetNodeAtPosition(e.Location.ToVector2()))
				.Where(node => node != null);

			void onNext(ObservableNode<T> node) => _graph.Nodes.Remove(node);

			return removeNode.Subscribe(onNext, _onError);
		}
	}
}
