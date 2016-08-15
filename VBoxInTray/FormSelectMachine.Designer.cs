namespace VBoxInTray
{
    partial class FormSelectMachine
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxPowerUp = new System.Windows.Forms.CheckBox();
            this.listBoxMachines = new System.Windows.Forms.ListBox();
            this.tableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 2;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.buttonOk, 1, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonCancel, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.checkBoxPowerUp, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.listBoxMachines, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 4;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(318, 200);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // buttonOk
            // 
            this.buttonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOk.Enabled = false;
            this.buttonOk.Location = new System.Drawing.Point(221, 3);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(94, 22);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "&OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCancel.Location = new System.Drawing.Point(221, 31);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(94, 22);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxPowerUp
            // 
            this.checkBoxPowerUp.AutoSize = true;
            this.checkBoxPowerUp.Checked = true;
            this.checkBoxPowerUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPowerUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPowerUp.Location = new System.Drawing.Point(221, 59);
            this.checkBoxPowerUp.Name = "checkBoxPowerUp";
            this.checkBoxPowerUp.Size = new System.Drawing.Size(94, 18);
            this.checkBoxPowerUp.TabIndex = 2;
            this.checkBoxPowerUp.Text = "&Power on";
            this.checkBoxPowerUp.UseVisualStyleBackColor = true;
            // 
            // listBoxMachines
            // 
            this.listBoxMachines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxMachines.FormattingEnabled = true;
            this.listBoxMachines.Location = new System.Drawing.Point(3, 3);
            this.listBoxMachines.Name = "listBoxMachines";
            this.tableLayoutPanelMain.SetRowSpan(this.listBoxMachines, 4);
            this.listBoxMachines.Size = new System.Drawing.Size(212, 194);
            this.listBoxMachines.TabIndex = 3;
            this.listBoxMachines.SelectedIndexChanged += new System.EventHandler(this.listBoxMachines_SelectedIndexChanged);
            // 
            // FormSelectMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 200);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectMachine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select machine";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.CheckBox checkBoxPowerUp;
        private System.Windows.Forms.ListBox listBoxMachines;
    }
}