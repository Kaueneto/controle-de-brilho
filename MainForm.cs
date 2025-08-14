using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenFilterWinForms
{
    public partial class MainForm : Form
    {
        private OverlayLayered ovBrilho;

        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;

        // Low-level keyboard hook (PrintScreen)
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static IntPtr _hookID = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc;

        public MainForm()
        {
            InitializeComponent();
            ConfigurarControles();
            ConectarEventos();
            ConfigurarTray();
            IniciarHookPrintScreen();

    
            this.Hide();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true; // Sobre todas as janelas
        }

        private void ConfigurarControles()
        {
            numericIntensidadeBrilho.Minimum = 0;
            numericIntensidadeBrilho.Maximum = 100;
            numericIntensidadeBrilho.Value = 50;

            TrackBrilhoControle.Minimum = 0;
            TrackBrilhoControle.Maximum = 100;
            TrackBrilhoControle.Value = 50;
        }

        private void ConectarEventos()
        {
            checkboxOnOff.CheckedChanged += (s, e) => AlternarOverlay(checkboxOnOff.Checked);

            TrackBrilhoControle.Scroll += (s, e) =>
            {
                numericIntensidadeBrilho.Value = TrackBrilhoControle.Value;
                ovBrilho?.SetOpacity(TrackBrilhoControle.Value / 100.0);
            };

            numericIntensidadeBrilho.ValueChanged += (s, e) =>
            {
                TrackBrilhoControle.Value = (int)numericIntensidadeBrilho.Value;
                ovBrilho?.SetOpacity(TrackBrilhoControle.Value / 100.0);
            };

            btFechar.Click += (s, e) => SairAplicacao(s, e);
        }

        private void AlternarOverlay(bool ligado)
        {
            if (ligado)
            {
                if (ovBrilho == null || ovBrilho.IsDisposed)
                    ovBrilho = new OverlayLayered(Color.Black, TrackBrilhoControle.Value / 100.0);

                ovBrilho.SetOpacity(TrackBrilhoControle.Value / 100.0);
                ovBrilho.ShowOverlay();
            }
            else
            {
                if (ovBrilho != null)
                {
                    ovBrilho.HideOverlay();
                    ovBrilho.Dispose();
                    ovBrilho = null;
                }
            }
        }
        private void ConfigurarTray()
        {
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Abrir", null, AbrirJanela);
            trayMenu.Items.Add("Sair", null, SairAplicacao);

            trayIcon = new NotifyIcon();
            trayIcon.Text = "Screen Filter";


            trayIcon.Icon = Properties.Resources.TrayIcon;
            trayIcon.Visible = true;
            this.Icon = Properties.Resources.TrayIcon;

            trayIcon.ContextMenuStrip = trayMenu;


            trayIcon.DoubleClick += (s, e) => AbrirJanela(s, e);
        }

        private void AbrirJanela(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        // Fecha a aplicação
        private void SairAplicacao(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

  
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }

            trayIcon.Visible = false;
            base.OnFormClosing(e);
         

        }



        private void IniciarHookPrintScreen()
        {
            _proc = HookCallback;
            _hookID = SetHook(_proc);
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    if (vkCode == (int)Keys.PrintScreen)
                    {
                        OcultarParaScreenshot().ConfigureAwait(false);
                    }
                }
            }
            catch { }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private async Task OcultarParaScreenshot()
        {
            bool ligado = checkboxOnOff.Checked;

            ovBrilho?.HideOverlay();
            await Task.Delay(600);

            if (ligado)
                ovBrilho?.ShowOverlay();
        }

 

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private void MainForm_Load_1(object sender, EventArgs e)
        {

        }

        private void btFechar_Click(object sender, EventArgs e)
        {

        }
    }
}
