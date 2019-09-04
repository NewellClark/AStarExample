namespace NewellClark.PathViewer.ImageBasedGraphBuilder
{
	partial class DrawingPanelUC
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
			this.drawingPanel = new NewellClark.PathViewer.ImageBasedGraphBuilder.DrawingPanel();
			this.toggleToolGroupBox = new NewellClark.PathViewer.ImageBasedGraphBuilder.ToggleToolGroupBox();
			this.optionsGroupBox = new System.Windows.Forms.GroupBox();
			this.useManhattanCheckBox = new System.Windows.Forms.CheckBox();
			this.diagonalsAllowedCheckBox = new System.Windows.Forms.CheckBox();
			this.useInvertedComparisonCheckBox = new System.Windows.Forms.CheckBox();
			this.optionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// drawingPanel
			// 
			this.drawingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.drawingPanel.Location = new System.Drawing.Point(172, 3);
			this.drawingPanel.Name = "drawingPanel";
			this.drawingPanel.Size = new System.Drawing.Size(739, 642);
			this.drawingPanel.TabIndex = 0;
			// 
			// toggleToolGroupBox
			// 
			this.toggleToolGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.toggleToolGroupBox.Location = new System.Drawing.Point(3, 3);
			this.toggleToolGroupBox.Name = "toggleToolGroupBox";
			this.toggleToolGroupBox.Size = new System.Drawing.Size(166, 320);
			this.toggleToolGroupBox.TabIndex = 1;
			// 
			// optionsGroupBox
			// 
			this.optionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.optionsGroupBox.Controls.Add(this.useInvertedComparisonCheckBox);
			this.optionsGroupBox.Controls.Add(this.useManhattanCheckBox);
			this.optionsGroupBox.Controls.Add(this.diagonalsAllowedCheckBox);
			this.optionsGroupBox.Location = new System.Drawing.Point(3, 329);
			this.optionsGroupBox.Name = "optionsGroupBox";
			this.optionsGroupBox.Size = new System.Drawing.Size(166, 316);
			this.optionsGroupBox.TabIndex = 2;
			this.optionsGroupBox.TabStop = false;
			this.optionsGroupBox.Text = "Options";
			// 
			// useManhattanCheckBox
			// 
			this.useManhattanCheckBox.AutoSize = true;
			this.useManhattanCheckBox.Location = new System.Drawing.Point(6, 42);
			this.useManhattanCheckBox.Name = "useManhattanCheckBox";
			this.useManhattanCheckBox.Size = new System.Drawing.Size(141, 17);
			this.useManhattanCheckBox.TabIndex = 1;
			this.useManhattanCheckBox.Text = "Use Manhattan heuristic";
			this.useManhattanCheckBox.UseVisualStyleBackColor = true;
			// 
			// diagonalsAllowedCheckBox
			// 
			this.diagonalsAllowedCheckBox.AutoSize = true;
			this.diagonalsAllowedCheckBox.Location = new System.Drawing.Point(6, 19);
			this.diagonalsAllowedCheckBox.Name = "diagonalsAllowedCheckBox";
			this.diagonalsAllowedCheckBox.Size = new System.Drawing.Size(112, 17);
			this.diagonalsAllowedCheckBox.TabIndex = 0;
			this.diagonalsAllowedCheckBox.Text = "Diagonals allowed";
			this.diagonalsAllowedCheckBox.UseVisualStyleBackColor = true;
			// 
			// useInvertedComparisonCheckBox
			// 
			this.useInvertedComparisonCheckBox.AutoSize = true;
			this.useInvertedComparisonCheckBox.Location = new System.Drawing.Point(6, 65);
			this.useInvertedComparisonCheckBox.Name = "useInvertedComparisonCheckBox";
			this.useInvertedComparisonCheckBox.Size = new System.Drawing.Size(143, 17);
			this.useInvertedComparisonCheckBox.TabIndex = 2;
			this.useInvertedComparisonCheckBox.Text = "Use inverted comparison";
			this.useInvertedComparisonCheckBox.UseVisualStyleBackColor = true;
			// 
			// DrawingPanelUC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.optionsGroupBox);
			this.Controls.Add(this.toggleToolGroupBox);
			this.Controls.Add(this.drawingPanel);
			this.Name = "DrawingPanelUC";
			this.Size = new System.Drawing.Size(914, 648);
			this.optionsGroupBox.ResumeLayout(false);
			this.optionsGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private DrawingPanel drawingPanel;
		private ToggleToolGroupBox toggleToolGroupBox;
		private System.Windows.Forms.GroupBox optionsGroupBox;
		private System.Windows.Forms.CheckBox diagonalsAllowedCheckBox;
		private System.Windows.Forms.CheckBox useManhattanCheckBox;
		private System.Windows.Forms.CheckBox useInvertedComparisonCheckBox;
	}
}
