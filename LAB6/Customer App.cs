using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnOrder.Enabled = false;
            numTableID.Enabled = false;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang thử kết nối đến Server...", "Thông báo");
            lblStatus.Text = "Đã kết nối";
            lblStatus.ForeColor = Color.Green;
            btnOrder.Enabled = true;
            numTableID.Enabled = true;
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            int tableID = (int)numTableID.Value;
            MessageBox.Show($"Đã gửi yêu cầu đặt món cho bàn số {tableID}!", "Thành công");
        }
    }
}