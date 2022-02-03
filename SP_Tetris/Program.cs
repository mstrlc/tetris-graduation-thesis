/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

using System;
using System.Windows.Forms;

namespace SP_Tetris
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormTitle());
        }
    }
}
