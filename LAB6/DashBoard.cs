using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestaurantSystem;

namespace LAB6
{
    public partial class DashBoard: Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            ServerForm f = new ServerForm();
            f.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show(); ;
        }

        private void btnCashier_Click(object sender, EventArgs e)
        {
            StaffApp f = new StaffApp();
            f.Show();
        }
    }
}
