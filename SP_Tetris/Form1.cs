using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_Tetris
{
    public partial class Form1 : Form
    {
        public static class Var
        {
            public static int boardWidth;           //sirka desky
            public static int boardHeight;          //vyska desky

            public static int[,] tetroArray;        //pole ve kterem se pohybuje currentTetro
            public static int[,] boardArray;        //pole hry ve kterem jsou uz polozena tetromina

            public static int[,] currentTetro;      //prave pouzivane tetromino
            public static int currentTetroWidth;    //sirka desky
            public static int currentTetroHeight;   //vyska desky

            public static int[,] nextTetro;      //prave pouzivane tetromino
            public static int nextTetroWidth;    //sirka desky
            public static int nextTetroHeight;   //vyska desky

            public static int tetroRotation;

            public static int[,] tempTetro;

            public static int currentTetroxPos;     //x souradnice prave pouzivaneho tetromina
            public static int currentTetroyPos;     //y souradnice prave pouzivaneho tetromina

            public static int waitTime;             //cas cekani mezi pohyby, default 500
            public static int score;
            public static bool isPlaced;
            public static bool gameOver;

            public static int[] rowsToClear;
        }
        public Form1()
        {

            InitializeComponent();

            Var.boardWidth = 10;                    //definuje sirku herni desky
            Var.boardHeight = 20;                   //definuje vysku herni desky

            Var.tetroArray = new int[Var.boardWidth, Var.boardHeight];  //inicializace arraye prave pouzivaneho tetromina
            Var.boardArray = new int[Var.boardWidth, Var.boardHeight];  //inicializace arraye ve kterem jsou uz polozena tetromina

            Var.waitTime = 800;                     //definuje cekani v milisekundach

            ClearTetroArray();
            InitializeDGV();

            Var.score = 0;

            Draw();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    Start();
                    break;
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
            }

            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
        }

        private void Start()
        {
            labelScore.Text = Convert.ToString(Var.score);

            Var.gameOver = false;

            Game.SpawnTetro();
            //DrawNext();
            Draw();
            Wait();
            

            while (!Var.isPlaced)
            {
                Fall(); 
                Draw();
                Wait();
            }
            if (Var.isPlaced)
            {
                Start();
            }    
            
        }
        private void Fall()
        {
            Game.TetroFall();
            Draw();
        }

        private void HardDrop()
        {
            while(!Var.isPlaced)
            {
                Game.TetroFall();
                Draw();
            }
        }

        private void MoveLeft()
        {
            Game.TetroMoveLeft();
            Draw();
        }
        private void MoveRight()
        {
            Game.TetroMoveRight();
            Draw();
        }

        private void RotateClockwise()
        {
            Game.TetroRotateClockwise();
            Draw();
        }
        private void RotateCounterClockwise()
        {
            Game.TetroRotateCounterClockwise();
            Draw();
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

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.ColumnCount = Var.boardWidth;
            dataGridView1.RowCount = Var.boardHeight;

            for (int i = 0; i < Var.boardWidth; i++)     //nastavi sirku dgv
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    dataGridView1.Rows[j].Height = dataGridView1.Height / Var.boardHeight;
                }
            }

            dataGridViewNext.RowHeadersVisible = false;
            dataGridViewNext.ColumnHeadersVisible = false;
            dataGridViewNext.ScrollBars = ScrollBars.None;

            dataGridViewNext.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridViewNext.ColumnCount = 4;
            dataGridViewNext.RowCount = 4;

            for (int i = 0; i < 4; i++)     //nastavi sirku dgv
            {
                for (int j = 0; j < 4; j++)
                {
                    dataGridViewNext.Rows[j].Height = dataGridView1.Height / 4;
                }
            }
        }
        private void Draw()
        {
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
            dataGridViewNext.GridColor = Color.Black;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    dataGridViewNext.Rows[j].Cells[i].Value = 0;
                    dataGridViewNext.Rows[j].Cells[i].Value = Var.nextTetro[j, i];

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
    }
}
