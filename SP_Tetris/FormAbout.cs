/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

using System;
using System.Windows.Forms;

namespace SP_Tetris
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://mstrlc.eu");
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            FormTitle title = new FormTitle();
            title.Show();
            this.Close();
        }
    }
}
