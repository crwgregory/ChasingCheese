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
        public enum PointStatus { Empty = 1, Cheese, Mouse, Cat}

        //call properties
        public int X { get; set; }
        public int Y { get; set; }

        public PointStatus Status { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Status = PointStatus.Empty;
        }
    }

    class CheeseFinder
    {
        //call properties
        public Point[,] Grid { get; set; }
        public Point Mouse { get; set; }
        public Point Cat { get; set; }
        public Point Cheese { get; set; }
        public int Round { get; set; }
        public Random rng { get; set; }

        public List<Point> ListOfCats { get; set; }
        //create constructor
        public CheeseFinder()
        {
            this.rng = new Random();
            this.Cat = new Point(0, 0);

            this.ListOfCats = new List<Point>();
            
            //create the grid but do not put anything an it
            this.Grid = new Point[10, 10];
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Grid[x, y] = new Point(x, y);
                }
            }
            

            //Put a mouse in a random spot in the grid
            
            int YAxis = rng.Next(0, 10);
            int XAxis = rng.Next(0, 10);

            Point Mouse = Grid[XAxis, YAxis];
            this.Mouse = Mouse;
            Mouse.Status = Point.PointStatus.Mouse;
            
            bool gettingNewPoint = true;
            //put two pieces of cheese to start with
            for (int i = 0; i < 6; i++)
            {
                gettingNewPoint = true;
                while (gettingNewPoint)
                {
                    //get new values for x and y axis
                    YAxis = rng.Next(0, 10);
                    XAxis = rng.Next(0, 10);

                    if (Mouse.X != XAxis && Mouse.Y != YAxis)
                    {
                        //and set an instants of cheese
                        Point Cheese = Grid[YAxis, XAxis];
                        Cheese.Status = Point.PointStatus.Cheese;
                        gettingNewPoint = false;
                    }
                } 
            }
        }

        //methods
        public void DrawGrid()
        {
            Console.Clear();

            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Point newPoint = Grid[x, y];
                    if (newPoint.Status == Point.PointStatus.Empty)
                    {
                        Console.Write("[ ]");
                    }
                    else if (newPoint.Status == Point.PointStatus.Cheese)
                    {
                        Console.Write("[C]");
                    }
                    else if (newPoint.Status == Point.PointStatus.Mouse)
                    {
                        Console.Write("[M]");
                    }
                    else if (newPoint.Status == Point.PointStatus.Cat)
                    {
                        Console.Write("[X]");   
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
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
                int XAxis = rng.Next(0, 10);
                int YAxis = rng.Next(0, 10);

                Point Cat = Grid[XAxis, YAxis];
                this.Cat = Cat;
                if (this.Cat.Status == Point.PointStatus.Empty)
                {
                    this.Cat.Status = Point.PointStatus.Cat;
                    break;
                }
            }
        }
        public void MoveCat()
        {


            if (ListOfCats.Count() > 0)
            {
                int XRelative = this.Mouse.X - this.Cat.X;
                int YRelative = this.Mouse.Y - this.Cat.Y;

                int catX = this.Cat.X; int catY = this.Cat.Y;
                int mouseX = this.Mouse.X; int mouseY = this.Mouse.Y;
                Point CatTarget = new Point(catX, catY);

                //minus on x means cat is to the right
                //minus on y cat on bottom

                bool tryUp = YRelative < 0;
                bool tryDown = YRelative > 0;
                bool tryRight = XRelative < 0;
                bool tryLeft = XRelative > 0;

                if (tryUp)
                {
                    CatTarget.Y -= 1;
                }
                if (tryDown)
                {
                    CatTarget.Y += 1;
                }
                if (tryRight)
                {
                    CatTarget.X += 1;
                }
                if (tryLeft)
                {
                    CatTarget.X -= 1;
                }
                else
                {
                    Console.WriteLine("error");
                }
                if (CatTarget.Status == Point.PointStatus.Empty)
                {
                    //means the spot going into is empty
                    this.Cat.Status = Point.PointStatus.Empty;
                    //this.Cat.X = CatTarget.X;
                    //this.Cat.Y = CatTarget.Y;
                    this.Cat = Grid[CatTarget.X, CatTarget.Y];
                    this.Cat.Status = Point.PointStatus.Cat;
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
                if (MousesX + 1<= 9)
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
                if (MousesY + 1 <= 9)
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
                
                Point CurrentMousePoint = Grid[MousesXAxis, MousesYAxis];
                Point FutureMousePoint = Grid[MousesXAxis, MousesYAxis];

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
                FutureMousePoint = Grid[MousesXAxis, MousesYAxis];

                foreach (Point cheesePlace in cheesyPoints)
                {
                    if (FutureMousePoint.X == cheesePlace.X)
                    {
                        if (FutureMousePoint.Y == cheesePlace.Y)
                        {
                            CurrentMousePoint.Status = Point.PointStatus.Empty;
                            FutureMousePoint.Status = Point.PointStatus.Mouse;
                            return true;
                        }
                    }
                }
                //if it is not the cheese
                CurrentMousePoint.Status = Point.PointStatus.Empty;
                FutureMousePoint.Status = Point.PointStatus.Mouse;
            }
            return false;
        }
        public void PlayGame()
        {
            bool foundCheese = false;
            int numOfMoves = 0;
            int numOfCheesesFound = 0;
            bool testingCat = true;
            while (!foundCheese)       //if the cheese has not been found
            {
                this.MoveCat();
                this.DrawGrid();
                Console.WriteLine("Number of moves taken so far: " + numOfMoves + "\nYou have found " + numOfCheesesFound + " peices of cheese!");
                int userMove = this.GetUserMove();
                if (this.MoveMouse(this.ValidMove(userMove), userMove))
                {
                    numOfCheesesFound++;
                }
                numOfMoves++;
                if (numOfMoves % 6 == 0)
                {
                    Random rng = new Random();
                    int YAxis = rng.Next(0, 10);
                    int XAxis = rng.Next(0, 10);

                    //and set an instants of cheese
                    Point Cheese = Grid[YAxis, XAxis];
                    if (Cheese.Status != Point.PointStatus.Mouse)
                    {
                        Cheese.Status = Point.PointStatus.Cheese; 
                    }
                }
                
                if (testingCat)
                {
                    this.PlaceCat();
                    testingCat = false;
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
