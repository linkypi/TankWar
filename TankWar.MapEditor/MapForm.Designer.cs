namespace TankWar.MapEditor
{
    partial class MapForm
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentPoint = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.请输入关卡号ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.pnlTile = new System.Windows.Forms.Panel();
            this.pnlMap = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.清空CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblCurrentPoint});
            this.statusStrip1.Location = new System.Drawing.Point(0, 632);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel1.Text = "当前坐标：";
            // 
            // lblCurrentPoint
            // 
            this.lblCurrentPoint.Name = "lblCurrentPoint";
            this.lblCurrentPoint.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.toolStripComboBox1,
            this.请输入关卡号ToolStripMenuItem,
            this.toolStripTextBox1,
            this.清空CToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 29);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存ToolStripMenuItem,
            this.加载ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 25);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.保存ToolStripMenuItem.Text = "保存";
            this.保存ToolStripMenuItem.Click += new System.EventHandler(this.保存ToolStripMenuItem_Click);
            // 
            // 加载ToolStripMenuItem
            // 
            this.加载ToolStripMenuItem.Name = "加载ToolStripMenuItem";
            this.加载ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.加载ToolStripMenuItem.Text = "加载";
            this.加载ToolStripMenuItem.Click += new System.EventHandler(this.加载ToolStripMenuItem_Click);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "地形",
            "地形物"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            // 
            // 请输入关卡号ToolStripMenuItem
            // 
            this.请输入关卡号ToolStripMenuItem.Name = "请输入关卡号ToolStripMenuItem";
            this.请输入关卡号ToolStripMenuItem.Size = new System.Drawing.Size(104, 25);
            this.请输入关卡号ToolStripMenuItem.Text = "请输入关卡号：";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBox1.Text = "1";
            // 
            // pnlTile
            // 
            this.pnlTile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlTile.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTile.Location = new System.Drawing.Point(0, 29);
            this.pnlTile.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTile.Name = "pnlTile";
            this.pnlTile.Size = new System.Drawing.Size(800, 32);
            this.pnlTile.TabIndex = 4;
            // 
            // pnlMap
            // 
            this.pnlMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMap.Location = new System.Drawing.Point(0, 61);
            this.pnlMap.Name = "pnlMap";
            this.pnlMap.Size = new System.Drawing.Size(800, 571);
            this.pnlMap.TabIndex = 5;
            this.pnlMap.MouseLeave += new System.EventHandler(this.pnlMap_MouseLeave);
            this.pnlMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlMap_MouseMove);
            this.pnlMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlMap_MouseDown);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "地图";
            this.saveFileDialog1.Filter = "地图文件|*.map";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "地图";
            this.openFileDialog1.Filter = "地图文件|*.map";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // 清空CToolStripMenuItem
            // 
            this.清空CToolStripMenuItem.Name = "清空CToolStripMenuItem";
            this.清空CToolStripMenuItem.Size = new System.Drawing.Size(60, 25);
            this.清空CToolStripMenuItem.Text = "清空(C)";
            this.清空CToolStripMenuItem.Click += new System.EventHandler(this.清空CToolStripMenuItem_Click);
            // 
            // MapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 654);
            this.Controls.Add(this.pnlMap);
            this.Controls.Add(this.pnlTile);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "坦克大战-地图编辑器";
            this.Load += new System.EventHandler(this.MapForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentPoint;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.Panel pnlTile;
        private System.Windows.Forms.Panel pnlMap;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem 请输入关卡号ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem 清空CToolStripMenuItem;
    }
}