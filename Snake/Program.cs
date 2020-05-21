using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;
using System.IO;

class start
{
    public void mainmenu()
    {
        string end = "loop";
        do
        {
            string menu1 = @"mainmenu.txt";
            string menutxt = Path.GetFullPath(menu1);
            string[] menu = System.IO.File.ReadAllLines(menutxt);
            foreach (string line in menu)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\t \t \t  " + line);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(40, 16);
            Console.Write("Choice: ");
            Console.ForegroundColor = ConsoleColor.Red;
            ConsoleKeyInfo keyInput = Console.ReadKey();
            if (keyInput.Key == ConsoleKey.D1)
            {
                Console.Clear();
                end = "exitloop";
            }

            else if (keyInput.Key == ConsoleKey.D2)
            {
                var insloop = true;
                while (insloop)
                {
                    Console.Clear();
                    string help1 = @"helppage.txt";
                    string helptxt = Path.GetFullPath(help1);
                    string[] help = System.IO.File.ReadAllLines(helptxt);
                    foreach (string line in help)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\t \t \t \t" + line);
                    }
                    Console.SetCursorPosition(57, 13);
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.Enter)
                    {
                        insloop = false;
                        Console.Clear();
                    }
                }
            }
            else if (keyInput.Key == ConsoleKey.D3)
            {
                Console.SetCursorPosition(40, 17);
                System.Environment.Exit(1);
            }
            else
            {
                Console.Clear();
            }
        } while (end == "loop");
    }
}

class gamevictory
{
    public void gamevictoryscr(int userpoints)
    {
        string score = @"score.txt";
        string scoretxt = Path.GetFullPath(score);

        Console.SetCursorPosition(40, 6);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Congrats! You've won the game!");

        Console.SetCursorPosition(43, 8);
        Console.WriteLine("Please Enter your name:");

        Console.SetCursorPosition(52, 10);
        Console.ForegroundColor = ConsoleColor.White;
        string nametext = Console.ReadLine();

        using (System.IO.StreamWriter file =

                new System.IO.StreamWriter(scoretxt, true))
        {
            file.WriteLine(nametext + "(Won)" + " - " + userpoints.ToString()); // indicates that the user won the game
        }
        Console.SetCursorPosition(36, 15);
        Console.WriteLine("Please press the ENTER key to exit the game.");
        Console.SetCursorPosition(40, 17);
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            return;
        }
    }
}

class gameover
{
    public void gameoverscr(int userpoints)
    {
        string score = @"score.txt";
        string scoretxt = Path.GetFullPath(score);

        Console.SetCursorPosition(50, 6);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Game over!");
        //if (userPoints < 0) userPoints = 0;

        Console.SetCursorPosition(45, 7);
        Console.WriteLine("Your points are: {0}", userpoints);

        Console.SetCursorPosition(43, 8);
        Console.WriteLine("Please Enter your name:");

        Console.SetCursorPosition(52, 10);
        Console.ForegroundColor = ConsoleColor.White;
        string nametext = Console.ReadLine();

        using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(scoretxt, true))
        {
            file.WriteLine(nametext + " - " + userpoints.ToString());
        }
        Console.SetCursorPosition(36, 15);
        Console.WriteLine("Please press the ENTER key to exit the game."); //true here mean we won't output the key to the console, just cleaner in my opinion.

        Console.SetCursorPosition(40, 17);
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (keyInfo.Key == ConsoleKey.Enter)
        {
            return;
        }
    }
}

