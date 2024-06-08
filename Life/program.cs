using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text.Json;

namespace cli_life
{
    public struct Data {
        public int width { get; set; }
        public int height { get; set; }
        public int cellSize { get; set; }
        public double liveDensity { get; set; } 
    }
    public class Cell
    {
        public bool IsAlive;
        public readonly List<Cell> neighbors = new List<Cell>();
        private bool IsAliveNext;
        public void DetermineNextLiveState()
        {
            int liveNeighbors = neighbors.Where(x => x.IsAlive).Count();
            if (IsAlive)
                IsAliveNext = liveNeighbors == 2 || liveNeighbors == 3;
            else
                IsAliveNext = liveNeighbors == 3;
        }
        public void Advance()
        {
            IsAlive = IsAliveNext;
        }
    }
    public class Board
    {
        public readonly Cell[,] Cells;
        public readonly int CellSize;

        public int Columns { get { return Cells.GetLength(0); } }
        public int Rows { get { return Cells.GetLength(1); } }
        public int Width { get { return Columns * CellSize; } }
        public int Height { get { return Rows * CellSize; } }

        public Board(int width, int height, int cellSize, double liveDensity = .1)
        {
            CellSize = cellSize;

            Cells = new Cell[width / cellSize, height / cellSize];
            for (int x = 0; x < Columns; x++)
                for (int y = 0; y < Rows; y++)
                    Cells[x, y] = new Cell();

            ConnectNeighbors();
        }

