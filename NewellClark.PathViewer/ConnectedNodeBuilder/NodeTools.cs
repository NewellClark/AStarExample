using NewellClark.DataStructures.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Linq;
using System.Numerics;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Drawing;

namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	/// <summary>
	/// Base class for a tool that can be used to edit nodes.
	/// </summary>
	abstract class NodeTool
	{
		protected NodeTool(GraphEditor editor)
		{
			if (editor is null) throw new ArgumentNullException(nameof(editor));

			this.Editor = editor;
		}

		public GraphEditor Editor { get; }

		public bool Enabled
		{
			get => _disposeToDisable != null;
			set
			{
				if (Enabled == value)
					return;

				if (value)
				{
					_disposeToDisable?.Dispose();
					_disposeToDisable = EnableTool();
				}
				else
				{
					_disposeToDisable?.Dispose();
					_disposeToDisable = null;
				}

				OnEnabledChanged(EventArgs.Empty);
			}
		}
		private IDisposable _disposeToDisable;

		public event EventHandler EnabledChanged;

		protected virtual void OnEnabledChanged(EventArgs e)
		{
			EnabledChanged?.Invoke(this, e);
		}

		/// <summary>
		/// Enables the current NodeTool and returns an <see cref="IDisposable"/>
		/// that can be used to disable the current <see cref="NodeTool"/>.
		/// </summary>
		/// <returns>
		/// An <see cref="IDisposable"/> object that will disable the current
		/// <see cref="NodeTool"/> when it is disposed.
		/// </returns>
		/// <remarks>
		/// This method will be called by the <see cref="Enabled"/> property setter. The setter 
		/// will ensure that <see cref="EnableTool()"/> will not be called if the tool is already
		/// enabled.
		/// </remarks>
		protected abstract IDisposable EnableTool();

		public abstract string DisplayName { get; }

		public abstract string Tooltip { get; }
	}

	/// <summary>
	/// Tool for adding and removing nodes via mouse clicks.
	/// </summary>
	class AddRemoveNodeTool : NodeTool
	{
		public AddRemoveNodeTool(GraphEditor editor)
			: this(editor, MouseButtons.Left, MouseButtons.Right) { }

		public AddRemoveNodeTool(GraphEditor editor, MouseButtons addButton, MouseButtons removeButton)
			: base(editor)
		{
			this.AddButton = addButton;
			this.RemoveButton = removeButton;
		}

		public MouseButtons AddButton { get; }

		public MouseButtons RemoveButton { get; }

		protected override IDisposable EnableTool()
		{
			(Vector2 position, ObservableNode<NodeData> node, MouseButtons button) selector(MouseEventArgs e)
			{
				Vector2 position = e.Location.ToVector2();
				var node = Editor.GetNodeAtPosition(position);

				return (position, node, e.Button);
			}

			void onAdd(ObservableNode<NodeData> node) => Editor.Nodes.Add(node);
			void onRemove(ObservableNode<NodeData> node) => Editor.Nodes.Remove(node);

			var clicks = Editor.Control.WhenMouseClicked()
				.Select(selector);

			var nodesToAdd = clicks
				.Where(t => t.button.HasFlag(AddButton) && t.node is null)
				.Select(t => new ObservableNode<NodeData>(new NodeData(t.position)));

			var nodesToRemove = clicks
				.Where(t => t.node != null && t.button.HasFlag(RemoveButton))
				.Select(t => t.node);

			var result = new CompositeDisposable
			{
				nodesToAdd.Subscribe(onAdd),
				nodesToRemove.Subscribe(onRemove),
				new NodeHighlighter(Editor, 20)
			};

			return result;
		}

		public override string DisplayName => "Add/remove node";

		public override string Tooltip
		{
			get
			{
				return $@"{AddButton}-click to add, {RemoveButton}-click to remove.";
			}
		}
	}

	/// <summary>
	/// Tool for connecting and disconnecting many nodes at once via mouse clicks.
	/// </summary>
	class ConnectDisconnectManyNodeTool : NodeTool
	{
		public ConnectDisconnectManyNodeTool(GraphEditor editor)
			: this(editor, MouseButtons.Left, MouseButtons.Right) { }
		public ConnectDisconnectManyNodeTool(
			GraphEditor editor, 
			MouseButtons connectButton, MouseButtons disconnectButton) 
			: base(editor)
		{
			this.ConnectButton = connectButton;
			this.DisconnectButton = disconnectButton;
		}

		protected override IDisposable EnableTool()
		{
			IEnumerable<ObservableNode<NodeData>> getNodesInArea(Vector2 position, Vector2 size)
			{
				var bounds = Geometry.CenteredRectangle(position, size);

				return Editor.Nodes
					.Where(node => bounds.IntersectsWith(node.Value.Bounds))
					.OrderBy(node => (node.Value.Position - position).Length());
			}

			void connectNodes(IEnumerable<ObservableNode<NodeData>> nodes)
			{
				using (var e = nodes.GetEnumerator())
				{
					if (!e.MoveNext())
						return;

					var first = e.Current;

					while (e.MoveNext())
						e.Current.Neighbors.Add(first);
				}
			}

			void disconnectNodes(IEnumerable<ObservableNode<NodeData>> nodes)
			{
				foreach (var node in nodes)
					node.Neighbors.Clear();
			}

			var clicks = Editor.Control.WhenMouseClicked();
			var connectClicks = clicks
				.Where(e => e.Button.HasFlag(ConnectButton))
				.Select(e => getNodesInArea(e.Location.ToVector2(), this.Size));
			var disconnectClicks = clicks
				.Where(e => e.Button.HasFlag(DisconnectButton))
				.Select(e => getNodesInArea(e.Location.ToVector2(), this.Size));

			var connectSub = connectClicks.Subscribe(connectNodes);
			var disconnectSub = disconnectClicks.Subscribe(disconnectNodes);

			return new CompositeDisposable { connectSub, disconnectSub };
		}

		public override string DisplayName => "Connect/Disconnect nodes in area";

		public override string Tooltip
		{
			get
			{
				return $@"{ConnectButton}-click to connect nearby nodes, {DisconnectButton}-click to disconnect nearby nodes.";
			}
		}

		public MouseButtons ConnectButton { get; }

		public MouseButtons DisconnectButton { get; }

		private Vector2 Size { get; } = 200 * Vector2.One;
	}

	class SetStartGoalNode : NodeTool
	{
		public SetStartGoalNode(GraphEditor editor)
			: this(editor, MouseButtons.Left, MouseButtons.Right) { }
		public SetStartGoalNode(GraphEditor editor, MouseButtons setStartButton, MouseButtons setGoalButton)
			: base(editor)
		{
			this.SetStartButton = setStartButton;
			this.SetGoalButton = setGoalButton;
		}

		public MouseButtons SetStartButton { get; }

		public MouseButtons SetGoalButton { get; }

		protected override IDisposable EnableTool()
		{
			var startClicks = Editor.Control.WhenMouseClicked()
				.Where(e => e.Button.HasFlag(SetStartButton))
				.Select(e => Editor.GetNodeAtPosition(e.Location.ToVector2()))
				.Where(node => node != null);

			var goalClicks = Editor.Control.WhenMouseClicked()
				.Where(e => e.Button.HasFlag(SetGoalButton))
				.Select(e => Editor.GetNodeAtPosition(e.Location.ToVector2()))
				.Where(node => node != null);

			return new CompositeDisposable
			{
				startClicks.Subscribe(node => Editor.StartNode = node),
				goalClicks.Subscribe(node => Editor.GoalNode = node)
			};
		}

		public override string DisplayName => $"Set start/goal nodes";

		public override string Tooltip
		{
			get
			{
				return $@"{SetStartButton}-click to set start node, {SetGoalButton}-click to set goal node.";
			}
		}
	}

	class ConnectDisconnectSingleNodeTool : NodeTool
	{
		public ConnectDisconnectSingleNodeTool(GraphEditor editor)
			: this(editor, MouseButtons.Left, MouseButtons.Right) { }
		public ConnectDisconnectSingleNodeTool(GraphEditor editor, MouseButtons connectButton, MouseButtons disconnectButton)
			: base(editor)
		{
			this.ConnectButton = connectButton;
			this.DisconnectButton = disconnectButton;
		}

		public MouseButtons ConnectButton { get; }

		public MouseButtons DisconnectButton { get; }

		public override string DisplayName => "Connect/Disconnect nodes";

		public override string Tooltip => $"{ConnectButton}-click and drag to connect nodes; " +
			$"{DisconnectButton}-click and drag to disconnect nodes.";

		protected override IDisposable EnableTool()
		{
			MouseButtons bothButtons = ConnectButton | DisconnectButton;

			bool isConnectOrDisconnectButton(MouseButtons button)
			{
				return (button & ~bothButtons) != MouseButtons.None;
			}

			void onBeginDrag((MouseEventArgs e, ObservableNode<NodeData> node) tuple)
			{
				Debug.Assert(!IsDragInProgress);

				_dragLineDrawer = new DragLineDrawer(Editor, tuple.node, tuple.e.Button);
			}

			void onEndDrag((MouseEventArgs e, ObservableNode<NodeData> node) tuple)
			{
				Debug.Assert(_dragLineDrawer != null);
				Debug.Assert(_dragLineDrawer.StartNode != null);
				Debug.Assert((tuple.e.Button & _dragLineDrawer.DragButton) != MouseButtons.None);

				var startNode = _dragLineDrawer.StartNode;
				var endNode = tuple.node;
				_dragLineDrawer.Dispose();
				_dragLineDrawer = null;

				if (endNode is null)
					return;

				if (tuple.e.Button.HasFlag(ConnectButton))
					startNode.Neighbors.Add(endNode);
				else if (tuple.e.Button.HasFlag(DisconnectButton))
					startNode.Neighbors.Remove(endNode);
			}

			var beginDrag = Editor.Control.WhenMouseDown()
				.Where(e => isConnectOrDisconnectButton(e.Button))
				.Where(_ => !IsDragInProgress)
				.Select(e => (e: e, node: Editor.GetNodeAtPosition(e.Location.ToVector2())))
				.Where(t => t.node != null);

			var endDrag = Editor.Control.WhenMouseUp()
				.Where(e => isConnectOrDisconnectButton(e.Button))
				.Where(_ => IsDragInProgress)
				.Where(e => _dragLineDrawer.DragButton.HasFlag(e.Button))
				.Select(e => (e: e, node: Editor.GetNodeAtPosition(e.Location.ToVector2())));

			var result = new CompositeDisposable
			{
				beginDrag.Subscribe(onBeginDrag),
				endDrag.Subscribe(onEndDrag),
				Disposable.Create(() => _dragLineDrawer?.Dispose())
			};

			return result;
		}

		private DragLineDrawer _dragLineDrawer;

		private bool IsDragInProgress => _dragLineDrawer != null;

		private class DragLineDrawer : IDisposable
		{
			private readonly GraphEditor _editor;
			public ObservableNode<NodeData> StartNode { get; }
			private readonly CompositeDisposable _disposables;
			private readonly Pen _dragLinePen;

			public DragLineDrawer(GraphEditor editor, ObservableNode<NodeData> startNode, MouseButtons dragButton)
			{
				Debug.Assert(editor != null);
				Debug.Assert(startNode != null);

				_editor = editor;
				this.StartNode = startNode;
				_dragLinePen = new Pen(Brushes.HotPink, 10);
				this.DragButton = dragButton;

				_disposables = new CompositeDisposable
				{
					_editor.Control.WhenPaint().Subscribe(HandlePaintEvent),
					_editor.Control.WhenMouseMoved().Subscribe(HandleMouseMove),
					_dragLinePen
				};
			}

			/// <summary>
			/// The mouse button that was used to initiate the drag.
			/// </summary>
			public MouseButtons DragButton { get; }

			public void Dispose() => _disposables.Dispose();

			private void HandlePaintEvent(PaintEventArgs e)
			{
				Debug.Assert(e != null);

				PointF cursor = _editor.Control.PointToClient(Cursor.Position);
				PointF start = StartNode.Value.Position.ToPointF();

				e.Graphics.DrawLine(_dragLinePen, start, cursor);
			}

			private void HandleMouseMove(MouseEventArgs e)
			{
				Debug.Assert(e != null);

				var bounds = RectangleF.Union(
					StartNode.Value.Bounds,
					Geometry.CenteredRectangle(
						e.Location.ToVector2(), 
						StartNode.Value.Size));

				_editor.Control.Invalidate(new Region(bounds));
			}
		}
	}
}
