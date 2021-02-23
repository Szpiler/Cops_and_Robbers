using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiZBoardModel
{
    public class Board
    {
        // the size of the board usually 20x20
        public int Size { get; set; }

        // 2D array of type cell
        public Cell[,] Table { get; set; }

        // constructor
        public Board (int s)
        {
            // initial size of the board
            Size = s;

            // create a new 2D array of type cell
            Table = new Cell[Size, Size];

            // fill the 2D arry with new Cells with unique x and y coordinates
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Table[i, j] = new Cell(i, j);
                }
            }
        }
    }
}
