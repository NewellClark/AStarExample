using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public partial class MutuallyExclusiveOptionSelectorBox : UserControl
	{
		public MutuallyExclusiveOptionSelectorBox()
		{
			InitializeComponent();
		}
		
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get => groupBox.Text;
			set => groupBox.Text = value;
		}

		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler TextChanged
		{
			add => base.TextChanged += value;
			remove => base.TextChanged -= value;
		}

		public void AddToggleable(Toggleable toggleable)
		{
			if (toggleable is null) throw new ArgumentNullException(nameof(toggleable));

			CreateToggleButton(toggleable);
		}

		/// <summary>
		/// Creates a <see cref="RadioButton"/> for the specified <see cref="Toggleable"/>, and adds
		/// it to the button container panel. 
		/// </summary>
		/// <param name="toggleable">The <see cref="Toggleable"/> to create a button for.</param>
		/// <returns>
		/// The newly-created button.
		/// </returns>
		private RadioButton CreateToggleButton(Toggleable toggleable)
		{
			Debug.Assert(toggleable != null);

			var button = new RadioButton();

			void button_CheckedChanged(object sender, EventArgs e) => toggleable.Enabled = button.Checked;
			void toggleable_EnabledChanged(object sender, EventArgs e) => button.Checked = toggleable.Enabled;

			button.CheckedChanged += button_CheckedChanged;
			toggleable.EnabledChanged += toggleable_EnabledChanged;

			button.Text = toggleable.DisplayName;
			flowLayoutPanel.Controls.Add(button);
			button.AutoSize = false;
			button.Width = flowLayoutPanel.Width - 
				(button.Margin.Horizontal + flowLayoutPanel.Padding.Horizontal);
			//button.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			button.Checked = toggleable.Enabled;

			return button;
		}

		private void groupBox_TextChanged(object sender, EventArgs e)
		{
			OnTextChanged(e);
		}
	}
}
