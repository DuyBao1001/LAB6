namespace RestaurantSystem
{
    partial class StaffApp
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtTableID = new System.Windows.Forms.TextBox();
            this.btnCharge = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvOrders
            // 
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrders.Location = new System.Drawing.Point(12, 50);
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.RowHeadersWidth = 51;
            this.dgvOrders.Size = new System.Drawing.Size(560, 250);
            this.dgvOrders.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 30);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect to Server";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtTableID
            // 
            this.txtTableID.Location = new System.Drawing.Point(70, 317);
            this.txtTableID.Name = "txtTableID";
            this.txtTableID.Size = new System.Drawing.Size(60, 22);
            this.txtTableID.TabIndex = 2;
            // 
            // btnCharge
            // 
            this.btnCharge.Location = new System.Drawing.Point(150, 315);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(100, 25);
            this.btnCharge.TabIndex = 3;
            this.btnCharge.Text = "Tính tiền (PAY)";
            this.btnCharge.Click += new System.EventHandler(this.btnCharge_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.Red;
            this.lblTotal.Location = new System.Drawing.Point(360, 318);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(65, 24);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "0 VNĐ";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(472, 315);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 25);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Xuất Hóa Đơn";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Số bàn:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(280, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Tổng cộng:";
            // 
            // StaffApp
            // 
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.dgvOrders);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtTableID);
            this.Controls.Add(this.btnCharge);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "StaffApp";
            this.Text = "Ứng dụng Thu ngân (Staff Client)";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtTableID;
        private System.Windows.Forms.Button btnCharge;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}