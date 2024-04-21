using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading;

namespace cli_life
{
    public class Board
    {
        public readonly Cell[,] Cells;
        public readonly int CellSize;

        public int Columns { get { return Cells.GetLength(1); } }
        public int Rows { get { return Cells.GetLength(0); } }
        public int Width { get { return Columns * CellSize; } }
        public int Height { get { return Rows * CellSize; } }

        public int WidthC { get; set; }
        public int HeightC { get; set; }
        public int CellSizeC { get; set; }
        public double LiveDensityC { get; set; }

        public Board(int widthC, int heightC, int cellSizeC, double liveDensityC)
        {
            CellSize = cellSizeC;

            Cells = new Cell[heightC / cellSizeC, widthC / cellSizeC];
            for (int x = 0; x < Rows; x++)
                for (int y = 0; y < Columns; y++)
                    Cells[x, y] = new Cell();

            ConnectNeighbors();
            Randomize(liveDensityC);
        }

        readonly Random rand = new Random();
        public void Randomize(double liveDensity)
        {
            foreach (var cell in Cells)
                cell.IsAlive = rand.NextDouble() < liveDensity;
        }

        public void Advance()
        {
            foreach (var cell in Cells)
                cell.DetermineNextLiveState();
            foreach (var cell in Cells)
                cell.Advance();
        }
        private void ConnectNeighbors()
        {
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    for (int i = (x == 0) ? Rows - 1 : x - 1, xNum = 0; xNum < 3; i++, xNum++)
                        for (int j = (y == 0) ? Columns - 1 : y - 1, yNum = 0; yNum < 3; j++, yNum++)
                            if (!(xNum == 1 && yNum == 1))
                                Cells[x, y].neighbors.Add(Cells[i < Rows ? i : i - Rows, j < Columns ? j : j - Columns]);
                }
            }
        }
    }
}
