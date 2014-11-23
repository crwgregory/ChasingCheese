using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheese_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            CheeseFinder Game = new CheeseFinder();
            Game.PlayGame();
            Console.ReadKey();
        }
    }

    class Point
    {
        //create enumeration
        public enum PointStatus { Empty = 1, Cheese, Mouse, Cat, StuckCat}
        public int CatLives { get; set; }
        //call properties
        public int X { get; set; }
        public int Y { get; set; }
        

        public PointStatus Status { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Status = PointStatus.Empty;
            this.CatLives = 0;
        }
    }

    class CheeseFinder
    {
        //call properties
        public Point[,] Grid { get; set; }
        public int Round { get; set; }
        public Random rng { get; set; }
        public int GridSize { get; set; }

        public List<Point> ListOfCats { get; set; }
        //create constructor
        public CheeseFinder()
        {
            this.GridSize = 15;
            this.rng = new Random();

            this.ListOfCats = new List<Point>();
            
            //create the grid but do not put anything an it
            this.Grid = new Point[GridSize, GridSize];
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    Grid[x, y] = new Point(x, y);
                }
            }
            

            //Put a mouse in a random spot in the grid
            
            int YAxis = rng.Next(0, GridSize);
            int XAxis = rng.Next(0, GridSize);

            Grid[XAxis, YAxis].Status = Point.PointStatus.Mouse;

            bool gettingNewPoint = true;
            //put two pieces of cheese to start with
            for (int i = 0; i < 6; i++)
            {
                gettingNewPoint = true;
                while (gettingNewPoint)
                {
                    //get new values for x and y axis
                    int CheesEX = rng.Next(0, GridSize);
                    int CheesEY = rng.Next(0, GridSize);

                    //and set an instants of cheese
                    if (Grid[CheesEX, CheesEY].Status == Point.PointStatus.Empty)
                    {
                        Grid[CheesEX, CheesEY].Status = Point.PointStatus.Cheese;
                        gettingNewPoint = false;
                    }
                } 
            }
        }

        //methods
        public void DrawGrid()
        {
            Console.Clear();
            Console.WriteLine();
            for (int y = 0; y < GridSize; y++)
            {
                Console.Write( "|");
                for (int x = 0; x < GridSize; x++)
                {
                    Point newPoint = Grid[x, y];
                    if (Grid[x, y].Status == Point.PointStatus.Empty)
                    {
                        Console.Write("[   ]");
                    }
                    else if (Grid[x, y].Status == Point.PointStatus.Cheese)
                    {
                        Console.Write("[ C ]");
                    }
                    else if (Grid[x, y].Status == Point.PointStatus.Mouse)
                    {
                        Console.Write("[ M ]");
                    }
                    else if (Grid[x, y].Status == Point.PointStatus.Cat)
                    {
                        Console.Write("[ X ]");   
                    }
                    else if (Grid[x, y].Status == Point.PointStatus.StuckCat)
                    {
                        Console.Write("[ ! ]");
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
                Console.Write("|");
                Console.WriteLine();
                
            }
        }

        public int GetUserMove()
        {
            //returning an integer for move, if up then 1, if 
            //down then 3, if right then 2, if right then 4.
            
            //[ ][ ][1][ ][ ]
            //[ ][4][0][2][ ]  
            //[ ][ ][3][ ][ ]
            
            ConsoleKey input = new ConsoleKey();
            bool gettingInput = true;
            while (gettingInput)
            {
                input = Console.ReadKey().Key;

                if (input == ConsoleKey.UpArrow)
                {
                    
                    gettingInput = false;
                    return 1;
                }
                else if (input == ConsoleKey.DownArrow)
                {
                    
                    gettingInput = false;
                    return 3;
                }
                else if (input == ConsoleKey.RightArrow)
                {
                    
                    gettingInput = false;
                    return 2;
                }
                else if (input == ConsoleKey.LeftArrow)
                {
                    
                    gettingInput = false;
                    return 4;
                }
                else
                {
                    Console.WriteLine("Press up arrow, down arrow, left arrow, right arrow.");
                    
                } 
            }
            return 0;
        }

        public void PlaceCat()
        {
            while (true)
            {
                int XAxiS = rng.Next(0, GridSize);
                int YAxiS = rng.Next(0, GridSize);

                if (Grid[XAxiS, YAxiS].Status == Point.PointStatus.Empty)
                {
                    
                    Grid[XAxiS, XAxiS].Status = Point.PointStatus.Cat;
                    ListOfCats.Add(Grid[XAxiS, XAxiS]);
                    break;
                }
            }
            
        }
        public void MoveCat()
        {
            
            int chanceToMove = rng.Next(0, 10);
            foreach (Point catInList in Grid)
            {
                bool wasOnCheese = false;
                if (catInList.Status == Point.PointStatus.StuckCat)
                {
                    catInList.CatLives--;
                    if (catInList.CatLives < 1)
                    {
                        catInList.Status = Point.PointStatus.Cat;
                        wasOnCheese = true;
                    }
                }

                if (catInList.Status == Point.PointStatus.Cat)
                {
                    if (chanceToMove > 2)
                    {

                        
                        int catX = catInList.X;
                        int catY = catInList.Y;
                        int mouseX = 0;
                        int mouseY = 0;
                        foreach (Point mouse in Grid)
                        {
                            if (mouse.Status == Point.PointStatus.Mouse)
                            {
                                mouseX = mouse.X; mouseY = mouse.Y;
                                break;
                            }
                        }
                        int XRelative = mouseX - catInList.X;
                        int YRelative = mouseY - catInList.Y;

                        Point CatTarget = new Point(catX, catY);

                        //minus on x means cat is to the right
                        //minus on y cat on bottom

                        bool tryUp = YRelative < 0;
                        bool tryDown = YRelative > 0;
                        bool tryRight = XRelative > 0;
                        bool tryLeft = XRelative < 0;



                        while (true)
                        {
                            if (tryUp)
                            {
                                CatTarget.Y -= 1;
                                break;
                            }
                            else if (tryDown)
                            {
                                CatTarget.Y += 1;
                                break;

                            }
                            else if (tryRight)
                            {
                                CatTarget.X += 1;
                                break;

                            }
                            else if (tryLeft)
                            {
                                CatTarget.X -= 1;
                                break;
                            } 
                        }

                        
                        if (Grid[CatTarget.X, CatTarget.Y].Status == Point.PointStatus.Empty)
                        {
                            //means the spot going into is empty

                            Grid[catX, catY].Status = Point.PointStatus.Empty;
                            Grid[CatTarget.X, CatTarget.Y].Status = Point.PointStatus.Cat;
                            if (wasOnCheese)
                            {
                                Grid[catX, catY].Status = Point.PointStatus.Cheese;
                            }

                        }
                        else if (Grid[CatTarget.X, CatTarget.Y].Status == Point.PointStatus.Mouse)
                        {
                            ListOfCats.Remove(catInList);
                            Grid[catX, catY].Status = Point.PointStatus.Empty;
                            //Grid[catX, catY].Status = Point.PointStatus.Empty;
                        }
                        else if (Grid[CatTarget.X, CatTarget.Y].Status == Point.PointStatus.Cheese)
                        {
                            Grid[catX, catY].Status = Point.PointStatus.Empty;
                            Grid[CatTarget.X, CatTarget.Y].Status = Point.PointStatus.StuckCat;
                            Grid[CatTarget.X, CatTarget.Y].CatLives = 4;
                        }
                        else if (Grid[CatTarget.X, CatTarget.Y].Status == Point.PointStatus.Cat)
                        {
                            Grid[catX, catY].Status = Point.PointStatus.Cat;
                        }
                        
                    } 
                }

            }

        }

        public bool ValidMove(int input)
        {
            int MousesX = 0;
            int MousesY = 0;

            foreach (var item in Grid)                          //loop threw all items in array and find the one that is mouse
            {
                if (item.Status == Point.PointStatus.Mouse)
                {
                    //set mouses properties
                    MousesX = item.X;
                    MousesY = item.Y;
                    break;              //break loop cause we found what we needed.
                }
            }

            if (input == 1)
            {
                if (MousesY - 1 >= 0)
                {
                    //valid move return true
                    return true;
                }
                else
                {
                    //not valid
                    return false;
                }
            }
            else if (input == 2)
            {
                if (MousesX + 1 < GridSize)
                {
                    //valid move return true
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (input == 3)
            {
                if (MousesY + 1 < GridSize)
                {
                    //valid move return true
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                if (MousesX - 1 >= 0)
                {
                    //valid move return true
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool MoveMouse(bool ValidMove, int theMove)
        {
            int MousesXAxis = 0;
            int MousesYAxis = 0;
            
            if (ValidMove)
            {
                foreach (var item in Grid)             //loop threw all items in array and find the one that is mouse
                {
                    if (item.Status == Point.PointStatus.Mouse )
	                {
		                //set mouses properties
                        MousesXAxis = item.X;
                        MousesYAxis = item.Y;
                        break;
	                }
                }

                //create list of cheese points
                List<Point> cheesyPoints = new List<Point>();
                foreach (var item in Grid)
                {
                    if (item.Status == Point.PointStatus.Cheese)     
                    {
                        cheesyPoints.Add(item);
                    }
                }
                
                Point CurrentMousePoint = new Point(MousesXAxis, MousesYAxis);
               

                if (theMove % 2 == 0)        //means an xAxis Move
                {
                    if (theMove == 2)        //move to the right
                    {
                        MousesXAxis += 1;
                    }
                    else
                    {
                        //it is the four
                        MousesXAxis -= 1;
                    }
                }
                else
                {
                    //move is either 1 or 3
                    if (theMove == 1)
                    {
                        //move the y axis 'up'
                        MousesYAxis -= 1;
                    }
                    else
                    {
                        //move mouse down
                        MousesYAxis += 1;
                    }
                }
                //new values for cordinates have been set. Set future mouse position and see if it is the same as the cheese
                Point FutureMousePoint = new Point(MousesXAxis, MousesYAxis);

                foreach (Point cheesePlace in cheesyPoints)
                {
                    if (FutureMousePoint.X == cheesePlace.X)
                    {
                        if (FutureMousePoint.Y == cheesePlace.Y)
                        {
                            Grid[CurrentMousePoint.X, CurrentMousePoint.Y].Status = Point.PointStatus.Empty;

                            Grid[FutureMousePoint.X, FutureMousePoint.Y].Status = Point.PointStatus.Mouse;
                            
                            return true;
                        }
                    }
                }
                //if it is not the cheese
                Grid[CurrentMousePoint.X, CurrentMousePoint.Y].Status = Point.PointStatus.Empty;

                Grid[FutureMousePoint.X, FutureMousePoint.Y].Status = Point.PointStatus.Mouse;
                
            }
            return false;
        }
        public void PlayGame()
        {
            bool foundCheese = false;
            int numOfMoves = 0;
            int numOfCheesesFound = 0;
            int cheeseCounterToAddCat = 0;
            //bool testingCat = true;
            while (!foundCheese)       //if the cheese has not been found
            {
                if (numOfCheesesFound != 2)
                {
                    this.MoveCat(); 
                }
                this.DrawGrid();
                Console.WriteLine("Number of moves taken so far: " + numOfMoves + "\nYou have found " + numOfCheesesFound + " peices of cheese!");
                int userMove = this.GetUserMove();
                if (this.MoveMouse(this.ValidMove(userMove), userMove))
                {
                    numOfCheesesFound++;
                    cheeseCounterToAddCat++;
                }
                numOfMoves++;
                if (numOfMoves % 6 == 0)
                {
                    int CheeseX = rng.Next(0, GridSize);
                    int CheeseY = rng.Next(0, GridSize);

                    //and set an instants of cheese
                    if (Grid[CheeseX, CheeseY].Status == Point.PointStatus.Empty)
                    {
                        Grid[CheeseX, CheeseY].Status = Point.PointStatus.Cheese;
                    }
                }
                
                if (cheeseCounterToAddCat == 2)
                {
                    this.PlaceCat();

                    cheeseCounterToAddCat = 0;
                }
                //found how many pieces of cheese are on the board if it is zero then the player will win
                int numOfCheeseOnBoard = 0;
                foreach (Point cheessy in Grid)
                {
                    if (cheessy.Status == Point.PointStatus.Cheese)
                    {
                        numOfCheeseOnBoard++;
                    }
                }
                if (numOfCheeseOnBoard == 0)
                {
                    foundCheese = true;
                }
            }
            //if you get here you win.
            Console.Clear();
            Console.WriteLine("\nCongrats you won homie. It only took you: " + numOfMoves + " moves to find that stinky cheese! \nI think its so stinky that you can do it faster!");
        }

    }
}
