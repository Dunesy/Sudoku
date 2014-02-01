using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Sudoku_Solver
{
   
     public class ScoredState : IComparable    
     {
        public int[,] State { get; set; }
        public int Score { get; set; }
        public string Key { get; set; }

        public int CompareTo(object obj)
        {
            var comparedObject = obj as ScoredState;

            if (comparedObject == null)
                throw new Exception("Uncastable object");

            if (Score < comparedObject.Score)
                return -1;
            else if (Score > comparedObject.Score)
                return 1;
            else
                return 0;
                     
        }
     }
}
