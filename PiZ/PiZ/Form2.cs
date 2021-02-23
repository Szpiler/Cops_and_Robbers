using PiZBoardModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PiZ
{
    public partial class Form2 : Form
    {
        // reference to the class Board. Contains the values of the board
        static Board myBoard = new Board(20);

        // second reference to the class Board. Contains the values of the board. This is ancillary board  
        static Board anciBoard = new Board(20);

        // 2D array of pictures that's size is equal to myBoard
        public PictureBox[,] imageBoard = new PictureBox[myBoard.Size, myBoard.Size];        

        //variable that can 1 or 2 depending on game mode(1 - thief team, 2 - police team)
        public int gameMode;

        //variable that determines whose turn it is
        public int tour = 1;

        //object that describes thief position, team and skin style 
        public Character thief;

        //object that describes policeman1 position, team and skin style 
        public Character policeman1;

        //object that describes policeman2 position, team and skin style 
        public Character policeman2;

        //tour counter
        public int tourCounter = 0;

        public Form2(int pol1, int pol2, int thi, int gm)
        {
            policeman1 = new Character(0, pol1, 0, 2, 2);
            policeman2 = new Character(0, pol2, 0, 14, 2);
            thief = new Character(0, thi, 2, 12, 1);
            gameMode = gm;
            InitializeComponent();
            KeyPreview = true;
            fillImageBoard();           
        }

        private void fillImageBoard()
        {
            if(gameMode == 1)
            {
                label4.Text = "Zlodziej";
                if (thief.CharacterNumber == 1)
                {
                    pictureBox2.Image = Properties.Resources.Adam_złodziej;
                }
                else
                {
                    pictureBox2.Image = Properties.Resources.Jaca_złodziej;
                }
            }
            else if(gameMode == 2)
            {
                label4.Text = "Policjant1";
                if (policeman1.CharacterNumber == 1)
                {
                    pictureBox2.Image = Properties.Resources.Policjant_Filip;
                }
                else
                {
                    pictureBox2.Image = Properties.Resources.Policjant_Piotrek;
                }
            }
            int pictureSize = panel1.Width / myBoard.Size;
            panel1.Height = panel1.Width;           
            int counter = 0;
            for (int i = 0; i < myBoard.Size; i++)
            {
                for (int j = 0; j < myBoard.Size; j++)
                {                   
                    imageBoard[i, j] = new PictureBox();
                    imageBoard[i, j].Height = pictureSize;
                    imageBoard[i, j].Width = pictureSize;
                    if(Properties.Resources.Board1[counter] == '0')
                    {
                        imageBoard[i, j].Image = Properties.Resources.Droga_kwadrat_biały;
                        myBoard.Table[i, j].IsBulding = false;
                        myBoard.Table[i, j].CurrentState = 0;

                        // ancillary board
                        anciBoard.Table[i, j].CurrentState = 0;
                        anciBoard.Table[i, j].IsBulding = false;
                    }                       
                    else if (Properties.Resources.Board1[counter] == '1')
                    {
                        imageBoard[i, j].Image = Properties.Resources.Budynek_kwadrat_ciemny;
                        myBoard.Table[i, j].IsBulding = true;
                        myBoard.Table[i, j].CurrentState = 1;

                        // ancillary board
                        anciBoard.Table[i, j].CurrentState = 1;
                        anciBoard.Table[i, j].IsBulding = true;
                    }
                    else if (Properties.Resources.Board1[counter] == '3')
                    {                        
                        myBoard.Table[i, j].IsBulding = false;
                        myBoard.Table[i, j].CurrentState = 3;

                        // ancillary board
                        anciBoard.Table[i, j].CurrentState = 0;
                        anciBoard.Table[i, j].IsBulding = false;
                        if (thief.CharacterNumber == 1)
                            imageBoard[i, j].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                        else
                            imageBoard[i, j].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                    }
                    else if (Properties.Resources.Board1[counter] == '2')
                    {                       
                        myBoard.Table[i, j].IsBulding = false;
                        myBoard.Table[i, j].CurrentState = 2;

                        // ancillary board
                        anciBoard.Table[i, j].CurrentState = 0;
                        anciBoard.Table[i, j].IsBulding = false;
                        if (policeman1.CharacterNumber == 1)
                            imageBoard[i, j].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                        else
                            imageBoard[i, j].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                    }
                    else if (Properties.Resources.Board1[counter] == '4')
                    {                       
                        myBoard.Table[i, j].IsBulding = false;
                        myBoard.Table[i, j].CurrentState = 4;

                        // ancillary board
                        anciBoard.Table[i, j].CurrentState = 0;
                        anciBoard.Table[i, j].IsBulding = false;
                        if (policeman2.CharacterNumber == 1)
                            imageBoard[i, j].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                        else
                            imageBoard[i, j].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                    }
                    imageBoard[i, j].SizeMode = PictureBoxSizeMode.Zoom;        
                    panel1.Controls.Add(imageBoard[i, j]);
                    imageBoard[i, j].Location = new Point(j * pictureSize, i * pictureSize);
                    counter++;
                }
                counter += 2;
            }
        }

        // function that checks if the cell is in range of the board
        private static bool isValid(int row, int col)
        {
            // return true if row number and column number is in range 
            return (row >= 0) && (row < myBoard.Size) &&
                   (col >= 0) && (col < myBoard.Size);
        }

        // function that clears all the changes that function BestMoveFinder did in the anciBoard.Table except position of the starting cell
        private void anciBoardCleaner()
        {
            for(int i = 0; i < myBoard.Size; i++)
            {
                for(int j = 0; j < myBoard.Size; j++)
                {
                    if (anciBoard.Table[i, j].CurrentState != 1 && anciBoard.Table[i, j].CurrentState != -1 && anciBoard.Table[i, j].CurrentState != 9)
                        anciBoard.Table[i, j].CurrentState = 0;
                }
            }
        }

        // function that returns true if checking direction has already reached the destination cell in BestMoveFinder function
        private static bool hasReached(int direction, int[] reached)
        {
            foreach(int obj in reached)
            {
                if (obj == direction)
                    return true;
            }
            return false;
        }

        /* function that's finding the best move for policeman and thief by using BFS algorithm and returns array that stores in first cell best move for policeman
        and in second best momve for thief */
        private int[] BestMoveFinder(Character start, Character destination)
        {
            anciBoardCleaner();
            int[] bestAndWorstMove = new int[2];
            Queue<Cell> q = new Queue<Cell>();
            Cell s = new Cell(start.PositionX, start.PositionY);
            q.Enqueue(s);
            anciBoard.Table[start.PositionX, start.PositionY].CurrentState = -1;          
            int toFirstadjcell = 0;
            int[] checkedCell = new int[4] { 0, 0, 0, 0 };          
            int temp = 0;
            int reachedDest = 0;           
            while(q.Count != 0)
            {               
                Cell curr = q.Peek();              
                if (curr.RowNumber == destination.PositionX && curr.ColumnNumber == destination.PositionY)
                {
                    checkedCell[reachedDest] = anciBoard.Table[curr.RowNumber, curr.ColumnNumber].CurrentState;
                    reachedDest++;
                    if (reachedDest == toFirstadjcell)
                    {
                        bestAndWorstMove[0] = checkedCell[0];
                        bestAndWorstMove[1] = checkedCell[reachedDest - 1];
                        anciBoard.Table[start.PositionX, start.PositionY].CurrentState = 0;
                        return bestAndWorstMove;
                    }
                    else
                    {
                        q.Clear();
                        anciBoardCleaner();                       
                        q.Enqueue(s);
                        curr = q.Peek();
                    }                   
                }                                 
                q.Dequeue();
                for(int i = 0; i < 4; i++)
                {
                    int row = new int();
                    int col = new int();
                    if (i == 0)
                    {
                        row = curr.RowNumber - 1;
                        col = curr.ColumnNumber;
                    }
                    else if(i == 1)
                    {
                        row = curr.RowNumber;
                        col = curr.ColumnNumber + 1;
                    }
                    else if (i == 2)
                    {
                        row = curr.RowNumber + 1;
                        col = curr.ColumnNumber;
                    }
                    else if (i == 3)
                    {
                        row = curr.RowNumber;
                        col = curr.ColumnNumber - 1;
                    }
                    if (isValid(row, col) && anciBoard.Table[row, col].CurrentState == 0)
                    {                       
                        if(anciBoard.Table[curr.RowNumber, curr.ColumnNumber].CurrentState == -1)
                        {
                            if(i == 0)
                            {
                                if(reachedDest > 0)
                                {
                                    if(!hasReached(2, checkedCell))
                                    {
                                        anciBoard.Table[row, col].CurrentState = 2;                                       
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    anciBoard.Table[row, col].CurrentState = 2;
                                    if(temp == 0)
                                       toFirstadjcell++;
                                }                               
                            }
                            else if(i == 1)
                            {
                                if (reachedDest > 0)
                                {
                                    if (!hasReached(3, checkedCell))
                                    {
                                        anciBoard.Table[row, col].CurrentState = 3;                                       
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    anciBoard.Table[row, col].CurrentState = 3;
                                    if (temp == 0)
                                        toFirstadjcell++;
                                }                               
                            }
                            else if (i == 2)
                            {
                                if (reachedDest > 0)
                                {
                                    if (!hasReached(4, checkedCell))
                                    {
                                        anciBoard.Table[row, col].CurrentState = 4;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    anciBoard.Table[row, col].CurrentState = 4;
                                    if (temp == 0)
                                        toFirstadjcell++;
                                }                                   
                            }
                            else if (i == 3)
                            {
                                if (reachedDest > 0)
                                {
                                    if (!hasReached(5, checkedCell))
                                    {
                                        anciBoard.Table[row, col].CurrentState = 5;
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    anciBoard.Table[row, col].CurrentState = 5;
                                    if (temp == 0)
                                        toFirstadjcell++;
                                }                                   
                            }
                        }
                        else if(anciBoard.Table[curr.RowNumber, curr.ColumnNumber].CurrentState == 2)
                        {
                            anciBoard.Table[row, col].CurrentState = 2;                           
                        }
                        else if (anciBoard.Table[curr.RowNumber, curr.ColumnNumber].CurrentState == 3)
                        {
                            anciBoard.Table[row, col].CurrentState = 3;
                        }
                        else if (anciBoard.Table[curr.RowNumber, curr.ColumnNumber].CurrentState == 4)
                        {
                            anciBoard.Table[row, col].CurrentState = 4;
                        }
                        else if (anciBoard.Table[curr.RowNumber, curr.ColumnNumber].CurrentState == 5)
                        {
                            anciBoard.Table[row, col].CurrentState = 5;
                        }                        
                        Cell adjcell = new Cell(row, col);                       
                        q.Enqueue(adjcell);
                    }                   
                }
                temp++;
            }
            anciBoardCleaner();
            int temp2 = 0;
            Stack<int> stack = new Stack<int>();
            Random rng = new Random();
            bestAndWorstMove[0] = checkedCell[0];
            if (reachedDest > 0)
            {
                if(reachedDest > 1)
                {
                    bestAndWorstMove[1] = checkedCell[reachedDest - 1];
                }
                else if(toFirstadjcell > 1)
                {
                    if (isValid(start.PositionX - 1, start.PositionY) && anciBoard.Table[start.PositionX - 1, start.PositionY].CurrentState == 0 && checkedCell[0] != 2 && checkedCell[1] != 2 && checkedCell[2] != 2 && checkedCell[3] != 2)
                    {
                        stack.Push(2);
                        temp2++;
                    }
                    if (isValid(start.PositionX, start.PositionY + 1) && anciBoard.Table[start.PositionX, start.PositionY + 1].CurrentState == 0 && checkedCell[0] != 3 && checkedCell[1] != 3 && checkedCell[2] != 3 && checkedCell[3] != 3)
                    {
                        stack.Push(3);
                        temp2++;
                    }
                    if (isValid(start.PositionX + 1, start.PositionY) && anciBoard.Table[start.PositionX + 1, start.PositionY].CurrentState == 0 && checkedCell[0] != 4 && checkedCell[1] != 4 && checkedCell[2] != 4 && checkedCell[3] != 4)
                    {
                        stack.Push(4);
                        temp2++;
                    }
                    if (isValid(start.PositionX, start.PositionY - 1) && anciBoard.Table[start.PositionX, start.PositionY - 1].CurrentState == 0 && checkedCell[0] != 5 && checkedCell[1] != 5 && checkedCell[2] != 5 && checkedCell[3] != 5)
                    {
                        stack.Push(5);
                        temp2++;
                    }
                    temp2 = rng.Next(0, temp2);                   
                    for (int i = 0; i < temp2; i++)
                    {
                        stack.Pop();
                    }
                    bestAndWorstMove[1] = stack.Pop();
                }
                else
                {
                    bestAndWorstMove[1] = checkedCell[0];
                }
            }
            else
            {
                if (isValid(start.PositionX - 1, start.PositionY) && anciBoard.Table[start.PositionX - 1, start.PositionY].CurrentState == 0)
                {
                    stack.Push(2);
                    temp2++;
                }
                if (isValid(start.PositionX, start.PositionY + 1) && anciBoard.Table[start.PositionX, start.PositionY + 1].CurrentState == 0)
                {
                    stack.Push(3);
                    temp2++;
                }
                if (isValid(start.PositionX + 1, start.PositionY) && anciBoard.Table[start.PositionX + 1, start.PositionY].CurrentState == 0)
                {                  
                    stack.Push(4);
                    temp2++;
                }
                if (isValid(start.PositionX, start.PositionY - 1) && anciBoard.Table[start.PositionX, start.PositionY - 1].CurrentState == 0)
                {                   
                    stack.Push(5);
                    temp2++;
                }
                temp2 = rng.Next(0, temp2);
                for(int i = 0; i < temp2; i++)
                {
                    stack.Pop();
                }
                bestAndWorstMove[0] = stack.Pop();
                bestAndWorstMove[1] = bestAndWorstMove[0];
            }
            anciBoard.Table[start.PositionX, start.PositionY].CurrentState = 0;
            return bestAndWorstMove;
        }

        //function that's check if 2 policemen are in the same road
        //if they are, the function makes the wall in anciBoard.Table on the perpendicular way between policemen
        //in the cell adjacent to the moving policeman

        private void PolicemanChecker (Character movingP, Character P2)
        {
            int temp = 0;
            int temp2 = 0;
            int temp3 = 0;
            bool isWall = false;
            bool isThief = false;
            if(movingP.PositionX == P2.PositionX)
            {
                if (movingP.PositionY > P2.PositionY)
                {
                    temp3 = -1;
                    temp2 = movingP.PositionY;
                    temp = P2.PositionY + 1;
                }
                else
                {
                    temp3 = 1;
                    temp2 = P2.PositionY;
                    temp = movingP.PositionY + 1;
                }  
                for(int i = temp; i < temp2; i++)
                {
                    if (myBoard.Table[movingP.PositionX, i].CurrentState == 1)
                    {
                        isWall = true;
                    }
                    else if(myBoard.Table[movingP.PositionX, i].CurrentState == 3)
                    {
                        isThief = true;
                    }
                }
            }
            else if(movingP.PositionY == P2.PositionY)
            {
                if (movingP.PositionX > P2.PositionX)
                {
                    temp3 = -1;
                    temp2 = movingP.PositionX;
                    temp = P2.PositionX + 1;
                }
                else
                {
                    temp3 = 1;
                    temp2 = P2.PositionX;
                    temp = movingP.PositionX + 1;
                }
                for (int i = temp; i < temp2; i++)
                {
                    if (myBoard.Table[i, movingP.PositionY].CurrentState == 1)
                    {
                        isWall = true;
                    }
                    else if (myBoard.Table[i, movingP.PositionY].CurrentState == 3)
                    {
                        isThief = true;
                    }
                }
            }
            if((movingP.PositionX == P2.PositionX || movingP.PositionY == P2.PositionY) && isThief == false && isWall == false)
            {
                if(movingP.PositionX == P2.PositionX)
                {
                    anciBoard.Table[movingP.PositionX, movingP.PositionY + temp3].CurrentState = 9;
                }
                else if (movingP.PositionY == P2.PositionY)
                {
                    anciBoard.Table[movingP.PositionX + temp3, movingP.PositionY].CurrentState = 9;
                }                
            }
        }

        //function that's deleting wall made by function PolicemanChecker 

        private void WallCleaner ()
        {
            for (int i = 0; i < anciBoard.Size; i++)
            {
                for(int j = 0; j < anciBoard.Size; j++)
                {
                    if (anciBoard.Table[i, j].CurrentState == 9)
                        anciBoard.Table[i, j].CurrentState = 0;
                }
            }
        }

        //function that's checking distance between thief and policeman1 and thief and policeman2 and decides wich one of policeman is closer to thief
        //and then with help of BestMoveFinder find and return best move for thief

        private int ThiefMoveFinder()
        {
            anciBoardCleaner();
            int[] move = new int[2] { 0, 0 };                    
            Queue<Cell> q = new Queue<Cell>();
            Cell s = new Cell(thief.PositionX, thief.PositionY);           
            q.Enqueue(s);
            anciBoard.Table[thief.PositionX, thief.PositionY].CurrentState = -1;
            while(q.Count != 0)
            {
                Cell curr = q.Peek();
                if(curr.RowNumber == policeman1.PositionX && curr.ColumnNumber == policeman1.PositionY)
                {
                    anciBoard.Table[thief.PositionX, thief.PositionY].CurrentState = 0;
                    move = BestMoveFinder(thief, policeman1);
                    return move[1];
                }
                else if(curr.RowNumber == policeman2.PositionX && curr.ColumnNumber == policeman2.PositionY)
                {
                    anciBoard.Table[thief.PositionX, thief.PositionY].CurrentState = 0;
                    move = BestMoveFinder(thief, policeman2);
                    return move[1];
                }
                q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int row = new int();
                    int col = new int();
                    if (i == 0)
                    {
                        row = curr.RowNumber - 1;
                        col = curr.ColumnNumber;
                    }
                    else if (i == 1)
                    {
                        row = curr.RowNumber;
                        col = curr.ColumnNumber + 1;
                    }
                    else if (i == 2)
                    {
                        row = curr.RowNumber + 1;
                        col = curr.ColumnNumber;
                    }
                    else if (i == 3)
                    {
                        row = curr.RowNumber;
                        col = curr.ColumnNumber - 1;
                    }
                    if (isValid(row, col) && anciBoard.Table[row, col].CurrentState == 0)
                    {
                        anciBoard.Table[row, col].CurrentState = 2;
                        Cell adjcell = new Cell(row, col);
                        q.Enqueue(adjcell);
                    }
                }
            }
            return move[1];
        }

        //Closeing form button
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        //Function taht checks if thief or policemen win
        private void WinChecker()
        {
            if((policeman1.PositionX == thief.PositionX && policeman1.PositionY == thief.PositionY) || (policeman2.PositionX == thief.PositionX && policeman2.PositionY == thief.PositionY))
            {
                this.Close();
                Form3 VILAS = new Form3(0);
                VILAS.Show();
            }
            else if (tourCounter == 150)
            {
                this.Close();
                Form3 VILAS = new Form3(1);
                VILAS.Show();
            }
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            int x, y;
            bool didPlayerMove = false;

            //thief team
            if (gameMode == 1)
            {
                x = thief.PositionX;
                y = thief.PositionY;
                if (e.KeyData == Keys.W)
                {
                    if (thief.Move(1, myBoard))
                    {
                        didPlayerMove = true;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 2;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 2;
                        }
                    }
                }
                else if (e.KeyCode == Keys.S)
                {
                    if (thief.Move(2, myBoard))
                    {
                        didPlayerMove = true;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 2;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 2;
                        }
                    }
                }
                else if (e.KeyData == Keys.A)
                {
                    if (thief.Move(3, myBoard))
                    {
                        didPlayerMove = true;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 2;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 2;
                        }
                    }
                }
                else if (e.KeyData == Keys.D)
                {
                    if (thief.Move(4, myBoard))
                    {
                        didPlayerMove = true;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 2;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 2;
                        }
                    }
                }
                if (didPlayerMove)
                {
                    tourCounter++;
                    label2.Text = tourCounter.ToString();
                    WinChecker();                         
                    if (policeman1.CharacterNumber == 1)
                    {
                        pictureBox2.Image = Properties.Resources.Policjant_Filip;
                        label4.Text = "Policjant1";
                    }
                    else
                    {
                        pictureBox2.Image = Properties.Resources.Policjant_Piotrek;
                        label4.Text = "Policjant1";
                    }
                    x = policeman1.PositionX;
                    y = policeman1.PositionY;
                    int[] tab = new int[2];
                    PolicemanChecker(policeman1, policeman2);
                    tab = BestMoveFinder(policeman1, thief);
                    WallCleaner();
                    if (tab[0] == 2)
                    {
                        policeman1.PositionX--;
                        if (policeman1.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                            tour = 3;
                        }
                    }
                    else if (tab[0] == 3)
                    {
                        policeman1.PositionY++;
                        if (policeman1.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                            tour = 3;
                        }
                    }
                    else if (tab[0] == 4)
                    {
                        policeman1.PositionX++;
                        if (policeman1.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                            tour = 3;
                        }
                    }
                    else if (tab[0] == 5)
                    {
                        policeman1.PositionY--;
                        if (policeman1.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                            tour = 3;
                        }
                    }
                    tourCounter++;
                    label2.Text = tourCounter.ToString();
                    WinChecker();
                    if (policeman1.CharacterNumber == 1)
                    {
                        pictureBox2.Image = Properties.Resources.Policjant_Korczak;
                        label4.Text = "Policjant2";
                    }
                    else
                    {
                        pictureBox2.Image = Properties.Resources.Policjant_Karol;
                        label4.Text = "Policjant2";
                    }
                    x = policeman2.PositionX;
                    y = policeman2.PositionY;
                    PolicemanChecker(policeman2, policeman1);
                    tab = BestMoveFinder(policeman2, thief);
                    WallCleaner();
                    if (tab[0] == 2)
                    {
                        policeman2.PositionX--;
                        if (policeman2.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                            tour = 3;
                        }
                    }
                    else if (tab[0] == 3)
                    {
                        policeman2.PositionY++;
                        if (policeman2.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                            tour = 3;
                        }
                    }
                    else if (tab[0] == 4)
                    {
                        policeman2.PositionX++;
                        if (policeman2.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                            tour = 3;
                        }
                    }
                    else if (tab[0] == 5)
                    {
                        policeman2.PositionY--;
                        if (policeman2.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                            tour = 3;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                            tour = 3;
                        }
                    }
                    tourCounter++;
                    label2.Text = tourCounter.ToString();
                    WinChecker();
                    if (thief.CharacterNumber == 1)
                    {
                        pictureBox2.Image = Properties.Resources.Adam_złodziej;
                        label4.Text = "Zlodziej";
                    }
                    else
                    {
                        pictureBox2.Image = Properties.Resources.Jaca_złodziej;
                        label4.Text = "Zlodziej";
                    }
                }
            }

            //police team
            else if (gameMode == 2)
            {
                if (tour == 1)
                {
                    x = policeman1.PositionX;
                    y = policeman1.PositionY;
                    if (e.KeyData == Keys.W)
                    {
                        if (policeman1.Move(1, myBoard))
                        {
                            tourCounter++;
                            label2.Text = tourCounter.ToString();
                            WinChecker();
                            if (policeman2.CharacterNumber == 1)
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Korczak;
                                label4.Text = "Policjant2";
                            }
                            else
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Karol;
                                label4.Text = "Policjant2";
                            }
                            if (policeman1.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                                tour = 2;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                                tour = 2;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.S)
                    {
                        if (policeman1.Move(2, myBoard))
                        {
                            tourCounter++;
                            label2.Text = tourCounter.ToString();
                            WinChecker();
                            if (policeman2.CharacterNumber == 1)
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Korczak;
                                label4.Text = "Policjant2";
                            }
                            else
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Karol;
                                label4.Text = "Policjant2";
                            }
                            if (policeman1.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                                tour = 2;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                                tour = 2;
                            }
                        }
                    }
                    else if (e.KeyData == Keys.A)
                    {
                        if (policeman1.Move(3, myBoard))
                        {
                            tourCounter++;
                            label2.Text = tourCounter.ToString();
                            WinChecker();
                            if (policeman2.CharacterNumber == 1)
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Korczak;
                                label4.Text = "Policjant2";
                            }
                            else
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Karol;
                                label4.Text = "Policjant2";
                            }
                            if (policeman1.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                                tour = 2;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                                tour = 2;
                            }
                        }
                    }
                    else if (e.KeyData == Keys.D)
                    {
                        if (policeman1.Move(4, myBoard))
                        {
                            tourCounter++;
                            label2.Text = tourCounter.ToString();
                            WinChecker();
                            if (policeman2.CharacterNumber == 1)
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Korczak;
                                label4.Text = "Policjant2";
                            }
                            else
                            {
                                pictureBox2.Image = Properties.Resources.Policjant_Karol;
                                label4.Text = "Policjant2";
                            }
                            if (policeman1.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Filip_z_tłemb;
                                tour = 2;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman1.PositionX, policeman1.PositionY].Image = Properties.Resources.Policjant_Piotrek_z_tłemb_40x40;
                                tour = 2;
                            }
                        }
                    }                   
                }               
                else if (tour == 2)
                {                   
                    x = policeman2.PositionX;
                    y = policeman2.PositionY;
                    if (e.KeyData == Keys.W)
                    {
                        if (policeman2.Move(1, myBoard))
                        {
                            if (policeman2.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                                tour = 3;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                                tour = 3;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.S)
                    {
                        if (policeman2.Move(2, myBoard))
                        {
                            if (policeman2.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                                tour = 3;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                                tour = 3;
                            }
                        }
                    }
                    else if (e.KeyData == Keys.A)
                    {
                        if (policeman2.Move(3, myBoard))
                        {
                            if (policeman2.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                                tour = 3;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                                tour = 3;
                            }
                        }
                    }
                    else if (e.KeyData == Keys.D)
                    {
                        if (policeman2.Move(4, myBoard))
                        {
                            if (policeman2.CharacterNumber == 1)
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Korczak_z_tłemb;
                                tour = 3;
                            }
                            else
                            {
                                imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                                imageBoard[policeman2.PositionX, policeman2.PositionY].Image = Properties.Resources.Policjant_Karol_z_tłemb;
                                tour = 3;
                            }
                        }
                    }                   
                }
                if(tour == 3)
                {
                    tourCounter++;
                    label2.Text = tourCounter.ToString();
                    WinChecker();
                    if (thief.CharacterNumber == 1)
                    {
                        pictureBox2.Image = Properties.Resources.Adam_złodziej;
                        label4.Text = "Zlodziej";
                    }
                    else
                    {
                        pictureBox2.Image = Properties.Resources.Jaca_złodziej;
                        label4.Text = "Zlodziej";
                    }
                    x = thief.PositionX;
                    y = thief.PositionY;
                    int thiefMove = 0;
                    thiefMove = ThiefMoveFinder();
                    if (thiefMove == 2)
                    {
                        thief.PositionX--;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 1;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 1;
                        }
                    }
                    else if (thiefMove == 3)
                    {
                        thief.PositionY++;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 1;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 1;
                        }
                    }
                    else if (thiefMove == 4)
                    {
                        thief.PositionX++;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 1;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 1;
                        }
                    }
                    else if (thiefMove == 5)
                    {
                        thief.PositionY--;
                        if (thief.CharacterNumber == 1)
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Adam_złodziej_z_tłemb;
                            tour = 1;
                        }
                        else
                        {
                            imageBoard[x, y].Image = Properties.Resources.Droga_kwadrat_biały;
                            imageBoard[thief.PositionX, thief.PositionY].Image = Properties.Resources.Jaca_złodziej_z_tłemb;
                            tour = 1;
                        }
                    }
                    if (policeman1.CharacterNumber == 1)
                    {
                        pictureBox2.Image = Properties.Resources.Policjant_Filip;
                        label4.Text = "Policjant1";
                    }
                    else
                    {
                        pictureBox2.Image = Properties.Resources.Policjant_Piotrek;
                        label4.Text = "Policjant1";
                    }
                    tourCounter++;
                    label2.Text = tourCounter.ToString();
                    WinChecker();
                }
            }               
        }    
    }
}
