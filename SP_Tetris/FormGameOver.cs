/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

using System;
using System.Windows.Forms;
using static SP_Tetris.FormGame;


namespace SP_Tetris
{
    public partial class FormGameOver : Form
    {
        public FormGameOver()
        {
            InitializeComponent();
            labelScore.Text = Convert.ToString(Var.score);
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}