        readonly Random rand = new Random();
        public void Randomize(string filename)
        {
            string json = File.ReadAllText(filename);
            Data data = JsonSerializer.Deserialize<Data>(json);
            double liveDensity = data.liveDensity;
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
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    int xL = (x > 0) ? x - 1 : Columns - 1;
                    int xR = (x < Columns - 1) ? x + 1 : 0;

                    int yT = (y > 0) ? y - 1 : Rows - 1;
                    int yB = (y < Rows - 1) ? y + 1 : 0;

                    Cells[x, y].neighbors.Add(Cells[xL, yT]);
                    Cells[x, y].neighbors.Add(Cells[x, yT]);
                    Cells[x, y].neighbors.Add(Cells[xR, yT]);
                    Cells[x, y].neighbors.Add(Cells[xL, y]);
                    Cells[x, y].neighbors.Add(Cells[xR, y]);
                    Cells[x, y].neighbors.Add(Cells[xL, yB]);
                    Cells[x, y].neighbors.Add(Cells[x, yB]);
                    Cells[x, y].neighbors.Add(Cells[xR, yB]);
                }
            }
        }

        public void Load(string filename) {
            string[] info = File.ReadAllLines(filename);
            char[][] board = new char[Rows][];
            for (int i = 0; i < info.Length; i++)
            {
                board[i] = new char[Columns];
                for (int j = 0; j < Rows; j++)
                {
                    board[i][j] = info[i][j];
                }
            }
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (board[i][j] == '*')
                    {
                        Cells[i,j].IsAlive = true;
                    }
                }
            }
        }

        public void Save() {
            char[][] board = new char[20][];
            for (int k = 0; k < 20; k++)
            {
                board[k] = new char[50];
            }
            for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)   
                    {
                        var cell = Cells[j, i];
                        if (cell.IsAlive)
                        {
                            board[i][j] = '*';
                        }
                        else
                        {
                            board[i][j] = ' ';
                        }
                    }
                }
            File.Create(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/systemState.txt"))).Close();
            using (StreamWriter writer = new StreamWriter(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/systemState.txt")), true))
            {
                for (int i = 0; i < board.Length; i++)
                {
                    string info = new string(board[i]);
                    writer.WriteLineAsync(info);
                }
            }
        }
        public int BlocksAmount()
        {
            int num = 0;
            for (int i = 1; i < Rows - 2; i++)
            {
                for (int j = 1; j < Columns - 2; j++)
                {
                    if (Cells[j, i].IsAlive && Cells[j, i + 1].IsAlive && Cells[j + 1, i].IsAlive && Cells[j + 1, i + 1].IsAlive)
                    {
                        if (!Cells[j - 1, i - 1].IsAlive && !Cells[j, i - 1].IsAlive && !Cells[j + 1, i - 1].IsAlive && !Cells[j + 2, i - 1].IsAlive 
                        && !Cells[j - 1, i + 2].IsAlive && !Cells[j, i + 2].IsAlive && !Cells[j + 1, i + 2].IsAlive && !Cells[j + 2, i + 2].IsAlive 
                        && !Cells[j - 1, i].IsAlive && !Cells[j + 2, i].IsAlive && !Cells[j - 1, i + 2].IsAlive && !Cells[j + 2, i + 2].IsAlive)
                        {
                            num++;
                        }
                    }
                }
            }
            return num;
        }

        public int BoxesAmount()
        {
            int num = 0;
            for (int i = 0; i < Rows - 2; i++)
            {
                for (int j = 1; j < Columns - 1; j++)
                {
                    if (Cells[j, i].IsAlive && Cells[j - 1, i + 1].IsAlive && Cells[j + 1, i + 1].IsAlive && Cells[j, i + 2].IsAlive 
                    && !Cells[j, i + 1].IsAlive && !Cells[j - 1, i].IsAlive && !Cells[j + 1, i].IsAlive && !Cells[j - 1, i + 2].IsAlive && !Cells[j + 1, i + 2].IsAlive)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int HivesAmount()
        {
            int num = 0;
            for (int i = 0; i < Rows - 3; i++)
            {
                for (int j = 1; j < Columns - 1; j++)
                {
                    if (Cells[j, i].IsAlive && Cells[j - 1, i + 1].IsAlive && Cells[j - 1, i + 2].IsAlive 
                    && Cells[j, i + 3].IsAlive && Cells[j + 1, i + 1].IsAlive && Cells[j + 1, i + 2].IsAlive 
                    && !Cells[j, i + 1].IsAlive && !Cells[j, i + 2].IsAlive && !Cells[j - 1, i].IsAlive 
                    && !Cells[j + 1, i].IsAlive && !Cells[j - 1, i + 3].IsAlive && !Cells[j + 1, i + 3].IsAlive)
                    {
                        num++;
                    }
                }
            }
            return num;
        }

        public int SymmetryFiguresAmount()
        {
            return BoxesAmount() + HivesAmount()+ BlocksAmount() ;
        }
    }
    public class LifeGame
    {
        public static Board board;
        private int Reset(string settings, string filename)
        {
            string json = File.ReadAllText(settings);
            Data data = JsonSerializer.Deserialize<Data>(json);
            int width = data.width;
            int height = data.height;
            int cellSize = data.cellSize;
            double liveDensity = data.liveDensity;
            board = new Board(width, height, cellSize, liveDensity);
            board.Load(filename);
            return board.Width * board.Height;
        }

        public int Render()
        {
            int count = 0;
            for (int i = 0; i < board.Rows; i++)
            {
                for (int j = 0; j < board.Columns; j++)   
                {
                    var cell = board.Cells[j, i];
                    if (cell.IsAlive)
                    {
                        Console.Write('*');
                        count++;
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.Write('\n');
            }
            return count;
        }

        public void SaveToFile()
        {
            board.Save();
        }

        public (int allCells, int aliveCells, int Iters) Run(string settings, string filename)
        {
            int[] list = {-1, -1, -1, -1, -1};
            int iters = 0;
            int alive_cells = 0;
            int all_cells = 0;

            all_cells = Reset(settings, filename);

            while(true)
            {
                iters++;
                Console.Clear();
                alive_cells = Render();
                list[iters % 5] = alive_cells;
                if ((list[0] == list[1]) && (list[0] == list[2]) && (list[0] == list[3]) && (list[0] == list[4]))
                {
                    break;
                }
                board.Advance();
                Thread.Sleep(100);
            }

            Console.WriteLine("\n\tBlocks: " + board.BlocksAmount());
            Console.WriteLine("\tBoxes: " + board.BoxesAmount());
            Console.WriteLine("\tHives: " + board.HivesAmount());

            (int, int, int) cells = (all_cells, alive_cells, iters-2);
            return cells;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            LifeGame life = new LifeGame();
            var cells = life.Run(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/settings.json")), Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/example3.txt")));

            Console.Write("\n\tAlive Cells: " + cells.aliveCells);
            Console.Write("\n\tDied Cells: " + (cells.allCells - cells.aliveCells));
            Console.Write("\n\tAlive density: " + ((double)cells.aliveCells / cells.allCells));
            Console.Write("\n\tDied density: " + ((double)(cells.allCells - cells.aliveCells)/cells.allCells));
            Console.Write("\n\n\tStability on " + (cells.Iters) + " iteration.\n\n");
            Thread.Sleep(1000);
            life.SaveToFile();
        }
    }
}
