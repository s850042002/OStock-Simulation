using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Threading;

namespace OStock_Simulation
{
    public partial class Form1 : Form
    {
        delegate void ShowInfoCallback(string strMsg, ListBox lstBox, int nMax);
        private ShowInfoCallback ShowInfo;

        public delegate void UI_ShowInfo(string strMsg, params object[] args);
        public static UI_ShowInfo g_UI_ShowInfo;

        private Config m_Config = null;
        private SimulateSystem m_SimulateSystem = null;

        public Form1()
        {
            InitializeComponent();
            ShowInfo = new ShowInfoCallback(ShowInfoProc);
            g_UI_ShowInfo = ShowInfoOnUI;

            dateTimePicker1.Value = dateTimePicker1.MinDate;
            toolTip1.SetToolTip(button1, "用於篩選多個Excel, 相同日期區間但停利停損不同");
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            m_Config = new Config();
            string sConfigPath = GetConfigPath();
            m_Config.Init(sConfigPath);

            m_SimulateSystem = new SimulateSystem(m_Config);
            await m_SimulateSystem.Init();
            
            button2.Enabled = true;            
            button4.Enabled = true;
        }

        private string GetConfigPath()
        {
            Process p = Process.GetCurrentProcess();

            string sXml = p.MainModule.ModuleName;
            sXml = sXml.Replace(".exe", ".xml");
            sXml = sXml.Replace(".vshost", "");
            string sConfigPath = Application.StartupPath + "\\" + sXml;

            return sConfigPath;
        }

        private void ShowInfoOnUI(string format, params object[] arg)
        {
            string sMsg = string.Format(format, arg);
            sMsg = string.Format("[{0:HH:mm:ss}] {1}", DateTime.Now, sMsg);
            ShowInfoProc(sMsg, listBox1, 1000);
        }

        private void ShowInfoProc(string strMsg, ListBox lstBox, int nMax)
        {
            lstBox.BeginInvoke((Action)delegate ()
            {
                lstBox.BeginUpdate();
                lstBox.Items.Add(strMsg);
                if (nMax > 0)
                {
                    while (lstBox.Items.Count > nMax)
                        lstBox.Items.RemoveAt(0);
                }

                // Scroll to end
                //
                lstBox.SelectedIndex = lstBox.Items.Count - 1;
                lstBox.EndUpdate();
            });
        }        

        private async void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Excel File|*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] sFileNames = openFileDialog1.FileNames;
                await m_SimulateSystem.FilterStrategy(sFileNames);                
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await m_SimulateSystem.StartSimulateAsync(checkBox3.Checked, dateTimePicker1.Value, dateTimePicker2.Value);
        }        

        private async void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "Excel File|*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string sFileName = openFileDialog1.FileName;
                await m_SimulateSystem.ApplyStrategy(sFileName, dateTimePicker1.Value, dateTimePicker2.Value, checkBox1.Checked, checkBox2.Checked);                
            }
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await m_SimulateSystem.SimulateOptions();
        }

        private void 重載設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Config.Init(GetConfigPath());
        }
    }
}
