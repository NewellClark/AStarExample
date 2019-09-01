namespace NewellClark.PathViewer.NodeBuilder2
{
	partial class GraphPresenter
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
			this.toolLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.graphDisplayPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// toolLayoutPanel
			// 
			this.toolLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.toolLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.toolLayoutPanel.Name = "toolLayoutPanel";
			this.toolLayoutPanel.Size = new System.Drawing.Size(114, 365);
			this.toolLayoutPanel.TabIndex = 0;
			// 
			// graphDisplayPanel
			// 
			this.graphDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.graphDisplayPanel.Location = new System.Drawing.Point(123, 3);
			this.graphDisplayPanel.Name = "graphDisplayPanel";
			this.graphDisplayPanel.Size = new System.Drawing.Size(460, 365);
			this.graphDisplayPanel.TabIndex = 1;
			// 
			// GraphPresenter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.graphDisplayPanel);
			this.Controls.Add(this.toolLayoutPanel);
			this.Name = "GraphPresenter";
			this.Size = new System.Drawing.Size(586, 371);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel toolLayoutPanel;
		private System.Windows.Forms.Panel graphDisplayPanel;
	}
}
