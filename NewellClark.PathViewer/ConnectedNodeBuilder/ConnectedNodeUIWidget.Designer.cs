namespace NewellClark.PathViewer.ConnectedNodeBuilder
{
	partial class ConnectedNodeUIWidget
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
			this.nodeToolGroupBox = new System.Windows.Forms.GroupBox();
			this.nodeToolFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
			this.nodePresenterPanel = new System.Windows.Forms.Panel();
			this.nodeToolGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// nodeToolGroupBox
			// 
			this.nodeToolGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.nodeToolGroupBox.Controls.Add(this.nodeToolFlowLayout);
			this.nodeToolGroupBox.Location = new System.Drawing.Point(3, 3);
			this.nodeToolGroupBox.Name = "nodeToolGroupBox";
			this.nodeToolGroupBox.Size = new System.Drawing.Size(172, 417);
			this.nodeToolGroupBox.TabIndex = 0;
			this.nodeToolGroupBox.TabStop = false;
			this.nodeToolGroupBox.Text = "Node Tools";
			// 
			// nodeToolFlowLayout
			// 
			this.nodeToolFlowLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nodeToolFlowLayout.Location = new System.Drawing.Point(6, 19);
			this.nodeToolFlowLayout.Name = "nodeToolFlowLayout";
			this.nodeToolFlowLayout.Size = new System.Drawing.Size(160, 392);
			this.nodeToolFlowLayout.TabIndex = 0;
			// 
			// nodePresenterPanel
			// 
			this.nodePresenterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nodePresenterPanel.Location = new System.Drawing.Point(181, 3);
			this.nodePresenterPanel.Name = "nodePresenterPanel";
			this.nodePresenterPanel.Size = new System.Drawing.Size(495, 417);
			this.nodePresenterPanel.TabIndex = 1;
			// 
			// ConnectedNodeUIWidget
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.nodePresenterPanel);
			this.Controls.Add(this.nodeToolGroupBox);
			this.Name = "ConnectedNodeUIWidget";
			this.Size = new System.Drawing.Size(679, 423);
			this.nodeToolGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox nodeToolGroupBox;
		private System.Windows.Forms.FlowLayoutPanel nodeToolFlowLayout;
		private System.Windows.Forms.Panel nodePresenterPanel;
	}
}
