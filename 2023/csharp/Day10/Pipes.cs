using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    internal class Pipes
    {
        public static readonly Dictionary<char, char> SymbolMap = new Dictionary<char, char>
        {
            {'|', '║'},
            {'-', '═'},
            {'L', '╚'},
            {'J', '╝'},
            {'7', '╗'},
            {'F', '╔'},
            {'.', '.'},
            {'S', 'S'},
        };

        public static HashSet<(int, int)> GetAllPipeLocations((int Row, int Col) startPos, string[] input)
        {
            var row = startPos.Row;
            var col = startPos.Col;
            var startingDirections = PossibleStartingDirections(startPos, input);
            foreach (var dir in startingDirections)
            {
                Console.WriteLine(dir);
            }

            var currentDir = startingDirections.First();

            var result = new HashSet<(int, int)>();

            var currentTile = SymbolMap[input[row][col]];

            var moves = 0;

            while (true)
            {
                result.Add((row, col));

                if (moves > 1 && currentTile == 'S')
                {
                    break;
                }

                var (newTileA, newDirA) = DoMove(ref row, ref col, currentTile, currentDir, input);

                currentDir = newDirA;
                currentTile = newTileA;

                moves++;
            }

            return result;
        }

        public static int WalkPartOne((int Row, int Col) startPos, string[] input)
        {
            var rowA = startPos.Row;
            var colA = startPos.Col;
            var currentDirA = Direction.Up;

            var rowB = startPos.Row;
            var colB = startPos.Col;
            var currentDirB = Direction.Left;

            var currentTileA = SymbolMap[input[rowA][colA]];
            var currentTileB = SymbolMap[input[rowB][colB]];

            var moves = 0;

            while (true)
            {
                if (moves > 1 && rowA == rowB && colA == colB)
                {
                    break;
                }

                var (newTileA, newDirA) = DoMove(ref rowA, ref colA, currentTileA, currentDirA, input);
                var (newTileB, newDirB) = DoMove(ref rowB, ref colB, currentTileB, currentDirB, input);

                currentDirA = newDirA;
                currentTileA = newTileA;
                currentDirB = newDirB;
                currentTileB = newTileB;

                moves++;
            }

            return moves;
        }

        public static Direction[] PossibleStartingDirections((int Row, int Col) startPos, string[] input)
        {
            var result = new List<Direction>();
            if (SymbolMap[input[startPos.Row - 1][startPos.Col]] == '║'
                || SymbolMap[input[startPos.Row - 1][startPos.Col]] == '╗'
                || SymbolMap[input[startPos.Row - 1][startPos.Col]] == '╔')
            {
                result.Add(Direction.Up);
            }

            if (SymbolMap[input[startPos.Row + 1][startPos.Col]] == '║'
                || SymbolMap[input[startPos.Row + 1][startPos.Col]] == '╚'
                || SymbolMap[input[startPos.Row + 1][startPos.Col]] == '╝')
            {
                result.Add(Direction.Down);
            }

            if (SymbolMap[input[startPos.Row][startPos.Col + 1]] == '═'
                || SymbolMap[input[startPos.Row][startPos.Col + 1]] == '╗'
                || SymbolMap[input[startPos.Row][startPos.Col + 1]] == '╝')
            {
                result.Add(Direction.Right);
            }

            if (SymbolMap[input[startPos.Row][startPos.Col - 1]] == '═'
                || SymbolMap[input[startPos.Row][startPos.Col - 1]] == '╚'
                || SymbolMap[input[startPos.Row][startPos.Col - 1]] == '╔')
            {
                result.Add(Direction.Left);
            }

            return result.ToArray();
            
        }

        static (char NewTile, Direction NewDirection) DoMove(
            ref int row,
            ref int col,
            char currentTile,
            Direction currentDir,
            string[] _input)
        {
            var (newTile, newDir) = currentDir switch
            {
                Direction.Up when currentTile is 'S' => (SymbolMap[_input[--row][col]], Direction.Up),
                Direction.Up when currentTile is '║' => (SymbolMap[_input[--row][col]], Direction.Up),
                Direction.Up when currentTile is '╗' => (SymbolMap[_input[row][--col]], Direction.Left),
                Direction.Up when currentTile is '╔' => (SymbolMap[_input[row][++col]], Direction.Right),
                Direction.Down when currentTile is 'S' => (SymbolMap[_input[++row][col]], Direction.Down),
                Direction.Down when currentTile is '║' => (SymbolMap[_input[++row][col]], Direction.Down),
                Direction.Down when currentTile is '╚' => (SymbolMap[_input[row][++col]], Direction.Right),
                Direction.Down when currentTile is '╝' => (SymbolMap[_input[row][--col]], Direction.Left),
                Direction.Left when currentTile is 'S' => (SymbolMap[_input[row][--col]], Direction.Left),
                Direction.Left when currentTile is '═' => (SymbolMap[_input[row][--col]], Direction.Left),
                Direction.Left when currentTile is '╚' => (SymbolMap[_input[--row][col]], Direction.Up),
                Direction.Left when currentTile is '╔' => (SymbolMap[_input[++row][col]], Direction.Down),
                Direction.Right when currentTile is 'S' => (SymbolMap[_input[row][++col]], Direction.Right),
                Direction.Right when currentTile is '═' => (SymbolMap[_input[row][++col]], Direction.Right),
                Direction.Right when currentTile is '╝' => (SymbolMap[_input[--row][col]], Direction.Up),
                Direction.Right when currentTile is '╗' => (SymbolMap[_input[++row][col]], Direction.Down),
                _ => throw new InvalidCastException($"NOPE: {currentDir} - {currentTile}")
            };

            return (newTile, newDir);
        }
    }
}
