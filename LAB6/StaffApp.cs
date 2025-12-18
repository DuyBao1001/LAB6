using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace RestaurantSystem
{
    public partial class StaffApp : Form
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private bool isRunning = false;

        public StaffApp()
        {
            InitializeComponent();
            InitializeDataGridView(); 
        }

        private void InitializeDataGridView()
        {
            dgvOrders.ColumnCount = 4;
            dgvOrders.Columns[0].Name = "Số bàn";
            dgvOrders.Columns[1].Name = "Tên món";
            dgvOrders.Columns[2].Name = "Số lượng";
            dgvOrders.Columns[3].Name = "Thành tiền";
            dgvOrders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient("127.0.0.1", 8888);
                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };

                writer.WriteLine("AUTH STAFF");

                isRunning = true;
                Thread listenThread = new Thread(ListenFromServer);
                listenThread.IsBackground = true;
                listenThread.Start();

                writer.WriteLine("GET_ORDERS");

                btnConnect.Enabled = false;
                MessageBox.Show("Kết nối Thu ngân thành công!");
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
                while (isRunning)
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
            if (msg.StartsWith("TOTAL"))
            {
                lblTotal.Text = msg.Split(' ')[1] + " VNĐ";
            }
            else if (msg == "UPDATE_REQUIRED")
            {
                writer.WriteLine("GET_ORDERS");
            }
            else if (!string.IsNullOrEmpty(msg) && msg.Contains(";"))
            {
                UpdateOrdersGrid(msg);
            }
        }

        private void UpdateOrdersGrid(string data)
        {
            dgvOrders.Rows.Clear();
            string[] orders = data.Split('|');
            foreach (string order in orders)
            {
                string[] info = order.Split(';');
                if (info.Length == 4)
                {
                    dgvOrders.Rows.Add(info[0], info[1], info[2], info[3]);
                }
            }
        }

        private void btnCharge_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTableID.Text))
            {
                MessageBox.Show("Vui lòng nhập số bàn cần thanh toán.");
                return;
            }
            writer.WriteLine($"PAY {txtTableID.Text}");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTableID.Text) || lblTotal.Text == "0 VNĐ") return;

                string fileName = $"bill_Ban{txtTableID.Text}.txt";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("--- HÓA ĐƠN THANH TOÁN ---");
                sb.AppendLine($"Thời gian: {DateTime.Now}");
                sb.AppendLine($"Số bàn: {txtTableID.Text}");
                sb.AppendLine("---------------------------");

                foreach (DataGridViewRow row in dgvOrders.Rows)
                {
                    if (row.Cells[0].Value?.ToString() == txtTableID.Text)
                    {
                        sb.AppendLine($"{row.Cells[1].Value} x{row.Cells[2].Value}: {row.Cells[3].Value} VNĐ");
                    }
                }

                sb.AppendLine("---------------------------");
                sb.AppendLine($"TỔNG CỘNG: {lblTotal.Text}");

                File.WriteAllText(fileName, sb.ToString()); 
                MessageBox.Show("Đã xuất hóa đơn thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (writer != null) writer.WriteLine("QUIT"); 
            isRunning = false;
            client?.Close();
            base.OnFormClosing(e);
        }
    }
}