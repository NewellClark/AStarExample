using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	class SimpleConnectDisconnectNodeTool : NodeTool
	{
		public SimpleConnectDisconnectNodeTool(GraphEditor editor)
			: this(editor, MouseButtons.Left, MouseButtons.Right) { }
		public SimpleConnectDisconnectNodeTool(GraphEditor editor, MouseButtons connectButton, MouseButtons disconnectButton) 
			: base(editor)
		{
			this.ConnectButton = connectButton;
			this.DisconnectButton = disconnectButton;
		}

		protected override IDisposable EnableTool()
		{
			void onNext((MouseEventArgs e, ObservableNode<NodeData> node) t)
			{
				if (_previousNode != null && t.node != null)
				{
					if (t.e.Button.HasFlag(ConnectButton))
						_previousNode.Neighbors.Add(t.node);
					else if (t.e.Button.HasFlag(DisconnectButton))
						_previousNode.Neighbors.Remove(t.node);
					else
						Debug.Fail($"Should filter out all mouse clicks that don't include {nameof(ConnectButton)} or " +
							$"{nameof(DisconnectButton)}.");
				}

				_previousNode = t.node;
			}

			var clicks = Editor.Control.WhenMouseClicked()
				.Select(e => (e: e, node: Editor.GetNodeAtPosition(e.Location.ToVector2())));

			return clicks.Subscribe(onNext);
		}

		public MouseButtons ConnectButton { get; }

		public MouseButtons DisconnectButton { get; }

		public override string DisplayName => "Connect/Disconnect single node";

		public override string Tooltip => $"{ConnectButton}-click to connect, {DisconnectButton}-click to disconnect.";

		private ObservableNode<NodeData> _previousNode;
	}
}
