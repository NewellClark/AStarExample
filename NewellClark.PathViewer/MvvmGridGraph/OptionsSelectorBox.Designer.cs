namespace NewellClark.PathViewer.MvvmGridGraph
{
	partial class OptionsSelectorBox
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
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.allowDiagonalCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox
			// 
			this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox.Controls.Add(this.allowDiagonalCheckBox);
			this.groupBox.Location = new System.Drawing.Point(3, 3);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(168, 45);
			this.groupBox.TabIndex = 0;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "Options";
			this.groupBox.TextChanged += new System.EventHandler(this.groupBox_TextChanged);
			// 
			// allowDiagonalCheckBox
			// 
			this.allowDiagonalCheckBox.AutoSize = true;
			this.allowDiagonalCheckBox.Location = new System.Drawing.Point(6, 19);
			this.allowDiagonalCheckBox.Name = "allowDiagonalCheckBox";
			this.allowDiagonalCheckBox.Size = new System.Drawing.Size(146, 17);
			this.allowDiagonalCheckBox.TabIndex = 0;
			this.allowDiagonalCheckBox.Text = "Allow diagonal movement";
			this.allowDiagonalCheckBox.UseVisualStyleBackColor = true;
			this.allowDiagonalCheckBox.CheckedChanged += new System.EventHandler(this.allowDiagonalCheckBox_CheckedChanged);
			// 
			// OptionsSelectorBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox);
			this.Name = "OptionsSelectorBox";
			this.Size = new System.Drawing.Size(174, 51);
			this.groupBox.ResumeLayout(false);
			this.groupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.CheckBox allowDiagonalCheckBox;
	}
}
