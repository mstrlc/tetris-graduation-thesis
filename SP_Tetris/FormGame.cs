/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace SP_Tetris
{
    public partial class FormGame : Form
    {
        public static class Var
        {
            public static int boardWidth;           // Game board width
            public static int boardHeight;          // Game board height

            public static int[,] tetroArray;        // Array where the current tetromino is moving in
            public static int[,] boardArray;        // Array storing already placed tetrominos

            public static int[,] currentTetro;      // Current tetromino shape
            public static int currentTetroWidth;    // Width of current tetromino
            public static int currentTetroHeight;   // Height of current tetromino
            public static int currentTetroxPos;     // X-axis position of current tetromino
            public static int currentTetroyPos;     // Y-axis position of current tetromino

            public static int[,] nextTetro;         // Next tetromino shape
            public static int nextTetroWidth;       // Width of next tetromino
            public static int nextTetroHeight;      // Height of next tetromino

            public static int defaultWaitTime = 500;// Default time to wait before tetromino falls down a block
            public static int waitTime;             // Current time to wait before tetromino falls down a block, changes throughout the game
            public static int score;                // Score of current game
            
            public static int tetroRotation;        // Rotation of current tetromino
            public static bool isPlaced;            // State of a tetromino, if it cannot fall down anymore, it is placed
            public static bool gameOver;            // State of the game, is true if game cannot go on

            public static int rowsToClear;          // Number of rows to clear out
            public static int rowsCleared;          // Total number of rows that have been cleared in current game
            public static int gameLevel;            // Level of current game
        }

        public FormGame()
        {
            InitializeComponent();
        }

        private void FormGame_Load(object sender, EventArgs e)  // Set everything up on game load
        {
            Var.boardWidth = 10;    // Board width and height, could theoretically be changed here. Although many other changes would need to be done
            Var.boardHeight = 20;

            Var.tetroArray = new int[Var.boardWidth, Var.boardHeight];  // Create array for current tetromino to move in
            Var.boardArray = new int[Var.boardWidth, Var.boardHeight];  // Create array for placed tetrominos to be stored

            Var.score = 0;  // Set default variables at the start of the game
            Var.rowsCleared = 0;
            Var.gameLevel = 0;
            Var.isPlaced = false;
            Var.gameOver = false;

            Var.waitTime = Var.defaultWaitTime;

            Var.nextTetro = GameLogic.RandomTetro();            // Pick a random tetromino as the first one
            Var.nextTetroWidth = Var.nextTetro.GetLength(1);    // Get the tetromino's dimensions
            Var.nextTetroHeight = Var.nextTetro.GetLength(0);

            ClearTetroArray();  // Clear array before starting
            InitializeDGV();    // Prepare the game board

            Draw();
            DrawNext();
        }


        private void FormGame_Shown(object sender, EventArgs e) // When everything is loaded and the game board is shown to the user, start the game
        {
            Wait();  // Wait before starting
            Start(); // Start the game
        }

        private void FormGame_KeyDown(object sender, KeyEventArgs e)    // Function to handle user keyboard input
        {
            switch (e.KeyCode)  // Depending on which key was pressed, perform actions
            {
                case Keys.Down:
                    Fall();
                    break;
                case Keys.Left:
                    MoveLeft();
                    break;
                case Keys.Right:
                    MoveRight();
                    break;
                case Keys.Z:
                    RotateCounterClockwise();
                    break;
                case Keys.X:
                    RotateClockwise();
                    break;
                case Keys.Space:
                    HardDrop();
                    break;
                case Keys.Escape:
                    ExitGame();
                    break;
            }
        }

        private void Start()    // Function called at the beggining of the game and with each new tetromino
        {
            GameLogic.CheckGameOver();  // Check for game over

            if (!Var.gameOver)  
            {
                labelScore.Text = Convert.ToString(Var.score);
                labelLevel.Text = Convert.ToString(Var.gameLevel);
                labelLines.Text = Convert.ToString(Var.rowsCleared);

                GameLogic.SpawnTetro();
                DrawNext();
                Draw();
                Wait();

                while (!Var.isPlaced)
                {
                    Fall();
                    Wait();
                }

                if (Var.isPlaced)
                {
                    Start();
                }
            }
            else
            {
                GameOver();
            }
        }
        private void Fall()
        {
            GameLogic.TetroFall();
            Draw();
        }

        private void HardDrop()
        {
            while (!Var.isPlaced)
            {
                GameLogic.TetroFall();
                Draw();
            }
        }

        private void MoveLeft()
        {
            GameLogic.TetroMoveLeft();
            Draw();
        }
        private void MoveRight()
        {
            GameLogic.TetroMoveRight();
            Draw();
        }

        private void RotateClockwise()
        {
            GameLogic.TetroRotateClockwise();
            Draw();
        }
        private void RotateCounterClockwise()
        {
            GameLogic.TetroRotateCounterClockwise();
            Draw();
        }

        private void ExitGame()
        {
            Application.Restart();
        }

        private void GameOver()
        {
            this.Hide();
            FormGameOver gameover = new FormGameOver();
            gameover.ShowDialog();

        }


        public static void ClearTetroArray()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    Var.tetroArray[i, j] = 0;
                }
            }
        }

        public static void ClearBoardArray()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    Var.boardArray[i, j] = 0;
                }
            }
        }

        public static void ClearCurrentTetro()
        {
            for (int i = 0; i < Var.currentTetroHeight; i++)
            {
                for (int j = 0; j < Var.currentTetroWidth; j++)
                {
                    Var.currentTetro[i, j] = 0;
                }
            }
        }

        private void InitializeDGV()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.ScrollBars = ScrollBars.None;

            dataGridView1.ColumnCount = Var.boardWidth;
            dataGridView1.RowCount = Var.boardHeight;

            for (int i = 0; i < Var.boardWidth; i++)     //nastavi sirku dgv
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    dataGridView1.Columns[i].Width = dataGridView1.Width / Var.boardWidth;
                    dataGridView1.Rows[j].Height = dataGridView1.Height / Var.boardHeight;
                }
            }

            dataGridViewNext.RowHeadersVisible = false;
            dataGridViewNext.ColumnHeadersVisible = false;
            dataGridViewNext.ScrollBars = ScrollBars.None;

            dataGridViewNext.ColumnCount = 4;
            dataGridViewNext.RowCount = 4;

            for (int i = 0; i < 4; i++)     //nastavi sirku dgv
            {
                for (int j = 0; j < 4; j++)
                {
                    dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.Black;
                    dataGridViewNext.Columns[i].Width = dataGridViewNext.Width / 4;
                    dataGridViewNext.Rows[j].Height = dataGridViewNext.Height / 4;
                }
            }
        }
        private void Draw()
        {
            dataGridView1.ClearSelection();
            dataGridView1.GridColor = Color.Black;

            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    dataGridView1.Rows[j].Cells[i].Value = 0;
                    dataGridView1.Rows[j].Cells[i].Value = Var.boardArray[i, j];

                    if (Var.tetroArray[i, j] != 0)
                    {
                        dataGridView1.Rows[j].Cells[i].Value = Var.tetroArray[i, j];
                    }

                    switch (Convert.ToInt32(dataGridView1.Rows[j].Cells[i].Value))
                    {
                        case 0:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.Black;
                            break;
                        case 1:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(32, 178, 170);
                            break;
                        case 2:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(255, 69, 0);
                            break;
                        case 3:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(0, 0, 205);
                            break;
                        case 4:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(255, 215, 0);
                            break;
                        case 5:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(34, 139, 34);
                            break;
                        case 6:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(178, 34, 34);
                            break;
                        case 7:
                            dataGridView1.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(128, 0, 128);
                            break;
                    }

                    dataGridView1.Rows[j].Cells[i].Style.ForeColor = dataGridView1.Rows[j].Cells[i].Style.BackColor;
                }
            }
        }
        private void DrawNext()
        {
            dataGridViewNext.ClearSelection();
            dataGridViewNext.GridColor = Color.Black;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    dataGridViewNext.Rows[j].Cells[i].Value = 0;
                }
            }

            for (int i = 0; i < Var.nextTetroWidth; i++)
            {
                for (int j = 0; j < Var.nextTetroHeight; j++)
                {

                    dataGridViewNext.Rows[j].Cells[i].Value = Var.nextTetro[j, i];
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    switch (Convert.ToInt32(dataGridViewNext.Rows[j].Cells[i].Value))
                    {
                        case 0:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.Black;
                            break;
                        case 1:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(32, 178, 170);
                            break;
                        case 2:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(255, 69, 0);
                            break;
                        case 3:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(0, 0, 205);
                            break;
                        case 4:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(255, 215, 0);
                            break;
                        case 5:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(34, 139, 34);
                            break;
                        case 6:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(178, 34, 34);
                            break;
                        case 7:
                            dataGridViewNext.Rows[j].Cells[i].Style.BackColor = Color.FromArgb(128, 0, 128);
                            break;
                    }

                    dataGridViewNext.Rows[j].Cells[i].Style.ForeColor = dataGridViewNext.Rows[j].Cells[i].Style.BackColor;
                }
            }

        }

        private void Wait()
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (Var.waitTime == 0 || Var.waitTime < 0) return;

            timer1.Interval = Var.waitTime;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        private void dataGridViewNext_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewNext.ClearSelection();

        }

    }
}
