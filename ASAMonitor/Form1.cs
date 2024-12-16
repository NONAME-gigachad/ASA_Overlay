using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Gma.System.MouseKeyHook;
using Guna.UI2.WinForms;
using System.Text.RegularExpressions;

namespace ASAMonitor
{
    public partial class Form1 : Form
    {
        private IKeyboardMouseEvents _hook;
        private Overlay overlay = new Overlay();
        private ColorDialog colorDialog;
        public static Color TextColor = Color.White;
        public Form1()
        {
            InitializeComponent();

            _hook = Hook.GlobalEvents();
            _hook.KeyDown += Hook_KeyDown;

            colorDialog = new ColorDialog();

        }
        public ListBox.ObjectCollection ListBoxItems
        {
            get { return listBox1.Items; }
        }
        public Guna2TextBox TextBoxWithTextSize
        {
            get { return guna2TextBox3; }
        }
        public Guna2CustomCheckBox CheckBoxUpload
        {
            get { return guna2CustomCheckBox1; }
        }
        public Guna2CustomCheckBox CheckBoxDays
        {
            get { return guna2CustomCheckBox2; }
        }
        public Guna2CustomCheckBox CheckBoxUploadInfo
        {
            get { return guna2CustomCheckBox3; }
        }
        public bool overlayenable = false;
        public bool overlayborder = false;
        private void Hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                if (!overlayenable)
                {
                    overlay.Addlistonoverlay(this);
                    overlay.Show();
                    overlayenable = true;
                }
                else
                {
                    overlay.Addlistonoverlay(this);
                    overlay.Hide();
                    overlayenable = false;
                }
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                if (!overlayborder)
                {
                    overlay.FormBorderStyle = FormBorderStyle.Sizable;
                    overlay.TransparencyKey = Color.Empty;
                    overlay.TopMost = false;
                    overlayborder = true;
                }
                else
                {
                    overlay.FormBorderStyle = FormBorderStyle.None;
                    overlay.TopMost = true;
                    overlay.TransparencyKey = System.Drawing.Color.Black;
                    overlayborder = false;
                }
            }
        }
        
        public bool OverlayStart = false;
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            var serverresult = await ServerParser.GetServerDetailsAsync(guna2TextBox1.Text);

            guna2TextBox1.Text = "";

            if (serverresult.Count == 1)
            {
                foreach (var server in serverresult)
                {

                    string serversting = $"{server.Item1} {server.Item2}/70";
                    

                    if (guna2CustomCheckBox1.Checked)
                    {
                        if (server.Item3 == 1)
                        {
                            serversting += " | Items: True";
                        }
                        else
                        {
                            serversting += " | Items: False";
                        }

                        if (server.Item4 == 1)
                        {
                            serversting += " | Chars: True";
                        }
                        else
                        {
                            serversting += " | Chars: False";
                        }
                    }
                    if (guna2CustomCheckBox2.Checked)
                    {
                        serversting += $" | Day: {server.Item5}";
                    }

                    listBox1.Items.Add(serversting);

                    if (!OverlayStart)
                    {
                        Task.Run(() => overlay.UpdateOverlay(this));
                        OverlayStart = true;
                    }

                    overlay.Addlistonoverlay(this);
                }
            }
            else if (serverresult.Count > 1)
            {
                MessageBox.Show("Found more than one ark server.\nPlease select ark server from list.");

                MoreThanOneServer moreThanOneServer = new MoreThanOneServer();
                moreThanOneServer.Show();

                StringBuilder sb = new StringBuilder();

                moreThanOneServer.TextBox1.Font = new Font("Courier New", 10);
                int maxNameLength = Math.Max("Name".Length, serverresult.Max(s => s.Item1.Length));
                int maxPlayersLength = Math.Max("Players".Length, serverresult.Max(s => s.Item2.ToString().Length));

                sb.AppendLine($"| {"Name".PadRight(maxNameLength)} | {"Players".PadRight(maxPlayersLength)} |");

                sb.AppendLine($"| {new string('-', maxNameLength)} | {new string('-', maxPlayersLength)} |");

                foreach (var server in serverresult)
                {
                    sb.AppendLine($"| {server.Item1.PadRight(maxNameLength)} | {server.Item2.ToString().PadRight(maxPlayersLength)} |");
                }

                if (serverresult.Count > 23)
                {
                    moreThanOneServer.TextBox1.ScrollBars = ScrollBars.Vertical;
                }
                moreThanOneServer.TextBox1.Text = sb.ToString();
            }
            else
            {
                MessageBox.Show("Server not found!");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (int.TryParse(guna2TextBox3.Text, out int fontSize))
            {
                //listBox1.Font = new Font("Courier New", fontSize);

                foreach(var label in Overlay.createdLabels)
                {
                    label.Font = new Font("Microsoft Sans Serif", fontSize);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (base.WindowState == FormWindowState.Normal)
            {
                base.WindowState = FormWindowState.Minimized;
            }
        }

        private void guna2Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            guna2Panel1.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void label5_MouseDown(object sender, MouseEventArgs e)
        {
            label5.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void guna2Panel5_MouseDown(object sender, MouseEventArgs e)
        {
            guna2Panel5.Capture = false;
            Message m = Message.Create(base.Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
            this.WndProc(ref m);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                
                string cleanedItem = Regex.Replace(listBox1.SelectedItem.ToString(), @"\s?\d+\/\d+.*", "").Trim();

                var label = Overlay.createdLabels.FirstOrDefault(l => l.Text.Contains(cleanedItem));

                listBox1.Items.Remove(listBox1.SelectedItem);

                if (label != null)
                {
                    label.Dispose();
                    // Обновляем визуальное представление
                    label.Invalidate();
                    label.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select server from list first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog.Color;

                TextColor = colorDialog.Color;

                listBox1.ForeColor = colorDialog.Color;
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }

    public class ServerParser
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<List<(string, int, int, int, string)>> GetServerDetailsAsync(string servernumber)
        {
            List<(string, int, int, int, string)> serverDetails = new List<(string, int, int, int, string)>();

            try
            {
                var response = await client.GetStringAsync("https://cdn2.arkdedicated.com/servers/asa/officialserverlist.json");

                JArray serversArray = JArray.Parse(response);

                foreach (var server in serversArray)
                {
                    string name = server["Name"].ToString();

                    if (name.Contains(servernumber))
                    {
                        serverDetails.Add((
                            name,
                            (int)server["NumPlayers"],
                            (int)server["AllowDownloadItems"],
                            (int)server["AllowDownloadChars"],
                            server["DayTime"].ToString()
                        ));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in Get Server Details: {ex.Message}");
            }

            return serverDetails;
        }
    }
}