namespace Snake
{
    struct Position
    {
        public int x;
        public int y;
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Program
    {
        public static int lives = 3; // initialize the lives of the snake, in this case: 3.
        static void Main(string[] args)
        {
            // calling the main menu;
            start start = new start();
            start.mainmenu();

            var move = new System.Media.SoundPlayer();
            string move1 = @"move.wav";
            string movewav = Path.GetFullPath(move1);
            move.SoundLocation = movewav;
            System.Media.SoundPlayer eat = new System.Media.SoundPlayer();
            string eat1 = @"eat.wav";
            string eatwav = Path.GetFullPath(eat1);
            eat.SoundLocation = eatwav;
            System.Media.SoundPlayer gameover = new System.Media.SoundPlayer();
            string gameover1 = @"gameover.wav";
            string gameoverwav = Path.GetFullPath(gameover1);
            gameover.SoundLocation = gameoverwav;
            System.Media.SoundPlayer crash = new System.Media.SoundPlayer();
            string crash1 = @"crash.wav";
            string crashwav = Path.GetFullPath(crash1);
            crash.SoundLocation = crashwav;
            System.Media.SoundPlayer powerupsound = new System.Media.SoundPlayer();
            string powerup1 = @"powerup.wav";
            string powerupwav = Path.GetFullPath(powerup1);
            powerupsound.SoundLocation = powerupwav;
            byte right = 0;
            byte left = 1;
            byte down = 2;
            byte up = 3;
            int lastFoodTime = 0;
            int foodDissapearTime = 12000;
            int negativePoints = 0;
            double sleepTime = 100;
            int direction = right; // To make the snake go to the right when the program starts

            Random randomNumbersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
            lastFoodTime = Environment.TickCount;

            // Make sure the snake spawn with just 3 "*"s and spawn it on the top left of the screen

            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i <= 3; i++) //spawn snake body
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            List<Position> Scoreboard = new List<Position>() // scoreboard boundaries
            {
                    new Position(1, 20)
            };

            // Spawn the first 5 obstacles in the game 

            List<Position> obstacles = new List<Position>() //spawn the first obstacles
            {
                new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth)),
                new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth)),
                new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth)),
                new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth)),
                new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth)),
            };

            foreach (Position obstacle in obstacles) //write obstacle as "=" on declared position
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.SetCursorPosition(obstacle.y, obstacle.x);
                Console.Write("▒");
            }

            List<Position> powerups = new List<Position>()
            {
                new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                   randomNumbersGenerator.Next(0, Console.WindowWidth))
            };

            foreach (Position powerup in powerups) // powerup is created
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(powerup.y, powerup.x);
                Console.Write("+");
            }

            Position food;
            do //randomize where the food spawns
            {
                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                    randomNumbersGenerator.Next(0, Console.WindowWidth));
            }
            while (snakeElements.Contains(food) || obstacles.Contains(food) || Scoreboard.Contains(food)); //to make sure that food doesnt spawn on both snake, obstacles and scoreboard



            //Movement implementation

            Position[] directions = new Position[]
            {
                new Position(0, 1), // right
                new Position(0, -1), // left
                new Position(1, 0), // down
                new Position(-1, 0), // up
            };

            while (true) //read the direction of arrow key which user inputted
            {
                negativePoints++;

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey(true);
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                        move.Play();
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left) direction = right;
                        move.Play();
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down) direction = up;
                        move.Play();
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up) direction = down;
                        move.Play();
                    }
                }


                //Creating the snake:

                Position snakeHead = snakeElements.Last(); //make sure the head of the snake is spawned at the end of the "*" position
                Position nextDirection = directions[direction]; //initialize which direction is inputted

                Position snakeNewHead = new Position(snakeHead.x + nextDirection.x,
                                                     snakeHead.y + nextDirection.y); //snakehead will move to the same direction to which the user inputted

                // make sure the snake wont be able to go outside the screen
                if (snakeNewHead.y < 0)
                {
                    if (snakeNewHead.x == 0)
                    {
                        snakeNewHead.y = Console.WindowWidth - 21;
                    }

                    else
                    {
                        snakeNewHead.y = Console.WindowWidth - 1;
                    }
                }
                if (snakeNewHead.x < 0) snakeNewHead.x = Console.WindowHeight - 1;
                if (snakeNewHead.x >= Console.WindowHeight)
                {
                    if (snakeNewHead.y >= 20)
                    {
                        snakeNewHead.x = 1;
                    }

                    else
                    {
                        snakeNewHead.x = 0;
                    }
                }
                if (snakeNewHead.y >= Console.WindowWidth) snakeNewHead.y = 0;
                if (snakeNewHead.y >= Console.WindowWidth - 20 && snakeNewHead.x < 1) //scoreboard boundaries
                {
                    if (snakeNewHead.x == 1)
                    {
                        snakeNewHead.y = 0;
                    }

                    else
                    {
                        snakeNewHead.x = Console.WindowHeight - 1;
                    }
                }

                foreach (Position position in snakeElements) //writes the body of the snake as "*" on declared position
                {
                    Console.SetCursorPosition(position.y, position.x);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("*");
                }

                int userPoints = (snakeElements.Count - 4) * 100;

                // Show and update the score of the player

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(Console.WindowWidth - 10, 0);
                Console.Write("Score: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(Console.WindowWidth - 3, 0);
                Console.Write(userPoints);

                // Show and update the lives of the snake

                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(Console.WindowWidth - 20, 0);
                Console.WriteLine("Lives: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(Console.WindowWidth - 13, 0);
                Console.Write(lives);

                // the game will be over when the snake hits it body or the obstacles 3 times

                if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    crash.Play();
                    lives -= 1;
                }

                if (snakeElements.Contains(snakeNewHead) || powerups.Contains(snakeNewHead))
                {
                    powerupsound.Play();
                    lives += 1;
                }


                if (lives == 0)
                {
                    gameover.Play();
                    gameover _gameover = new gameover();
                    _gameover.gameoverscr(userPoints);
                    break;
                }

                // The game will be over and user will win if they reached 1000 points
                if (userPoints == 1000)
                {
                    gamevictory _gamevictory = new gamevictory();
                    _gamevictory.gamevictoryscr(userPoints);
                    break;
                }

                // writes the head of the snake as ">","<","^","v" to the position it is declared
                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.y, snakeNewHead.x);
                Console.ForegroundColor = ConsoleColor.Gray;
                if (direction == right) Console.Write(">");
                if (direction == left) Console.Write("<");
                if (direction == up) Console.Write("^");
                if (direction == down) Console.Write("v");


                //What will happened if the snake got fed:
                if (snakeNewHead.y == food.y && snakeNewHead.x == food.x)
                {
                    // Things that will be happening with the FOOD once it got ate by the snake
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight), //randomize the new position of the food
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food)); //writes "@" to indicate food to the designated position it randomized
                    eat.Play();
                    lastFoodTime = Environment.TickCount;
                    sleepTime--;

                    // Things that will be happening with the OBSTACLE once the FOOD got ate by the snake

                    Position obstacle = new Position(); // randomize the position of the obstacles
                    do
                    {
                        obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(obstacle) ||
                        obstacles.Contains(obstacle) ||
                        (food.x != obstacle.x && food.y != obstacle.y) || Scoreboard.Contains(food)); //
                    obstacles.Add(obstacle);
                    Console.SetCursorPosition(obstacle.y, obstacle.x);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("▒");
                }
                else
                {
                    // moving...
                    Position last = snakeElements.Dequeue(); // basically moving the snake and delete the last "body part" of the snake to maintain the length of the snake
                    Console.SetCursorPosition(last.y, last.x);
                    Console.Write(" ");
                }


                // Initialize the time taken for the food to spawn if the snake doesn't eat it

                if (Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    negativePoints = negativePoints + 50;
                    Console.SetCursorPosition(food.y, food.x);
                    Console.Write(" ");
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                            randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(food) || Scoreboard.Contains(food));
                    lastFoodTime = Environment.TickCount;
                }

                Console.SetCursorPosition(food.y, food.x);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("♥♥");
                sleepTime -= 0.01;

                Thread.Sleep((int)sleepTime);



            }
        }
    }
}