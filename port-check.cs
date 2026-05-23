using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PortChecker
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new App(args));
        }
    }

    struct Connection
    {
        public int Port;
        public int Pid;
        public string State;
        public string Protocol;
        public string RemoteAddress;
    }

    struct CpuSample
    {
        public TimeSpan TotalTime;
        public DateTime Time;
    }

    class App : Form
    {
        DataGridView table = new DataGridView();
        TextBox portFilter = new TextBox();
        TextBox nameFilter = new TextBox();
        Label statusBar = new Label();
        CheckBox autoRefresh = new CheckBox();
        Timer refreshTimer = new Timer();

        Dictionary<int, CpuSample> cpuHistory = new Dictionary<int, CpuSample>();
        Dictionary<int, string> nameCache = new Dictionary<int, string>();
        string settingsPath;

        public App(string[] args)
        {
            settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "PortChecker", "settings.cfg");

            BuildForm();
            LoadSettings();

            if (args.Length > 0)
            {
                int portArg;
                if (int.TryParse(args[0], out portArg))
                    portFilter.Text = args[0];
            }

            refreshTimer.Interval = 5000;
            refreshTimer.Tick += (senderT, argsT) => { if (autoRefresh.Checked) RefreshData(); };
            refreshTimer.Start();

            RefreshData();
        }

        void BuildForm()
        {
            Text = "Port Checker";
            Size = new Size(1200, 600);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9.5f);
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            var toolbar = new Panel { Size = new Size(1160, 40), Location = new Point(15, 10) };

            var portLabel = new Label { Text = "Port:", Location = new Point(5, 10), Size = new Size(30, 20) };
            portFilter.Location = new Point(34, 8); portFilter.Size = new Size(60, 22);
            portFilter.KeyDown += (senderK, eK) =>
            {
                if (eK.KeyCode == Keys.Enter) { RefreshData(); eK.SuppressKeyPress = true; }
            };

            var nameLabel = new Label { Text = "Name:", Location = new Point(100, 10), Size = new Size(38, 20) };
            nameFilter.Location = new Point(138, 8); nameFilter.Size = new Size(100, 22);
            nameFilter.KeyDown += (senderK, eK) =>
            {
                if (eK.KeyCode == Keys.Enter) { RefreshData(); eK.SuppressKeyPress = true; }
            };

            var refreshButton = new Button { Text = "Refresh", Location = new Point(250, 6), Size = new Size(80, 25) };
            refreshButton.Click += (senderB, argsB) => RefreshData();

            var killButton = new Button { Text = "Kill", Location = new Point(340, 6), Size = new Size(70, 25) };
            killButton.BackColor = Color.IndianRed; killButton.ForeColor = Color.White;
            killButton.Click += (senderK, argsK) => KillSelected();

            autoRefresh.Text = "Auto 5s"; autoRefresh.Location = new Point(430, 8); autoRefresh.Size = new Size(80, 20);
            autoRefresh.Checked = true;

            statusBar.Location = new Point(520, 10); statusBar.Size = new Size(620, 20);
            statusBar.ForeColor = Color.Gray;

            toolbar.Controls.AddRange(new Control[] {
                portLabel, portFilter, nameLabel, nameFilter,
                refreshButton, killButton, autoRefresh, statusBar
            });

            table.Location = new Point(15, 55); table.Size = new Size(1160, 490);
            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            table.ReadOnly = true; table.AllowUserToAddRows = false;
            table.RowHeadersVisible = false;
            table.BackgroundColor = Color.White; table.BorderStyle = BorderStyle.Fixed3D;
            table.RowTemplate.Height = 28;
            table.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(240, 240, 240),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };
            table.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(247, 247, 248)
            };

            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, table, new object[] { true });

            table.Columns.Add("Port", "Port");
            table.Columns.Add("PID", "PID");
            table.Columns.Add("Name", "Name");
            table.Columns.Add("Proto", "Proto");
            table.Columns.Add("State", "State");
            table.Columns.Add("Remote", "Remote");
            table.Columns.Add("MemMB", "Mem(MB)");
            table.Columns.Add("CpuPct", "CPU%");
            table.Columns.Add("Path", "Path");

            table.Columns["Port"].Width = 50; table.Columns["PID"].Width = 50;
            table.Columns["Name"].Width = 100; table.Columns["Proto"].Width = 50;
            table.Columns["State"].Width = 60; table.Columns["Remote"].Width = 130;
            table.Columns["MemMB"].Width = 75; table.Columns["CpuPct"].Width = 60;

            table.CellFormatting += (senderF, eF) =>
            {
                if (eF.Value == null) { eF.Value = "-"; eF.FormattingApplied = true; return; }
                if ((eF.ColumnIndex == 6 || eF.ColumnIndex == 7) && eF.Value is double)
                {
                    double d = (double)eF.Value;
                    eF.Value = d.ToString("F1");
                    eF.FormattingApplied = true;
                }
            };

            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Kill", null, (senderC, argsC) => KillSelected());
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Copy Port", null, (senderC, argsC) => CopyCell("Port"));
            ctxMenu.Items.Add("Copy PID", null, (senderC, argsC) => CopyCell("PID"));
            ctxMenu.Items.Add("Copy Name", null, (senderC, argsC) => CopyCell("Name"));
            ctxMenu.Items.Add("Copy Remote", null, (senderC, argsC) => CopyCell("Remote"));
            ctxMenu.Items.Add("Copy Path", null, (senderC, argsC) => CopyCell("Path"));
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Copy Row", null, (senderC, argsC) => CopyRow());
            table.ContextMenuStrip = ctxMenu;

            table.KeyDown += (senderK, eK) =>
            {
                if (eK.KeyCode == Keys.Delete || eK.KeyCode == Keys.K) KillSelected();
                if (eK.Control && eK.KeyCode == Keys.C) CopyRow();
            };
            table.CellMouseDoubleClick += (senderD, eD) => { if (eD.RowIndex >= 0) KillSelected(); };

            table.ColumnHeaderMouseClick += (senderH, eH) =>
            {
                if (eH.Button != MouseButtons.Right) return;
                var menu = new ContextMenuStrip();
                foreach (DataGridViewColumn col in table.Columns)
                {
                    string colText = col.HeaderText;
                    bool colVisible = col.Visible;
                    var item = new ToolStripMenuItem(colText) { Checked = colVisible };
                    item.Click += (senderI, argsI) => col.Visible = !col.Visible;
                    menu.Items.Add(item);
                }
                menu.Show(table, eH.Location);
            };

            Controls.Add(toolbar);
            Controls.Add(table);
        }

        void SaveSettings()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(settingsPath));
                File.WriteAllLines(settingsPath, new string[]
                {
                    "Port=" + portFilter.Text,
                    "Name=" + nameFilter.Text,
                    "Width=" + Width,
                    "Height=" + Height
                });
            }
            catch { }
        }

        void LoadSettings()
        {
            try
            {
                if (!File.Exists(settingsPath)) return;
                foreach (var line in File.ReadAllLines(settingsPath))
                {
                    var parts = line.Split('=');
                    if (parts.Length != 2) continue;
                    if (parts[0] == "Port") { portFilter.Text = parts[1]; continue; }
                    if (parts[0] == "Name") { nameFilter.Text = parts[1]; continue; }
                    int v;
                    if (int.TryParse(parts[1], out v))
                    {
                        if (parts[0] == "Width") Width = Math.Max(800, v);
                        if (parts[0] == "Height") Height = Math.Max(400, v);
                    }
                }
            }
            catch { }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
            base.OnFormClosing(e);
        }

        void KillSelected()
        {
            if (table.SelectedRows.Count == 0)
            { statusBar.Text = "Select a process first."; return; }

            var row = table.SelectedRows[0];
            int pid = (int)row.Cells["PID"].Value;
            string name = (string)row.Cells["Name"].Value;
            int port = (int)row.Cells["Port"].Value;

            string msg = "Kill " + name + " (PID:" + pid + ") on port " + port + "?";
            if (MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                Process.GetProcessById(pid).Kill();
                statusBar.Text = "Killed " + name;
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void CopyCell(string columnName)
        {
            if (table.SelectedRows.Count == 0) return;
            var val = table.SelectedRows[0].Cells[columnName].Value;
            if (val != null) Clipboard.SetText(val.ToString());
        }

        void CopyRow()
        {
            if (table.SelectedRows.Count == 0) return;
            var row = table.SelectedRows[0];
            var sb = new StringBuilder();
            foreach (DataGridViewCell cell in row.Cells)
            {
                object val = cell.Value;
                sb.Append(val != null ? val.ToString() : "-").Append("\t");
            }
            Clipboard.SetText(sb.ToString().TrimEnd('\t'));
        }

        void RefreshData()
        {
            var connections = FetchConnections();
            var processCache = new Dictionary<int, Process>();
            var currentKeys = new HashSet<string>();
            var existing = new Dictionary<string, int>();

            for (int i = 0; i < table.Rows.Count; i++)
                if (table.Rows[i].Cells["Port"].Value != null)
                    existing[RowKey(table.Rows[i])] = i;

            bool pathVisible = table.Columns["Path"].Visible;

            int filterPort;
            int.TryParse(portFilter.Text, out filterPort);
            bool hasPortFilter = portFilter.Text.Length > 0;
            string filterName = nameFilter.Text.Trim().ToLowerInvariant();
            bool hasNameFilter = filterName.Length > 0;

            int connectionCount = 0;
            double totalMemoryMb = 0;
            var processIds = new HashSet<int>();

            foreach (var conn in connections)
            {
                if (hasPortFilter && conn.Port != filterPort) continue;

                string key = conn.Port + ":" + conn.Pid + ":" + conn.Protocol;
                currentKeys.Add(key);

                Process process;
                if (!processCache.TryGetValue(conn.Pid, out process))
                {
                    try { process = Process.GetProcessById(conn.Pid); }
                    catch { process = null; }
                    processCache[conn.Pid] = process;
                }

                string procName = "-";
                object memValue = null;
                object cpuValue = null;
                string procPath = "-";

                if (process != null)
                {
                    procName = process.ProcessName;
                    if (hasNameFilter && !process.ProcessName.ToLowerInvariant().Contains(filterName))
                    { currentKeys.Remove(key); continue; }

                    try
                    {
                        double mb = process.WorkingSet64 / 1048576.0;
                        memValue = Math.Round(mb, 1);
                        totalMemoryMb += mb;
                    }
                    catch { }

                    double? cpuPct = GetCpuUsage(conn.Pid, process);
                    if (cpuPct.HasValue)
                        cpuValue = cpuPct.Value;

                    if (pathVisible)
                    {
                        try { procPath = process.MainModule.FileName; }
                        catch { }
                    }

                    processIds.Add(conn.Pid);
                }
                else if (hasNameFilter)
                { currentKeys.Remove(key); continue; }

                connectionCount++;

                int rowIndex;
                if (existing.TryGetValue(key, out rowIndex))
                {
                    var row = table.Rows[rowIndex];
                    row.Cells["Name"].Value = procName;
                    row.Cells["State"].Value = conn.State;
                    row.Cells["Remote"].Value = conn.RemoteAddress;
                    row.Cells["MemMB"].Value = memValue;
                    row.Cells["CpuPct"].Value = cpuValue;
                    row.Cells["Path"].Value = procPath;
                }
                else
                {
                    table.Rows.Add(conn.Port, conn.Pid, procName, conn.Protocol,
                                   conn.State, conn.RemoteAddress, memValue, cpuValue, procPath);
                }
            }

            for (int i = table.Rows.Count - 1; i >= 0; i--)
                if (table.Rows[i].Cells["Port"].Value != null
                    && !currentKeys.Contains(RowKey(table.Rows[i])))
                    table.Rows.RemoveAt(i);

            statusBar.Text = connectionCount + " connections  |  "
                + processIds.Count + " processes  |  "
                + (int)totalMemoryMb + " MB  |  "
                + DateTime.Now.ToString("HH:mm:ss");
        }

        string RowKey(DataGridViewRow row)
        {
            return row.Cells["Port"].Value + ":" + row.Cells["PID"].Value + ":" + row.Cells["Proto"].Value;
        }

        double? GetCpuUsage(int pid, Process process)
        {
            try
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan total = process.TotalProcessorTime;
                CpuSample prev;
                if (cpuHistory.TryGetValue(pid, out prev))
                {
                    double elapsedSec = (now - prev.Time).TotalSeconds;
                    double usedSec = (total - prev.TotalTime).TotalSeconds;
                    if (elapsedSec > 0.001)
                    {
                        cpuHistory[pid] = new CpuSample { TotalTime = total, Time = now };
                        return Math.Round(usedSec / elapsedSec / Environment.ProcessorCount * 100, 1);
                    }
                }
                cpuHistory[pid] = new CpuSample { TotalTime = total, Time = now };
            }
            catch { }
            return null;
        }

        static List<Connection> FetchConnections()
        {
            var result = new List<Connection>();
            try
            {
                var psi = new ProcessStartInfo("netstat", "-ano")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };
                using (var proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();

                    foreach (var rawLine in output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string line = rawLine.Trim().ToLowerInvariant();
                        if (!line.StartsWith("tcp") && !line.StartsWith("udp")) continue;

                        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < 4) continue;

                        string protocol = parts[0].ToUpper();
                        string localAddr = parts[1];
                        string remoteAddr = parts[2];
                        string state = "";
                        int pid = 0;

                        if (protocol == "TCP")
                        {
                            state = parts[3].ToUpper();
                            int.TryParse(parts[parts.Length - 1], out pid);
                        }
                        else
                        {
                            state = "UDP";
                            int.TryParse(parts[parts.Length - 1], out pid);
                        }

                        int colonIdx = localAddr.LastIndexOf(':');
                        int port;
                        int.TryParse(localAddr.Substring(colonIdx + 1), out port);

                        if (port > 0 && pid > 0)
                            result.Add(new Connection
                            {
                                Port = port,
                                Pid = pid,
                                State = state,
                                Protocol = protocol,
                                RemoteAddress = remoteAddr.ToUpper()
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("netstat error: " + ex.Message);
            }
            return result;
        }
    }
}
