using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VBoxInTray
{
    static class Utils
    {
        private static string[] stateStrings = new string[]
        {
            "Null",
            "PoweredOff",
            "Saved",
            "Teleported",
            "Aborted",
            "Running",
            "Paused",
            "Stuck",
            "Teleporting",
            "LiveSnapshotting",
            "Starting",
            "Stopping",
            "Saving",
            "Restoring",
            "TeleportingPausedVM",
            "TeleportingIn",
            "FaultTolerantSyncing",
            "DeletingSnapshotOnline",
            "DeletingSnapshotPaused",
            "OnlineSnapshotting",
            "RestoringSnapshot",
            "DeletingSnapshot",
            "SettingUp",
            "Snapshotting"
        };

        public static string MachineStateToString(VirtualBox.MachineState state)
        {
            if (state >= VirtualBox.MachineState.MachineState_Null
                && state <= VirtualBox.MachineState.MachineState_LastTransient)
            {
                return stateStrings[(int)state];
            }
            else
            {
                return "Unknown";
            }
        }

        public static string GetVirtualBoxInstallationPath()
        {
            string keypath = @"SOFTWARE\Oracle\VirtualBox";
            string ret = null;
            if (Environment.Is64BitOperatingSystem)
            {
                var hklm32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                var hklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                var key = hklm64.OpenSubKey(keypath);
                if (key == null) key = hklm32.OpenSubKey(keypath);
                if (key == null) return null;
                ret = key.GetValue("InstallDir").ToString();
                key.Close();
                hklm32.Close();
                hklm64.Close();
            }
            else
            {
                var key = Registry.LocalMachine.OpenSubKey(keypath);
                if (key == null) return null;
                ret = key.GetValue("InstallDir").ToString();
                key.Close();
            }
            return ret;
        }

        public static bool AskFor(string question)
        {
            return MessageBox.Show(question, Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes;
        }

        public static string DateTimeShortRepr(DateTime dt)
        {
            return string.Format("{0:D4}{1:D2}{2:D2}-{3:D2}{4:D2}{5:D2}-{6:D4}", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond);
        }

        public static double SecondsSinceStartUp()
        {
            return (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
        }
    }
}
