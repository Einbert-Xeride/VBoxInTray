using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VBoxInTray
{
    static class Program
    {
        private static VirtualBox.IVirtualBox vbox = null;
        public static VirtualBox.IVirtualBox VBox
        {
            get { return vbox; }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += unhandledException;

            string optMachineName = null;
            bool optPowerUp = true;

            try
            {
                foreach (var kvp in GetOpt.Getopt(Environment.GetCommandLineArgs(), "m:sh"))
                {
                    if (kvp.Key == 'm') optMachineName = kvp.Value;
                    if (kvp.Key == 's') optPowerUp = false;
                    if (kvp.Key == 'h')
                    {
                        MessageBox.Show("-m MachineName: Launch machine MachineName.\n" +
                                        "-s: Don't power up immediately.\n" +
                                        "-h: Show this help.");
                        Application.Exit();
                        Environment.Exit(0);
                    }
                }
            }
            catch (GetOpt.GetoptException ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
                Environment.Exit(1);
            }

            vbox = new VirtualBox.VirtualBox();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                VirtualBox.IMachine vm = null;

                if (optMachineName == null)
                {
                    var formSelectMachine = new FormSelectMachine(optPowerUp);
                    if (formSelectMachine.ShowDialog() != DialogResult.OK)
                    {
                        Application.Exit();
                        return;
                    }
                    vm = formSelectMachine.SelectedVm;
                    optPowerUp = formSelectMachine.ShouldPowerOn;
                }
                else
                {
                    try
                    {
                        vm = vbox.FindMachine(optMachineName);
                    }
                    catch (COMException ex)
                    {
                        MessageBox.Show(ex.Message);
                        Application.Exit();
                        return;
                    }
                }

                var log = new VboxLogWatcher(vm);
                Logging.Instance.AddWatcher(log);

                Application.Run(new FormMain(vm, optPowerUp));
            }
            finally
            {
                Marshal.ReleaseComObject(vbox);
                vbox = null;
            }
        }

        private static void unhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Logging.Instance.WatcherCount != 0)
            {
                Logging.Instance.Fatal("unhandled", e.ExceptionObject.ToString());
            }
            else
            {
                MessageBox.Show(e.ExceptionObject.ToString(), "Internal Error");
            }
        }
    }
}
