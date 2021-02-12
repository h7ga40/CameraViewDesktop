namespace CameraViewDesktop
{
	partial class CameraView
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
			if (disposing && (components != null)) {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraView));
			this.SuspendLayout();
			// 
			// CameraView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = global::CameraViewDesktop.Properties.Settings.Default.CameraViewSize;
			this.ControlBox = false;
			this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::CameraViewDesktop.Properties.Settings.Default, "CameraViewLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::CameraViewDesktop.Properties.Settings.Default, "CameraViewSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = global::CameraViewDesktop.Properties.Settings.Default.CameraViewLocation;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(79, 68);
			this.Name = "CameraView";
			this.ShowIcon = false;
			this.Text = "CameraView";
			this.Activated += new System.EventHandler(this.CameraView_Activated);
			this.Deactivate += new System.EventHandler(this.CameraView_Deactivate);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CameraView_FormClosed);
			this.Load += new System.EventHandler(this.CameraView_Load);
			this.DoubleClick += new System.EventHandler(this.CameraView_DoubleClick);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CameraView_KeyDown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CameraView_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CameraView_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CameraView_MouseUp);
			this.Resize += new System.EventHandler(this.CameraView_Resize);
			this.ResumeLayout(false);

		}

		#endregion
	}
}