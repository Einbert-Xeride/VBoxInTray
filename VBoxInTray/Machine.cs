using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBoxInTray
{
    class Machine
    {
        private VirtualBox.IMachine vm;

        public Machine(VirtualBox.IMachine vm)
        {
            if (vm == null) throw new ArgumentNullException();
            this.vm = vm;
        }

        #region Properties

        public string Name
        {
            get { return vm.Name; }
        }

        public VirtualBox.MachineState State
        {
            get { return vm.State; }
        }

        public bool IsOnline
        {
            get
            {
                return vm.State >= VirtualBox.MachineState.MachineState_FirstOnline
                    && vm.State <= VirtualBox.MachineState.MachineState_LastOnline;
            }
        }

        public bool IsTransient
        {
            get
            {
                return vm.State >= VirtualBox.MachineState.MachineState_FirstTransient
                    && vm.State <= VirtualBox.MachineState.MachineState_LastTransient;
            }
        }

        public bool CanPowerUp
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_PoweredOff
                    || vm.State == VirtualBox.MachineState.MachineState_Saved
                    || vm.State == VirtualBox.MachineState.MachineState_Aborted;
            }
        }

        public bool CanPowerDown
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Stuck
                    || vm.State == VirtualBox.MachineState.MachineState_Running
                    || vm.State == VirtualBox.MachineState.MachineState_Paused;
            }
        }

        public bool CanSaveState
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Running
                    || vm.State == VirtualBox.MachineState.MachineState_Paused;
            }
        }

        public bool CanPause
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Running;
            }
        }

        public bool CanResume
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Paused;
            }
        }

        public bool CanReset
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Running;
            }
        }

        public bool CanAcpiPower
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Running;
            }
        }

        public bool CanAcpiSleep
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Running;
            }
        }

        public bool CanShowSeparate
        {
            get
            {
                return vm.State == VirtualBox.MachineState.MachineState_Paused
                    || vm.State == VirtualBox.MachineState.MachineState_Running;
            }
        }

        #endregion

        #region Methods

        public VirtualBox.IProgress PowerUp()
        {
            if (!CanPowerUp) return null;
            VirtualBox.Session session = new VirtualBox.Session();
            return vm.LaunchVMProcess(session, "headless", "");
        }

        public VirtualBox.IProgress PowerDown()
        {
            if (!CanPowerDown) return null;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            VirtualBox.IProgress progress;
            try
            {
                progress = session.Console.PowerDown();
            }
            finally
            {
                session.UnlockMachine();
            }
            return progress;
        }

        public VirtualBox.IProgress SaveState()
        {
            if (!CanSaveState) return null;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            var progress = session.Machine.SaveState();
            session.UnlockMachine();
            return progress;
        }

        public void Pause()
        {
            if (!CanPause) return;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            try
            {
                session.Console.Pause();
            }
            finally
            {
                session.UnlockMachine();
            }
        }

        public void Resume()
        {
            if (!CanResume) return;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            try
            {
                session.Console.Resume();
            }
            finally
            {
                session.UnlockMachine();
            }
        }

        public void Reset()
        {
            if (!CanReset) return;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            try
            {
                session.Console.Reset();
            }
            finally
            {
                session.UnlockMachine();
            }
        }

        public void AcpiPower()
        {
            if (!CanAcpiPower) return;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            try
            {
                session.Console.PowerButton();
            }
            finally
            {
                session.UnlockMachine();
            }
        }

        public void AcpiSleep()
        {
            if (!CanAcpiSleep) return;
            VirtualBox.Session session = new VirtualBox.Session();
            vm.LockMachine(session, VirtualBox.LockType.LockType_Shared);
            try
            {
                session.Console.SleepButton();
            }
            finally
            {
                session.UnlockMachine();
            }
        }

        public static void LaunchVBoxManager()
        {
            string path = Utils.GetVirtualBoxInstallationPath();
            if (path == null) return;
            Process.Start(path + "VirtualBox.exe");
        }

        public VirtualBox.IProgress ShowSeparate()
        {
            if (!CanShowSeparate) return null;
            if (State != VirtualBox.MachineState.MachineState_Paused) Pause();
            VirtualBox.Session session = new VirtualBox.Session();
            var progress = vm.LaunchVMProcess(session, "separate", ""); // WARNING: undocumented feature
            progress.WaitForCompletion(-1);
            if (progress.Completed == WTypes.VARIANT_TRUE) Resume();
            return progress;
        }

        #endregion
    }
}
