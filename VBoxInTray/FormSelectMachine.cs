using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VBoxInTray
{
    public partial class FormSelectMachine : Form
    {
        private VirtualBox.IMachine selectedVm = null;
        public VirtualBox.IMachine SelectedVm
        {
            get { return selectedVm; }
        }

        private bool shouldPowerOn = true;
        public bool ShouldPowerOn
        {
            get { return shouldPowerOn; }
        }

        private List<VirtualBox.IMachine> vms;
        public FormSelectMachine()
        {
            Icon = Properties.Resources.vboxintray;
            vms = new List<VirtualBox.IMachine>(Program.VBox.Machines.Cast<VirtualBox.IMachine>());
            string[] vmNames = new string[vms.Count];
            for (int i = 0; i < vms.Count; ++i)
            {
                vmNames[i] = string.Format("{0} [{1}]", vms[i].Name, Utils.MachineStateToString(vms[i].State));
            }

            InitializeComponent();

            listBoxMachines.Items.AddRange(vmNames);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            selectedVm = vms[listBoxMachines.SelectedIndex];
            shouldPowerOn = checkBoxPowerUp.Checked;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listBoxMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOk.Enabled = (listBoxMachines.SelectedIndex >= 0);
            buttonOk.Focus();
        }
    }
}
