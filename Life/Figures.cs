using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cli_life
{
    class Figure
    {
        string name;
        int rows;
        int columns;
        bool[][] matrix;

        public string Name { get { return name; } }
        public int Rows { get { return rows; } }
        public int Columns { get { return columns; } }
        public bool[][] Matrix { get { return matrix; } }

        public Figure(string inputName, int inputRows, int inputColumns, string[] inputMatrix)
        {
            name = inputName;
            rows = inputRows;
            columns = inputColumns;
            matrix = new bool[rows][];
            for (int i = 0; i < rows; i++)
            {
                matrix[i] = new bool[columns];
                for (int j = 0; j < columns; j++)
                    matrix[i][j] = inputMatrix[i][j] == '*';
            }
        }
    }
}
