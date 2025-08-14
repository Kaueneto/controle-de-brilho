using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ScreenFilterWinForms
{
  
    public class OverlayLayered : Form
    {
        private Color _color = Color.Black;
        private double _opacity = 0.5;

        public OverlayLayered(Color baseColor, double opacity)
        {
            _color = baseColor;
            _opacity = opacity;

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.TopMost = true;

      
            Rectangle vb = SystemInformation.VirtualScreen;
            this.Location = new Point(vb.Left, vb.Top);
            this.Size = vb.Size;

 
            this.Visible = false;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_LAYERED = 0x00080000;
                const int WS_EX_TRANSPARENT = 0x00000020;
                const int WS_EX_TOOLWINDOW = 0x00000080;
                const int WS_EX_NOACTIVATE = 0x08000000;

                var cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
                return cp;
            }
        }

        protected override bool ShowWithoutActivation => true;

        public void SetColor(Color c)
        {
            _color = c;
            Redraw();
        }

        public void SetOpacity(double value)
        {
            _opacity = Math.Max(0, Math.Min(1, value));
            Redraw();
        }

        public void ShowOverlay()
        {
      
            Rectangle vb = SystemInformation.VirtualScreen;
            this.Location = new Point(vb.Left, vb.Top);
            this.Size = vb.Size;

            if (!this.Visible) this.Show();
            Redraw();
        }

        public void HideOverlay()
        {
            if (this.Visible) this.Hide();
        }

        private void Redraw()
        {
            if (!this.IsHandleCreated) this.CreateHandle();

  
            Rectangle vb = SystemInformation.VirtualScreen;
            int w = Math.Max(1, vb.Width);
            int h = Math.Max(1, vb.Height);

            using (var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb))
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                using (var br = new SolidBrush(Color.FromArgb((int)(_opacity * 255), _color)))
                {
                    g.FillRectangle(br, new Rectangle(0, 0, w, h));
                }

                IntPtr hBitmap = bmp.GetHbitmap(Color.FromArgb(0));
                try
                {
                    IntPtr screenDC = GetDC(IntPtr.Zero);
                    IntPtr memDC = CreateCompatibleDC(screenDC);
                    IntPtr oldBmp = SelectObject(memDC, hBitmap);

                    try
                    {
                        POINT dstPt = new POINT { x = vb.Left, y = vb.Top };
                        SIZE size = new SIZE { cx = w, cy = h };
                        POINT srcPt = new POINT { x = 0, y = 0 };

                        BLENDFUNCTION blend = new BLENDFUNCTION
                        {
                            BlendOp = AC_SRC_OVER,
                            BlendFlags = 0,
                            SourceConstantAlpha = 255,  
                            AlphaFormat = AC_SRC_ALPHA  
                        };

                        UpdateLayeredWindow(this.Handle, screenDC, ref dstPt, ref size, memDC, ref srcPt, 0, ref blend, ULW_ALPHA);
                    }
                    finally
                    {
                        SelectObject(memDC, oldBmp);
                        DeleteDC(memDC);
                        ReleaseDC(IntPtr.Zero, screenDC);
                    }
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }
        }

        #region WinAPI

        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;
        private const int ULW_ALPHA = 0x02;

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT { public int x; public int y; }

        [StructLayout(LayoutKind.Sequential)]
        private struct SIZE { public int cx; public int cy; }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool UpdateLayeredWindow(IntPtr hWnd, IntPtr hdcDst, ref POINT pptDst, ref SIZE psize,
            IntPtr hdcSrc, ref POINT pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion
    }
}
