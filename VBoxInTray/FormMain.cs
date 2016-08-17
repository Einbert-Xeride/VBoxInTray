﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace VBoxInTray
{
    public partial class FormMain : Form
    {
        private delegate void machineStateChangedHandler(VirtualBox.MachineState newState);
        private event machineStateChangedHandler machineStateChanged;
        private Machine machine;
        private DispatcherTimer checkTimer;
        private VirtualBox.MachineState oldState = VirtualBox.MachineState.MachineState_Null;

        public FormMain(VirtualBox.IMachine vm, bool powerUp = false)
        {
            machine = new Machine(vm);
            Icon = Properties.Resources.vboxintray;
            InitializeComponent();
            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItemR("Power up", menuPowerUp_Click, () => machine.CanPowerUp),
                new ToolStripMenuItemR("Save state", menuSaveState_Click, () => machine.CanSaveState),
                new ToolStripMenuItemR("Pause", menuPause_Click, () => machine.CanPause),
                new ToolStripMenuItemR("Resume", menuResume_Click, () => machine.CanResume),
                new ToolStripMenuItemR("ACPI Power", menuAcpiPower_Click, () => machine.CanAcpiPower),
                new ToolStripMenuItemR("ACPI Sleep", menuAcpiSleep_Click, () => machine.CanAcpiSleep),
                new ToolStripMenuItemR("Reset", menuReset_Click, () => machine.CanReset),
                new ToolStripMenuItemR("Power down", menuPowerDown_Click, () => machine.CanPowerDown),
                new ToolStripSeparator(),
                new ToolStripMenuItemR("Show", menuShowSeparate_Click, () => machine.CanShowSeparate),
                new ToolStripMenuItemR("Launch Manager", menuLaunchManager_Click, () => true),
                new ToolStripSeparator(),
                new ToolStripMenuItemR("Exit now", menuExitNow_Click, () => true),
                new ToolStripMenuItemR("Exit", menuExit_Click, () => machine.CanSaveState),
            });
            notifyIcon.Icon = Icon;

            machineStateChanged += notifyIconTextUpdator;
            machineStateChanged += updateMenuItemsUsability;
            machineStateChanged += updateNotifyIcon;
            machineStateChanged += notifyForStateChangement;

            checkTimer = new DispatcherTimer();
            checkTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            checkTimer.Tick += checkTimer_Tick;
            checkTimer.Start();

            if (powerUp)
            {
                machine.PowerUp();
            }
        }

        // Never show this form.
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            this.Visible = false;
        }

        private void checkTimer_Tick(object sender, EventArgs e)
        {
            var newState = machine.State;
            if (newState == oldState) return;
            oldState = newState;
            machineStateChanged(newState);
        }

        private void notifyIconTextUpdator(VirtualBox.MachineState newState)
        {
            notifyIcon.Text = string.Format("{0} - {1}", machine.Name, Utils.MachineStateToString(newState));
        }

        private void updateMenuItemsUsability(VirtualBox.MachineState newState)
        {
            foreach (var item in contextMenuStrip.Items)
            {
                var mitem = item as ToolStripMenuItemR;
                if (mitem == null) continue;
                mitem.CheckUsability();
            }
        }

        private void updateNotifyIcon(VirtualBox.MachineState newState)
        {
            if (newState == VirtualBox.MachineState.MachineState_Running)
            {
                notifyIcon.Icon = Properties.Resources.vbox_run;
            }
            else if (newState == VirtualBox.MachineState.MachineState_Saved)
            {
                notifyIcon.Icon = Properties.Resources.vbox_saved;
            }
            else if (newState == VirtualBox.MachineState.MachineState_PoweredOff)
            {
                notifyIcon.Icon = Properties.Resources.vbox_stop;
            }
            else if (newState == VirtualBox.MachineState.MachineState_Paused)
            {
                notifyIcon.Icon = Properties.Resources.vbox_pause;
            }
            else if (newState == VirtualBox.MachineState.MachineState_Aborted)
            {
                notifyIcon.Icon = Properties.Resources.vbox_abort;
            }
            else if (newState >= VirtualBox.MachineState.MachineState_FirstTransient
                && newState <= VirtualBox.MachineState.MachineState_LastTransient)
            {
                notifyIcon.Icon = Properties.Resources.vbox_trans;
            }
            else
            {
                notifyIcon.Icon = Icon;
            }
        }

        private void notifyForStateChangement(VirtualBox.MachineState newState)
        {
            notifyIcon.BalloonTipText = Utils.MachineStateToString(newState);
            notifyIcon.BalloonTipTitle = machine.Name;
            notifyIcon.ShowBalloonTip(1000);
        }

        private void notifyForFailedOperations(string toDoWhat, int errCode)
        {
            notifyIcon.BalloonTipText = string.Format("Failed to {0}: 0x{1:X8}.", toDoWhat, errCode);
            notifyIcon.BalloonTipTitle = machine.Name;
            notifyIcon.ShowBalloonTip(1000);
        }

        private void tryToDoAndCatchCOMExcept(Action action, string toDoWhat)
        {
            try
            {
                action();
            }
            catch (COMException ex)
            {
                notifyForFailedOperations(toDoWhat, ex.ErrorCode);
            }
        }

        private void menuPowerUp_Click(object sender, EventArgs e)
        {
            tryToDoAndCatchCOMExcept(() => machine.PowerUp(), "power up");
        }

        private void menuPowerDown_Click(object sender, EventArgs e)
        {
            if (!Utils.AskFor(string.Format("Really power down {0}?", machine.Name))) return;
            tryToDoAndCatchCOMExcept(() => machine.PowerDown(), "power down");
        }

        private void menuSaveState_Click(object sender, EventArgs e)
        {
            tryToDoAndCatchCOMExcept(() => machine.SaveState(), "save state");
        }

        private void menuPause_Click(object sender, EventArgs e)
        {
            tryToDoAndCatchCOMExcept(() => machine.Pause(), "pause");
        }

        private void menuResume_Click(object sender, EventArgs e)
        {
            tryToDoAndCatchCOMExcept(() => machine.Resume(), "resume");
        }

        private void menuReset_Click(object sender, EventArgs e)
        {
            if (!Utils.AskFor(string.Format("Really reset {0}?", machine.Name))) return;
            tryToDoAndCatchCOMExcept(() => machine.Reset(), "reset");
        }

        private void menuAcpiPower_Click(object sender, EventArgs e)
        {
            if (!Utils.AskFor(string.Format("Really press power button of {0}?", machine.Name))) return;
            tryToDoAndCatchCOMExcept(() => machine.AcpiPower(), "send ACPI power button event");
        }

        private void menuAcpiSleep_Click(object sender, EventArgs e)
        {
            if (!Utils.AskFor(string.Format("Really press sleep button of {0}?", machine.Name))) return;
            tryToDoAndCatchCOMExcept(() => machine.AcpiSleep(), "send ACPI power button event");
        }

        private void menuLaunchManager_Click(object sender, EventArgs e)
        {
            Machine.LaunchVBoxManager();
        }

        private void menuShowSeparate_Click(object sender, EventArgs e)
        {
            machine.ShowSeparate();
            tryToDoAndCatchCOMExcept(() => machine.AcpiSleep(), "show this machine");
        }

        private void menuExitNow_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            if (machine.CanSaveState) machine.SaveState();
            notifyIcon.ContextMenuStrip.Enabled = false;
            machineStateChanged += closeWindowOnNonTrnsient;
        }

        private void closeWindowOnNonTrnsient(VirtualBox.MachineState newState)
        {
            if (!machine.IsTransient) Close();
        }
    }
}
