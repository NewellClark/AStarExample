using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NewellClark.DataStructures.Collections;
using NewellClark.DataStructures.Graphs;

namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	public partial class ConnectedNodeUIWidget : UserControl
	{
		private readonly GraphEditor _editor;

		public ConnectedNodeUIWidget()
		{
			InitializeComponent();

			_editor = new GraphEditor(nodePresenterPanel);
			SetupToolSelector(_editor);
		}

		private void SetupToolSelector(GraphEditor editor)
		{
			void createAndAddButton(NodeTool tool)
			{
				var button = new RadioButton
				{
					Checked = tool.Enabled,
					Text = tool.DisplayName
				};
				void handleToolEnabledChanged(object sender, EventArgs e)
				{
					button.Checked = tool.Enabled;
				}
				void handleToolRemoved(object sender, SetChangedEventArgs<NodeTool> e)
				{
					if (!e.Removed.Contains(tool))
						return;

					nodeToolFlowLayout.Controls.Remove(button);
					button.Dispose();
				}
				void handleCheckedChanged(object sender, EventArgs e)
				{
					tool.Enabled = button.Checked;
				}
				void handleButtonDisposed(object sender, EventArgs e)
				{
					tool.EnabledChanged -= handleToolEnabledChanged;
					editor.Tools.SetChanged -= handleToolRemoved;
				}

				tool.EnabledChanged += handleToolEnabledChanged;
				editor.Tools.SetChanged += handleToolRemoved;
				button.CheckedChanged += handleCheckedChanged;
				button.Disposed += handleButtonDisposed;

				nodeToolFlowLayout.Controls.Add(button);
			}

			void handleToolAdded(object sender, SetChangedEventArgs<NodeTool> e)
			{
				foreach (var tool in e.Added)
					createAndAddButton(tool);
			}

			editor.Tools.SetChanged += handleToolAdded;
			foreach (var tool in editor.Tools)
				createAndAddButton(tool);
		}
	}
}
