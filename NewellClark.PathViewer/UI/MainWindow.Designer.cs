namespace NewellClark.PathViewer.UI
{
	partial class MainWindow
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gridDisplayPanelUC1 = new NewellClark.PathViewer.MvvmGridGraph.GridDisplayPanelUC();
			this.SuspendLayout();
			// 
			// gridDisplayPanelUC1
			// 
			this.gridDisplayPanelUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridDisplayPanelUC1.Location = new System.Drawing.Point(12, 12);
			this.gridDisplayPanelUC1.Name = "gridDisplayPanelUC1";
			this.gridDisplayPanelUC1.Size = new System.Drawing.Size(1086, 733);
			this.gridDisplayPanelUC1.TabIndex = 0;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1110, 757);
			this.Controls.Add(this.gridDisplayPanelUC1);
			this.Name = "MainWindow";
			this.Text = "MainWindow";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private MvvmGridGraph.GridDisplayPanelUC gridDisplayPanelUC1;
	}
}