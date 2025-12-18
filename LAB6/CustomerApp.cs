using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LAB6
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isConnected = false;

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
            try
            {
                client = new TcpClient("127.0.0.1", 8888); 
                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };

                writer.WriteLine("AUTH CUSTOMER"); 

                isConnected = true;

                Thread listenThread = new Thread(ListenFromServer);
                listenThread.IsBackground = true;
                listenThread.Start();

                writer.WriteLine("MENU");

                lblStatus.Text = "Đã kết nối";
                lblStatus.ForeColor = Color.Green;
                btnOrder.Enabled = true;
                numTableID.Enabled = true;
                btnConnect.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private void ListenFromServer()
        {
            try
            {
                while (isConnected)
                {
                    string response = reader.ReadLine();
                    if (response != null)
                    {
                        this.Invoke(new Action(() => ProcessServerMessage(response)));
                    }
                }
            }
            catch { }
        }

        private void ProcessServerMessage(string msg)
        {
            if (msg.StartsWith("OK")) 
            {
                this.Invoke(new Action(() => {
                    lblStatus.Text = "Đặt món thành công!";
                }));
            }
            else if (msg.Contains(";")) // Dữ liệu thực đơn
            {
                UpdateMenuGrid(msg);
            }
        }

        private void UpdateMenuGrid(string data)
        {
            dgvMenu.Rows.Clear();
            string[] items = data.Split('|');
            foreach (string item in items)
            {
                string[] info = item.Split(';');
                if (info.Length == 3)
                {

                    dgvMenu.Rows.Add(info[0], info[1], info[2], "0");
                }
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            if (!isConnected) return;
            int tableID = (int)numTableID.Value;

            foreach (DataGridViewRow row in dgvMenu.Rows)
            {
                if (row.Cells["colQuantity"].Value != null)
                {
                    int qty = 0;
                    int.TryParse(row.Cells["colQuantity"].Value.ToString(), out qty);
                    if (qty > 0)
                    {
                        string foodId = row.Cells["colID"].Value.ToString();
                        writer.WriteLine($"ORDER {tableID} {foodId} {qty}");
                    }
                }
            }
            MessageBox.Show("Đã gửi yêu cầu đặt món lên Server!", "Thông báo");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (writer != null) writer.WriteLine("QUIT"); 
                isConnected = false;
                client?.Close();
            }
            catch { }
            base.OnFormClosing(e);
        }
    }
}