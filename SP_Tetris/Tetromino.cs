﻿/* SP_Tetris
 * Seminar paper - Tetris in C# and Windows Forms
 * Matyas Strelec, 04/2021
 * mstrlc.eu
 */

namespace SP_Tetris
{
    class Tetromino // Definitions of tetromino shapes as 2D arrays
    {
        public static int[,] tetromino_I = new int[1, 4] { { 1, 1, 1, 1 } };

        public static int[,] tetromino_L = new int[2, 3] {          { 2, 0, 0 },
                                                                    { 2, 2, 2 }};

        public static int[,] tetromino_J = new int[2, 3] {          { 0, 0, 3 },
                                                                    { 3, 3, 3 } };

        public static int[,] tetromino_O = new int[2, 2] {          { 4, 4 },
                                                                    { 4, 4 } };

        public static int[,] tetromino_S = new int[2, 3] {          { 0, 5, 5 },
                                                                    { 5, 5, 0 } };

        public static int[,] tetromino_Z = new int[2, 3] {          { 6, 6, 0 },
                                                                    { 0, 6, 6 } };

        public static int[,] tetromino_T = new int[2, 3] {          { 0, 7, 0 },
                                                                    { 7, 7, 7 } };
    }
}