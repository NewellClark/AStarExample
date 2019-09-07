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
	public partial class PathFinderHeuristicSelectorBox : UserControl
	{
		public PathFinderHeuristicSelectorBox()
		{
			InitializeComponent();

			_Heuristic = new PropertyChangedEvent<PathFinderHeuristic>(this, PathFinderHeuristic.Euclidian);
			InitializeRadioButtons();
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

		public PathFinderHeuristic Heuristic
		{
			get => _Heuristic.Value;
			set => _Heuristic.Value = value;
		}
		public event EventHandler HeuristicChanged
		{
			add => _Heuristic.ValueChanged += value;
			remove => _Heuristic.ValueChanged -= value;
		}
		private PropertyChangedEvent<PathFinderHeuristic> _Heuristic;

		private void InitializeRadioButtons()
		{
			foreach (PathFinderHeuristic value in Enum.GetValues(typeof(PathFinderHeuristic)))
				CreateRadioButtonForValue(value);
		}

		RadioButton CreateRadioButtonForValue(PathFinderHeuristic value)
		{
			var button = new RadioButton();
			button.Text = value.ToString();
			flowLayoutPanel.Controls.Add(button);
			button.AutoSize = false;
			button.Width = flowLayoutPanel.Width -
				(button.Margin.Horizontal + flowLayoutPanel.Padding.Horizontal);
			button.Checked = (Heuristic == value);

			void button_CheckedChanged(object sender, EventArgs e) => Heuristic = value;
			void heuristicChanged(object sender, EventArgs e)
			{
				button.Checked = (Heuristic == value);
			}

			button.CheckedChanged += button_CheckedChanged;
			_Heuristic.ValueChanged += heuristicChanged;

			return button;
		}

		private void groupBox_TextChanged(object sender, EventArgs e)
		{
			OnTextChanged(e);
		}
	}
}
