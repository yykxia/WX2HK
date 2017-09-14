namespace PLM_OffLine
{
    partial class f_OffLine_dealList
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
            this.gv_receiveData = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gv_receiveData)).BeginInit();
            this.SuspendLayout();
            // 
            // gv_receiveData
            // 
            this.gv_receiveData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gv_receiveData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3});
            this.gv_receiveData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gv_receiveData.Location = new System.Drawing.Point(0, 0);
            this.gv_receiveData.Name = "gv_receiveData";
            this.gv_receiveData.RowTemplate.Height = 23;
            this.gv_receiveData.Size = new System.Drawing.Size(665, 469);
            this.gv_receiveData.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "条码号";
            this.Column1.Name = "Column1";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "读卡时间";
            this.Column3.Name = "Column3";
            // 
            // f_OffLine_dealList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 469);
            this.Controls.Add(this.gv_receiveData);
            this.Name = "f_OffLine_dealList";
            this.Text = "下架清单";
            ((System.ComponentModel.ISupportInitialize)(this.gv_receiveData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gv_receiveData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}