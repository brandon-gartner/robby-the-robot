using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobbyGeneticAlgo
{
    /// <summary>
    /// Delegates used in the project, creted here so all the classes have access
    /// </summary>
    public delegate double Fitness(Chromosome c);
    public delegate int AlleleMoveAndFitness(Chromosome c, Contents[,] grid, ref int x, ref int y);
    public delegate Chromosome[] Crossover(Chromosome a, Chromosome b);
    public delegate void GenerationEventHandler(int num, Generation g);



    /// <summary>
    /// This class contains some static helper methods
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Use this field to get any random number. Give it a seed for unit testing.
        /// </summary>
        public static readonly Random rand = new Random();
        private static string Result = "start";

        /// <summary>
        /// Staring point of the Console application
        /// </summary>
        public static void Main()
        {
            RobbyRobotProblem robby = new RobbyRobotProblem(4000, 200, Helpers.ScoreForAllele);
            //TODO subscribe to the RobbyRobotProblem’s GenerationReplaced event with the 
            // Display and the Print methods
            robby.GenerationReplacedEvent += Display;
            robby.GenerationReplacedEvent += Print;
            robby.Start();

        }


        /// <summary>
        /// Method to print to the console the generation  number and the fitness of the top chromoseme
        /// </summary>
        /// <param name="num"> The number of the current generation </param>
        /// <param name="gen"> The current generation </param>
        public static void Display(int num, Generation gen)
        {
            Console.WriteLine("Current Generation: " + num);
            Console.WriteLine("Fitness of Top Chromosome: " + gen[0].Fitness);
        }

        /// <summary>
        /// Method to write to a file the info of the 1st, 20th, 100th, 200th, 500th and 1000th generation. 
        /// </summary>
        /// <param name="num"> The number of the current generation </param>
        /// <param name="gen"> The current generation </param>
        public static void Print(int num, Generation gen)
        {
            Console.WriteLine("-----------------------------------------------------");
            string path = @"C:\JoaoPedro\C#\robby03\RobbyTheRobot\RobbyGeneticAlgo\info.txt";
            //String where we will append all the other results to
            string currentOne;
            //It will write the correspondent generation to the file
            //It should be "start;1,number;20,number;100...."
            switch (num)
            {
                //It will write the first generation to the file
                case 1:
                    currentOne = 1 + "," + gen[0].Fitness;
                    Result = Result + ";" + currentOne;
                    break;

                case 20:
                    currentOne = 20 + "," + gen[0].Fitness;
                    Result = Result + ";" + currentOne;
                    break;

                case 100:
                    currentOne = 100 + "," + gen[0].Fitness;
                    Result = Result + ";" + currentOne;
                    break;

                case 200:
                    currentOne = 200 + "," + gen[0].Fitness;
                    Result = Result + ";" + currentOne;
                    break;

                case 500:
                    currentOne = 500 + "," + gen[0].Fitness;
                    Result = Result + ";" + currentOne;
                    break;

                case 1000:
                    currentOne = 1000 + "," + gen[0].Fitness;
                    Result = Result + ";" + currentOne;
                    break;

            }
            File.WriteAllText(path, Result);
            Console.WriteLine(Result);
        }


        /// <summary>
        /// Applies the fitness function to move Robby through the given testgrid for numActions moves based on the
        /// Chromosome. The fitness function returns the fitness score after each move.
        /// </summary>
        /// <param name="testgrid"> Testgrid for Robby</param>
        /// <param name="c">Chromosome being tested</param>
        /// <param name="numActions">Number of moves that Robby is allowed</param>
        /// <param name="f">Fitness fuction that makes 1 move</param>
        /// <returns></returns>
        public static int RunRobbyInGrid(Contents[,] testgrid, Chromosome c, int numActions, AlleleMoveAndFitness f)
        {
            //starting point
            int x = Helpers.rand.Next(0, testgrid.GetLength(0));
            int y = Helpers.rand.Next(0, testgrid.GetLength(1));

            //make a copy of the grid
            Contents[,] grid = (Contents[,])testgrid.Clone();

            //make moves, count score
            int score = 0;
            for (int i = 0; i < numActions; i++)
            {
                //get score
                score += f(c, grid, ref x, ref y);
            }
            return score;
        }

        /// <summary>
        /// Used to fill up a DirectionsContent struct based on Robby's position in the 
        /// grid and what is immediately adjacent to him.
        /// </summary>
        /// <param name="x">Robby's x coordinates</param>
        /// <param name="y">Robby's y coordinates</param>
        /// <param name="grid">The test grid where Robby is</param>
        /// <returns>What Robby sees in all directions plus current</returns>
        public static DirectionContents LookAround(int x, int y, Contents[,] grid)
        {
            //what do you see?
            DirectionContents dir = new DirectionContents();
            //where are the walls?
            if (y == 0)
                dir.N = Contents.Wall; //wall
            else
                dir.N = grid[x, y - 1];

            if (y == grid.GetLength(1) - 1)
                dir.S = Contents.Wall;
            else
                dir.S = grid[x, y + 1];

            if (x == grid.GetLength(0) - 1)
                dir.E = Contents.Wall;
            else
                dir.E = grid[x + 1, y];

            if (x == 0)
                dir.W = Contents.Wall;
            else
                dir.W = grid[x - 1, y];

            dir.Current = grid[x, y];

            return dir;
        }

        /// <summary>
        /// Translates Robby's DirectionContents into the appropriate gene index
        /// </summary>
        /// <param name="dir">The struct returned by LookAround</param>
        /// <returns>The index of the Chromosome</returns>
        public static int FindGeneIndex(DirectionContents dir)
        {
            int gene = 0;
            gene += getIndexForDirection(dir.N, 4);
            gene += getIndexForDirection(dir.S, 3);
            gene += getIndexForDirection(dir.E, 2);
            gene += getIndexForDirection(dir.W, 1);
            gene += getIndexForDirection(dir.Current, 0);
            return gene;
        }

        /// <summary>
        /// Used to build up the index of the gene in the Chromosome
        /// </summary>
        /// <param name="content">Content in a given direction</param>
        /// <param name="power">Exponent of 10</param>
        /// <returns>Partial calculation of the gene's index</returns>
        private static int getIndexForDirection(Contents content, int power)
        {
            if (content == Contents.Empty)
                return 0;
            if (content == Contents.Can)
                return (int)(Math.Pow(3, power));
            //Wall
            return (int)(2 * Math.Pow(3, power));
        }

        /// <summary>
        /// Used to generate a single test grid filled with cans in random locations. Half of 
        /// the grid (rounded down) will be filled with cans.
        /// </summary>
        /// <param name="gridSize">Width or height of a square grid</param>
        /// <returns>Rectangular array of Contents filled with 50% Cans, and 50% Empty </returns>
        public static Contents[,] GenerateRandomTestGrid(int gridSize)
        {
            Contents[,] grid = new Contents[gridSize, gridSize];
            int numOfCans = (int)Math.Sqrt(gridSize) / 2;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Contents content = GetRandomContent(numOfCans);
                    if (content == Contents.Can)
                    {
                        numOfCans--;
                    }
                    grid[i, j] = content;
                }
            }

            return grid;
        }

        /// <summary>
        /// Moves Robby and returns to score for a single move in the grid, given the
        /// Chromosome and his position.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="grid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int ScoreForAllele(Chromosome c, Contents[,] grid, ref int x, ref int y)
        {
            DirectionContents dir = Helpers.LookAround(x, y, grid);
            //find the gene
            int gene = Helpers.FindGeneIndex(dir);
            //find the move
            Allele move = c[gene];
            bool done;
            do
            {
                done = true;
                switch (move)
                {
                    case Allele.North://move north
                        if (dir.N == Contents.Wall)
                            return -5;
                        y -= 1;
                        break;
                    case Allele.South://move south
                        if (dir.S == Contents.Wall)
                            return -5;
                        y += 1;
                        break;
                    case Allele.East: //move east
                        if (dir.E == Contents.Wall)
                            return -5;
                        x += 1;
                        break;
                    case Allele.West: //move west
                        if (dir.W == Contents.Wall)
                            return -5;
                        x -= 1;
                        break;
                    case Allele.Nothing: //do nothong
                        break;
                    case Allele.PickUp: //pick up can
                        if (grid[x, y] == Contents.Can) //there is a can
                        {
                            grid[x, y] = Contents.Empty;
                            return +10;
                        }
                        else
                            return -1; //penalty for picking up nothing
                    case Allele.Random: //random move
                        done = false;
                        int num = rand.Next(0, Enum.GetNames(typeof(Allele)).Length);
                        move = (Allele)num;
                        break;
                }
            }
            while (!done);
            return 0;
        }

        public static int FindMin(int[] arr)
        {
            int min = arr[0];

            for (int i = 1; i < arr.Length - 1; i++)
            {
                if (min > arr[i])
                {
                    min = arr[i];
                }
            }
            return min;
        }

        public static Contents GetRandomContent(int numOfcans)
        {
            if (numOfcans > 0)
            {
                switch (rand.Next(1))
                {
                    case 0:
                        return Contents.Can;

                    case 1:
                        return Contents.Empty;
                }
            }

            return Contents.Empty;
        }
    }
}
