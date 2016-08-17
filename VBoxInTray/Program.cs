using System;
using System.Collections.Generic;
using System.Linq;
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

            vbox = new VirtualBox.VirtualBox();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var formSelectMachine = new FormSelectMachine();
                if (formSelectMachine.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }

                var log = new VboxLogWatcher(formSelectMachine.SelectedVm);
                Logging.Instance.AddWatcher(log);

                Application.Run(new FormMain(formSelectMachine.SelectedVm, formSelectMachine.ShouldPowerOn));
            }
            finally
            {
                vbox = null;
            }
        }

        private static void unhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Logging.Instance.Fatal("unhandled", e.ExceptionObject.ToString());
        }
    }
}
