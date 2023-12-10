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

        public static int Walk((int Row, int Col) startPos, string[] _input)
        {
            var rowA = startPos.Row;
            var colA = startPos.Col;
            var currentDirA = Direction.Up;

            var rowB = startPos.Row;
            var colB = startPos.Col;
            var currentDirB = Direction.Left;

            var currentTileA = SymbolMap[_input[rowA][colA]];
            var currentTileB = SymbolMap[_input[rowB][colB]];

            var moves = 0;

            while (true)
            {
                if (moves > 1 && rowA == rowB && colA == colB)
                {
                    break;
                }

                var (newTileA, newDirA) = DoMove(ref rowA, ref colA, currentTileA, currentDirA, _input);
                var (newTileB, newDirB) = DoMove(ref rowB, ref colB, currentTileB, currentDirB, _input);

                currentDirA = newDirA;
                currentTileA = newTileA;
                currentDirB = newDirB;
                currentTileB = newTileB;

                moves++;
            }

            return moves;
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
