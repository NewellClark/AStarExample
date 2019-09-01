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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.connectedNodeBuilderPage = new System.Windows.Forms.TabPage();
			this.connectedNodeUIWidget1 = new NewellClark.PathViewer.ConnectedNodeBuilder.ConnectedNodeUIWidget();
			this.controlBasedBuilderPage = new System.Windows.Forms.TabPage();
			this.tabControl.SuspendLayout();
			this.connectedNodeBuilderPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.connectedNodeBuilderPage);
			this.tabControl.Controls.Add(this.controlBasedBuilderPage);
			this.tabControl.Location = new System.Drawing.Point(9, 9);
			this.tabControl.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1092, 739);
			this.tabControl.TabIndex = 0;
			// 
			// connectedNodeBuilderPage
			// 
			this.connectedNodeBuilderPage.Controls.Add(this.connectedNodeUIWidget1);
			this.connectedNodeBuilderPage.Location = new System.Drawing.Point(4, 22);
			this.connectedNodeBuilderPage.Name = "connectedNodeBuilderPage";
			this.connectedNodeBuilderPage.Padding = new System.Windows.Forms.Padding(3);
			this.connectedNodeBuilderPage.Size = new System.Drawing.Size(1084, 713);
			this.connectedNodeBuilderPage.TabIndex = 0;
			this.connectedNodeBuilderPage.Text = "Connected Node Builder";
			this.connectedNodeBuilderPage.UseVisualStyleBackColor = true;
			// 
			// connectedNodeUIWidget1
			// 
			this.connectedNodeUIWidget1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.connectedNodeUIWidget1.Location = new System.Drawing.Point(3, 3);
			this.connectedNodeUIWidget1.Margin = new System.Windows.Forms.Padding(0);
			this.connectedNodeUIWidget1.Name = "connectedNodeUIWidget1";
			this.connectedNodeUIWidget1.Size = new System.Drawing.Size(1078, 707);
			this.connectedNodeUIWidget1.TabIndex = 0;
			// 
			// controlBasedBuilderPage
			// 
			this.controlBasedBuilderPage.Location = new System.Drawing.Point(4, 22);
			this.controlBasedBuilderPage.Name = "controlBasedBuilderPage";
			this.controlBasedBuilderPage.Padding = new System.Windows.Forms.Padding(3);
			this.controlBasedBuilderPage.Size = new System.Drawing.Size(1084, 713);
			this.controlBasedBuilderPage.TabIndex = 1;
			this.controlBasedBuilderPage.Text = "Control-Based Node Builder";
			this.controlBasedBuilderPage.UseVisualStyleBackColor = true;
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1110, 757);
			this.Controls.Add(this.tabControl);
			this.Name = "MainWindow";
			this.Text = "MainWindow";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.tabControl.ResumeLayout(false);
			this.connectedNodeBuilderPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage connectedNodeBuilderPage;
		private System.Windows.Forms.TabPage controlBasedBuilderPage;
		private ConnectedNodeBuilder.ConnectedNodeUIWidget connectedNodeUIWidget1;
	}
}