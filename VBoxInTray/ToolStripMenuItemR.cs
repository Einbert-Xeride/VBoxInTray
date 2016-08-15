using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VBoxInTray
{
    class ToolStripMenuItemR : ToolStripMenuItem
    {
        public delegate bool UsabilityChecker();
        private UsabilityChecker usabilityChecker;
        public ToolStripMenuItemR(string text, EventHandler onClick, UsabilityChecker checker) : base(text)
        {
            usabilityChecker = checker;
            CheckUsability();
            Click += onClick;
        }

        public void CheckUsability()
        {
            if (usabilityChecker != null) Visible = Enabled = usabilityChecker();
        }
    }
}
