using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Helper;

namespace DS4_Emulator
{
    class Program
    {
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);

        static void Main(string[] args)
        {
            ControllerHelper cHelper = new ControllerHelper();
            cHelper.Start();

            Hide();
            Tray();
        }

        #region Methods
        private static void Hide()
        {
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(handle, 0);
        }
        private static void Quit(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        private static bool Menu()
        {
            Console.Clear();
            Console.Write("Welcome DS4 Emulator.\nPlease press «End» or ^C to quit.\n> ");

            return Console.ReadKey().Key.Equals(ConsoleKey.End);
        }
        private static void Tray()
        {
            NotifyIcon tIcon = new NotifyIcon();
            tIcon.Text = "DS4 Emulator";
            tIcon.Icon = new Icon(DS4_Emulator.Properties.Resources.AppLogo, 32, 32);

            ContextMenu cMenu = new ContextMenu();
            cMenu.MenuItems.Add("Quit");
            cMenu.MenuItems[0].Click += new EventHandler(Quit);

            tIcon.ContextMenu = cMenu;
            tIcon.Visible = true;
            Application.Run();
        }
        #endregion
    }
}
