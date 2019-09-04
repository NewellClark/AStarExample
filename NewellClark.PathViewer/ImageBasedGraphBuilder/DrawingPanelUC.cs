using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	public partial class DrawingPanelUC : UserControl
	{
		private readonly TogglablePathFinder _pathFinder;

		public DrawingPanelUC()
		{
			InitializeComponent();

			foreach (var tool in drawingPanel.Tools)
				toggleToolGroupBox.AddTool(tool);

			_pathFinder = new TogglablePathFinder(Graph, drawingPanel) { Enabled = true };

			Graph.DiagonalsAllowedChanged += (s, e) => diagonalsAllowedCheckBox.Checked = Graph.DiagonalsAllowed;
			diagonalsAllowedCheckBox.CheckedChanged += (s, e) => Graph.DiagonalsAllowed = diagonalsAllowedCheckBox.Checked;

			Graph.UseManhattanDistanceChanged += (s, e) => useManhattanCheckBox.Checked = Graph.UseManhattanDistance;
			useManhattanCheckBox.CheckedChanged += (s, e) => Graph.UseManhattanDistance = useManhattanCheckBox.Checked;

			Graph.UseInvertedComparisonChanged += (s, e) => useInvertedComparisonCheckBox.Checked = Graph.UseInvertedComparison;
			useInvertedComparisonCheckBox.CheckedChanged += (s, e) => Graph.UseInvertedComparison = useInvertedComparisonCheckBox.Checked;
		}

		private BitmapGraph Graph => drawingPanel.Graph;

		private IReadOnlyList<Toggleable> Tools => drawingPanel.Tools;
	}
}
