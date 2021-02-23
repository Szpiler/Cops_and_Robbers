using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiZBoardModel
{
    public class Cell
    {
        // the number of row
        public int RowNumber { get; set; }

        // the number of column
        public int ColumnNumber { get; set; }

        // is the cell a building or a road
        public bool IsBulding { get; set; }

        // is there any charakter staying on the cell
        public int CurrentState { get; set; }        

        //constructor
        public Cell(int x, int y)
        {
            RowNumber = x;
            ColumnNumber = y;
        }
    }
}
