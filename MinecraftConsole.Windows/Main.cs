using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinecraftConsole.Windows
{
    public partial class Main : Form
    {
        private string ip = "3.139.107.252";
        public Main()
        {
            InitializeComponent();
        }

        private void Connect()
        {
            MineStat ms = new MineStat(ip, 25565);
            labelServerName.Text = ms.Motd;
            labelServerVersion.Text = ms.Version;
            labelIp.Text = ip;
            if (ms.ServerUp)
            {
                checkBoxServerOnline.Checked = true;
                checkBoxServerOnline.ForeColor = Color.Blue;               
            }
            else
            {
                checkBoxServerOnline.Checked = false;
                checkBoxServerOnline.ForeColor = Color.Red;
            }

            labelPlayers.Text = $"{ms.CurrentPlayers} de {ms.MaximumPlayers}";
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Connect();
            //ServerPing.Excecute(ip);
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
           // Connect();
        }
    }
}
