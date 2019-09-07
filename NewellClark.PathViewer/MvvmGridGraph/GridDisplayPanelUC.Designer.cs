namespace NewellClark.PathViewer.MvvmGridGraph
{
	partial class GridDisplayPanelUC
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pathInfoDisplayer = new NewellClark.PathViewer.MvvmGridGraph.PathInfoDisplayer();
			this.heuristicSelectorBox = new NewellClark.PathViewer.MvvmGridGraph.PathFinderHeuristicSelectorBox();
			this.toolSelectorBox = new NewellClark.PathViewer.MvvmGridGraph.MutuallyExclusiveOptionSelectorBox();
			this.gridGraphViewPanel = new NewellClark.PathViewer.MvvmGridGraph.GridGraphViewPanel();
			this.allowedMovementSelectorBox = new NewellClark.PathViewer.MvvmGridGraph.AllowedMovementSelectorBox();
			this.SuspendLayout();
			// 
			// pathInfoDisplayer
			// 
			this.pathInfoDisplayer.EuclidianDistance = null;
			this.pathInfoDisplayer.Location = new System.Drawing.Point(3, 456);
			this.pathInfoDisplayer.ManhattanDistance = null;
			this.pathInfoDisplayer.MinimumSize = new System.Drawing.Size(166, 118);
			this.pathInfoDisplayer.Name = "pathInfoDisplayer";
			this.pathInfoDisplayer.NodeCount = null;
			this.pathInfoDisplayer.NullText = "---";
			this.pathInfoDisplayer.PathCost = null;
			this.pathInfoDisplayer.Size = new System.Drawing.Size(212, 118);
			this.pathInfoDisplayer.TabIndex = 4;
			// 
			// heuristicSelectorBox
			// 
			this.heuristicSelectorBox.Heuristic = NewellClark.PathViewer.MvvmGridGraph.PathFinderHeuristic.Euclidian;
			this.heuristicSelectorBox.Location = new System.Drawing.Point(3, 182);
			this.heuristicSelectorBox.Name = "heuristicSelectorBox";
			this.heuristicSelectorBox.Size = new System.Drawing.Size(212, 123);
			this.heuristicSelectorBox.TabIndex = 2;
			// 
			// toolSelectorBox
			// 
			this.toolSelectorBox.Location = new System.Drawing.Point(3, 3);
			this.toolSelectorBox.Name = "toolSelectorBox";
			this.toolSelectorBox.Size = new System.Drawing.Size(212, 173);
			this.toolSelectorBox.TabIndex = 1;
			// 
			// gridGraphViewPanel
			// 
			this.gridGraphViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridGraphViewPanel.Location = new System.Drawing.Point(221, 3);
			this.gridGraphViewPanel.Name = "gridGraphViewPanel";
			this.gridGraphViewPanel.Size = new System.Drawing.Size(778, 718);
			this.gridGraphViewPanel.TabIndex = 0;
			// 
			// allowedMovementSelectorBox
			// 
			this.allowedMovementSelectorBox.Location = new System.Drawing.Point(3, 311);
			this.allowedMovementSelectorBox.Name = "allowedMovementSelectorBox";
			this.allowedMovementSelectorBox.Size = new System.Drawing.Size(212, 139);
			this.allowedMovementSelectorBox.TabIndex = 5;
			this.allowedMovementSelectorBox.Value = NewellClark.PathViewer.MvvmGridGraph.GraphMovements.Orthogonal;
			// 
			// GridDisplayPanelUC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.allowedMovementSelectorBox);
			this.Controls.Add(this.pathInfoDisplayer);
			this.Controls.Add(this.heuristicSelectorBox);
			this.Controls.Add(this.toolSelectorBox);
			this.Controls.Add(this.gridGraphViewPanel);
			this.Name = "GridDisplayPanelUC";
			this.Size = new System.Drawing.Size(1002, 724);
			this.Load += new System.EventHandler(this.GridDisplayPanelUC_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private GridGraphViewPanel gridGraphViewPanel;
		private MutuallyExclusiveOptionSelectorBox toolSelectorBox;
		private PathFinderHeuristicSelectorBox heuristicSelectorBox;
		private PathInfoDisplayer pathInfoDisplayer;
		private AllowedMovementSelectorBox allowedMovementSelectorBox;
	}
}
