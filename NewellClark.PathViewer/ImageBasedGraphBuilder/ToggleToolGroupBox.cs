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
using System.Diagnostics;
using System.Reactive.Disposables;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	public partial class ToggleToolGroupBox : UserControl
	{
		private readonly DisposableList<Toggleable> _tools;
		private readonly CompositeDisposable _disposables;

		public ToggleToolGroupBox()
		{
			InitializeComponent();

			_tools = new DisposableList<Toggleable>();
			_disposables = new CompositeDisposable { _tools };

			InitializeEventHandlers();
		}

		private void InitializeEventHandlers()
		{
			void onDisposed(object sender, EventArgs e) => _disposables.Dispose();

			this.Disposed += onDisposed;
		}

		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		public override string Text
		{
			get => groupBox.Text;
			set => groupBox.Text = value;
		}

		internal void AddTool(Toggleable tool)
		{
			_tools.Add(tool);

			CreateToolControl(tool);
		}
		
		private RadioButton CreateToolControl(Toggleable tool)
		{
			Debug.Assert(tool != null);

			var button = new RadioButton();
			button.Text = tool.DisplayName;
			button.AutoSize = false;
			flowLayoutPanel.Controls.Add(button);

			void updateTool(object sender, EventArgs e) => tool.Enabled = button.Checked;
			void updateButton(object sender, EventArgs e) => button.Checked = tool.Enabled;
			void unregisterEvents()
			{
				button.CheckedChanged -= updateTool;
				tool.EnabledChanged -= updateButton;
			}

			button.Checked = tool.Enabled;
			button.CheckedChanged += updateTool;
			tool.EnabledChanged += updateButton;

			_disposables.Add(Disposable.Create(unregisterEvents));

			return button;
		}
	}
}
