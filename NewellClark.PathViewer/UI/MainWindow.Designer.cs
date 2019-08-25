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
			this.nodePresenterPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// nodePresenterPanel
			// 
			this.nodePresenterPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nodePresenterPanel.Location = new System.Drawing.Point(214, 12);
			this.nodePresenterPanel.Name = "nodePresenterPanel";
			this.nodePresenterPanel.Size = new System.Drawing.Size(574, 426);
			this.nodePresenterPanel.TabIndex = 0;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.nodePresenterPanel);
			this.Name = "MainWindow";
			this.Text = "MainWindow";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel nodePresenterPanel;
	}
}