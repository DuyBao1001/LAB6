using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace LAB6
{
    public partial class ServerForm : Form
    {
        private TcpListener listener;
        private Thread listenThread;
        private bool isRunning = false;

        private List<MenuItem> menuList = new List<MenuItem>();
        private List<OrderItem> orderList = new List<OrderItem>();

        private List<StreamWriter> staffWriters = new List<StreamWriter>();

        public ServerForm()
        {
            InitializeComponent();
            LoadMenu(); 
        }

        private void LoadMenu()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Menu.txt");
                if (File.Exists(path))
                {
                    string[] lines = File.ReadAllLines(path);
                    foreach (string line in lines)
                    {
                        var parts = line.Split(';');
                        if (parts.Length == 3)
                        {
                            menuList.Add(new MenuItem
                            {
                                Id = int.Parse(parts[0].Trim()),
                                Name = parts[1].Trim(),
                                Price = int.Parse(parts[2].Trim())
                            });
                        }
                    }
                    AppendLog("Hệ thống: Đã tải " + menuList.Count + " món ăn từ Menu.txt.");
                }
            }
            catch (Exception ex)
            {
                AppendLog("Lỗi load menu: " + ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                listener = new TcpListener(IPAddress.Any, 8888);
                listener.Start();
                isRunning = true;

                listenThread = new Thread(ListenForClients);
                listenThread.IsBackground = true;
                listenThread.Start();

                lblStatus.Text = "Đang lắng nghe ";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                btnStart.Enabled = false;
                AppendLog("Server: Bắt đầu lắng nghe kết nối...");
            }
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClient));
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                }
                catch { break; }
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            string clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            try
            {
                while (isRunning)
                {
                    string request = reader.ReadLine();
                    if (string.IsNullOrEmpty(request)) break;

                    AppendLog($"[{clientIP}]: {request}");

                    string[] parts = request.Split(' ');
                    string cmd = parts[0].ToUpper();

                    switch (cmd)
                    {
                        case "AUTH": 
                        if (parts.Length > 1 && parts[1] == "STAFF")
                            lock (staffWriters) { staffWriters.Add(writer); }
                        break;

                        case "MENU":
                            string menuData = string.Join("|", menuList.Select(m => $"{m.Id};{m.Name};{m.Price}"));
                            writer.WriteLine(menuData);
                            break;

                        case "ORDER": 
                                ProcessOrder(parts, writer);
                                break;

                        case "GET_ORDERS": 
                                    SendAllOrders(writer);
                                    break;

                        case "PAY": 
                                ProcessPayment(parts, writer);
                                break;

                        case "QUIT": 
                                goto EndConnection;
                    }
                }
            }
            catch { }

        EndConnection:
            lock (staffWriters) { staffWriters.Remove(writer); }
            client.Close();
            AppendLog($"Server: Client {clientIP} đã thoát.");
        }

        private void ProcessOrder(string[] parts, StreamWriter writer)
        {
            if (parts.Length < 4) return;
            int tableId = int.Parse(parts[1]);
            int foodId = int.Parse(parts[2]);
            int qty = int.Parse(parts[3]);

            var item = menuList.FirstOrDefault(m => m.Id == foodId);
            if (item != null)
            {
                lock (orderList)
                {
                    orderList.Add(new OrderItem
                    {
                        TableId = tableId,
                        FoodId = foodId,
                        FoodName = item.Name,
                        Quantity = qty,
                        Price = item.Price * qty
                    });
                }
                writer.WriteLine("OK " + (item.Price * qty));
                NotifyStaff(); 
            }
        }

        private void ProcessPayment(string[] parts, StreamWriter writer)
        {
            if (parts.Length < 2) return;
            int tableId = int.Parse(parts[1]);
            int total = 0;

            lock (orderList)
            {
                var tableOrders = orderList.Where(o => o.TableId == tableId).ToList();
                total = tableOrders.Sum(o => o.Price);
                orderList.RemoveAll(o => o.TableId == tableId); 
            }
            writer.WriteLine($"TOTAL {total}");
            NotifyStaff();
        }

        private void SendAllOrders(StreamWriter writer)
        {
            lock (orderList)
            {
                string data = string.Join("|", orderList.Select(o => $"{o.TableId};{o.FoodName};{o.Quantity};{o.Price}"));
                writer.WriteLine(data);
            }
        }

        private void NotifyStaff() 
        {
            lock (staffWriters)
            {
                foreach (var sw in staffWriters)
                {
                    try { sw.WriteLine("UPDATE_REQUIRED"); } catch { }
                }
            }
        }

        private void AppendLog(string message)
        {
            if (rtbLog.InvokeRequired)
            {
                rtbLog.Invoke(new Action(() => AppendLog(message)));
                return;
            }
            rtbLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}\n");
            rtbLog.ScrollToCaret();
        }
    }

    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }

    public class OrderItem
    {
        public int TableId { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}