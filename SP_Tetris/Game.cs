using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SP_Tetris.Form1;

namespace SP_Tetris
{
    class Game
    {   /// <summary>
        /// Zacatek hry
        /// </summary>

        public static void SpawnTetro()
        {
            PickTetro();

            Var.isPlaced = false;

            Var.currentTetroWidth = Var.currentTetro.GetLength(1);
            Var.currentTetroHeight = Var.currentTetro.GetLength(0);

            Var.nextTetroWidth = Var.nextTetro.GetLength(1);
            Var.nextTetroHeight = Var.nextTetro.GetLength(0);

            Var.currentTetroxPos = 4;
            Var.currentTetroyPos = 0;

            Var.tetroRotation = 0;

            MoveTetro();
        }
        public static void PickTetro()
        {
            Random rnd = new Random();
            int random = rnd.Next(1, 8);

            switch (random)
            {
                case 1:
                    Var.nextTetro = Tetromino.tetromino_I;
                    break;
                case 2:
                    Var.nextTetro = Tetromino.tetromino_L;
                    break;
                case 3:
                    Var.nextTetro = Tetromino.tetromino_J;
                    break;
                case 4:
                    Var.nextTetro = Tetromino.tetromino_O;
                    break;
                case 5:
                    Var.nextTetro = Tetromino.tetromino_S;
                    break;
                case 6:
                    Var.nextTetro = Tetromino.tetromino_T;
                    break;
                case 7:
                    Var.nextTetro = Tetromino.tetromino_Z;
                    break;
             }

            if(Var.currentTetro == null)
            {
                Var.currentTetro = Var.nextTetro;
                PickTetro();
            }
        }


        public static void MoveTetro()
        {
            ClearTetroArray();

            for (int i = 0; i < Var.currentTetroWidth; i++)
            {
                for (int j = 0; j < Var.currentTetroHeight; j++)
                {
                    Var.tetroArray[Var.currentTetroxPos + i, Var.currentTetroyPos + j] = Var.currentTetro[j, i];
                }
            }
        }

        public static void TetroFall()
        {
            CheckGameOver();
            
            if (CheckCollisionDown() && !Var.gameOver)
            {
                Var.currentTetroyPos++;
                MoveTetro();
            }
            else
            {
                PlaceTetro();
                CheckFilledRows();
            }

        }

        public static void TetroMoveLeft()
        {
            if (CheckCollisionLeft())
            {
                Var.currentTetroxPos--;
            }
            MoveTetro();
        }
        public static void TetroMoveRight()
        {
            if (CheckCollisionRight())
            {
                Var.currentTetroxPos++;
            }
            MoveTetro();
        }

        public static void TetroRotateClockwise()
        {
            int width;
            int height;
            int[,] newTetro;

            width = Var.currentTetro.GetUpperBound(0) + 1;
            height = Var.currentTetro.GetUpperBound(1) + 1;
            newTetro = new int[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = width - (col + 1);
                    newCol = row;

                    newTetro[newCol, newRow] = Var.currentTetro[col, row];
                }
            }

            Var.tempTetro = Var.currentTetro;

            Var.currentTetro = newTetro;

            if (Var.tetroRotation == 3)
            {
                Var.tetroRotation = 0;
            }
            else
            {
                Var.tetroRotation++;
            }

            //posun pro SRS
            if (Var.currentTetroWidth == 3 || Var.currentTetroHeight == 3) //tetromino je 2x3
            {
                switch (Var.tetroRotation)
                {
                    case 0:
                        break;
                    case 1:
                        Var.currentTetroxPos++;
                        break;
                    case 2:
                        Var.currentTetroxPos--;
                        Var.currentTetroyPos++;
                        break;
                    case 3:
                        Var.currentTetroyPos--;
                        break;
                }
            }

            else if (Var.currentTetroWidth == 4 || Var.currentTetroHeight == 4) //tetromino je 4x1
            {
                switch (Var.tetroRotation)
                {
                    case 0:
                        Var.currentTetroxPos--;
                        Var.currentTetroyPos++;
                        break;
                    case 1:
                        Var.currentTetroxPos++;
                        Var.currentTetroxPos++;
                        Var.currentTetroyPos--;
                        break;
                    case 2:
                        Var.currentTetroxPos--;
                        Var.currentTetroxPos--;
                        Var.currentTetroyPos++;
                        Var.currentTetroyPos++;
                        break;
                    case 3:
                        Var.currentTetroxPos++;
                        Var.currentTetroyPos--;
                        Var.currentTetroyPos--;
                        break;
                }
            }


            Var.currentTetroWidth = Var.currentTetro.GetLength(1);
            Var.currentTetroHeight = Var.currentTetro.GetLength(0);

