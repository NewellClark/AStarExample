namespace NewellClark.PathViewer.MvvmGridGraph
{
	partial class PathInfoDisplayer
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
			System.Windows.Forms.Label euclidianDistanceCaptionLabel;
			System.Windows.Forms.Label manhattanDistanceCaptionLabel;
			System.Windows.Forms.Label nodeCountCaptionLabel;
			System.Windows.Forms.Label pathCostCaptionLabel;
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.euclidianDistanceLabel = new System.Windows.Forms.Label();
			this.manhattanDistanceLabel = new System.Windows.Forms.Label();
			this.nodeCountLabel = new System.Windows.Forms.Label();
			this.pathCostLabel = new System.Windows.Forms.Label();
			euclidianDistanceCaptionLabel = new System.Windows.Forms.Label();
			manhattanDistanceCaptionLabel = new System.Windows.Forms.Label();
			nodeCountCaptionLabel = new System.Windows.Forms.Label();
			pathCostCaptionLabel = new System.Windows.Forms.Label();
			this.groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox
			// 
			this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox.Controls.Add(this.pathCostLabel);
			this.groupBox.Controls.Add(this.nodeCountLabel);
			this.groupBox.Controls.Add(this.manhattanDistanceLabel);
			this.groupBox.Controls.Add(this.euclidianDistanceLabel);
			this.groupBox.Controls.Add(pathCostCaptionLabel);
			this.groupBox.Controls.Add(nodeCountCaptionLabel);
			this.groupBox.Controls.Add(manhattanDistanceCaptionLabel);
			this.groupBox.Controls.Add(euclidianDistanceCaptionLabel);
			this.groupBox.Location = new System.Drawing.Point(3, 3);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(160, 112);
			this.groupBox.TabIndex = 0;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "Path Info";
			// 
			// euclidianDistanceCaptionLabel
			// 
			euclidianDistanceCaptionLabel.AutoSize = true;
			euclidianDistanceCaptionLabel.Location = new System.Drawing.Point(14, 21);
			euclidianDistanceCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			euclidianDistanceCaptionLabel.Name = "euclidianDistanceCaptionLabel";
			euclidianDistanceCaptionLabel.Size = new System.Drawing.Size(99, 13);
			euclidianDistanceCaptionLabel.TabIndex = 0;
			euclidianDistanceCaptionLabel.Text = "Euclidian distance: ";
			// 
			// manhattanDistanceCaptionLabel
			// 
			manhattanDistanceCaptionLabel.AutoSize = true;
			manhattanDistanceCaptionLabel.Location = new System.Drawing.Point(6, 44);
			manhattanDistanceCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			manhattanDistanceCaptionLabel.Name = "manhattanDistanceCaptionLabel";
			manhattanDistanceCaptionLabel.Size = new System.Drawing.Size(107, 13);
			manhattanDistanceCaptionLabel.TabIndex = 1;
			manhattanDistanceCaptionLabel.Text = "Manhattan distance: ";
			// 
			// nodeCountCaptionLabel
			// 
			nodeCountCaptionLabel.AutoSize = true;
			nodeCountCaptionLabel.Location = new System.Drawing.Point(45, 67);
			nodeCountCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			nodeCountCaptionLabel.Name = "nodeCountCaptionLabel";
			nodeCountCaptionLabel.Size = new System.Drawing.Size(69, 13);
			nodeCountCaptionLabel.TabIndex = 2;
			nodeCountCaptionLabel.Text = "Node count: ";
			// 
			// pathCostCaptionLabel
			// 
			pathCostCaptionLabel.AutoSize = true;
			pathCostCaptionLabel.Location = new System.Drawing.Point(55, 90);
			pathCostCaptionLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			pathCostCaptionLabel.Name = "pathCostCaptionLabel";
			pathCostCaptionLabel.Size = new System.Drawing.Size(58, 13);
			pathCostCaptionLabel.TabIndex = 3;
			pathCostCaptionLabel.Text = "Path cost: ";
			// 
			// euclidianDistanceLabel
			// 
			this.euclidianDistanceLabel.AutoSize = true;
			this.euclidianDistanceLabel.Location = new System.Drawing.Point(121, 21);
			this.euclidianDistanceLabel.Margin = new System.Windows.Forms.Padding(5);
			this.euclidianDistanceLabel.MinimumSize = new System.Drawing.Size(25, 13);
			this.euclidianDistanceLabel.Name = "euclidianDistanceLabel";
			this.euclidianDistanceLabel.Size = new System.Drawing.Size(25, 13);
			this.euclidianDistanceLabel.TabIndex = 4;
			this.euclidianDistanceLabel.Text = "000";
			// 
			// manhattanDistanceLabel
			// 
			this.manhattanDistanceLabel.AutoSize = true;
			this.manhattanDistanceLabel.Location = new System.Drawing.Point(121, 44);
			this.manhattanDistanceLabel.Margin = new System.Windows.Forms.Padding(5);
			this.manhattanDistanceLabel.MinimumSize = new System.Drawing.Size(25, 13);
			this.manhattanDistanceLabel.Name = "manhattanDistanceLabel";
			this.manhattanDistanceLabel.Size = new System.Drawing.Size(25, 13);
			this.manhattanDistanceLabel.TabIndex = 5;
			this.manhattanDistanceLabel.Text = "000";
			// 
			// nodeCountLabel
			// 
			this.nodeCountLabel.AutoSize = true;
			this.nodeCountLabel.Location = new System.Drawing.Point(121, 67);
			this.nodeCountLabel.Margin = new System.Windows.Forms.Padding(5);
			this.nodeCountLabel.MinimumSize = new System.Drawing.Size(25, 13);
			this.nodeCountLabel.Name = "nodeCountLabel";
			this.nodeCountLabel.Size = new System.Drawing.Size(25, 13);
			this.nodeCountLabel.TabIndex = 6;
			this.nodeCountLabel.Text = "000";
			// 
			// pathCostLabel
			// 
			this.pathCostLabel.AutoSize = true;
			this.pathCostLabel.Location = new System.Drawing.Point(121, 90);
			this.pathCostLabel.Margin = new System.Windows.Forms.Padding(5);
			this.pathCostLabel.MinimumSize = new System.Drawing.Size(25, 13);
			this.pathCostLabel.Name = "pathCostLabel";
			this.pathCostLabel.Size = new System.Drawing.Size(25, 13);
			this.pathCostLabel.TabIndex = 7;
			this.pathCostLabel.Text = "000";
			// 
			// PathInfoDisplayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox);
			this.MinimumSize = new System.Drawing.Size(166, 118);
			this.Name = "PathInfoDisplayer";
			this.Size = new System.Drawing.Size(166, 118);
			this.groupBox.ResumeLayout(false);
			this.groupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Label pathCostLabel;
		private System.Windows.Forms.Label nodeCountLabel;
		private System.Windows.Forms.Label manhattanDistanceLabel;
		private System.Windows.Forms.Label euclidianDistanceLabel;
	}
}
