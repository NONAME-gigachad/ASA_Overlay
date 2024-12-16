using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static TheArtOfDevHtmlRenderer.Adapters.RGraphicsPath;

namespace ASAMonitor
{
    public partial class Overlay : Form
    {
        private Form1 mainForm;
        public Overlay()
        {
            InitializeComponent();
        }


        public async Task UpdateOverlay(Form1 form1)
        {
            mainForm = form1;
            while (true) 
            {
                try
                {
                    foreach (var item in mainForm.ListBoxItems)
                    {
                        string cleanedItem = Regex.Replace(item.ToString(), @"\s?\d+\/\d+.*", "").Trim();

                        var serverresult = await ServerParser.GetServerDetailsAsync(cleanedItem);

                        if (serverresult.Count == 1)
                        {
                            foreach (var server in serverresult)
                            {
                                string serversting = $"{server.Item1} {server.Item2}/70";

                                if (mainForm.CheckBoxUpload.Checked)
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

                                if (mainForm.CheckBoxDays.Checked)
                                {
                                    serversting += $" | Day: {server.Item5}";
                                }

                                EditLabels(serversting, mainForm);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Overlay Crash! {ex.Message}");
                }



            await Task.Delay(15000);
            }


        }
        private void EditLabels(string newname, Form1 mainForm)
        {
            try
            {
                mainForm.Invoke(new Action(() =>
                {
                    string cleanedItem = Regex.Replace(newname.ToString(), @"\s?\d+\/\d+.*", "").Trim();

                    var label = createdLabels.FirstOrDefault(l => l.Text.Contains(cleanedItem));

                    if (label != null)
                    {
                        // Изменяем текст метки
                        label.Text = newname;

                        label.ForeColor = Form1.TextColor;
                        // Обновляем визуальное представление
                        label.Invalidate();
                        label.Refresh();
                    }
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Overlay Crash! {ex.Message}");
            }

            

        }
        public static List<System.Windows.Forms.Label> createdLabels = new List<System.Windows.Forms.Label>();
        public void Addlistonoverlay(Form1 form1)
        {
            try
            {
                mainForm = form1;

                // Обновляем текст существующих меток
                int yPosition = 10;
                int FontSize = int.Parse(mainForm.TextBoxWithTextSize.Text);

                // Перебираем все элементы в ListBoxItems
                foreach (var item in mainForm.ListBoxItems)
                {
                    // Ищем метку по текущему тексту
                    var label = createdLabels.FirstOrDefault(l => l.Text == item.ToString());

                    if (label != null)
                    {
                        // Если метка найдена, обновляем ее текст
                        label.Text = item.ToString();
                    }
                    else
                    {
                        // Если метки нет, создаем новую
                        label = new System.Windows.Forms.Label
                        {
                            ForeColor = Form1.TextColor,
                            Font = new Font("Microsoft Sans Serif", FontSize),
                            Text = item.ToString(),
                            Location = new System.Drawing.Point(10, yPosition),
                            AutoSize = true
                        };
                        this.Controls.Add(label); // Добавляем новую метку на форму
                        createdLabels.Add(label); // Добавляем в список
                    }

                    yPosition += 30; // Смещаем по вертикали для следующей метки
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Add list on overlay Crash! {ex.Message}");
            }
            
        }

    }
}
