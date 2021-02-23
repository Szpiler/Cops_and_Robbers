using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PiZBoardModel
{
    public class Character 
    {
        public int Team { get; set; }
        public int CharacterNumber { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public Character(int t, int a, int x, int y, int p)
        {
            Team = t;
            CharacterNumber = a;
            PositionX = x;
            PositionY = y;
        }       

        // method that changing position of the character
        public bool Move(int direction, Board myBoard)
        {                     
            // move up
            if (direction == 1)
            {
                if (PositionX != (myBoard.Size - myBoard.Size))
                {
                    if (myBoard.Table[PositionX - 1, PositionY].IsBulding == false)
                    {
                        myBoard.Table[PositionX - 1, PositionY].CurrentState = CharacterNumber;
                        myBoard.Table[PositionX, PositionY].CurrentState = 0;
                        PositionX = PositionX - 1;                       
                        return true;
                    }
                }
            }

            // move down
            else if (direction == 2)
            {
                if (PositionX != (myBoard.Size - 1))
                {
                    if (myBoard.Table[PositionX + 1, PositionY].IsBulding == false)
                    {
                        myBoard.Table[PositionX + 1, PositionY].CurrentState = CharacterNumber;
                        myBoard.Table[PositionX, PositionY].CurrentState = 0;
                        PositionX = PositionX + 1;                        
                        return true;
                    }
                }
            }

            // move left
            else if (direction == 3)
            {
                if (PositionY != (myBoard.Size - myBoard.Size))
                {
                    if (myBoard.Table[PositionX, PositionY - 1].IsBulding == false)
                    {
                        myBoard.Table[PositionX, PositionY - 1].CurrentState = CharacterNumber;
                        myBoard.Table[PositionX, PositionY].CurrentState = 0;
                        PositionY = PositionY - 1;                       
                        return true;
                    }
                }
            }

            // move right
            else if (direction == 4)
            {
                if (PositionY != (myBoard.Size - 1))
                {
                    if (myBoard.Table[PositionX, PositionY + 1].IsBulding == false)
                    {
                        myBoard.Table[PositionX, PositionY + 1].CurrentState = CharacterNumber;
                        myBoard.Table[PositionX, PositionY].CurrentState = 0;
                        PositionY = PositionY + 1;                      
                        return true;
                    }
                }
            }            
            return false;            
        }
    }
}
