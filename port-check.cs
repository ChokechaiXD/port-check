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

        List<object[]> data = new List<object[]>();
        Dictionary<int, CpuSample> cpuHistory = new Dictionary<int, CpuSample>();
        string settingsPath;
        int sortCol = -1;
        bool sortDesc;

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
            refreshTimer.Tick += (_, __) => { if (autoRefresh.Checked) RefreshData(); };
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
            portFilter.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { RefreshData(); e.SuppressKeyPress = true; } };

            var nameLabel = new Label { Text = "Name:", Location = new Point(100, 10), Size = new Size(38, 20) };
            nameFilter.Location = new Point(138, 8); nameFilter.Size = new Size(100, 22);
            nameFilter.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { RefreshData(); e.SuppressKeyPress = true; } };

            var refreshButton = new Button { Text = "Refresh", Location = new Point(250, 6), Size = new Size(80, 25) };
            refreshButton.Click += (_, __) => RefreshData();

            var killButton = new Button { Text = "Kill", Location = new Point(340, 6), Size = new Size(70, 25) };
            killButton.BackColor = Color.IndianRed; killButton.ForeColor = Color.White;
            killButton.Click += (_, __) => KillSelected();

            autoRefresh.Text = "Auto 5s"; autoRefresh.Location = new Point(430, 8); autoRefresh.Size = new Size(80, 20);
            autoRefresh.Checked = true;

            var exportButton = new Button { Text = "Export", Location = new Point(515, 6), Size = new Size(70, 25) };
            exportButton.Click += (_, __) => ExportCsv();

            statusBar.Location = new Point(595, 10); statusBar.Size = new Size(550, 20);
            statusBar.ForeColor = Color.Gray;

            toolbar.Controls.AddRange(new Control[] {
                portLabel, portFilter, nameLabel, nameFilter,
                refreshButton, killButton, autoRefresh, exportButton, statusBar
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

            // Virtual Mode = no cell objects for off-screen rows
            table.VirtualMode = true;
            table.CellValueNeeded += (_, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < data.Count)
                    e.Value = data[e.RowIndex][e.ColumnIndex];
            };

            table.CellFormatting += (_, e) =>
            {
                if (e.Value == null) { e.Value = "-"; e.FormattingApplied = true; return; }
                if ((e.ColumnIndex == 6 || e.ColumnIndex == 7) && e.Value is double)
                {
                    e.Value = ((double)e.Value).ToString("F1");
                    e.FormattingApplied = true;
                }
            };

            var ctxMenu = new ContextMenuStrip();
            ctxMenu.Items.Add("Kill", null, (_, __) => KillSelected());
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Copy Port", null, (_, __) => CopyCell(0));
            ctxMenu.Items.Add("Copy PID", null, (_, __) => CopyCell(1));
            ctxMenu.Items.Add("Copy Name", null, (_, __) => CopyCell(2));
            ctxMenu.Items.Add("Copy Remote", null, (_, __) => CopyCell(5));
            ctxMenu.Items.Add("Copy Path", null, (_, __) => CopyCell(8));
            ctxMenu.Items.Add("-");
            ctxMenu.Items.Add("Copy Row", null, (_, __) => CopyRow());
            table.ContextMenuStrip = ctxMenu;

            table.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.K) KillSelected();
                if (e.Control && e.KeyCode == Keys.C) CopyRow();
            };
            table.CellMouseDoubleClick += (_, e) => { if (e.RowIndex >= 0) KillSelected(); };

            table.ColumnHeaderMouseClick += (_, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var menu = new ContextMenuStrip();
                    foreach (DataGridViewColumn col in table.Columns)
                    {
                        var item = new ToolStripMenuItem(col.HeaderText) { Checked = col.Visible };
                        item.Click += (__, ___) => col.Visible = !col.Visible;
                        menu.Items.Add(item);
                    }
                    menu.Show(table, e.Location);
                }
                else if (e.Button == MouseButtons.Left && data.Count > 0)
                {
                    if (sortCol == e.ColumnIndex)
                        sortDesc = !sortDesc;
                    else { sortCol = e.ColumnIndex; sortDesc = false; }
                    data.Sort((a, b) =>
                    {
                        int c = CompareObj(a[sortCol], b[sortCol]);
                        return sortDesc ? -c : c;
                    });
                    table.Refresh();
                }
            };

            Controls.Add(toolbar);
            Controls.Add(table);
        }

        static int CompareObj(object a, object b)
        {
            if (a == null && b == null) return 0;
            if (a == null) return -1;
            if (b == null) return 1;
            if (a is int && b is int) return ((int)a).CompareTo((int)b);
            if (a is double && b is double) return ((double)a).CompareTo((double)b);
            return string.Compare(a.ToString(), b.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        void SaveSettings()
        {
            try
            {
                var dir = Path.GetDirectoryName(settingsPath);
                Directory.CreateDirectory(dir);
                File.WriteAllLines(settingsPath, new string[] {
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
                    var p = line.Split('=');
                    if (p.Length != 2) continue;
                    if (p[0] == "Port") { portFilter.Text = p[1]; continue; }
                    if (p[0] == "Name") { nameFilter.Text = p[1]; continue; }
                    int v;
                    if (int.TryParse(p[1], out v))
                    {
                        if (p[0] == "Width") Width = Math.Max(800, v);
                        if (p[0] == "Height") Height = Math.Max(400, v);
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
            if (table.SelectedRows.Count == 0) { statusBar.Text = "Select a process first."; return; }
            int rowIdx = table.SelectedRows[0].Index;
            if (rowIdx < 0 || rowIdx >= data.Count) return;
            object[] row = data[rowIdx];
            int pid = (int)row[1];
            string name = (string)row[2];
            int port = (int)row[0];
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
                string hint = "";
                string lowerMsg = ex.Message.ToLowerInvariant();
                if (lowerMsg.Contains("access")) hint = "\n\nTip: Run the app as Administrator for this process.";
                else if (lowerMsg.Contains("already exited") || lowerMsg.Contains("not found"))
                    hint = "\n\nNote: Process already exited.";

                MessageBox.Show("Failed to kill " + name + " (PID:" + pid + ")." + hint,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void CopyCell(int col)
        {
            if (table.SelectedRows.Count == 0) return;
            int i = table.SelectedRows[0].Index;
            if (i >= 0 && i < data.Count)
            {
                object v = data[i][col];
                Clipboard.SetText(v != null ? v.ToString() : "-");
            }
        }

        void CopyRow()
        {
            if (table.SelectedRows.Count == 0) return;
            int i = table.SelectedRows[0].Index;
            if (i < 0 || i >= data.Count) return;
            var sb = new StringBuilder();
            foreach (object v in data[i])
                sb.Append(v != null ? v.ToString() : "-").Append("\t");
            Clipboard.SetText(sb.ToString().TrimEnd('\t'));
        }

        void ExportCsv()
        {
            if (data.Count == 0) { statusBar.Text = "Nothing to export."; return; }
            var dlg = new SaveFileDialog { Filter = "CSV files|*.csv", FileName = "ports.csv" };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            var sb = new StringBuilder();
            sb.AppendLine("Port,PID,Name,Proto,State,Remote,MemMB,CpuPct,Path");
            foreach (var row in data)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    if (i > 0) sb.Append(',');
                    object v = row[i];
                    string s = v != null ? v.ToString() : "";
                    if (s.Contains(",") || s.Contains("\""))
                        s = "\"" + s.Replace("\"", "\"\"") + "\"";
                    sb.Append(s);
                }
                sb.AppendLine();
            }
            File.WriteAllText(dlg.FileName, sb.ToString());
            statusBar.Text = "Exported " + data.Count + " rows to " + Path.GetFileName(dlg.FileName);
        }

        void RefreshData()
        {
            var connections = FetchConnections();
            var procCache = new Dictionary<int, Process>();
            bool pathVis = table.Columns["Path"].Visible;

            int fp;
            int.TryParse(portFilter.Text, out fp);
            bool filtPort = portFilter.Text.Length > 0;
            string filtName = nameFilter.Text.Trim().ToLowerInvariant();
            bool filtNameOn = filtName.Length > 0;

            var newData = new List<object[]>();
            int connCount = 0;
            double totalMem = 0;
            var pids = new HashSet<int>();

            foreach (var c in connections)
            {
                if (filtPort && c.Port != fp) continue;

                Process proc;
                if (!procCache.TryGetValue(c.Pid, out proc))
                {
                    try { proc = Process.GetProcessById(c.Pid); } catch { proc = null; }
                    procCache[c.Pid] = proc;
                }

                string nm = "-";
                object mem = null;
                object cpu = null;
                string path = "-";

                if (proc != null)
                {
                    nm = proc.ProcessName;
                    if (filtNameOn && !proc.ProcessName.ToLowerInvariant().Contains(filtName)) continue;

                    try { double mb = proc.WorkingSet64 / 1048576.0; mem = Math.Round(mb, 1); totalMem += mb; } catch { }
                    double? cpuPct = GetCpuUsage(c.Pid, proc);
                    if (cpuPct.HasValue) cpu = cpuPct.Value;
                    if (pathVis) { try { path = proc.MainModule.FileName; } catch { } }
                    pids.Add(c.Pid);
                }
                else if (filtNameOn) continue;

                connCount++;
                newData.Add(new object[] { c.Port, c.Pid, nm, c.Protocol, c.State, c.RemoteAddress, mem, cpu, path });
            }

            // Dispose process handles
            foreach (var p in procCache.Values)
                if (p != null) p.Dispose();

            data = newData;
            sortCol = -1;
            table.RowCount = data.Count;
            table.Refresh();

            statusBar.Text = connCount + " connections  |  "
                + pids.Count + " processes  |  "
                + (int)totalMem + " MB  |  "
                + DateTime.Now.ToString("HH:mm:ss");
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
                    double elapsed = (now - prev.Time).TotalSeconds;
                    double used = (total - prev.TotalTime).TotalSeconds;
                    if (elapsed > 0.001)
                    {
                        cpuHistory[pid] = new CpuSample { TotalTime = total, Time = now };
                        return Math.Round(used / elapsed / Environment.ProcessorCount * 100, 1);
                    }
                }
                cpuHistory[pid] = new CpuSample { TotalTime = total, Time = now };
            }
            catch { }
            return null;
        }

        static List<Connection> FetchConnections()
        {
            var r = new List<Connection>();
            try
            {
                var psi = new ProcessStartInfo("netstat", "-ano")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };
                using (var p = Process.Start(psi))
                {
                    string o = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                    foreach (var raw in o.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string t = raw.Trim().ToLowerInvariant();
                        if (!t.StartsWith("tcp") && !t.StartsWith("udp")) continue;
                        var parts = t.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length < 4) continue;
                        string proto = parts[0].ToUpper();
                        string local = parts[1], remote = parts[2];
                        string state = "";
                        int pid = 0;
                        if (proto == "TCP") { state = parts[3].ToUpper(); int.TryParse(parts[parts.Length - 1], out pid); }
                        else { state = "UDP"; int.TryParse(parts[parts.Length - 1], out pid); }
                        int port;
                        int.TryParse(local.Substring(local.LastIndexOf(':') + 1), out port);
                        if (port > 0 && pid > 0)
                            r.Add(new Connection { Port = port, Pid = pid, State = state, Protocol = proto, RemoteAddress = remote.ToUpper() });
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine("netstat: " + ex.Message); }
            return r;
        }
    }
}
