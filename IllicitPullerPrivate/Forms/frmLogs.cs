using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IllicitPullerPrivate.Forms
{
    using static frmMain;
    using IllicitPullerPrivate.Classes;
    using static Classes.LogsLib;
    using System.Runtime.InteropServices;
    using static Classes.Connection;
    using static Classes.NameChanger;
    using static Classes.GameDetector;

    public partial class frmLogs : Form
    {
        public frmLogs()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            LogsOpened = false;
            Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void frmLogs_Load(object sender, EventArgs e)
        {
            LogsOpened = true;
            RefreshLogs();
            if (IsConnected)
            {
                GrabOriginalGamertag();
                lblDefaultName.Text = ResetGamertag;
                lblDefaultXUID.Text = GenerateXUID(ResetGamertag.ToLower()).ToString("X");
            }
        }

        private void btnRefreshClients_Click(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void RefreshLogs()
        {
            ReadLogs();
            DataLogs.Rows.Clear();
            if (LogsInfoGrabbed.Count != 0)
            {
                DataLogs.RowCount = LogsInfoGrabbed.Count;
                for (int i = 0; i < DataLogs.RowCount; i++)
                {
                    Application.DoEvents();
                    DataLogs[0, i].Value = LogsInfoGrabbed[i].Gamertag;
                    DataLogs[1, i].Value = LogsInfoGrabbed[i].ExternalIP;
                    DataLogs[2, i].Value = LogsInfoGrabbed[i].XUID;
                }
            }
            LoggedClientsLabel.Text = $"Logged Clients: {LogsInfoGrabbed.Count}";
        }

        private void txtResolvePSN_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ReadLogs();
                string Search = txtResolvePSN.Text;
                BindingList<LogsInfo> SearchedLogsList = new BindingList<LogsInfo>();
                foreach (LogsInfo item in LogsInfoGrabbed)
                {
                    Application.DoEvents();
                    var props = item.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        Application.DoEvents();
                        if (Convert.ToString(prop.GetValue(item, null)).ToLower().Contains(Search.ToLower()))
                        {
                            SearchedLogsList.Add(item);
                            break;
                        }
                    }
                }
                DataLogs.Rows.Clear();
                if (Search == "")
                {
                    DataLogs.RowCount = LogsInfoGrabbed.Count;
                    for (int i = 0; i < DataLogs.RowCount; i++)
                    {
                        Application.DoEvents();
                        DataLogs[0, i].Value = LogsInfoGrabbed[i].Gamertag;
                        DataLogs[1, i].Value = LogsInfoGrabbed[i].ExternalIP;
                        DataLogs[2, i].Value = LogsInfoGrabbed[i].XUID;
                    }
                }
                else
                {
                    DataLogs.RowCount = SearchedLogsList.Count;
                    for (int i = 0; i < DataLogs.RowCount; i++)
                    {
                        Application.DoEvents();
                        DataLogs[0, i].Value = SearchedLogsList[i].Gamertag;
                        DataLogs[1, i].Value = SearchedLogsList[i].ExternalIP;
                        DataLogs[2, i].Value = SearchedLogsList[i].XUID;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void frmLogs_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogsOpened = false;
        }

        private void DataLogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string Gamertag = DataLogs[0, DataLogs.CurrentRow.Index].Value.ToString();
                string Xuid = DataLogs[2, DataLogs.CurrentRow.Index].Value.ToString();
                txtSpoofName.Text = Gamertag;
                txtSpoofXUID.Text = Xuid;
            }
            catch (Exception)
            {
                txtSpoofName.Text = string.Empty;
                txtSpoofXUID.Text = string.Empty;
            }
        }

        [DllImport("XUID.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong GenerateXUID(string text);

        private void btnSpoofNameXUID_Click(object sender, EventArgs e)
        {
            if (IsConnected && GameSelected != Games.Ghosts && GameSelected != Games.AW && GameSelected != Games.GTA && GameSelected != Games.NONE)
            {
                SetGamertag(txtSpoofName.Text);
                SetXuid(txtSpoofXUID.Text);
            }
        }

        private void btnResetNameXUID_Click(object sender, EventArgs e)
        {
            if (IsConnected && GameSelected != Games.Ghosts && GameSelected != Games.AW && GameSelected != Games.GTA && GameSelected != Games.NONE)
            {
                GrabOriginalGamertag();
                SetGamertag(ResetGamertag);
                SetXuid(GenerateXUID(ResetGamertag.ToLower()).ToString("X"));
            }
        }
    }
}
