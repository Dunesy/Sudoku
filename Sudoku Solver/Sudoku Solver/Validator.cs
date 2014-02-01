using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku_Solver
{
    public class Validator
    {
        //Brute Force Solution
        public static Cell GeneratePossibilitiesForCell(int x, int y, int[,] state)
        {
            if (state[y, x] != 0)
                return null;

            int startColumn = (x / 3) * 3;
            int startRow = (y / 3) * 3;
            List<int> possibleValues = new List<int> {1,2,3,4,5,6,7,8,9};
            for (int n = 1; n <= 9; n++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (n == state[i, x])
                        possibleValues.Remove(n);

                }

                for (int i = 0; i < 9; i++)
                {
                    if (n == state[y, i])
                        possibleValues.Remove(n);

                }

                //Check The Mini Square                
                for (int i = startRow; i < startRow + 3; i++)
                {
                    for (int j = startColumn; j < startColumn + 3; j++)
                    {
                        if (n == state[i, j])
                        {
                            possibleValues.Remove(n);
                        }
                    }
                }
            }
            return new Cell
                {
                    X = x,
                    Y = y,
                    PossibleValues = possibleValues
                };                
        }
        public static HeapQ<ScoredState> states = new HeapQ<ScoredState>();        
        public static List<Cell> GeneratePossibilitiesForTable(int[,] state)
        {
            List<Cell> PossibilityQueue = new List<Cell>();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    var result = GeneratePossibilitiesForCell(j, i, state);
                    if (result != null)
                    {
                        PossibilityQueue.Add(result);
                    }
                }
            }

            for (int i = 0; i < 9; i += 3)
            {
                for (int j = 0; j < 9; j += 3)
                {
                    var toOperateOn = PossibilityQueue.Where(a => a.X < j + 3 && a.X >= j && a.Y < i + 3 && a.Y >= i).ToList();
                    SetComparisons(toOperateOn);
                }
            }
            
            for (int i = 0; i < 9; i += 3)
            {
                    var toOperateOn = PossibilityQueue.Where(a=>a.X == i).ToList();
                    SetComparisons(toOperateOn);
            }

            for (int i = 0; i < 9; i += 3)
            {
                var toOperateOn = PossibilityQueue.Where(a => a.Y == i).ToList();
                SetComparisons(toOperateOn);
            }          

            return PossibilityQueue;
        }
        public static void SetComparisons(List<Cell> cells)
        {
            int[] counts = {0,0,0,0,0,0,0,0,0};
            foreach (var cell in cells)
            {
                foreach( var values in cell.PossibleValues)
                {
                    counts[values-1]++;
                }
            }

            for(int i = 0 ; i < counts.Length; i++)
            {
                if (counts[i] == 1)
                {
                    var toModify = cells.Find(a=>a.PossibleValues.Contains(i + 1));
                    if (toModify != null)
                    {
                        toModify.PossibleValues.Clear();
                        toModify.PossibleValues.Add(i + 1);
                    }
                }           
            }
        }

        public static bool IsValid(int[,] state)
        {

            int[] HorizontalSegment = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] VerticalSegment = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<int> Blocks = new List<int>();
            //CheckColumns
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    HorizontalSegment[j] = state[i, j];
                    VerticalSegment[j] = state[j, i];
                }

                if (!IsSegmentValid(HorizontalSegment))
                    return false;
                if (!IsSegmentValid(VerticalSegment)) 
                    return false;                
            }

                      
            for ( int startColumn = 0 ; startColumn < 9; startColumn+= 3)
                for (int startRow = 0; startRow < 9; startRow += 3)
                {
                    //Check The Mini Square                
                    for (int i = startRow; i < startRow+ 3; i++)
                    {
                        for (int j = startColumn; j < startColumn + 3; j++)
                        {
                           Blocks.Add(state[i,j]);
                        }
                    }
                    if (!IsSegmentValid(Blocks.ToArray()))
                        return false;
                }
            return true;
        }

        public static bool IsCompleted(int[,] state)
        {
            for (int i = 0 ; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (state[i, j] == 0)
                        return false;
                }
            
            return IsValid(state);
        }

        public static bool IsSegmentValid(int[] segment)
        {
            int[] currentValues = {1,1,1,1,1,1,1,1,1,1};           
            //CheckColumns
            
            for (int i = 0; i < 9; i++)
            {
               if (segment[i] != 0)
                   if (currentValues[segment[i] - 1] != 0)
                   {
                        currentValues[segment[i] - 1] = 0;
                   }
                   else
                   {
                       return false;
                   }                
            }
            return true;
        }

        public static void DeterminantSolve(ref int [,] state)
        {
            var currentSolution = GeneratePossibilitiesForTable(state);
            while (currentSolution.Exists(a => a.Key == 1))
            {
                foreach (var cell in currentSolution.Where(a => a.Key == 1))
                {
                    state[cell.Y, cell.X] = cell.PossibleValues.First();
                }
                currentSolution = GeneratePossibilitiesForTable(state);
            }
        }

        public static void IndeterminantSolve(int[,] state)
        {
            
            DeterminantSolve(ref state);

            var cells = GeneratePossibilitiesForTable(state);
            if (cells.Sum(a => a.Key) == 0 && IsCompleted(state))
            {
                var scoredState = new ScoredState
                {
                    State = state,
                    Score = 0
                };
                states.Push(scoredState);
            }

            foreach (var cell in cells)
            {
                foreach (var pos in cell.PossibleValues)
                {
                    int[,] newState = Copy(state);
                    newState[cell.Y, cell.X] = pos;
                    if (IsValid(newState))
                    {

                        var scoredState = new ScoredState
                        {
                            State = newState,
                            Score = GeneratePossibilitiesForTable(newState).Sum(a=>a.Key)
                        };
                        states.Push(scoredState);
                    }
                }
            }
        }

        public static int[,] Copy(int[,] state)
        {
            int[,] copiedState = new int[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    copiedState[i,j] = state[i, j];
                }
            }
            return copiedState;
        }

        public static void Solve(int[,] StartingState)
        {
            var startingState = new ScoredState
            {
                State = StartingState,
                Score = GeneratePossibilitiesForTable(StartingState).Sum(a=>a.Key)
            };
            
            states.Push(startingState);                
            while (!IsCompleted(states.First.State))
            {
                var state = states.Pop();
                Console.WriteLine(state.Score);
                IndeterminantSolve(state.State);
            }

            Console.WriteLine("\nSOLUTION\n");          

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(states.First.State[i, j]);
                    if (j % 3 == 2)
                        Console.Write(" ");
                }
                Console.WriteLine(" ");
                if (i % 3 == 2)
                    Console.WriteLine(" ");
            }

        }

    }

    public class Cell
    {
        public List<int> PossibleValues { get; set;}
        public int X { get; set; }
        public int Y { get; set; }

        public int Key {
            get { 
                if (PossibleValues == null)
                    return 0;   
                else                                
                    return PossibleValues.Count;
            }
        }
    }

    

}

