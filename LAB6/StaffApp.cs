using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
                MessageBox.Show("Kết nối thành công!");
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
        }

        private void btnCharge_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTableID.Text)) return;
            writer.WriteLine($"PAY {txtTableID.Text}");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = $"bill_Ban{txtTableID.Text}.txt";
                string content = $"HOA DON THANH TOAN\nBan: {txtTableID.Text}\nTong tien: {lblTotal.Text}";
                File.WriteAllText(fileName, content);
                MessageBox.Show("Đã xuất hóa đơn ra file " + fileName);
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
            base.OnFormClosing(e);
        }
    }
}