/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

using System;
using static SP_Tetris.FormGame;

namespace SP_Tetris
{
    public class GameLogic  // Definiton of game logic and overall behavior
    {
        public static void SpawnTetro() // Function to spawn a new tetromino
        {
            Var.isPlaced = false;       // If the tetromino is placed or not
            Var.tetroRotation = 0;      // Rotation of tetromino, can be 0, 1, 2, or 3. Each number is a 90° clockwise rotation

            PickTetro();    // Calls a function to randomly pick a tetromino

            Var.currentTetroWidth = Var.currentTetro.GetLength(1);  // Get the dimensions of the current tetromino in both dimensions
            Var.currentTetroHeight = Var.currentTetro.GetLength(0);

            Var.nextTetroWidth = Var.nextTetro.GetLength(1);    // Get the dimensions of the next tetromino
            Var.nextTetroHeight = Var.nextTetro.GetLength(0);

            switch (Var.currentTetroWidth)  // Decide at which position on the x-axis to spawn the tetromino based on its width
            {
                case 2:
                    Var.currentTetroxPos = 4;
                    break;
                case 3:
                    Var.currentTetroxPos = 3;
                    break;
                case 4:
                    Var.currentTetroxPos = 3;
                    break;
            }

            Var.currentTetroyPos = 0;   // Set the current tetromino to the very top of the board

            MoveTetro();    // Call function to move tetromino
        }

        public static void PickTetro()  // Function to get next tetromino
        {
            Var.currentTetro = Var.nextTetro;   // The tetro from the stack is moved up to current tetromino
            Var.nextTetro = RandomTetro();      // Next tetromino is randomly picked - this is needed to display the upcoming tetromino in game
        }

        public static int[,] RandomTetro()  // Function to randomly select and return one of seven tetromino shapes 
        {
            Random rnd = new Random();
            int random = rnd.Next(1, 8);    // Based on the generated random number, select and return the tetromino shape

            switch (random)
            {
                case 1:
                    return Tetromino.tetromino_I;
                case 2:
                    return Tetromino.tetromino_L;
                case 3:
                    return Tetromino.tetromino_J;
                case 4:
                    return Tetromino.tetromino_O;
                case 5:
                    return Tetromino.tetromino_S;
                case 6:
                    return Tetromino.tetromino_T;
                case 7:
                    return Tetromino.tetromino_Z;
            }

            return null;
        }

        public static void MoveTetro()  // Function to move the tetromino
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
            int[,] newTetro = null;

            try
            {
                newTetro = new int[Var.currentTetroWidth, Var.currentTetroHeight];
            }
            catch (StackOverflowException)
            {
                SpawnTetro();
            }

            for (int row = 0; row < Var.currentTetroWidth; row++)
            {
                for (int col = 0; col < Var.currentTetroHeight; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = Var.currentTetroHeight - (col + 1);
                    newCol = row;

                    newTetro[newCol, newRow] = Var.currentTetro[col, row];
                }
            }

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
                        try
                        {
                            TetroRotateCounterClockwise();
                        }
                        catch (Exception)
                        {
                            SpawnTetro();
                        }
                    }
                }
            }
            MoveTetro();
        }

        public static void TetroRotateCounterClockwise()
        {
            int[,] newTetro = null;

            try
            {
                newTetro = new int[Var.currentTetroWidth, Var.currentTetroHeight];
            }
            catch (StackOverflowException)
            {
                SpawnTetro();
            }

            for (int row = 0; row < Var.currentTetroWidth; row++)
            {
                for (int col = 0; col < Var.currentTetroHeight; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = Var.currentTetroWidth - (row + 1);

                    newTetro[newCol, newRow] = Var.currentTetro[col, row];
                }
            }

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
                        try
                        {
                            TetroRotateClockwise();
                        }
                        catch (Exception)
                        {
                            SpawnTetro();
                        }
                    }
                }
            }
            MoveTetro();

        }

        static void PlaceTetro()
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
            Var.rowsToClear = 0;

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

                if (isFilled)
                {
                    Var.rowsToClear += 1;
                    Var.rowsCleared += 1;
                    ClearRow(h);
                }
            }

        }

        public static void ClearRow(int row)
        {
            for (int i = row; i > 0; i--)
            {
                for (int j = 0; j < Var.boardWidth; j++)
                {
                    Var.boardArray[j, i] = Var.boardArray[j, i - 1];
                }
            }

            for (int i = 0; i < Var.boardWidth; i++)
            {
                Var.boardArray[i, 0] = 0;
            }

            Var.gameLevel = Var.rowsCleared / 5;

            switch (Var.rowsToClear)
            {
                case 1:
                    Var.score += (Var.gameLevel + 1) * 40;
                    break;
                case 2:
                    Var.score += (Var.gameLevel + 1) * 100;
                    break;
                case 3:
                    Var.score += (Var.gameLevel + 1) * 300;
                    break;
                case 4:
                    Var.score += (Var.gameLevel + 1) * 1200;
                    break;
            }

            if ((Var.gameLevel * 30) > 450)
            {
                Var.waitTime = Var.defaultWaitTime - (Var.gameLevel * 30);
            }

        }

        public static void CheckGameOver()
        {
            if (Var.currentTetroyPos == 0 && !CheckCollisionDown())
            {
                ClearTetroArray();
                ClearBoardArray();
                ClearCurrentTetro();

                Var.currentTetro = null;
                Var.nextTetro = null;

                Var.gameOver = true;
            }
        }

        public static bool CheckCollisionDown()
        {
            for (int i = 0; i < Var.boardWidth; i++)
            {
                for (int j = 0; j < Var.boardHeight; j++)
                {
                    if (Var.currentTetroyPos + Var.currentTetroHeight + 1 > Var.boardHeight)
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