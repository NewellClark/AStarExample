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
using System.Diagnostics;

namespace NewellClark.PathViewer.NodeBuilder2
{
	public partial class GraphPresenter : UserControl
	{
		private readonly GraphModel<NodeData> _graph;
		private readonly IMouseProvider _mouseProvider;
		private readonly Dictionary<TogglableMouseTool, Entry> _toolLookup;

		public GraphPresenter()
		{
			InitializeComponent();

			_graph = new GraphModel<NodeData>();
			_mouseProvider = this.ToMouseProvider();
			_toolLookup = new Dictionary<TogglableMouseTool, Entry>();

			AddMouseTool(
				new TogglableAddNodeWidget<NodeData>(
				_graph, _mouseProvider, e => new NodeData(e.Location.ToVector2()),
				e => MessageBox.Show(e.Message), MouseButtons.Left));

			AddMouseTool(
				new TogglableRemoveNodeWidget<NodeData>(
					_graph, _mouseProvider, e => MessageBox.Show(e.Message), MouseButtons.Left));

		}

		private struct Entry
		{
			public Entry(Button button)
			{
				this.Button = button;
				Subscriptions = new CompositeDisposable();
			}

			public Button Button { get; }

			public CompositeDisposable Subscriptions { get; }
		}

		private void AddMouseTool(TogglableMouseTool tool)
		{
			Debug.Assert(tool != null);
			Debug.Assert(!_toolLookup.ContainsKey(tool));

			var entry = new Entry(new Button());
			entry.Button.AutoSizeMode = AutoSizeMode.GrowOnly;
			entry.Button.AutoSize = true;
			entry.Button.Text = tool.Name;
			entry.Button.Click += handleButtonClicked;
			entry.Subscriptions.Add(Disposable.Create(cleanup));

			if (tool.Enabled)
				disableAll();

			toolLayoutPanel.Controls.Add(entry.Button);
			_toolLookup.Add(tool, entry);

			void disableAll()
			{
				foreach (var key in _toolLookup.Keys)
					key.Enabled = false;
			}
			void handleButtonClicked(object sender, EventArgs e)
			{
				disableAll();
				tool.Enabled = true;
			}
			void cleanup()
			{
				entry.Button.Click -= handleButtonClicked;
				tool.Enabled = false;
			}
		}

		private void RemoveMouseTool(TogglableMouseTool tool)
		{
			Debug.Assert(tool != null);
			Debug.Assert(_toolLookup.ContainsKey(tool));

			var entry = _toolLookup[tool];
			entry.Subscriptions.Dispose();
			toolLayoutPanel.Controls.Remove(entry.Button);
			_toolLookup.Remove(tool);
		}
	}
}
