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
	public partial class AllowedMovementSelectorBox : UserControl
	{
		public AllowedMovementSelectorBox()
		{
			InitializeComponent();

			_Value = new PropertyChangedEvent<GraphMovements>(this, GraphMovements.Orthogonal);
			CreateCheckBoxes();
		}

		private void CreateCheckBoxes()
		{
			foreach (GraphMovements value in Enum.GetValues(typeof(GraphMovements)))
			{
				if (value == GraphMovements.None)
					continue;

				CreateCheckBox(value);
			}
		}

		private CheckBox CreateCheckBox(GraphMovements value)
		{
			bool getCheckedState() => this.Value.HasFlag(value);

			var button = new CheckBox();
			button.Text = value.ToString();
			flowLayoutPanel.Controls.Add(button);
			button.Checked = getCheckedState();

			void button_CheckedChanged(object sender, EventArgs e)
			{
				if (button.Checked)
					this.Value |= value;
				else
					this.Value &= ~value;
			}
			void this_ValueChanged(object sender, EventArgs e) => button.Checked = getCheckedState();

			button.CheckedChanged += button_CheckedChanged;
			this.ValueChanged += this_ValueChanged;

			return button;
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

		public GraphMovements Value
		{
			get => _Value.Value;
			set => _Value.Value = value;
		}
		public event EventHandler ValueChanged
		{
			add => _Value.ValueChanged += value;
			remove => _Value.ValueChanged -= value;
		}
		private PropertyChangedEvent<GraphMovements> _Value;

		private void groupBox_TextChanged(object sender, EventArgs e)
		{
			OnTextChanged(EventArgs.Empty);
		}
	}
}
