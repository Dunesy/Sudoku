using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] state = { { 5, 0, 0, 7, 0, 3, 6, 0, 0 }, { 0, 0, 0, 0, 5, 9, 0, 7, 0 }, { 3, 0, 9, 4, 0, 6, 2, 8, 0 }, { 4, 0, 6, 1, 0, 0, 0, 0, 0 }, { 0, 1, 0, 0, 0, 7, 0, 0, 6 }, { 9, 0, 0, 3, 6, 0, 8, 1, 0 }, { 1, 0, 0, 0, 0, 8, 0, 4, 2 }, { 0, 0, 0, 2, 0, 4, 0, 0, 9 }, { 6, 2, 0, 5, 9, 0, 0, 0, 0 } };

            int[,] state2 = { {2,0,0,0,0,0,6,0,4},
                              {0,1,0,0,0,0,5,0,0},
                              {0,8,7,1,0,4,0,0,0},
                              {4,0,0,0,0,9,0,0,3},
                              {0,0,0,0,0,0,0,5,0},
                              {7,0,0,6,0,0,2,0,0},
                              {0,0,0,0,5,3,9,0,0},
                              {0,0,0,0,9,0,4,0,0},
                              {0,0,0,2,0,0,0,8,0}
                             };

            int[,] state3 = {{0,3,6,4,0,1,0,0,0},
            {0,0,1,0,0,0,0,0,0},
            {8,0,0,0,9,7,0,0,0},
            {0,9,0,0,0,2,4,0,7},
            {1,0,0,0,0,0,0,0,2},
            {6,0,2,8,0,0,0,1,0},
            {0,0,0,6,5,0,0,0,4},
            {0,0,0,0,0,0,3,0,0},
            {0,0,0,3,0,4,2,8,0}};


   /*         for (int i = 0; i < 9; i++)
            {             
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(state[i, j]);
                    if (j % 3 == 2)
                        Console.Write(" ");                    
                }
                Console.WriteLine(" ");
                if (i % 3 == 2)
                    Console.WriteLine(" ");
            }

           Validator.Solve(state);
            */

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(state3[i, j]);
                    if (j % 3 == 2)
                        Console.Write(" ");
                }
                Console.WriteLine(" ");
                if (i % 3 == 2)
                    Console.WriteLine(" ");
            }

            Validator.Solve(state3);

        }
    }
}
