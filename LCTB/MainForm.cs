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

namespace LCTB
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            DefineServersCombo();
        }

        private void DefineServersCombo()
        {
            Dictionary<string, string> Servers = new Dictionary<string, string>();
            Servers.Add("EUW1", "EU West");
            Servers.Add("RU", "Russia");
            Servers.Add("EUN1", "EU Nordic & East");
            Servers.Add("NA1", "North America");
            Servers.Add("BR1", "Brazil");
            Servers.Add("KR", "Korea");
            Servers.Add("JP1", "Japan");
            Servers.Add("LA1", "LA North");
            Servers.Add("LA2", "LA South");
            Servers.Add("OC1", "Oceania");
            Servers.Add("TR1", "Turkey");
            serverCombo.DataSource = new BindingSource(Servers, null);
            serverCombo.DisplayMember = "Value";
            serverCombo.ValueMember = "Key";
        }

        private void InputBrowseClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text files (*.txt)|*.txt";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    inputPathTb.Text = openFileDialog.FileName;
                }
            }
        }

        private void OutputBrowseClick(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                outputPathTb.Text = saveFileDialog.FileName;
            }

        }

        private async void ProcessButtonClick(object sender, EventArgs e)
        {
            var input = inputPathTb.Text;
            var output = outputPathTb.Text;
            var server = serverCombo.SelectedValue.ToString();
            Balancer balancer = new Balancer();
            balancer.ProgressUpdate += OnUpdate;

            foreach (Control cnt in this.Controls)
            {
                cnt.Enabled = false;
            }
            processBtn.Visible = false;
            infoLabel.Enabled = true;
            infoLabel.Visible = true;

            await Task.Run(() => balancer.Process(input, output, server));
            MessageBox.Show($"Output saved to {output}", "Info");
            infoLabel.Text = "";

            foreach (Control cnt in this.Controls)
            {
                cnt.Enabled = true;
            }
            processBtn.Visible = true;
            infoLabel.Visible = false;
            infoLabel.Enabled = false;
        }

        private void OnUpdate(string message)
        {
            if (infoLabel.InvokeRequired == true)
                infoLabel.Invoke((MethodInvoker)delegate { infoLabel.Text = message; });
            else
                infoLabel.Text = message;
        }
    }
}
