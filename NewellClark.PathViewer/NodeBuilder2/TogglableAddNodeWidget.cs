using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewellClark.DataStructures.Graphs;

namespace NewellClark.PathViewer.NodeBuilder2
{
	/// <summary>
	/// Togglable widget for adding a node to a <see cref="GraphModel{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type of node data.</typeparam>
	class TogglableAddNodeWidget<T> : TogglableMouseTool
		where T: INodeData
	{
		private readonly GraphModel<T> _graph;
		private readonly Func<MouseEventArgs, T> _nodeFactory;
		private readonly Action<Exception> _onError;

		public TogglableAddNodeWidget(
			GraphModel<T> graph, IMouseProvider mouseProvider,
			Func<MouseEventArgs, T> nodeFactory,
			Action<Exception> onError,
			MouseButtons addButton)
			: base(mouseProvider, addButton)
		{
			if (graph is null) throw new ArgumentNullException(nameof(graph));
			if (nodeFactory is null) throw new ArgumentNullException(nameof(nodeFactory));
			if (onError is null) throw new ArgumentNullException(nameof(onError));

			_graph = graph;
			_nodeFactory = nodeFactory;
			_onError = onError;
		}

		protected override IDisposable Enable()
		{
			var addClick = MouseProvider.WhenMouseClicked
				.Where(e => e.Button.HasFlag(MouseButton))
				.Where(e => _graph.GetNodeAtPosition(e.Location.ToVector2()) == null);

			void onNext(MouseEventArgs e)
			{
				var node = new ObservableNode<T>(_nodeFactory(e));
				_graph.Nodes.Add(node);
			}

			return addClick.Subscribe(onNext, _onError);
		}

		public override string Name => "Add";

		public override string Tooltip => $"{MouseButton}-click to add new node.";
	}
}
