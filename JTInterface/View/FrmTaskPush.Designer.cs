namespace JTInterface.View
{
    partial class FrmTaskPush
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
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.全部刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开启自动排单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭自动排单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.手动排任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.上移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.暂停ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.恢复ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.carid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.intime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.menuStrip3 = new System.Windows.Forms.MenuStrip();
            this.上移ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.下移ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.移除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.no_bill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_bill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_productorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_fahuolou = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_type_bengsong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_qiangdu_ranks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shigongbuwei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.distance_car = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预排任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.menuStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no_bill,
            this.date_bill,
            this.no_productorder,
            this.no_fahuolou,
            this.no_type_bengsong,
            this.no_qiangdu_ranks,
            this.shigongbuwei,
            this.distance_car});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 71);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1498, 1085);
            this.dataGridView2.TabIndex = 34;
            this.dataGridView2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellClick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.预排任务ToolStripMenuItem,
            this.全部刷新ToolStripMenuItem,
            this.开启自动排单ToolStripMenuItem,
            this.关闭自动排单ToolStripMenuItem,
            this.手动排任务ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1951, 42);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 全部刷新ToolStripMenuItem
            // 
            this.全部刷新ToolStripMenuItem.Name = "全部刷新ToolStripMenuItem";
            this.全部刷新ToolStripMenuItem.Size = new System.Drawing.Size(127, 36);
            this.全部刷新ToolStripMenuItem.Text = "全部刷新";
            this.全部刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 开启自动排单ToolStripMenuItem
            // 
            this.开启自动排单ToolStripMenuItem.Name = "开启自动排单ToolStripMenuItem";
            this.开启自动排单ToolStripMenuItem.Size = new System.Drawing.Size(177, 36);
            this.开启自动排单ToolStripMenuItem.Text = "开启自动排单";
            this.开启自动排单ToolStripMenuItem.Click += new System.EventHandler(this.开启自动刷新ToolStripMenuItem_Click);
            // 
            // 关闭自动排单ToolStripMenuItem
            // 
            this.关闭自动排单ToolStripMenuItem.Name = "关闭自动排单ToolStripMenuItem";
            this.关闭自动排单ToolStripMenuItem.Size = new System.Drawing.Size(177, 36);
            this.关闭自动排单ToolStripMenuItem.Text = "关闭自动排单";
            this.关闭自动排单ToolStripMenuItem.Click += new System.EventHandler(this.关闭自动刷新ToolStripMenuItem_Click);
            // 
            // 手动排任务ToolStripMenuItem
            // 
            this.手动排任务ToolStripMenuItem.Name = "手动排任务ToolStripMenuItem";
            this.手动排任务ToolStripMenuItem.Size = new System.Drawing.Size(152, 36);
            this.手动排任务ToolStripMenuItem.Text = "手动排任务";
            this.手动排任务ToolStripMenuItem.Click += new System.EventHandler(this.手动排任务ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView2);
            this.groupBox1.Controls.Add(this.menuStrip2);
            this.groupBox1.Location = new System.Drawing.Point(1, 43);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1504, 1159);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预排任务";
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上移ToolStripMenuItem,
            this.下移ToolStripMenuItem,
            this.移除ToolStripMenuItem,
            this.暂停ToolStripMenuItem,
            this.恢复ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(3, 31);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(1498, 40);
            this.menuStrip2.TabIndex = 35;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // 上移ToolStripMenuItem
            // 
            this.上移ToolStripMenuItem.Name = "上移ToolStripMenuItem";
            this.上移ToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.上移ToolStripMenuItem.Text = "上移";
            this.上移ToolStripMenuItem.Click += new System.EventHandler(this.上移ToolStripMenuItem_Click);
            // 
            // 下移ToolStripMenuItem
            // 
            this.下移ToolStripMenuItem.Name = "下移ToolStripMenuItem";
            this.下移ToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.下移ToolStripMenuItem.Text = "下移";
            this.下移ToolStripMenuItem.Click += new System.EventHandler(this.下移ToolStripMenuItem_Click);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.移除ToolStripMenuItem.Text = "移除";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // 暂停ToolStripMenuItem
            // 
            this.暂停ToolStripMenuItem.Name = "暂停ToolStripMenuItem";
            this.暂停ToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.暂停ToolStripMenuItem.Text = "暂停";
            this.暂停ToolStripMenuItem.Click += new System.EventHandler(this.暂停ToolStripMenuItem_Click);
            // 
            // 恢复ToolStripMenuItem
            // 
            this.恢复ToolStripMenuItem.Name = "恢复ToolStripMenuItem";
            this.恢复ToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.恢复ToolStripMenuItem.Text = "恢复";
            this.恢复ToolStripMenuItem.Click += new System.EventHandler(this.恢复ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(77, 36);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.carid,
            this.intime});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 71);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(440, 1085);
            this.dataGridView1.TabIndex = 33;
            // 
            // carid
            // 
            this.carid.DataPropertyName = "carid";
            this.carid.HeaderText = "车辆编号";
            this.carid.Name = "carid";
            // 
            // intime
            // 
            this.intime.DataPropertyName = "intime";
            this.intime.HeaderText = "进厂时间";
            this.intime.Name = "intime";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Controls.Add(this.menuStrip3);
            this.groupBox3.Location = new System.Drawing.Point(1505, 43);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(446, 1159);
            this.groupBox3.TabIndex = 37;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "车辆队列";
            // 
            // menuStrip3
            // 
            this.menuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上移ToolStripMenuItem1,
            this.下移ToolStripMenuItem1,
            this.移除ToolStripMenuItem1,
            this.刷新ToolStripMenuItem1});
            this.menuStrip3.Location = new System.Drawing.Point(3, 31);
            this.menuStrip3.Name = "menuStrip3";
            this.menuStrip3.Size = new System.Drawing.Size(440, 40);
            this.menuStrip3.TabIndex = 34;
            this.menuStrip3.Text = "menuStrip3";
            // 
            // 上移ToolStripMenuItem1
            // 
            this.上移ToolStripMenuItem1.Name = "上移ToolStripMenuItem1";
            this.上移ToolStripMenuItem1.Size = new System.Drawing.Size(77, 36);
            this.上移ToolStripMenuItem1.Text = "上移";
            this.上移ToolStripMenuItem1.Click += new System.EventHandler(this.上移ToolStripMenuItem1_Click);
            // 
            // 下移ToolStripMenuItem1
            // 
            this.下移ToolStripMenuItem1.Name = "下移ToolStripMenuItem1";
            this.下移ToolStripMenuItem1.Size = new System.Drawing.Size(77, 36);
            this.下移ToolStripMenuItem1.Text = "下移";
            this.下移ToolStripMenuItem1.Click += new System.EventHandler(this.下移ToolStripMenuItem1_Click);
            // 
            // 移除ToolStripMenuItem1
            // 
            this.移除ToolStripMenuItem1.Name = "移除ToolStripMenuItem1";
            this.移除ToolStripMenuItem1.Size = new System.Drawing.Size(77, 36);
            this.移除ToolStripMenuItem1.Text = "移除";
            this.移除ToolStripMenuItem1.Click += new System.EventHandler(this.移除ToolStripMenuItem1_Click);
            // 
            // 刷新ToolStripMenuItem1
            // 
            this.刷新ToolStripMenuItem1.Name = "刷新ToolStripMenuItem1";
            this.刷新ToolStripMenuItem1.Size = new System.Drawing.Size(77, 36);
            this.刷新ToolStripMenuItem1.Text = "刷新";
            this.刷新ToolStripMenuItem1.Click += new System.EventHandler(this.刷新ToolStripMenuItem1_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // no_bill
            // 
            this.no_bill.DataPropertyName = "no_bill";
            this.no_bill.HeaderText = "预排单号";
            this.no_bill.Name = "no_bill";
            this.no_bill.ReadOnly = true;
            // 
            // date_bill
            // 
            this.date_bill.DataPropertyName = "date_bill";
            this.date_bill.HeaderText = "排单日期";
            this.date_bill.Name = "date_bill";
            this.date_bill.ReadOnly = true;
            // 
            // no_productorder
            // 
            this.no_productorder.DataPropertyName = "no_productorder";
            this.no_productorder.HeaderText = "任务单号";
            this.no_productorder.Name = "no_productorder";
            this.no_productorder.ReadOnly = true;
            // 
            // no_fahuolou
            // 
            this.no_fahuolou.DataPropertyName = "no_fahuolou";
            this.no_fahuolou.HeaderText = "发货楼";
            this.no_fahuolou.Name = "no_fahuolou";
            this.no_fahuolou.ReadOnly = true;
            // 
            // no_type_bengsong
            // 
            this.no_type_bengsong.DataPropertyName = "name_bengsong";
            this.no_type_bengsong.HeaderText = "是否泵送";
            this.no_type_bengsong.Name = "no_type_bengsong";
            this.no_type_bengsong.ReadOnly = true;
            // 
            // no_qiangdu_ranks
            // 
            this.no_qiangdu_ranks.DataPropertyName = "no_qiangdu_ranks";
            this.no_qiangdu_ranks.HeaderText = "强度等级";
            this.no_qiangdu_ranks.Name = "no_qiangdu_ranks";
            this.no_qiangdu_ranks.ReadOnly = true;
            // 
            // shigongbuwei
            // 
            this.shigongbuwei.DataPropertyName = "shigongbuwei";
            this.shigongbuwei.HeaderText = "施工部位";
            this.shigongbuwei.Name = "shigongbuwei";
            this.shigongbuwei.ReadOnly = true;
            // 
            // distance_car
            // 
            this.distance_car.DataPropertyName = "distance_car";
            this.distance_car.HeaderText = "运距";
            this.distance_car.Name = "distance_car";
            this.distance_car.ReadOnly = true;
            // 
            // 预排任务ToolStripMenuItem
            // 
            this.预排任务ToolStripMenuItem.Name = "预排任务ToolStripMenuItem";
            this.预排任务ToolStripMenuItem.Size = new System.Drawing.Size(127, 38);
            this.预排任务ToolStripMenuItem.Text = "预排任务";
            this.预排任务ToolStripMenuItem.Click += new System.EventHandler(this.预排任务ToolStripMenuItem_Click);
            // 
            // FrmTaskPush
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1951, 1205);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmTaskPush";
            this.Text = "任务推送界面";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.menuStrip3.ResumeLayout(false);
            this.menuStrip3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 开启自动排单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭自动排单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部刷新ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn carid;
        private System.Windows.Forms.DataGridViewTextBoxColumn intime;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 上移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下移ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 上移ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 下移ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 手动排任务ToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem 暂停ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 恢复ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_bill;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_bill;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_productorder;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_fahuolou;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_type_bengsong;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_qiangdu_ranks;
        private System.Windows.Forms.DataGridViewTextBoxColumn shigongbuwei;
        private System.Windows.Forms.DataGridViewTextBoxColumn distance_car;
        private System.Windows.Forms.ToolStripMenuItem 预排任务ToolStripMenuItem;
    }
}