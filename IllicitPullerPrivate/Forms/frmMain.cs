using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS3Lib;

namespace IllicitPullerPrivate.Forms
{
    using System.Threading;
    using static Classes.Connection;
    using static Classes.GameDetector;
    using static Classes.Geolocator;
    using static Classes.Grabber;
    using static Classes.ServerReader;
    using static Classes.Spoofer;
    using static Classes.NameChanger;
    public partial class frmMain : Form
    {
        public static bool LogsOpened = false;
        BackgroundWorker GameInfoWorker;
        public static Thread GrabOnce;
        public static int x = 0, y = 0;
        public static Point newpoint = new Point();
        const int WS_MINIMIZEBOX = 0x20000, CS_DBLCLKS = 0x8;
        public frmMain()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            GameInfoWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
            GameInfoWorker.DoWork += GameInfoWorker_DoWork;
        }

        private void Puller_Load(object sender, EventArgs e)
        {
            DataClients.RowCount = 22;
            if (!GameInfoWorker.IsBusy)
            {
                GameInfoWorker.RunWorkerAsync();
            }
            InitializeName();
        }

        #region "Form Controls"
        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void panelTitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            x = MousePosition.X - Location.X;
            y = MousePosition.Y - Location.Y;
        }

        private void panelTitleBar_MouseMove(object sender, MouseEventArgs e)
        {

            switch (e.Button)
            {
                case MouseButtons.Left: MouseMover(); break;
                case MouseButtons.Right: MouseMover(); break;
            }
        }

        private void MouseMover()
        {
            newpoint = MousePosition;
            newpoint.X -= x;
            newpoint.Y -= y;
            Location = newpoint;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= WS_MINIMIZEBOX;
                cp.ClassStyle |= CS_DBLCLKS;
                return cp;
            }
        }
        #endregion

        private void btnCCAPI_Click(object sender, EventArgs e)
        {
            CFW.ChangeAPI(SelectAPI.ControlConsole);
        }

        private void btnTMAPI_Click(object sender, EventArgs e)
        {
            CFW.ChangeAPI(SelectAPI.TargetManager);
        }

        private void btnConnectAttach_Click(object sender, EventArgs e)
        {
            lblTitle.Text = $"Exception >> {Connect()}";
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            CFW.DisconnectTarget();
            lblTitle.Text = "Exception >> Disconnected";
        }

        private void lblTitle_TextChanged(object sender, EventArgs e)
        {
            IsConnected = lblTitle.Text == "Exception >> Connected";
            if (IsConnected)
            {
                DetectGame();
            }
        }

        private void SetGameInfo(string Game, string ClientsGrabbed, string Host, string Map, string Gamemode)
        {
            GameInfoLabel.Text = $"Game: {Game}\nClients Grabbed: {ClientsGrabbed}\nHost: {Host}\nMap: {Map}\nGamemode: {Gamemode}";
        }

        private void GameInfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Connect();
            for (; ; )
            {
                if (IsConnected)
                {
                    if (GameSelected != Games.NONE)
                    {
                        string Grabbed = "0/0";
                        if (ClientsGrabbed < 18)
                        {
                            Grabbed = $"{ClientsGrabbed}/{MaxClients}";
                        }
                        ReadServerInfo();
                        SetGameInfo($"{GameSelected.ToString()}", $"{Grabbed}", $"{Hostname}", $"{Mapname}", $"{Gamemode}");
                    }
                    else
                    {
                        SetGameInfo("None", "0/0", "N/A", "N/A", "N/A");
                        DetectGame();
                    }
                }
                Thread.Sleep(250);
            }
        }

        private void btnRefreshClients_Click(object sender, EventArgs e)
        {
            DataClients.Rows.Clear();
            DataClients.RowCount = 22;
            if (IsConnected)
            {
                GrabOnce = new Thread(() => { Connect(); GrabClientsOnce(DataClients); }) { IsBackground = true };
                GrabOnce.Start();
            }
        }

        private string CopyFunction(int column)
        {
            try
            {
                return DataClients[column, DataClients.CurrentRow.Index].Value.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Must have a client selected.", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                string ClipboardText = Clipboard.GetText();
                return string.IsNullOrEmpty(ClipboardText) ? string.Empty : ClipboardText;
            }
        }
        private void CopyPrestigeItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(0));
        private void CopyRankItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(1));
        private void CopyClantagItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(2));
        private void CopyGamertagItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(3));
        private void CopyExternalItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(4));
        private void CopyInternalItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(5));
        private void CopyPortItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(6));
        private void CopyNpidItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(7));
        private void CopyMicItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(8));
        private void CopyNatTypeItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(9));
        private void CopyXuidItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(10));
        private void CopyPartyIdItem_Click(object sender, EventArgs e) => Clipboard.SetText(CopyFunction(11));

        #region Spoofing
        private void btnSpoof_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP($"{txtSpoofIP.Text}", $"{txtSpoofPort.Text}");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSpoofNASA_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP("198.119.56.43", "5123");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btSpoofFBI_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP("153.31.113.27", "37625");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btSpoofPentagon_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP("141.116.212.32", "2537");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSpoofWhiteHouse_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP("156.33.241.5", "16824");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSpoofMicrosoft_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP("131.107.0.89", "2374");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSpoofNFO_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    SpoofIP("74.91.113.57", "63846");
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnResetSpoof_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                if (SetSpoofAddrs())
                {
                    ResetSpoofIP();
                }
                else
                {
                    MessageBox.Show("Failed to find spoofer address!\nTry restarting your game.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        private void DataClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string clantag = DataClients[2, DataClients.CurrentRow.Index].Value.ToString();
                string gamertag = DataClients[3, DataClients.CurrentRow.Index].Value.ToString();
                string client = clantag == "N/A" ? gamertag == "N/A" ? "None" : gamertag : $"[{clantag}] {gamertag}";
                switch (label18.Text.Contains(client))
                {
                    case false:
                        label18.Text = "Selected User: " + client;
                        string ipTrack = DataClients[4, DataClients.CurrentRow.Index].Value.ToString();
                        string[] resp = GetTrackerDetails(ipTrack).Split('\n');
                        txtExternal.Text = string.IsNullOrEmpty(resp[0]) == true ? "Not Available" : resp[0];
                        txtHostname.Text = string.IsNullOrEmpty(resp[1]) == true ? "Not Available" : resp[1];
                        txtCountry.Text = string.IsNullOrEmpty(resp[2]) == true ? "Not Available" : resp[2];
                        txtState.Text = string.IsNullOrEmpty(resp[3]) == true ? "Not Available" : resp[3];
                        txtCity.Text = string.IsNullOrEmpty(resp[4]) == true ? "Not Available" : resp[4];
                        txtZip.Text = string.IsNullOrEmpty(resp[5]) == true ? "Not Available" : resp[5];
                        txtISP.Text = string.IsNullOrEmpty(resp[6]) == true ? "Not Available" : resp[6];
                        txtProtected.Text = VpnDetect(ipTrack);
                        resp = null;
                        break;
                }
            }
            catch (Exception)
            {
                label18.Text = "Selected User: None";
                txtExternal.Text = "";
                txtHostname.Text = "";
                txtCountry.Text = "";
                txtState.Text = "";
                txtCity.Text = "";
                txtZip.Text = "";
                txtISP.Text = "";
                txtProtected.Text = "";
            }
        }

        private void txtNameChanger_TextChanged(object sender, EventArgs e)
        {
            CurrentGT = txtNameChanger.Text;
        }

        private void btnSetName_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                SetGamertag(CurrentGT);
            }
        }

        private void btnResetName_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                GrabOriginalGamertag();
                SetGamertag(ResetGamertag);
            }
        }

        private void btnOpenLogs_Click(object sender, EventArgs e)
        {
            if (LogsOpened == false)
            {
                new frmLogs().Show();
                return;
            }
            MessageBox.Show("Logs are already shown!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnAutoName_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                switch (btnAutoName.Text)
                {
                    case "Auto Update: OFF":
                        btnAutoName.Text = "Auto Update: ON";
                        AutoGT = true;
                        break;
                    case "Auto Update: ON":
                        btnAutoName.Text = "Auto Update: OFF";
                        AutoGT = false;
                        break;
                }
            }
        }

        private void btnFlashName_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                switch (btnFlashName.Text)
                {
                    case "Flash Name: OFF":
                        btnFlashName.Text = "Flash Name: ON";
                        FlashGT = true;
                        break;
                    case "Flash Name: ON":
                        btnFlashName.Text = "Flash Name: OFF";
                        FlashGT = false;
                        break;
                }
            }
        }

    }
}
