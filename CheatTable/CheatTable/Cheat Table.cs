using Memory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheatTable
{
    public partial class Form1 : Form
    {

        Mem m = new Mem();
        int PID = 0;

        List<string> dataCheat = new List<string>();

        public Form1()
        {
            InitializeComponent();
            Task.Run(() => {
                if (PID == 0)
                {
                    Process p;
                    while (true)
                    {
                        if (Process.GetProcessesByName(processName.Text.ToLower().Replace(".exe", "")).Length > 0)
                        {
                            p = Process.GetProcessesByName(processName.Text.ToLower().Replace(".exe", ""))[0];
                            PID = p.Id;
                            m.OpenProcess(PID);
                            processName.Text += " Attached To " + PID;
                            break;
                        }
                        Thread.Sleep(100);
                    }
                }
            });

            dataCheat = File.ReadAllLines("cheat.ini").ToList();

            foreach (var item in dataCheat)
            {
                var tmp = item.Split('#');
                dataGridView1.Rows.Add(tmp);
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (PID != 0)
            {
                try
                {
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        Console.WriteLine(item.Cells["Address"].Value.ToString());
                        if (item.Cells["Address"].Value != null && item.Cells["Type"].Value != null && item.Cells["Value"].Value != null)
                        {
                            m.WriteMemory(item.Cells["Address"].Value.ToString(), item.Cells["Type"].Value.ToString(), item.Cells["Value"].Value.ToString());
                        }
                    }
                }
                catch { }
            }
        }
    }
}
