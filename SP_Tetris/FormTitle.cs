/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

using System;
using System.Windows.Forms;

namespace SP_Tetris
{
    public partial class FormTitle : Form
    {
        public FormTitle()
        {
            InitializeComponent();
        }

        private void FormTitle_Load(object sender, EventArgs e)
        {
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            FormGame game = new FormGame();
            game.Show();
            this.Hide();
        }

        private void FormTitle_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            FormAbout about = new FormAbout();
            about.Show();
            this.Hide();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
