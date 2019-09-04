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
			this.sidebarGroupBox = new System.Windows.Forms.GroupBox();
			this.blockUnblockRadioButton = new System.Windows.Forms.RadioButton();
			this.gridGraphViewPanel = new NewellClark.PathViewer.MvvmGridGraph.GridGraphViewPanel();
			this.setStartGoalRadioButton = new System.Windows.Forms.RadioButton();
			this.sidebarGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// sidebarGroupBox
			// 
			this.sidebarGroupBox.Controls.Add(this.setStartGoalRadioButton);
			this.sidebarGroupBox.Controls.Add(this.blockUnblockRadioButton);
			this.sidebarGroupBox.Location = new System.Drawing.Point(0, 3);
			this.sidebarGroupBox.Name = "sidebarGroupBox";
			this.sidebarGroupBox.Size = new System.Drawing.Size(215, 718);
			this.sidebarGroupBox.TabIndex = 1;
			this.sidebarGroupBox.TabStop = false;
			this.sidebarGroupBox.Text = "Node Tools";
			// 
			// blockUnblockRadioButton
			// 
			this.blockUnblockRadioButton.AutoSize = true;
			this.blockUnblockRadioButton.Location = new System.Drawing.Point(6, 19);
			this.blockUnblockRadioButton.Name = "blockUnblockRadioButton";
			this.blockUnblockRadioButton.Size = new System.Drawing.Size(127, 17);
			this.blockUnblockRadioButton.TabIndex = 0;
			this.blockUnblockRadioButton.TabStop = true;
			this.blockUnblockRadioButton.Text = "Block/unblock nodes";
			this.blockUnblockRadioButton.UseVisualStyleBackColor = true;
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
			// setStartGoalRadioButton
			// 
			this.setStartGoalRadioButton.AutoSize = true;
			this.setStartGoalRadioButton.Location = new System.Drawing.Point(6, 42);
			this.setStartGoalRadioButton.Name = "setStartGoalRadioButton";
			this.setStartGoalRadioButton.Size = new System.Drawing.Size(89, 17);
			this.setStartGoalRadioButton.TabIndex = 1;
			this.setStartGoalRadioButton.TabStop = true;
			this.setStartGoalRadioButton.Text = "Set start/goal";
			this.setStartGoalRadioButton.UseVisualStyleBackColor = true;
			// 
			// GridDisplayPanelUC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.sidebarGroupBox);
			this.Controls.Add(this.gridGraphViewPanel);
			this.Name = "GridDisplayPanelUC";
			this.Size = new System.Drawing.Size(1002, 724);
			this.sidebarGroupBox.ResumeLayout(false);
			this.sidebarGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private GridGraphViewPanel gridGraphViewPanel;
		private System.Windows.Forms.GroupBox sidebarGroupBox;
		private System.Windows.Forms.RadioButton blockUnblockRadioButton;
		private System.Windows.Forms.RadioButton setStartGoalRadioButton;
	}
}
