using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.MvvmGridGraph
{
	public partial class OptionsSelectorBox : UserControl
	{
		public OptionsSelectorBox()
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

		public bool AllowDiagonalMovement
		{
			get => allowDiagonalCheckBox.Checked;
			set => allowDiagonalCheckBox.Checked = value;
		}

		public event EventHandler AllowDiagonalMovementChanged;
		protected virtual void OnAllowDiagonalMovementChanged(EventArgs e)
		{
			AllowDiagonalMovementChanged?.Invoke(this, e);
		}

		private void groupBox_TextChanged(object sender, EventArgs e)
		{
			OnTextChanged(e);
		}

		private void allowDiagonalCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			OnAllowDiagonalMovementChanged(EventArgs.Empty);
		}
	}
}
