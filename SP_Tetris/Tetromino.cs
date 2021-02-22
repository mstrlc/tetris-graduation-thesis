using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP_Tetris
{
    class Tetromino
    {
        public static int[,] tetromino_I = new int[1, 4] {          { 1, 1, 1, 1 }};

        public static int[,] tetromino_L = new int[2, 3] {          { 1, 0, 0 },
                                                                    { 1, 1, 1 }};

        public static int[,] tetromino_J = new int[2, 3] {          { 0, 0, 1 },
                                                                    { 1, 1, 1 } };     

        public static int[,] tetromino_O = new int[2, 2] {          { 1, 1 },      
                                                                    { 1, 1 } };

        public static int[,] tetromino_S = new int[2, 3] {          { 0, 1, 1 },
                                                                    { 1, 1, 0 } };

        public static int[,] tetromino_Z = new int[2, 3] {          { 1, 1, 0 },
                                                                    { 0, 1, 1 } };

        public static int[,] tetromino_T = new int[2, 3] {          { 0, 1, 0 },
                                                                    { 1, 1, 1 } };    
    }
}