            try
            {
                MoveTetro();
            }
            catch (Exception)
            {
                TetroRotateCounterClockwise();
            }

            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if (Var.tetroArray[i, j] != 0 && Var.boardArray[i, j] != 0)
                    {
                        TetroRotateCounterClockwise();
                    }
                }
            }
            MoveTetro();
        }

        public static void TetroRotateCounterClockwise()
        {
            int width;
            int height;
            int[,] newTetro;

            width = Var.currentTetro.GetUpperBound(0) + 1;
            height = Var.currentTetro.GetUpperBound(1) + 1;
            newTetro = new int[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = height - (row + 1);

                    newTetro[newCol, newRow] = Var.currentTetro[col, row];
                }
            }

            Var.tempTetro = Var.currentTetro;

            Var.currentTetro = newTetro;

            if (Var.tetroRotation == 0)
            {
                Var.tetroRotation = 3;
            }
            else
            {
                Var.tetroRotation--;
            }

            //posun pro SRS
            if (Var.currentTetroWidth == 3 || Var.currentTetroHeight == 3) //tetromino je 2x3
            {
                switch (Var.tetroRotation)
                {
                    case 0:
                        Var.currentTetroxPos--;
                        break;
                    case 1:
                        Var.currentTetroxPos++;
                        Var.currentTetroyPos--;
                        break;
                    case 2:
                        Var.currentTetroyPos++;
                        break;
                    case 3:
                        break;
                }
            }
            else if (Var.currentTetroWidth == 4 || Var.currentTetroHeight == 4) //tetromino je 4x1
            {
                switch (Var.tetroRotation)
                {
                    case 0:
                        Var.currentTetroxPos--;
                        Var.currentTetroxPos--;
                        Var.currentTetroyPos++;
                        break;
                    case 1:
                        Var.currentTetroxPos++;
                        Var.currentTetroxPos++;
                        Var.currentTetroyPos--;
                        Var.currentTetroyPos--;
                        break;
                    case 2:
                        Var.currentTetroxPos--;
                        Var.currentTetroyPos++;
                        Var.currentTetroyPos++;
                        break;
                    case 3:
                        Var.currentTetroxPos++;
                        Var.currentTetroyPos--;
                        break;
                }
            }

            Var.currentTetroWidth = Var.currentTetro.GetLength(1);
            Var.currentTetroHeight = Var.currentTetro.GetLength(0);

            try
            {
                MoveTetro();
            }
            catch (Exception)
            {
                TetroRotateClockwise();
            }

            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if (Var.tetroArray[i, j] != 0 && Var.boardArray[i, j] != 0)
                    {
                        TetroRotateClockwise();
                    }
                }
            }
            MoveTetro();

        }

        public static void PlaceTetro()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if (Var.tetroArray[i, j] != 0)
                    {
                        Var.boardArray[i, j] = Var.tetroArray[i, j];
                    }
                }
            }
            
            ClearTetroArray();

            Var.isPlaced = true;
        }

        public static void CheckFilledRows()
        {
            for (int h = 0; h < Var.boardHeight; h++)
            {
                bool isFilled = true;

                for (int w = 0; w < Var.boardWidth; w++)
                {
                    if (Var.boardArray[w, h] == 0)
                    {
                        isFilled = false;
                    }
                }

                if(isFilled)
                {
                    ClearRow(h);
                    Var.score += 100;
                    if(Var.waitTime > 100)
                    {
                        Var.waitTime -= 50;
                    }
                }
            }
        }

        public static void ClearRow(int row)
        {
            for (int i = row; i > 0; i--)
            {
                for(int j = 0; j < Var.boardWidth; j++)
                {
                    Var.boardArray[j, i] = Var.boardArray[j, i - 1];
                }
            }

            for (int i = 0; i < Var.boardWidth; i++)
            {
                Var.boardArray[i, 0] = 0;
            }
        }

        public static void CheckGameOver()
        {
            if (Var.currentTetroyPos == 0 && !CheckCollisionDown())
            {
                ClearTetroArray();
                ClearBoardArray();
                ClearCurrentTetro();
               
                Var.gameOver = true;

                MessageBox.Show("GAME OVER");
            }
        }

        public static bool CheckCollisionDown()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if(Var.currentTetroyPos + Var.currentTetroHeight + 1 > Var.boardHeight)
                    {
                        return false;
                    }
                    if (Var.tetroArray[i, j] != 0 && Var.boardArray[i, j + 1] != 0)
                    {
                        return false;
                    }

                }
            }
            return true;
        }

        public static bool CheckCollisionLeft()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if (Var.currentTetroxPos == 0)
                    {
                        return false;
                    }
                    if (Var.tetroArray[i, j] != 0 && Var.boardArray[i - 1, j] != 0)
                    {
                        return false;
                    }

                }
            }
            
            return true;
        }

        public static bool CheckCollisionRight()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if (Var.currentTetroxPos + 1 > Var.boardWidth - Var.currentTetroWidth)
                    {
                        return false;
                    }
                    if (Var.tetroArray[i, j] != 0 && Var.boardArray[i + 1, j] != 0)
                    {
                        return false;
                    }

                }
            }

            return true;
        }
    }
}