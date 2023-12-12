using Microsoft.Diagnostics.Tracing.Parsers.Clr;
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

        public static int WalkBackToStart((int Row, int Col) startPos, string[] input)
        {
            var rowA = startPos.Row;
            var colA = startPos.Col;
            var currentDirA = Direction.Up;

            var currentTileA = SymbolMap[input[rowA][colA]];

            var moves = 0;

            while (true)
            {
                if (moves > 1 && currentTileA == 'S')
                {
                    break;
                }

                var (newTileA, newDirA) = DoMove(ref rowA, ref colA, currentTileA, currentDirA, input);

                currentDirA = newDirA;
                currentTileA = newTileA;

                moves++;
            }

            return moves;
        }

        public static int WalkAndFill((int Row, int Col) startPos, char[][] input)
        {
            var rowA = startPos.Row;
            var colA = startPos.Col;
            var currentDirA = Direction.Up;

            var currentTileA = input[rowA][colA];

            var moves = 0;

            while (true)
            {
                if (moves > 1 && currentTileA == 'S')
                {
                    break;
                }

                var (newTileA, newDirA) = DoMovePart2(ref rowA, ref colA, currentTileA, currentDirA, input);

                currentDirA = newDirA;
                currentTileA = newTileA;

                moves++;
            }

            return moves;
        }

        public static Direction[] PossibleStartingDirections((int Row, int Col) startPos, string[] input)
        {
            var result = new List<Direction>();
            if (SymbolMap[input[Math.Max(0, startPos.Row - 1)][startPos.Col]] == '║'
                || SymbolMap[input[Math.Max(0, startPos.Row - 1)][startPos.Col]] == '╗'
                || SymbolMap[input[Math.Max(0, startPos.Row - 1)][startPos.Col]] == '╔')
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

        static (char NewTile, Direction NewDirection) DoMovePart2(
            ref int row,
            ref int col,
            char currentTile,
            Direction currentDir,
            char[][] input)
        {
            var (newTile, newDir) = currentDir switch
            {
                Direction.Up when currentTile is 'S' => (input[--row][col], Direction.Up),
                Direction.Up when currentTile is '║' => (input[--row][col], Direction.Up),
                Direction.Up when currentTile is '╗' => (input[row][--col], Direction.Left),
                Direction.Up when currentTile is '╔' => (input[row][++col], Direction.Right),
                Direction.Down when currentTile is 'S' => (input[++row][col], Direction.Down),
                Direction.Down when currentTile is '║' => (input[++row][col], Direction.Down),
                Direction.Down when currentTile is '╚' => (input[row][++col], Direction.Right),
                Direction.Down when currentTile is '╝' => (input[row][--col], Direction.Left),
                Direction.Left when currentTile is 'S' => (input[row][--col], Direction.Left),
                Direction.Left when currentTile is '═' => (input[row][--col], Direction.Left),
                Direction.Left when currentTile is '╚' => (input[--row][col], Direction.Up),
                Direction.Left when currentTile is '╔' => (input[++row][col], Direction.Down),
                Direction.Right when currentTile is 'S' => (input[row][++col], Direction.Right),
                Direction.Right when currentTile is '═' => (input[row][++col], Direction.Right),
                Direction.Right when currentTile is '╝' => (input[--row][col], Direction.Up),
                Direction.Right when currentTile is '╗' => (input[++row][col], Direction.Down),
                _ => (currentTile, currentDir)
            };

            newTile = (newDir, currentTile) switch
            {
                (Direction.Up, '.') => '║',
                (Direction.Down, '.') => '║',
                (Direction.Left, '.') => '═',
                (Direction.Right, '.') => '═',
                _ => newTile
            };

            input[row][col] = newTile;

            return (newTile, newDir);
        }

        public static char[][] GetGrid(string[] input)
        {
            var result = new char[input.Length][];

            for (int i = 0; i < input.Length; i++)
            {
                result[i] = input[i].ToCharArray();
            }

            return result;
        }

        public static char[][] RescaleGrid(char[][] originalGrid, int scaleFactor)
        {
            var divide = scaleFactor < 0 ? true : false;
            scaleFactor = Math.Abs(scaleFactor);
            var newGrid = divide ? new char[originalGrid.Length / scaleFactor][] : new char[originalGrid.Length * scaleFactor][];
            for (int i = 0; i < newGrid.Length; i++)
            {
                newGrid[i] = divide ? new char[originalGrid[0].Length / scaleFactor] : new char[originalGrid[0].Length * scaleFactor];
                Array.Fill(newGrid[i], '.');
            }

            return newGrid;
        }

        public static void FloodFill(char[][] input, char target, char replacement)
        {
            var row = 0;
            var col = 0;
            var pos = (row, col);

            var q = new Queue<(int, int)>();
            q.Enqueue(pos);

            while (q.Count > 0)
            {
                var loc = q.Dequeue();
                var tile = input[loc.Item1][loc.Item2];
                if (tile == target)
                {
                    input[loc.Item1][loc.Item2] = replacement;

                    // West
                    if (loc.Item2 > 0)
                    {
                        q.Enqueue((loc.Item1, loc.Item2 - 1));
                    }

                    // East
                    if (loc.Item2 < input[0].Length - 1)
                    {
                        q.Enqueue((loc.Item1, loc.Item2 + 1));
                    }

                    // North
                    if (loc.Item1 > 0)
                    {
                        q.Enqueue((loc.Item1 - 1, loc.Item2));
                    }

                    // South
                    if (loc.Item1 < input.Length - 1)
                    {
                        q.Enqueue((loc.Item1 + 1, loc.Item2));
                    }
                }
            }
        }

        public static void FillRemainingOutsidePoints(char[][] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    char current = input[i][j];
                    if (current != '.')
                    {
                        continue;
                    }

                    var edgeCount = 0;
                    bool outside = true;

                    for (int k = j; k < input[i].Length; k++)
                    {
                        if (input[i][k] == '~' || input[i][k] == '.')
                        {
                            if (!outside)
                            {
                                edgeCount++;
                            }

                            outside = true;
                        }
                        else
                        {
                            if (outside)
                            {
                                edgeCount++;
                            }

                            outside = false;
                        }
                    }

                    Console.WriteLine(edgeCount % 2);
                }
            }
        }
    }
}
