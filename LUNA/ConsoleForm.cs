using System;
using System.Windows.Forms;

namespace LUNA
{
    public partial class DebugConsoleForm : Form
    {
        public DebugConsoleForm()
        {
            InitializeComponent();
            // Initialize the form settings
            this.Text = "Debug Console";
            this.Width = 600;
            this.Height = 400;
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.Black;
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Font = new System.Drawing.Font("Consolas", 10F);
            this.textBox.ForeColor = System.Drawing.Color.Lime;
            this.textBox.Location = new System.Drawing.Point(0, 0);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(800, 450);
            this.textBox.TabIndex = 0;
            // 
            // DebugConsoleForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textBox);
            this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.Name = "DebugConsoleForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox textBox;

        public void WriteLine(string message)
        {
            if (this.textBox.InvokeRequired)
            {
                this.textBox.Invoke(new Action(() => WriteLine(message)));
            }
            else
            {
                this.textBox.AppendText(message + Environment.NewLine);
                this.textBox.ScrollToCaret();
            }
        }
    }
}
