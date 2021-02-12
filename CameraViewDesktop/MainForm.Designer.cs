namespace CameraViewDesktop
{
	partial class MainForm
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.cmbVideoList = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.cmbAudioList = new System.Windows.Forms.ComboBox();
			this.cmbEffectorList = new System.Windows.Forms.ComboBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// cmbVideoList
			// 
			this.cmbVideoList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbVideoList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbVideoList.FormattingEnabled = true;
			this.cmbVideoList.Location = new System.Drawing.Point(16, 15);
			this.cmbVideoList.Margin = new System.Windows.Forms.Padding(4);
			this.cmbVideoList.Name = "cmbVideoList";
			this.cmbVideoList.Size = new System.Drawing.Size(283, 23);
			this.cmbVideoList.TabIndex = 0;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(16, 305);
			this.button1.Margin = new System.Windows.Forms.Padding(4);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(284, 29);
			this.button1.TabIndex = 1;
			this.button1.Text = "Run";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cmbAudioList
			// 
			this.cmbAudioList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbAudioList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAudioList.FormattingEnabled = true;
			this.cmbAudioList.Location = new System.Drawing.Point(16, 48);
			this.cmbAudioList.Margin = new System.Windows.Forms.Padding(4);
			this.cmbAudioList.Name = "cmbAudioList";
			this.cmbAudioList.Size = new System.Drawing.Size(283, 23);
			this.cmbAudioList.TabIndex = 2;
			// 
			// cmbEffectorList
			// 
			this.cmbEffectorList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbEffectorList.FormattingEnabled = true;
			this.cmbEffectorList.Location = new System.Drawing.Point(16, 102);
			this.cmbEffectorList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.cmbEffectorList.Name = "cmbEffectorList";
			this.cmbEffectorList.Size = new System.Drawing.Size(283, 23);
			this.cmbEffectorList.TabIndex = 7;
			this.cmbEffectorList.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Checked = global::CameraViewDesktop.Properties.Settings.Default.CameraViewTopMost;
			this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::CameraViewDesktop.Properties.Settings.Default, "CameraViewTopMost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.checkBox1.Location = new System.Drawing.Point(16, 78);
			this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(74, 19);
			this.checkBox1.TabIndex = 6;
			this.checkBox1.Text = "最前面";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
			// 
			// trackBar1
			// 
			this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBar1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::CameraViewDesktop.Properties.Settings.Default, "KeyColorRange", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.trackBar1.Location = new System.Drawing.Point(45, 132);
			this.trackBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.trackBar1.Maximum = 255;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(253, 56);
			this.trackBar1.TabIndex = 5;
			this.trackBar1.TickFrequency = 15;
			this.trackBar1.Value = global::CameraViewDesktop.Properties.Settings.Default.KeyColorRange;
			this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
			// 
			// label1
			// 
			this.label1.BackColor = global::CameraViewDesktop.Properties.Settings.Default.KeyColor;
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(16, 132);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 22);
			this.label1.TabIndex = 4;
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(316, 349);
			this.Controls.Add(this.cmbEffectorList);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cmbAudioList);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.cmbVideoList);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.Text = "CameraViwer";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cmbVideoList;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox cmbAudioList;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.ComboBox cmbEffectorList;
		private System.Windows.Forms.Timer timer1;
	}
}

